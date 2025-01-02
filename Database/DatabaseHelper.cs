using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace TrackMate.Database
{
    public class DatabaseHelper
    {
        public string ConnectionString { get; private set; }

        public DatabaseHelper(string databasePath)
        {
            ConnectionString = $"Data Source={databasePath};Version=3;";
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();
                    string createTableQuery = @"
                        CREATE TABLE IF NOT EXISTS Details (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Name TEXT NOT NULL,
                            Username TEXT NOT NULL,
                            Photo BLOB,
                            Date TEXT, 
                            Branch TEXT
                            
                        );";
                    using (SQLiteCommand command = new SQLiteCommand(createTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error initializing the database: " + ex.Message);
            }
        }

        // Method to load all names from the database
        public List<string> LoadNames()
        {
            List<string> names = new List<string>();

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();
                    string selectQuery = "SELECT Name FROM Details";
                    using (SQLiteCommand command = new SQLiteCommand(selectQuery, connection))
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            names.Add(reader["Name"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error loading names: " + ex.Message);
            }

            return names;
        }

        public List<string> LoadNamesbyUserName(string userName)
        {
            List<string> names = new List<string>();

            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentException("User name cannot be null or empty.", nameof(userName));
            }

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();
                    string selectQuery = "SELECT Name FROM Details WHERE Username = @Username";
                    using (SQLiteCommand command = new SQLiteCommand(selectQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Username", userName);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                names.Add(reader["Name"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error loading names: " + ex.Message);
            }

            return names;
        }


        // Method to insert details into the database
        public void InsertDetails(string name, string username, byte[] photo, string date, string branch = null)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();
                    string insertQuery = "INSERT INTO Details (Name, Username, Photo, Date, Branch) VALUES (@Name, @Username, @Photo, @Date, @Branch)";

                    using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Date", date);
                        command.Parameters.AddWithValue("@Branch", string.IsNullOrEmpty(branch) ? DBNull.Value : branch);

                        // Check if the photo is null and insert DBNull.Value if it is
                        if (photo == null)
                        {
                            command.Parameters.AddWithValue("@Photo", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Photo", photo);
                        }

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting details: " + ex.Message);
            }
        }


        // Method to retrieve an image by name
        public byte[] RetrieveImageByName(string name)
        {
            byte[] image = null;

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();
                    string selectQuery = "SELECT Photo FROM Details WHERE Name = @Name";
                    using (SQLiteCommand command = new SQLiteCommand(selectQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Name", name);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                if (reader["Photo"] != DBNull.Value)
                                {
                                    image = reader["Photo"] as byte[];
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving image: " + ex.Message);
            }

            return image;
        }

        // Method to load all product details (ID, Name, and Image)
        public List<Product> GetProductDetails()
        {
            var products = new List<Product>();
            string query = "SELECT Id, Name, Photo, Username, Date,Branch FROM Details";

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new Product
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Photo = reader["Photo"] as byte[],
                                Username = reader.GetString(3),
                                Date = reader.GetString(4),
                                Branch = reader.IsDBNull(5) ? null : reader.GetString(5)
                            });
                        }
                    }
                }
            }
            return products;
        }

        public List<Product> GetProductDetailsbyName(string userName)
        {
            var products = new List<Product>();
            string query = "SELECT Id, Name, Photo, Username, Date, Branch FROM Details WHERE Username = @Username";

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    // Add the parameter to prevent SQL injection
                    command.Parameters.AddWithValue("@Username", userName);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new Product
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Photo = reader["Photo"] as byte[],
                                Username = reader.GetString(3),
                                Date = reader.GetString(4),
                                Branch = reader.IsDBNull(5) ? null : reader.GetString(5)
                            });
                        }
                    }
                }
            }
            return products;
        }



        // Method to get the current owner of an item based on its ID
        public string GetCurrentOwner(int itemId)
        {
            string currentOwner = null;

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();
                    string selectQuery = "SELECT Username FROM Details WHERE Id = @ItemId";
                    using (SQLiteCommand command = new SQLiteCommand(selectQuery, connection))
                    {
                        command.Parameters.AddWithValue("@ItemId", itemId);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                currentOwner = reader["Username"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving current owner: " + ex.Message);
            }

            return currentOwner;
        }

        public void UpdateOwnership(int id, string username, string date, string branch = null)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();
                    string updateQuery = "UPDATE Details SET Username = @Username, Date = @Date, Branch = @Branch WHERE Id = @Id";
                    using (SQLiteCommand command = new SQLiteCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Date", date);
                        command.Parameters.AddWithValue("@Branch", string.IsNullOrEmpty(branch) ? DBNull.Value : branch);
                        command.Parameters.AddWithValue("@Id", id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating ownership details: " + ex.Message);
            }
        }
        public List<Product> GetProductDetailsByName(string name)
        {
            var products = new List<Product>();
            string query = "SELECT Id, Name, Photo, Username, Date,Branch FROM Details WHERE Name = @Name";

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new Product
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Photo = reader["Photo"] as byte[],
                                Username = reader.GetString(3),
                                Date = reader.GetString(4),
                                Branch = reader.IsDBNull(5) ? null : reader.GetString(5)
                            });
                        }
                    }
                }
            }
            return products;
        }
        public List<Product> GetProductDetailsById(int id)
        {
            var products = new List<Product>();
            string query = "SELECT Id, Name, Photo, Username, Date, Branch FROM Details WHERE Id = @Id";

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new Product
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Photo = reader["Photo"] as byte[],
                                Username = reader.GetString(3),
                                Date = reader.GetString(4),
                                Branch = reader.IsDBNull(5) ? null : reader.GetString(5)  // Handle potential null value for Branch
                            });
                        }
                    }
                }
            }
            return products;
        }


        public void UpdateProductDetails(int productId, string name, string branch, string username, DateTime date, byte[] image)
        {
            // Remove the time part from the DateTime object, keeping only the date
            string dateOnly = date.ToString("yyyy-MM-dd");

            string query = "UPDATE Details SET Name = @Name, Branch = @Branch, Date = @Date, Photo = @Photo WHERE Id = @Id";

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", productId);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Branch", branch);
                    command.Parameters.AddWithValue("@Date", dateOnly);  // Pass the formatted date
                    command.Parameters.AddWithValue("@Photo", (object)image ?? DBNull.Value); // Use DBNull.Value if no image
                    command.ExecuteNonQuery();
                }
            }
        }
        public int GetMachineryIdByNameAndBranch(string productName, string productBranch)
        {
            try
            {
                using (var connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();

                    // Adjust the query for when Branch is null
                    string query = productBranch == null
                        ? "SELECT Id FROM Details WHERE Name = @Name AND Branch IS NULL"
                        : "SELECT Id FROM Details WHERE Name = @Name AND Branch = @Branch";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", productName.Trim());

                        if (productBranch != null)
                        {
                            command.Parameters.AddWithValue("@Branch", productBranch.Trim());
                        }

                        var result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int machineryId))
                        {
                            return machineryId; // Found valid ID
                        }
                    }
                }

                return -1; // ID not found
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching MachineryId: {ex.Message}");
                return -1; // Error occurred
            }
        }





    }


    // Product class for binding to DataGrid
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Photo { get; set; }
        public string Username { get; set; }
        public string Date { get; set; }

        public string Branch { get; set; }
    }


}
