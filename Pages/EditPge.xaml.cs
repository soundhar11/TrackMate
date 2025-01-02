using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using TrackMate.Database;

namespace TrackMate.Pages
{
    public partial class EditPage : Page
    {
        private readonly DatabaseHelper _databaseHelper;
        private readonly Users _users;
        private readonly Transaction _transaction;
        private readonly ProductHistory _productHistory;
        private string _selectedImagePath;
        private int _productId;
        private string _productName;

        public EditPage(int productId,string productName) // Receive product ID to identify the record
        {
            InitializeComponent();
            string databasePath = "MaterialsTracker.db"; // Replace with your actual path
            _databaseHelper = new DatabaseHelper(databasePath);
            _users = new Users(databasePath);
            _transaction = new Transaction(databasePath);
            _productHistory = new ProductHistory(databasePath);

            _productId = productId;
            _productName = productName;
            LoadUsernames();
            LoadProductDetails(); // Load product details based on the productId
        }

        // Load usernames for the combo box (Ownership)
        private void LoadUsernames()
        {
            try
            {
                List<string> usernames = _users.GetAllUsernames();
                UsernameComboBox.ItemsSource = usernames;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading usernames: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Load existing product details for editing
        private void LoadProductDetails()
        {
            try
            {
                var products = _databaseHelper.GetProductDetailsById(_productId); // Get product details by ID

                if (products != null && products.Count > 0)
                {
                    var product = products[0]; // Since GetProductDetailsById returns a list, we take the first item

                    // Populate fields with existing data
                    NameTextBox.Text = product.Name;
                    branchTextBox.Text = product.Branch;
                    UsernameComboBox.SelectedItem = product.Username; // Set ownership to the current owner
                    DatePicker.SelectedDate = DateTime.TryParse(product.Date, out DateTime parsedDate) ? parsedDate : (DateTime?)null;

                    // If there's an image, load it
                   
                }
                else
                {
                    MessageBox.Show("Product not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading product details: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        // Handle image selection
        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _selectedImagePath = openFileDialog.FileName;
                ImagePreview.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(_selectedImagePath));
                ImagePreviewBorder.Visibility = Visibility.Visible;
            }
        }

        // Update product details in the database
        private void UpdateDetails_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text;
            string branch = branchTextBox.Text;
            string username = UsernameComboBox.SelectedItem?.ToString();
            DateTime? selectedDate = DatePicker.SelectedDate;

            // Validate if the username is selected
            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Please select a valid username.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Validate if the date is selected
            if (selectedDate == null)
            {
                MessageBox.Show("Please select a valid date.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            } 

            // Validate if the name is not empty
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Please enter a name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Check if an image is selected
            byte[] imageBytes = null;
            if (ImagePreview.Source != null)  // If an image is selected
            {
                try
                {
                    imageBytes = File.ReadAllBytes(_selectedImagePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error reading image: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            try
            {

                var oldProductDetails = _databaseHelper.GetProductDetailsById(_productId); // Fetch existing product details
                string oldName = oldProductDetails[0].Name;
                string oldBranch = oldProductDetails[0].Branch;

                // Update product details, passing null for the image if no image is selected
               

                if (oldName != name || oldBranch != branch)
                {
                    // Log history for Name change
                    if (oldName != name)
                    {
                        _productHistory.UpdateDetails(_productId, name, oldBranch); // Log Name change
                    }

                    // Log history for Branch change
                    if (oldBranch != branch)
                    {
                        _productHistory.UpdateDetails(_productId, oldName, branch); // Log Branch change

                    }
                }
                _databaseHelper.UpdateProductDetails(_productId, name, branch, username, selectedDate.Value, imageBytes);


                int userId = _users.GetUserIdByName(username);
                string date = DatePicker.Text;
                int transactionID = _transaction.GetTransactionIdByProductIdUserId(_productId, userId);
                _transaction.UpdateStartDate(transactionID,selectedDate.Value.ToString("yyyy-MM-dd"));

                MessageBox.Show("Product details updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating product details: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            // Check if a date is selected
            if (DatePicker.SelectedDate.HasValue)
            {
                // Get the selected date
                DateTime selectedDate = DatePicker.SelectedDate.Value;

                // Check if only the year has changed (keep the month and day)
                if (selectedDate.Year != DatePicker.SelectedDate.Value.Year)
                {
                    // Preserve the month and day but update the year
                    DateTime newDate = new DateTime(selectedDate.Year, DatePicker.SelectedDate.Value.Month, DatePicker.SelectedDate.Value.Day);

                    // Update the DatePicker to reflect the new date with the same month and day but a new year
                    DatePicker.SelectedDate = newDate;
                }
            }
        }

        // Navigate back to the previous page
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack(); // Replace with your actual page
        }
    }
}
