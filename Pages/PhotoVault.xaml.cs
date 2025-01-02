using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using TrackMate.Database;

namespace TrackMate.Pages
{
    public partial class PhotoVault : Page
    {

        private readonly Transaction _transaction;
        private readonly DatabaseHelper _databaseHelper;
        private readonly Users _Users;

        private string _selectedImagePath;
        public PhotoVault()
        {
            InitializeComponent();
            string databasePath = "MaterialsTracker.db";
            _databaseHelper = new DatabaseHelper(databasePath);
            _Users = new Users(databasePath);
            _transaction = new Transaction(databasePath);

            LoadUsernames();

        }
        private void UsernameComboBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string enteredText = UsernameComboBox.Text.ToLower();

            // Fetch matching usernames from the database
            List<string> filteredUsernames = _Users.GetUsernamesByPrefix(enteredText);

            if (filteredUsernames.Count > 0)
            {
                // Update the ComboBox with matching usernames
                UsernameComboBox.ItemsSource = filteredUsernames;
                UsernameComboBox.IsDropDownOpen = true; // Show dropdown with matches
            }
            else
            {
                // Clear the ComboBox if no matches are found
                UsernameComboBox.ItemsSource = null;
                UsernameComboBox.IsDropDownOpen = false; // Close dropdown
            }
        }


        private void LoadUsernames()
        {
            try
            {
                // Load all usernames from the database
                List<string> usernames = _Users.GetAllUsernames();
                UsernameComboBox.ItemsSource = usernames;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading usernames: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            // Open a file dialog to select an image
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                // Load the selected image
                _selectedImagePath = openFileDialog.FileName;

                // Display the selected image in the ImagePreview control
                ImagePreview.Source = new BitmapImage(new Uri(_selectedImagePath));

                // Show the ImagePreviewBorder
                ImagePreviewBorder.Visibility = Visibility.Visible;

                // Adjust layout dynamically if needed
                ImagePreviewBorder.Margin = new Thickness(0, 20, 0, 20); // Add spacing
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
                    DateTime newDate = new DateTime(selectedDate.Year, DatePicker.SelectedDate.Value.Month,DatePicker.SelectedDate.Value.Day);

                    // Update the DatePicker to reflect the new date with the same month and day but a new year
                    DatePicker.SelectedDate = newDate;
                }
            }
        }

        private void AddToDatabase_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text;
            string selectedUsername = UsernameComboBox.Text;
            string branch = branchTextBox.Text;
            string date = DatePicker.Text;

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Please enter a name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(selectedUsername))
            {
                MessageBox.Show("Please select a username.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!DateTime.TryParse(DatePicker.SelectedDate?.ToString(), out DateTime dobDate))
            {
                MessageBox.Show("Please select a valid date.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // If no image is selected, set imageBytes to null
            byte[] imageBytes = null;
            if (ImagePreview.Source != null)
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
                // Insert machinery details into the database
                _databaseHelper.InsertDetails(name, selectedUsername, imageBytes, dobDate.ToString("yyyy-MM-dd"), branch);
                MessageBox.Show("Details added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                // Retrieve IDs for transaction
                int productId = _databaseHelper.GetMachineryIdByNameAndBranch(name, branch);
                int userId = _Users.GetUserIdByName(selectedUsername);

                if (productId == -1)
                {
                    productId = _databaseHelper.GetMachineryIdByNameAndBranch(name, null);
                   
                }

                // Insert transaction with a null end date
                _transaction.InsertTransaction(productId, userId, dobDate.ToString("yyyy-MM-dd"), null);

                // Clear form fields
                NameTextBox.Clear();
                UsernameComboBox.SelectedIndex = -1;
                branchTextBox.Clear();
                DatePicker.SelectedDate = null;
                ImagePreview.Source = null;
                ImagePreviewBorder.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding details: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }




        //Back to previous Page
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EquipmentDetails());
        }




    }
}
