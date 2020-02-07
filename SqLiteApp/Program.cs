// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="United States Government">
//   © 2020 United States Government, as represented by the Secretary of the Army.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SqLiteApp
{
    using System;
    using Microsoft.Data.Sqlite;

    internal class Program
    {
        private static void Main()
        {
            using var connection = new SqliteConnection("Data Source=blogging.db;");
            using var command = connection.CreateCommand();

            connection.Open();

            var password = "Test123";
            command.CommandText = "SELECT quote($password);";
            command.Parameters.AddWithValue("$password", password);
            var quotedPassword = (string)command.ExecuteScalar();

            command.CommandText = "PRAGMA key = " + quotedPassword;
            command.Parameters.Clear();
            command.ExecuteNonQuery();

            command.CommandText = "SELECT COUNT(*) FROM sqlite_master WHERE type = 'table' AND name = 'test';";

            if ((long)command.ExecuteScalar() == 0)
            {
                command.CommandText = "CREATE TABLE test (content NVARCHR(20) NOT NULL);";
                command.ExecuteNonQuery();
            }

            command.CommandText = "SELECT COUNT(*) FROM test;";

            if ((long)command.ExecuteScalar() == 0)
            {
                command.CommandText = "INSERT INTO test (content) VALUES ('Hello world!');";
                command.ExecuteNonQuery();
            }

            command.CommandText = "SELECT content FROM test LIMIT 1;";
            var content = (string)command.ExecuteScalar();

            Console.WriteLine(content);
        }
    }
}