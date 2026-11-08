using RetroGames.Model.Model;

namespace RetroGames.Repository;

public class GeneroRepository(RepositoryConfig config) : RepositoryBase(config), IGeneroRepository
{
    public async Task<List<Genero>> ListarAsync()
    {
        await using var conexao = await CriarConexaoAsync();
        const string sql = "SELECT co_seq_genero, no_genero FROM tb_genero ORDER BY no_genero";
        await using var comando = CriarComando(conexao, sql);
        await using var leitor = await comando.ExecuteReaderAsync();

        var lista = new List<Genero>();
        while (await leitor.ReadAsync())
        {
            lista.Add(new Genero
            {
                Id = leitor.GetInt32("co_seq_genero"),
                Nome = leitor.GetString("no_genero")
            });
        }
        return lista;
    }
}
