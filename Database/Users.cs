using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TrackMate.Database
{
    public class Users
    {
        private readonly string connectionString;

        public Users(string databasePath)
        {
            connectionString = $"Data Source={databasePath};Version=3;";

        }
        public bool DoesUsersTableExist()
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT name FROM sqlite_master WHERE type='table' AND name='Users';";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        var result = command.ExecuteScalar();
                        return result != null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking Users table existence: {ex.Message}");
                return false;
            }
        }

        public void CreateDatabase()
        {
            using (var db = new SQLiteConnection(connectionString))
            {
                db.Open();
                string table = @"
                CREATE TABLE IF NOT EXISTS Users (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    MobileNumber TEXT NOT NULL,
                    Email TEXT NOT NULL UNIQUE,
                    Password TEXT NOT NULL,
                    UserRole TEXT NOT NULL
                );";
                var createTable = new SQLiteCommand(table, db);
                createTable.ExecuteNonQuery();
            }
        }
        public bool IsFirstTimeUser()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Users";
                using (var command = new SQLiteCommand(query, connection))
                {
                    var count = Convert.ToInt32(command.ExecuteScalar());
                    return count == 0;
                }
            }
        }
        public bool RegisterUser(string name, string mobileNumber, string email, string password, string userRole)
        {
            try
            {
                string hashedPassword = HashPassword(password);

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                    INSERT INTO Users (Name, MobileNumber, Email, Password, UserRole) 
                    VALUES (@Name, @MobileNumber, @Email, @Password, @UserRole)";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@MobileNumber", mobileNumber);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Password", hashedPassword);
                        command.Parameters.AddWithValue("@UserRole", userRole);

                        command.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"SQLite Error: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }


        public bool ValidateUser(string email, string password, string userRole)
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Password FROM Users WHERE Email = @Email AND UserRole = @UserRole COLLATE NOCASE";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email.Trim());
                        command.Parameters.AddWithValue("@UserRole", userRole.Trim());

                        Console.WriteLine($"Query: {query}");
                        Console.WriteLine($"Email: {email}, UserRole: {userRole}");

                        var result = command.ExecuteScalar();

                        if (result != null)
                        {
                            string storedHash = result.ToString();
                            string inputHash = HashPassword(password);

                            Console.WriteLine($"StoredHash: {storedHash}, InputHash: {inputHash}");

                            return storedHash == inputHash;
                        }
                        else
                        {
                            Console.WriteLine("Query returned no results. Verify data in the database.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during validation: {ex.Message}");
            }
            return false;
        }



        private static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (var byteValue in bytes)
                {
                    builder.Append(byteValue.ToString("x2"));
                }
                return builder.ToString();
            }
        }
        public (bool Success, string Name, string MobileNumber, string Email) GetUserDetails(string email)
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Name, MobileNumber, Email FROM Users WHERE Email = @Email";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return (true,
                                        reader["Name"].ToString(),
                                        reader["MobileNumber"].ToString(),
                                        reader["Email"].ToString());
                            }
                        }
                    }
                }
                return (false, null, null, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user details: {ex.Message}");
                return (false, null, null, null);
            }
        }
        public int GetUserIdByEmail(string email)
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Id FROM Users WHERE Email = @Email";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email.Trim());
                        var result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int userId))
                        {
                            return userId;
                        }
                    }
                }
                return -1; // Email not found
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching UserId: {ex.Message}");
                return -1;
            }
        }
        public string GetUserNameByEmail(string email)
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Name FROM Users WHERE Email = @Email";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email.Trim());
                        var result = command.ExecuteScalar();

                        if (result != null)
                        {
                            // Return the Name as a string
                            return result.ToString();
                        }
                    }
                }
                return null; // Email not found
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching UserName: {ex.Message}");
                return null;
            }
        }

        public List<string> GetAllUsernames()
        {
            List<string> usernames = new List<string>();
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Name FROM Users";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usernames.Add(reader["Name"].ToString());
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
                string query = "SELECT Name FROM Users WHERE Name LIKE @Prefix";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Prefix", prefix + "%");
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usernames.Add(reader["Name"].ToString());
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
                string query = "SELECT COUNT(*) FROM Users WHERE Name = @Name";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", username);
                    long count = (long)cmd.ExecuteScalar();
                    return count > 0;
                }
            }

        }
        public string GetUserRole(string email, string password)
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT UserRole, Password FROM Users WHERE Email = @Email COLLATE NOCASE";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email.Trim());

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string storedHash = reader["Password"].ToString();
                                string inputHash = HashPassword(password);

                                if (storedHash == inputHash)
                                {
                                    return reader["UserRole"].ToString();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during user role retrieval: {ex.Message}");
            }

            return null;
        }

        public int GetUserIdByName(string name)
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Id FROM Users WHERE Name = @Name";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", name.Trim());
                        var result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int userId))
                        {
                            return userId;
                        }
                    }
                }
                return -1; // Email not found
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching UserId: {ex.Message}");
                return -1;
            }
        }

    }
}
