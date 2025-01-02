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
using TrackMate.Pages;
using TrackMate.Views;

namespace TrackMate.Dashboards
{
    /// <summary>
    /// Interaction logic for AdminDashboard.xaml
    /// </summary>
    public partial class AdminDashboard : Page
    {
        public AdminDashboard()
        {
            InitializeComponent();
            DashboardFrame.Navigate(new EquipmentDetails());
        }
        private void CircularButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.ContextMenu != null)
            {
                button.ContextMenu.PlacementTarget = button;  // Attach context menu
                button.ContextMenu.IsOpen = true;             // Open context menu
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to logout?", "Logout Confirmation",
                                         MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                MessageBox.Show("You have been logged out.", "Logout", MessageBoxButton.OK, MessageBoxImage.Information);
                (Application.Current.MainWindow as MainWindow).MainFrame.Navigate(new LoginPage());
            }
        }

        private void AddUsers_Click(object sender, RoutedEventArgs e)
        {
            DashboardFrame.Navigate(new SignUpPage());

        }
        private void Home_Click(object sender, RoutedEventArgs e)
        {
            DashboardFrame.Navigate(new EquipmentDetails());

        }
    }
}
