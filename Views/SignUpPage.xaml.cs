using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TrackMate.Database;

namespace TrackMate.Views
{
    /// <summary>
    /// Interaction logic for SignUpPage.xaml
    /// </summary>
    public partial class SignUpPage : Page
    {
        private Users db;
        private bool isFirstTimeUser;

        public SignUpPage()
        {
            InitializeComponent();
            db = new Users("MaterialsTracker.db");
            db.CreateDatabase();
            isFirstTimeUser = db.IsFirstTimeUser();
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            string name = Name.Text.Trim();
            string mobileNumber = Mobile_Number.Text.Trim();
            string email = Email.Text.Trim();
            string password = Password.Password.Trim();
            string userRole = (UserRoleComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (!ValidInput(name, mobileNumber, email, password))
            {
                return;
            }
            if (string.IsNullOrEmpty(userRole))
            {
                MessageBox.Show("Please select a user role.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            bool isRegistered = db.RegisterUser(name, mobileNumber, email, password, userRole);

            if (isRegistered)
            {
                MessageBox.Show("Account created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                if (isFirstTimeUser)
                {
                    NavigationService.Navigate(new LoginPage());
                }
                // Navigate to Login Page or Home Page
            }
            else
            {
                MessageBox.Show("Error: Could not create account. Email might already be in use.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidInput(string name, string mobileNumber, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(name) || !Regex.IsMatch(name, @"^[a-zA-Z\s]+$"))
            {
                MessageBox.Show("Invalid Name: Name must contain only alphabets and spaces.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(mobileNumber) || !Regex.IsMatch(mobileNumber, @"^\d{10}$"))
            {
                MessageBox.Show("Invalid Mobile Number: Mobile number must be exactly 10 digits.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
            {
                MessageBox.Show("Invalid Email: Please enter a valid email address.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(password) || !Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#])[A-Za-z\d@$!%*?&#]{8,}$"))
            {
                MessageBox.Show("Invalid Password: Password must be at least 8 characters long and include an uppercase letter, a lowercase letter, a number, and a special character.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void GotoLoginPage(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new LoginPage());
        }
    }
}
