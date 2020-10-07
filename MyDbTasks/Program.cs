using Microsoft.Data.SqlClient;
using System;
using System.Linq;

namespace MyDbTasks
{
    class Program
    {
        static string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=Minions;Trusted_Connection=True";

        static void Task2()
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

        private static int GetTownId(string name)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using (connection)
            {
                string selectionCommandString = "SELECT Name FROM Towns " +
                                                "WHERE Name=@name";
                SqlCommand command = new SqlCommand(selectionCommandString, connection);
                command.Parameters.AddWithValue("@name", name);
                var result = Convert.ToInt32(command.ExecuteScalar());
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

        static void Task3()
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

        private static bool IsVillainExist(string name)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using (connection)
            {
                string selectionCommandString = "SELECT COUNT(*) FROM Villains " +
                                                "WHERE Name=@name";
                SqlCommand command = new SqlCommand(selectionCommandString, connection);
                command.Parameters.AddWithValue("@name", name);
                var result = (int)command.ExecuteScalar();
                return result > 0;
            }
        }

        private static bool IsTownExist(string name)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using (connection)
            {
                string selectionCommandString = "SELECT COUNT(*) FROM Towns " +
                                                "WHERE Name=@name";
                SqlCommand command = new SqlCommand(selectionCommandString, connection);
                command.Parameters.AddWithValue("@name", name);
                var result = (int)command.ExecuteScalar();
                return result > 0;
            }
        }

        private static int InsertTown(string name)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using (connection)
            {
                SqlCommand command = new SqlCommand(
                    "INSERT INTO Towns " +
                    "(Name, CountryCode) VALUES " +
                    "(@name, 2); SELECT SCOPE_IDENTITY()", connection);

                command.Parameters.AddWithValue("@name", name);

                var result = Convert.ToInt32(command.ExecuteScalar());
                return result;
            }
        }

        private static int InsertVillain(string name)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using (connection)
            {
                SqlCommand command = new SqlCommand(
                    "INSERT INTO Villains " +
                    "(Name, EvilnessFactorId) VALUES " +
                    "(@name, 2); SELECT SCOPE_IDENTITY()", connection);

                command.Parameters.AddWithValue("@name", name);

                var result = Convert.ToInt32(command.ExecuteScalar());
                return result;
            }
        }

        private static int InsertMinion(string name, int age, int townId)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using (connection)
            {
                SqlCommand command = new SqlCommand(
                    "INSERT INTO Minions " +
                    "(Name, Age, TownId) VALUES " +
                    "(@name, @age, @townId); SELECT SCOPE_IDENTITY()", connection);

                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@age", age);
                command.Parameters.AddWithValue("@townId", townId);

                var result = Convert.ToInt32(command.ExecuteScalar());
                return result;
            }
        }

        private static void InsertVillainsMinion(int minionId, int villainId)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using (connection)
            {
                SqlCommand command = new SqlCommand(
                    "INSERT INTO MinionsVillains " +
                    "(MinionId, VillainId) VALUES " +
                    "(@minionId, @villainId); SELECT SCOPE_IDENTITY()", connection);

                command.Parameters.AddWithValue("@minionId", minionId);
                command.Parameters.AddWithValue("@villainId", villainId);
            }
        }

        static void Task4()
        {
            string[] minionData = Console.ReadLine().Split(' ');
            string[] villainData = Console.ReadLine().Split(' ');
            string minionName = minionData[1];
            int minionAge = int.Parse(minionData[2]);
            string minionTown = minionData[3];
            int minionTownId;

            if (!IsTownExist(minionTown))
            {
                minionTownId = InsertTown(minionTown);
                Console.WriteLine($"Город {minionTown} был добавлен в БД, id={minionTownId}");
            }
            else
            {
                minionTownId = GetTownId(minionTown);
            }

            string villainName = villainData[1];
            int villainId = -1;

            if (!IsVillainExist(villainName))
            {
                villainId = InsertVillain(villainName);
                Console.WriteLine($"Злодей {villainName} был добавлен в БД, id={villainId}");
            }

            var minionId = InsertMinion(minionName, minionAge, minionTownId);
            InsertVillainsMinion(minionId, villainId);

            Console.WriteLine($"Успешно добавлен {minionName}, чтобы быть миньоном {villainName}");
        }

        private static int DeleteMinionsVillainsByVillainId(int villainId)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using (connection)
            {
                SqlCommand command = new SqlCommand(
                    "DELETE FROM MinionsVillains " +
                    "WHERE VillainId = @villainId", connection);

                command.Parameters.AddWithValue("@villainId", villainId);
                return command.ExecuteNonQuery();
            }
        }

        private static int DeleteVillainById(int id)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using (connection)
            {
                SqlCommand command = new SqlCommand(
                    "DELETE FROM Villains " +
                    "WHERE Id = @id", connection);

                command.Parameters.AddWithValue("@id", id);
                return command.ExecuteNonQuery();
            }
        }

        static void Task5()
        {
            int villainId = int.Parse(Console.ReadLine());

            if (!IsVillainExist(villainId))
            {
                Console.WriteLine("Такой злодей не найден.");
                return;
            }

            var villainName = GetVillainName(villainId);

            var minionsCount = DeleteMinionsVillainsByVillainId(villainId);

            DeleteVillainById(villainId);

            Console.WriteLine($"{villainName} был удалён.\n{minionsCount} миньонов было освобождено.");
        }

        private static int IncrementMinionsAge(int[] ids)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using (connection)
            {
                SqlCommand command = new SqlCommand(
                    $"UPDATE Minions SET Age=Age+1 WHERE Id IN ({string.Join(", ", ids)})", connection);

                return command.ExecuteNonQuery();
            }
        }

        static void Task6()
        {
            var minionsIds = Console.ReadLine().Split(' ').Select(int.Parse);
            var updated = IncrementMinionsAge(minionsIds.ToArray());

            Console.WriteLine($"{updated} строк обновлено.");

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using (connection)
            {
                string selectionCommandString =
                    "SELECT Minions.Name as MinionName, Minions.Age as MinionAge " +
                    "FROM Minions";
                SqlCommand command = new SqlCommand(selectionCommandString, connection);
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
            Task6();
        }
    }
}