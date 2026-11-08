using RetroGames.Model.Model;

namespace RetroGames.Repository;

public class CondicaoJogoRepository(RepositoryConfig config) : RepositoryBase(config), ICondicaoJogoRepository
{
    public async Task<List<CondicaoJogo>> ListarAsync()
    {
        await using var conexao = await CriarConexaoAsync();
        const string sql = "SELECT co_condicao_jogo, ds_condicao_jogo FROM tb_condicao_jogo ORDER BY co_condicao_jogo";
        await using var comando = CriarComando(conexao, sql);
        await using var leitor = await comando.ExecuteReaderAsync();

        var lista = new List<CondicaoJogo>();
        while (await leitor.ReadAsync())
        {
            lista.Add(new CondicaoJogo
            {
                Id = leitor.GetInt32("co_condicao_jogo"),
                Descricao = leitor.GetString("ds_condicao_jogo")
            });
        }
        return lista;
    }
}
