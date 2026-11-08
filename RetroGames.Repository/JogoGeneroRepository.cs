using RetroGames.Model.Model;

namespace RetroGames.Repository;

public class JogoGeneroRepository(RepositoryConfig config) : RepositoryBase(config), IJogoGeneroRepository
{
    public async Task<List<JogoGenero>> ListarTodosAsync()
    {
        await using var conexao = await CriarConexaoAsync();
        const string sql = "SELECT co_jogo, co_genero FROM tb_jogo_genero";
        await using var comando = CriarComando(conexao, sql);
        await using var leitor = await comando.ExecuteReaderAsync();

        var lista = new List<JogoGenero>();
        while (await leitor.ReadAsync())
        {
            lista.Add(new JogoGenero
            {
                IdJogo = leitor.GetInt32("co_jogo"),
                IdGenero = leitor.GetInt32("co_genero")
            });
        }
        return lista;
    }

    public async Task<List<JogoGenero>> ListarPorJogosAsync(List<int> idsJogos)
    {
        if (idsJogos == null || idsJogos.Count == 0) return [];

        await using var conexao = await CriarConexaoAsync();
        var sql = $"SELECT co_jogo, co_genero FROM tb_jogo_genero WHERE co_jogo IN ({string.Join(",", idsJogos)})";
        await using var comando = CriarComando(conexao, sql);
        await using var leitor = await comando.ExecuteReaderAsync();

        var lista = new List<JogoGenero>();
        while (await leitor.ReadAsync())
        {
            lista.Add(new JogoGenero
            {
                IdJogo = leitor.GetInt32("co_jogo"),
                IdGenero = leitor.GetInt32("co_genero")
            });
        }
        return lista;
    }

    public async Task InserirAsync(int idJogo, List<int> idsGeneros)
    {
        if (idsGeneros == null || idsGeneros.Count == 0) return;

        await using var conexao = await CriarConexaoAsync();
        foreach (var idGenero in idsGeneros)
        {
            const string sql = "INSERT INTO tb_jogo_genero (co_jogo, co_genero) VALUES (@idJogo, @idGenero)";
            await using var comando = CriarComando(conexao, sql);
            AdicionarParametro(comando, "@idJogo", idJogo);
            AdicionarParametro(comando, "@idGenero", idGenero);
            await comando.ExecuteNonQueryAsync();
        }
    }

    public async Task RemoverPorJogoAsync(int idJogo)
    {
        await using var conexao = await CriarConexaoAsync();
        const string sql = "DELETE FROM tb_jogo_genero WHERE co_jogo = @idJogo";
        await using var comando = CriarComando(conexao, sql);
        AdicionarParametro(comando, "@idJogo", idJogo);
        await comando.ExecuteNonQueryAsync();
    }
}
