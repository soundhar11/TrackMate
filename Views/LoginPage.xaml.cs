using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using TrackMate.Dashboards;
using TrackMate.Database;

namespace TrackMate.Views
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private readonly Users _dbHelper;

        public LoginPage()
        {
            InitializeComponent();
            _dbHelper = new Users("MaterialsTracker.db");  // Provide the actual path to your database
        }

        //private void LoginButton_Click(object sender, RoutedEventArgs e)
        //{
        //    string email = Email.Text.Trim();
        //    string password = Password.Password.Trim();


        //    Console.WriteLine($"Login Attempt: Email={email}");

        //    if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        //    {
        //        MessageBox.Show("Email and password are required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        //        return;
        //    }

        //    string userRole = _dbHelper.GetUserRole(email, password);

        //    if (!string.IsNullOrEmpty(userRole))
        //    {
        //        if (userRole.Equals("Admin", StringComparison.OrdinalIgnoreCase))
        //        {
        //            NavigationService.Navigate(new AdminDashboard());
        //        }
        //        else if (userRole.Equals("User", StringComparison.OrdinalIgnoreCase))
        //        {
        //            string userName = _dbHelper.GetUserNameByEmail(email);
        //            NavigationService.Navigate(new UserDashboard(userName));
        //        }
        //        else
        //        {
        //            MessageBox.Show("Unknown user role. Please contact the administrator.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Invalid email or password. Please try again.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = Email.Text.Trim();
            string password = Password.Password.Trim();

            Console.WriteLine($"Login Attempt: Email={email}");

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Email and password are required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Check if the Users table exists
            if (!_dbHelper.DoesUsersTableExist())
            {
                // Allow default credentials to navigate to the signup page
                if (email == "2115055@nec.edu.in" && password == "12345678")
                {
                    MessageBox.Show("No Users table found. Redirecting to signup page.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationService.Navigate(new SignUpPage());
                }
                else
                {
                    MessageBox.Show("No Users table exists. Use default credentials to proceed to signup.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                return;
            }

            // Validate login credentials
            string userRole = _dbHelper.GetUserRole(email, password);

            if (!string.IsNullOrEmpty(userRole))
            {
                if (userRole.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    NavigationService.Navigate(new AdminDashboard());
                }
                else if (userRole.Equals("User", StringComparison.OrdinalIgnoreCase))
                {
                    string userName = _dbHelper.GetUserNameByEmail(email);
                    NavigationService.Navigate(new UserDashboard(userName));
                }
                else
                {
                    MessageBox.Show("Unknown user role. Please contact the administrator.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Invalid email or password. Please try again.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void GoToSignupPage(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new SignUpPage());
        }
    }
}
