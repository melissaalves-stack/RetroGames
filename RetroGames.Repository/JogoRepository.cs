using RetroGames.Model.Model;

namespace RetroGames.Repository;

public class JogoRepository(RepositoryConfig config) : RepositoryBase(config), IJogoRepository
{
    public async Task<(List<Jogo> Itens, int Total)> ListarAsync(string? busca, int? idPlataforma, List<int>? idsJogos, bool incluirInativos, int pagina, int tamanhoPagina)
    {
        await using var conexao = await CriarConexaoAsync();

        var OndeClauses = new List<string>();
        if (!incluirInativos)
        {
            OndeClauses.Add("st_ativo = TRUE");
        }
        if (!string.IsNullOrWhiteSpace(busca))
        {
            OndeClauses.Add("no_jogo LIKE @busca");
        }
        if (idPlataforma.HasValue)
        {
            OndeClauses.Add("co_plataforma = @idPlataforma");
        }
        if (idsJogos != null && idsJogos.Count > 0)
        {
            OndeClauses.Add($"co_seq_jogo IN ({string.Join(",", idsJogos)})");
        }

        var filtroSql = OndeClauses.Count > 0 ? " WHERE " + string.Join(" AND ", OndeClauses) : "";

        var sqlTotal = "SELECT COUNT(*) FROM tb_jogo" + filtroSql;
        await using var cmdTotal = CriarComando(conexao, sqlTotal);
        AdicionarParametrosFiltro(cmdTotal, busca, idPlataforma);
        var total = Convert.ToInt32(await cmdTotal.ExecuteScalarAsync());

        var offset = (pagina - 1) * tamanhoPagina;
        var sqlListar = $@"SELECT co_seq_jogo, no_jogo, ds_jogo, nu_ano_lancamento, vl_preco, qt_estoque, 
                                  st_ativo, dt_cadastro, co_plataforma, co_condicao_jogo 
                           FROM tb_jogo
                           {filtroSql}
                           ORDER BY no_jogo
                           LIMIT @tamanhoPagina OFFSET @offset";

        await using var cmdListar = CriarComando(conexao, sqlListar);
        AdicionarParametrosFiltro(cmdListar, busca, idPlataforma);
        AdicionarParametro(cmdListar, "@tamanhoPagina", tamanhoPagina);
        AdicionarParametro(cmdListar, "@offset", offset);

        await using var leitor = await cmdListar.ExecuteReaderAsync();
        var lista = new List<Jogo>();
        while (await leitor.ReadAsync())
        {
            lista.Add(MapearJogo(leitor));
        }

        return (lista, total);
    }

    public async Task<Jogo?> BuscarPorIdAsync(int id)
    {
        await using var conexao = await CriarConexaoAsync();
        const string sql = @"SELECT co_seq_jogo, no_jogo, ds_jogo, nu_ano_lancamento, vl_preco, qt_estoque, 
                                    st_ativo, dt_cadastro, co_plataforma, co_condicao_jogo 
                             FROM tb_jogo WHERE co_seq_jogo = @id";
        await using var comando = CriarComando(conexao, sql);
        AdicionarParametro(comando, "@id", id);
        await using var leitor = await comando.ExecuteReaderAsync();

        if (await leitor.ReadAsync())
        {
            return MapearJogo(leitor);
        }
        return null;
    }

    public async Task<int> InserirAsync(Jogo jogo)
    {
        await using var conexao = await CriarConexaoAsync();
        const string sql = @"INSERT INTO tb_jogo (no_jogo, ds_jogo, nu_ano_lancamento, vl_preco, qt_estoque, st_ativo, dt_cadastro, co_plataforma, co_condicao_jogo) 
                            VALUES (@nome, @descricao, @anoLancamento, @preco, @estoque, @ativo, @dataCadastro, @idPlataforma, @idCondicaoJogo)";
        await using var comando = CriarComando(conexao, sql);
        AdicionarParametrosJogo(comando, jogo);

        return await ExecutarGravacaoAsync(comando);
    }

    public async Task AlterarAsync(Jogo jogo)
    {
        await using var conexao = await CriarConexaoAsync();
        const string sql = @"UPDATE tb_jogo 
                            SET no_jogo = @nome, ds_jogo = @descricao, nu_ano_lancamento = @anoLancamento, 
                                vl_preco = @preco, qt_estoque = @estoque, st_ativo = @ativo, 
                                co_plataforma = @idPlataforma, co_condicao_jogo = @idCondicaoJogo 
                            WHERE co_seq_jogo = @id";
        await using var comando = CriarComando(conexao, sql);
        AdicionarParametro(comando, "@id", jogo.Id);
        AdicionarParametrosJogo(comando, jogo);

        await ExecutarGravacaoAsync(comando);
    }

    public async Task DesativarAsync(int id)
    {
        await using var conexao = await CriarConexaoAsync();
        const string sql = "UPDATE tb_jogo SET st_ativo = FALSE WHERE co_seq_jogo = @id";
        await using var comando = CriarComando(conexao, sql);
        AdicionarParametro(comando, "@id", id);
        await comando.ExecuteNonQueryAsync();
    }

    private static void AdicionarParametrosFiltro(MySqlCommand comando, string? busca, int? idPlataforma)
    {
        if (!string.IsNullOrWhiteSpace(busca))
        {
            AdicionarParametro(comando, "@busca", $"%{busca}%");
        }
        if (idPlataforma.HasValue)
        {
            AdicionarParametro(comando, "@idPlataforma", idPlataforma.Value);
        }
    }

    private static void AdicionarParametrosJogo(MySqlCommand comando, Jogo jogo)
    {
        AdicionarParametro(comando, "@nome", jogo.Nome);
        AdicionarParametro(comando, "@descricao", jogo.Descricao);
        AdicionarParametro(comando, "@anoLancamento", jogo.AnoLancamento);
        AdicionarParametro(comando, "@preco", jogo.Preco);
        AdicionarParametro(comando, "@estoque", jogo.Estoque);
        AdicionarParametro(comando, "@ativo", jogo.Ativo);
        AdicionarParametro(comando, "@dataCadastro", jogo.DataCadastro);
        AdicionarParametro(comando, "@idPlataforma", jogo.IdPlataforma);
        AdicionarParametro(comando, "@idCondicaoJogo", jogo.IdCondicaoJogo);
    }

    private static Jogo MapearJogo(MySqlDataReader leitor)
    {
        return new Jogo
        {
            Id = leitor.GetInt32("co_seq_jogo"),
            Nome = leitor.GetString("no_jogo"),
            Descricao = leitor.IsDBNull(leitor.GetOrdinal("ds_jogo")) ? null : leitor.GetString("ds_jogo"),
            AnoLancamento = leitor.GetInt32("nu_ano_lancamento"),
            Preco = leitor.GetDecimal("vl_preco"),
            Estoque = leitor.GetInt32("qt_estoque"),
            Ativo = leitor.GetBoolean("st_ativo"),
            DataCadastro = leitor.GetDateTime("dt_cadastro"),
            IdPlataforma = leitor.GetInt32("co_plataforma"),
            IdCondicaoJogo = leitor.GetInt32("co_condicao_jogo")
        };
    }
}
