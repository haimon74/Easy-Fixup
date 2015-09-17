using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace ezFixUp.Classes
{
    public class SqlHelper
    {
        //public static Database GetDB(System.Data.SqlClient.SqlConnection conn)
        //{
        //    //DatabaseFactory.CreateDatabase();
        //    return DatabaseFactory.CreateDatabase("ezFixUpDB");
        //    //return EnterpriseLibraryContainer.Current.GetInstance<Database>();
        //}
        public static Database GetDB()
        {
            var db = DatabaseFactory.CreateDatabase("ezFixUpDB");
            return db;
            //db.GetStoredProcCommand("").ExecuteReader(CommandBehavior.CloseConnection).Close();
        }
    }


    //public class DataWorker
    //{
    //    private static Database _database = null;

    //    static DataWorker()
    //    {
    //        try
    //        {
    //            _database = DatabaseFactory.CreateDatabase();
    //        }
    //        catch (Exception excep)
    //        {
    //            throw excep;
    //        }
    //    }

    //    public static Database database
    //    {
    //        get { return _database; }
    //    }
    //}

    //public sealed class DatabaseFactorySectionHandler : ConfigurationSection
    //{
    //    [ConfigurationProperty("name")]
    //    public string Name
    //    {
    //        get { return (string)base["Name"]; }
    //    }

    //    [ConfigurationProperty("ezFixUpDB")]
    //    public string ConnectionStringName
    //    {
    //        get { return (string)base["ezFixUpDB"]; }
    //    }

    //    public string ConnectionString
    //    {
    //        get
    //        {
    //            try
    //            {
    //                return ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString;
    //            }
    //            catch (Exception excep)
    //            {
    //                throw new Exception("Connection string " + ConnectionStringName + " was not found in web.config. " + excep.Message);
    //            }
    //        }
    //    }
    //}

    //public class EasyFixUpSqlDatabase : Database
    //{
    //    public EasyFixUpSqlDatabase(string connectionString, DbProviderFactory dbProviderFactory) : 
    //        base(connectionString, dbProviderFactory)
    //    {
    //    }

    //    public EasyFixUpSqlDatabase(string connectionString, DbProviderFactory dbProviderFactory, IDataInstrumentationProvider instrumentationProvider) : 
    //        base(connectionString, dbProviderFactory, instrumentationProvider)
    //    {
    //    }

    //    public override DbConnection CreateConnection()
    //    {
    //        return new SqlConnection("ezFixUpDB");
    //    }

    //    public IDbCommand CreateCommand()
    //    {
    //        return new SqlCommand();
    //    }

    //    public IDbConnection CreateOpenConnection()
    //    {
    //        SqlConnection connection = (SqlConnection)CreateConnection();
    //        connection.Open();

    //        return connection;
    //    }

    //    public IDbCommand CreateCommand(string commandText, IDbConnection connection)
    //    {
    //        SqlCommand command = (SqlCommand)CreateCommand();

    //        command.CommandText = commandText;
    //        command.Connection = (SqlConnection)connection;
    //        command.CommandType = CommandType.Text;

    //        return command;
    //    }

    //    public IDbCommand CreateStoredProcCommand(string procName, IDbConnection connection)
    //    {
    //        SqlCommand command = (SqlCommand)CreateCommand();

    //        command.CommandText = procName;
    //        command.Connection = (SqlConnection)connection;
    //        command.CommandType = CommandType.StoredProcedure;

    //        return command;
    //    }

    //    public IDataParameter CreateParameter(string parameterName, object parameterValue)
    //    {
    //        return new SqlParameter(parameterName, parameterValue);
    //    }

    //    protected override void DeriveParameters(DbCommand discoveryCommand)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}


    //public class IoC
    //{
    //    public static void Intialise(UnityConfigurationSection section, string connectionString)
    //    {
    //        _connectionString = connectionString;
    //        _container = new UnityContainer();
    //        section.Configure(_container);
    //    }

    //    private static IUnityContainer _container;
    //    private static string _connectionString;

    //    public static IMovementRepository MovementRepository
    //    {
    //        get { return _container.Resolve<IMovementRepository>(); }
    //    }
    //}
}

















