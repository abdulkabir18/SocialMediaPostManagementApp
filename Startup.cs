using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using SocialMediaPostManager.Context;

namespace SocialMediaPostManager
{
    public class Startup
    {
        public static void CreateTables()
        {
            string _connectionString = SociaMediapostManagerContext.connectionString;
            string path = @"SqlQueries\Tables.sql";
            if (File.Exists(path))
            {
                string line = File.ReadAllText(path);
                var asd = line.Split(';');
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();

                    for (int i = 0; i < asd.Length - 1; i++)
                    {
                        MySqlCommand sqlCommand = new MySqlCommand(asd[i], connection);
                        sqlCommand.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}