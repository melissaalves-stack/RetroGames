using RetroGames.Model.Model;

namespace RetroGames.Repository;

public class PlataformaRepository(RepositoryConfig config) : RepositoryBase(config), IPlataformaRepository
{
    public async Task<List<Plataforma>> ListarAsync()
    {
        await using var conexao = await CriarConexaoAsync();
        const string sql = "SELECT co_seq_plataforma, no_plataforma, no_fabricante, nu_ano_lancamento FROM tb_plataforma ORDER BY no_plataforma";
        await using var comando = CriarComando(conexao, sql);
        await using var leitor = await comando.ExecuteReaderAsync();

        var lista = new List<Plataforma>();
        while (await leitor.ReadAsync())
        {
            lista.Add(new Plataforma
            {
                Id = leitor.GetInt32("co_seq_plataforma"),
                Nome = leitor.GetString("no_plataforma"),
                Fabricante = leitor.GetString("no_fabricante"),
                AnoLancamento = leitor.GetInt32("nu_ano_lancamento")
            });
        }
        return lista;
    }

    public async Task<Plataforma?> BuscarPorIdAsync(int id)
    {
        await using var conexao = await CriarConexaoAsync();
        const string sql = "SELECT co_seq_plataforma, no_plataforma, no_fabricante, nu_ano_lancamento FROM tb_plataforma WHERE co_seq_plataforma = @id";
        await using var comando = CriarComando(conexao, sql);
        AdicionarParametro(comando, "@id", id);
        await using var leitor = await comando.ExecuteReaderAsync();

        if (await leitor.ReadAsync())
        {
            return new Plataforma
            {
                Id = leitor.GetInt32("co_seq_plataforma"),
                Nome = leitor.GetString("no_plataforma"),
                Fabricante = leitor.GetString("no_fabricante"),
                AnoLancamento = leitor.GetInt32("nu_ano_lancamento")
            };
        }
        return null;
    }

    public async Task<bool> ExisteComNomeAsync(string nome, int? idIgnorar = null)
    {
        await using var conexao = await CriarConexaoAsync();
        var sql = "SELECT COUNT(*) FROM tb_plataforma WHERE no_plataforma = @nome";
        if (idIgnorar.HasValue)
        {
            sql += " AND co_seq_plataforma <> @idIgnorar";
        }
        await using var comando = CriarComando(conexao, sql);
        AdicionarParametro(comando, "@nome", nome);
        if (idIgnorar.HasValue)
        {
            AdicionarParametro(comando, "@idIgnorar", idIgnorar.Value);
        }

        var total = Convert.ToInt32(await comando.ExecuteScalarAsync());
        return total > 0;
    }

    public async Task<bool> PossuiJogosAsync(int id)
    {
        await using var conexao = await CriarConexaoAsync();
        const string sql = "SELECT COUNT(*) FROM tb_jogo WHERE co_plataforma = @id";
        await using var comando = CriarComando(conexao, sql);
        AdicionarParametro(comando, "@id", id);

        var total = Convert.ToInt32(await comando.ExecuteScalarAsync());
        return total > 0;
    }

    public async Task<int> InserirAsync(Plataforma plataforma)
    {
        await using var conexao = await CriarConexaoAsync();
        const string sql = @"INSERT INTO tb_plataforma (no_plataforma, no_fabricante, nu_ano_lancamento) 
                            VALUES (@nome, @fabricante, @anoLancamento)";
        await using var comando = CriarComando(conexao, sql);
        AdicionarParametro(comando, "@nome", plataforma.Nome);
        AdicionarParametro(comando, "@fabricante", plataforma.Fabricante);
        AdicionarParametro(comando, "@anoLancamento", plataforma.AnoLancamento);

        return await ExecutarGravacaoAsync(comando);
    }

    public async Task AlterarAsync(Plataforma plataforma)
    {
        await using var conexao = await CriarConexaoAsync();
        const string sql = @"UPDATE tb_plataforma 
                            SET no_plataforma = @nome, no_fabricante = @fabricante, nu_ano_lancamento = @anoLancamento 
                            WHERE co_seq_plataforma = @id";
        await using var comando = CriarComando(conexao, sql);
        AdicionarParametro(comando, "@id", plataforma.Id);
        AdicionarParametro(comando, "@nome", plataforma.Nome);
        AdicionarParametro(comando, "@fabricante", plataforma.Fabricante);
        AdicionarParametro(comando, "@anoLancamento", plataforma.AnoLancamento);

        await ExecutarGravacaoAsync(comando);
    }

    public async Task ExcluirAsync(int id)
    {
        await using var conexao = await CriarConexaoAsync();
        const string sql = "DELETE FROM tb_plataforma WHERE co_seq_plataforma = @id";
        await using var comando = CriarComando(conexao, sql);
        AdicionarParametro(comando, "@id", id);
        await comando.ExecuteNonQueryAsync();
    }
}
