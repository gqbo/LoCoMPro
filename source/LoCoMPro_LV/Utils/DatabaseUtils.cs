using System.Data.SqlClient;
namespace LoCoMPro_LV.Utils;
public class DatabaseUtils
{
    /// <summary>
    /// Se utiliza para acceder a las configuración de appsettings.json.
    /// </summary>
    private readonly IConfiguration _configuration;

    public DatabaseUtils(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetConnectionStringName()
    {
        return _configuration.GetSection("ConnectionStrings").GetChildren().FirstOrDefault()?.Key;
    }

    public string GetConnectionString()
    {
        string connectionStringName = GetConnectionStringName();
        if (!string.IsNullOrEmpty(connectionStringName))
        {
            return _configuration.GetConnectionString(connectionStringName);
        }
        else
        {
            throw new InvalidOperationException("A connection string name was not found in the configuration.");
        }
    }

    /// <summary>
    /// Método utilizado para ejecutar funciones escalares basado en un query y ciertos parámetros.
    /// <param name="connectionString">Connection string utilizado para la conexión de la base de datos.</param>
    /// <param name="sqlQueryScalar">String con una consulta SQL que hace un llamado a una función escalar .</param>
    /// <param name="parameters">Parámetros que van a ser utilizados dentro de la consulta SQL con la función escalar.</param>
    /// </summary>
    public static T ExecuteScalar<T>(string connectionString, string sqlQueryScalar, SqlParameter[] parameters)
    {
        if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(sqlQueryScalar))
        {
            throw new ArgumentException("Parameters 'connectionString' or 'sqlQuery' cannot be null or empty.");
        }

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(sqlQueryScalar, connection))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                connection.Open();
                var result = command.ExecuteScalar();

                if (result != DBNull.Value && result != null)
                {
                    return (T)Convert.ChangeType(result, typeof(T));
                }
            }
        }
        return default(T);
    }
}
