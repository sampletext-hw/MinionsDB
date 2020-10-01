using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace MyDbTasks
{
    class Program
    {
        static string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=Minions;Trusted_Connection=True";

        static void Task1()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using (connection)
            {
                string selectionCommandString =
                    "SELECT * FROM Villains " +
                    "WHERE Id IN " +
                    "(SELECT VillainId " +
                    "FROM MinionsVillains " +
                    "GROUP BY VillainId " +
                    "HAVING COUNT(MinionId) >= 3)";
                SqlCommand command = new SqlCommand(selectionCommandString, connection);
                SqlDataReader reader = command.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            Console.Write($"{reader[i]} ");
                        }

                        Console.WriteLine();
                    }
                }
            }
        }

        private static bool IsVillainExist(int id)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using (connection)
            {
                string selectionCommandString = "SELECT COUNT(*) FROM Villains " +
                                                "WHERE Id=@id";
                SqlCommand command = new SqlCommand(selectionCommandString, connection);
                command.Parameters.AddWithValue("@id", id);
                var result = (int)command.ExecuteScalar();
                return result > 0;
            }
        }

        private static string GetVillainName(int id)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using (connection)
            {
                string selectionCommandString = "SELECT Name FROM Villains " +
                                                "WHERE Id=@id";
                SqlCommand command = new SqlCommand(selectionCommandString, connection);
                command.Parameters.AddWithValue("@id", id);
                var result = (string)command.ExecuteScalar();
                return result;
            }
        }

        private static bool IsVillainHasMinions(int id)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using (connection)
            {
                string selectionCommandString = "SELECT COUNT(*) FROM MinionsVillains " +
                                                "WHERE VillainId=@id";
                SqlCommand command = new SqlCommand(selectionCommandString, connection);
                command.Parameters.AddWithValue("@id", id);
                var result = (int)command.ExecuteScalar();
                return result > 0;
            }
        }

        static void Task2()
        {
            Console.Write("Enter VillainId: ");
            int id = Int32.Parse(Console.ReadLine());

            if (!IsVillainExist(id))
            {
                Console.WriteLine($"В базе данных не существует злодея с идентификатором {id}");
                return;
            }

            string villainName = GetVillainName(id);
            Console.WriteLine($"Villain: {villainName}");

            if (!IsVillainHasMinions(id))
            {
                Console.WriteLine($"(no minions)");
                return;
            }

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using (connection)
            {
                string selectionCommandString =
                    "SELECT Minions.Name as MinionName, Minions.Age as MinionAge " +
                    "FROM MinionsVillains " +
                    "JOIN Villains ON Villains.Id=VillainId " +
                    "JOIN Minions ON Minions.Id=MinionId " +
                    "WHERE VillainId=@id " +
                    "ORDER BY MinionName ASC";
                SqlCommand command = new SqlCommand(selectionCommandString, connection);
                command.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = command.ExecuteReader();
                int count = 1;
                using (reader)
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"{count++}. {reader["MinionName"]} {reader["MinionAge"]}");
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            Task2();
        }
    }
}