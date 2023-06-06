using MySql.Data.MySqlClient;
using Rocket.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomoly.KitsPlus.Utils
{
    public class DbConnection : IDisposable
    {
        private MySqlConnection connection;
        private string connectionString;

        public DbConnection(string connectionString)
        {
            this.connectionString = connectionString;
        }

        private void OpenConnection()
        {
            if (connection == null)
            {
                connection = new MySqlConnection(connectionString);
            }

            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }
        }
        private MySqlCommand PrepareCommand(string query, params object[] parameterValues)
        {
            OpenConnection();

            var command = new MySqlCommand(query, connection);

            int parameterIndex = 0;

            for (int i = 0; i < query.Length; i++)
            {
                if (query[i] == '?')
                {
                    // Skip if the question mark is inside a string literal
                    if (i > 0 && query[i - 1] == '\'')
                        continue;

                    var parameterName = $"@p{++parameterIndex}";
                    var parameterValue = parameterValues[parameterIndex - 1];
                    command.Parameters.AddWithValue(parameterName, parameterValue);

                    query = query.Remove(i, 1).Insert(i, parameterName);
                }
            }

            command.CommandText = query;

            return command;
        }

        public int ExecuteUpdate(string query, params object[] parameterValues)
        {
            try
            {
                var command = PrepareCommand(query, parameterValues);
                return command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Failed to execute update query");
                throw ex;
            }
        }

        public MySqlDataReader Execute(string query, params object[] parameterValues)
        {
            try
            {
                var command = PrepareCommand(query, parameterValues);
                return command.ExecuteReader();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Failed to execute query");
                throw ex;
            }
        }


        public void Dispose()
        {
            connection?.Dispose();
        }
    }
}
