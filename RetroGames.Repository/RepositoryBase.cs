using MySqlConnector;

namespace RetroGames.Repository;

public abstract class RepositoryBase(RepositoryConfig config)
{
    protected async Task<MySqlConnection> CriarConexaoAsync()
    {
        var conexao = new MySqlConnection(config.ConnectionString);
        await conexao.OpenAsync();
        return conexao;
    }

    protected static MySqlCommand CriarComando(MySqlConnection conexao, string sql)
    {
        return new MySqlCommand(sql, conexao);
    }

    protected static void AdicionarParametro(MySqlCommand comando, string nome, object? valor)
    {
        comando.Parameters.AddWithValue(nome, valor ?? DBNull.Value);
    }

    protected async Task<int> ExecutarGravacaoAsync(MySqlCommand comando)
    {
        try
        {
            await comando.ExecuteNonQueryAsync();
            return (int)comando.LastInsertedId;
        }
        catch (MySqlException ex) when (ex.Number == 1062)
        {
            throw new RegistroDuplicadoException("Registro duplicado no banco de dados.", ex);
        }
    }
}
