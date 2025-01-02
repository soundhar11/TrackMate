using System;
using System.Data.SQLite;

namespace TrackMate.Database
{
    public class UserDetails
    {
        private readonly string connectionString;

        // Constructor to initialize the database path
        public UserDetails(string databasePath)
        {
            connectionString = $"Data Source={databasePath};Version=3;";
        }

        // Method to add a user to the UserDetails table
        public bool AddUser(string username)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO UserDetails (UserName) VALUES (@UserName)";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserName", username);
                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                // Log or handle exception (e.g., database connection failure)
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        // Method to check if the UserDetails table exists, if not, create it
        public void CreateTableIfNotExists()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    string createTableQuery = @"
                        CREATE TABLE IF NOT EXISTS UserDetails (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            UserName TEXT NOT NULL
                        );";

                    using (SQLiteCommand cmd = new SQLiteCommand(createTableQuery, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle exception
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        public List<string> GetAllUsernames()
        {
            List<string> usernames = new List<string>();
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT UserName FROM UserDetails";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usernames.Add(reader["UserName"].ToString());
                        }
                    }
                }
            }
            return usernames;
        }
        // Get usernames that start with the entered text (case insensitive)

        public List<string> GetUsernamesByPrefix(string prefix)
        {
            List<string> usernames = new List<string>();
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT UserName FROM UserDetails WHERE UserName LIKE @Prefix";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Prefix", prefix + "%");
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usernames.Add(reader["UserName"].ToString());
                        }
                    }
                }
            }
            return usernames;
        }
        // Check if the username exists in the database
        public bool IsUsernameValid(string username)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM UserDetails WHERE UserName = @UserName";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserName", username);
                    long count = (long)cmd.ExecuteScalar();
                    return count > 0;
                }
            }

        }
    }
}
