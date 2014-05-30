using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using Simpler.Data.Tasks;

namespace Simpler.Data
{
    public static class Db
    {
        public static IDbConnection Connect(string connectionName)
        {
            var connectionConfig = ConfigurationManager.ConnectionStrings[connectionName];
            if (connectionConfig == null) throw new ConnectException(String.Format(
                "A connectionString with name {0} was not found in the configuration file.", 
                connectionName
            ));

            var connectionString = connectionConfig.ConnectionString;
            var providerName = connectionConfig.ProviderName;
            var provider = DbProviderFactories.GetFactory(providerName);
            
            var connection = provider.CreateConnection();
            if (connection == null) throw new ConnectException(String.Format(
                @"Error creating DbProviderFactory connection using a connectionString {0} with a provider type of {1}.",
                connectionName,
                providerName
            ));

            connection.ConnectionString = connectionString;
            connection.Open();

            return connection;
        }

        public static Results Run(IDbConnection connection, string sql, object values = null, int timeout = 30)
        {
            using (var command = connection.CreateCommand())
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                command.Connection = connection;
                command.CommandText = sql;

                if (values != null)
                {
                    var buildParameters = Task.New<BuildParameters>();
                    buildParameters.In.Command = command;
                    buildParameters.In.Values = values;
                    buildParameters.Execute();
                }

                command.CommandTimeout = timeout;
                var reader = command.ExecuteReader();
                return new Results(reader);
            }
        }
    }
}
