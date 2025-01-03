using System;
using System.Data;
using System.Data.SQLite;
using System.Windows;

namespace TrackMate.Database
{
    public class ProductHistory
    {
        private readonly string connectionString;

        public ProductHistory(string databasePath)
        {
            connectionString = $"Data Source={databasePath};Version=3;";
            CreateTable();
        }

        // Create the database and tables if they don't exist
        public void CreateTable()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                // Create DetailsHistory table with updated schema
                string createDetailsHistoryTableQuery = @"
                CREATE TABLE IF NOT EXISTS DetailsHistory (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    OriginalId INTEGER NOT NULL,
                    OldName TEXT,
                    NewName TEXT,
                    OldBranch TEXT,
                    NewBranch TEXT,
                    Date TEXT DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY (OriginalId) REFERENCES Details (Id)
                );";
                using (var command = new SQLiteCommand(createDetailsHistoryTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        // Update an existing record in the Details table and log changes in DetailsHistory
        public void UpdateDetails(int id, string newName, string? newBranch)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Fetch old values from the Details table
                    string selectQuery = "SELECT Name, Branch FROM Details WHERE Id = @Id";
                    string oldName = string.Empty;
                    string oldBranch = string.Empty;

                    using (var selectCommand = new SQLiteCommand(selectQuery, connection))
                    {
                        selectCommand.Parameters.AddWithValue("@Id", id);
                        using (var reader = selectCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                oldName = reader["Name"]?.ToString() ?? string.Empty;
                                oldBranch = reader["Branch"]?.ToString() ?? string.Empty;
                            }
                            else
                            {
                                MessageBox.Show($"Record with ID {id} not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                return; // Exit if no matching record
                            }
                        }
                    }

                    // Normalize null or empty values for comparison
                    newBranch = string.IsNullOrWhiteSpace(newBranch) ? null : newBranch;
                    string? normalizedOldBranch = string.IsNullOrWhiteSpace(oldBranch) ? null : oldBranch;

                    // Determine if Name or Branch has changed
                    bool nameChanged = !string.Equals(oldName, newName, StringComparison.Ordinal);
                    bool branchChanged = !string.Equals(normalizedOldBranch, newBranch, StringComparison.Ordinal);

                    // Log history only if a meaningful change occurred
                    if (nameChanged || branchChanged)
                    {
                        LogHistory(
                            connection,
                            id,
                            oldName,
                            nameChanged ? newName : null,
                            oldBranch,
                            branchChanged ? newBranch : null
                        );
                    }
                    
                }
                catch (Exception ex)
                {
                    // Log the error
                    MessageBox.Show($"Error updating product details: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Log changes in the DetailsHistory table
        private void LogHistory(SQLiteConnection connection, int originalId, string oldName, string? newName, string oldBranch, string? newBranch)
        {
            string query = @"
        INSERT INTO DetailsHistory (OriginalId, OldName, NewName, OldBranch, NewBranch, Date)
        VALUES (@OriginalId, @OldName, @NewName, @OldBranch, @NewBranch, CURRENT_TIMESTAMP)";

            using (var command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@OriginalId", originalId);
                command.Parameters.AddWithValue("@OldName", oldName);
                command.Parameters.AddWithValue("@NewName", newName ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@OldBranch", oldBranch);
                command.Parameters.AddWithValue("@NewBranch", newBranch ?? (object)DBNull.Value);

                try
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine("History logged successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error logging history: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Fetch all records from the Details table
        //public DataTable GetAllDetails()
        //{
        //    using (var connection = new SQLiteConnection(connectionString))
        //    {
        //        connection.Open();
        //        string selectQuery = "SELECT * FROM Details";
        //        using (var command = new SQLiteCommand(selectQuery, connection))
        //        {
        //            using (var adapter = new SQLiteDataAdapter(command))
        //            {
        //                var table = new DataTable();
        //                adapter.Fill(table);
        //                return table;
        //            }
        //        }
        //    }
        //}

        public List<HistoryModel> GetHistoryByOriginalId(int originalId)
        {
            var historyList = new List<HistoryModel>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = @"
        SELECT 
            h.Id,
            h.OriginalId,
            d.Name AS MachineryName,
            h.OldName,
            h.NewName,
            h.OldBranch,
            h.NewBranch,
            h.Date
        FROM 
            DetailsHistory h
        LEFT JOIN 
            Details d ON h.OriginalId = d.Id
        WHERE 
            h.OriginalId = @OriginalId";

                using (var command = new SQLiteCommand(query, connection))
                {
                    // Add the filter parameter
                    command.Parameters.AddWithValue("@OriginalId", originalId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            historyList.Add(new HistoryModel
                            {
                                Id = reader.GetInt32(0),
                                OriginalId = reader.GetInt32(1),
                                MachineryName = reader.IsDBNull(2) ? null : reader.GetString(2),
                                OldName = reader.IsDBNull(3) ? null : reader.GetString(3),
                                NewName = reader.IsDBNull(4) ? null : reader.GetString(4),
                                OldBranch = reader.IsDBNull(5) ? null : reader.GetString(5),
                                NewBranch = reader.IsDBNull(6) ? null : reader.GetString(6),
                                Date = reader.GetString(7)
                            });
                        }
                    }
                }
            }
            return historyList;
        }


        // History model
        public class HistoryModel
        {
            public int Id { get; set; }
            public int OriginalId { get; set; }
            public string MachineryName { get; set; }
            public string OldName { get; set; }
            public string NewName { get; set; }
            public string OldBranch { get; set; }
            public string NewBranch { get; set; }
            public string Date { get; set; }
        }

    }
}
