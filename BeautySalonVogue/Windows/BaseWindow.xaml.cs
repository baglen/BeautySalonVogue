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
using System.Windows.Shapes;
using BeautySalonVogue.Pages;
using BeautySalonVogue.Data;

namespace BeautySalonVogue.Windows
{
    /// <summary>
    /// Логика взаимодействия для BaseWindow.xaml
    /// </summary>
    public partial class BaseWindow : Window
    {
        User currentUser;
        public BaseWindow(User user)
        {
            InitializeComponent();
            currentUser = user;
            Style = (Style)FindResource(typeof(Window));
            Manager.baseFrame = baseFrame;
            Manager.baseFrame.Navigate(new StartupPage(currentUser, this));
            txtLogin.Text = Manager.loginedUser.Login;
            try
            {
                roleImage.ImageSource = Manager.LoadImage(BeautySalonBaseEntities.getContext().Role.Where(p => p.Id == Manager.loginedUser.RoleId).Select(p => p.PhotoPath).First());
            }
            catch
            {
                MessageBox.Show("Ошибка загрузки изображения профиля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if(currentUser.Role.Name == "admin")
            {
                btnUsers.Visibility = Visibility.Visible;
                btnRecord.Visibility = Visibility.Collapsed;
                btnServices.Visibility = Visibility.Collapsed;
                btnClients.Visibility = Visibility.Collapsed;
                btnEmployeers.Visibility = Visibility.Collapsed;
                btnReports.Visibility = Visibility.Collapsed;
            }
            else if(currentUser.Role.Name == "manager")
            {
                btnUsers.Visibility = Visibility.Collapsed;
                btnRecord.Visibility = Visibility.Visible;
                btnServices.Visibility = Visibility.Visible;
                btnClients.Visibility = Visibility.Visible;
                btnEmployeers.Visibility = Visibility.Collapsed;
                btnReports.Visibility = Visibility.Collapsed;
            }
            else if (currentUser.Role.Name == "accountant")
            {
                btnUsers.Visibility = Visibility.Collapsed;
                btnRecord.Visibility = Visibility.Visible;
                btnServices.Visibility = Visibility.Visible;
                btnClients.Visibility = Visibility.Visible;
                btnEmployeers.Visibility = Visibility.Visible;
                btnReports.Visibility = Visibility.Visible;
            }
        }

        private void baseFrame_ContentRendered(object sender, EventArgs e)
        {
            if (baseFrame.CanGoForward)
            {
                btnForward.Cursor = Cursors.Arrow;
                btnForward.IsEnabled = true;
                btnForward.BorderBrush = Brushes.Black;
                navArrowRight.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/MenuImages/navigationArrow.png", UriKind.Absolute));
            }
            else
            {
                btnForward.Cursor = Cursors.No;
                btnForward.IsEnabled = false;
                btnForward.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#828282")); 
                navArrowRight.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/MenuImages/navigationArrowDisabled.png", UriKind.Absolute));
            }
            if (baseFrame.CanGoBack)
            {
                btnBack.Cursor = Cursors.Arrow;
                btnBack.IsEnabled = true;
                btnBack.BorderBrush = Brushes.Black;
                navArrowLeft.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/MenuImages/navigationArrow.png", UriKind.Absolute));
            }
            else
            {
                btnBack.Cursor = Cursors.No;
                btnBack.IsEnabled = false;
                btnBack.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#828282"));
                navArrowLeft.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/MenuImages/navigationArrowDisabled.png", UriKind.Absolute));
            }
        }

        private void btnUsers_Click(object sender, RoutedEventArgs e)
        {
            Manager.baseFrame.Navigate(new UsersPage());
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if(baseFrame.CanGoBack)
                Manager.baseFrame.GoBack();
        }

        private void btnForward_Click(object sender, RoutedEventArgs e)
        {
            if(baseFrame.CanGoForward)
                Manager.baseFrame.GoForward();
        }

        private void btnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            ChangePassword changePasssword = new ChangePassword(currentUser, this);
            changePasssword.ShowDialog();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            AuthWindow authWindow = new AuthWindow();
            authWindow.Show();
            this.Close();
        }

        private void btnServices_Click(object sender, RoutedEventArgs e)
        {
            Manager.baseFrame.Navigate(new CategoriesPage());
        }

        private void btnCategories_Click(object sender, RoutedEventArgs e)
        {
            Manager.baseFrame.Navigate(new CategoriesPage());
        }

        private void btnClients_Click(object sender, RoutedEventArgs e)
        {
            Manager.baseFrame.Navigate(new ClientsPage());
        }

        private void btnRecord_Click(object sender, RoutedEventArgs e)
        {
            Manager.baseFrame.Navigate(new RecordsPage());
        }
        private void btnProfile_Click(object sender, RoutedEventArgs e)
        {
            Manager.baseFrame.Navigate(new StartupPage(currentUser, this));
        }
        private void btnEmployeers_Click(object sender, RoutedEventArgs e)
        {
            Manager.baseFrame.Navigate(new EmployeersPage());
        }
        private void btnReports_Click(object sender, RoutedEventArgs e)
        {
            Manager.baseFrame.Navigate(new ReportsPage());
        }
        #region buttonsStyles
        private void btnUsers_MouseEnter(object sender, MouseEventArgs e)
        {
            btnUsers.Foreground = Brushes.Black;
            imageUser.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/MenuImages/userActive.png", UriKind.Absolute));
        }

        private void btnUsers_MouseLeave(object sender, MouseEventArgs e)
        {
            btnUsers.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#666666"));
            imageUser.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/MenuImages/userInactive.png", UriKind.Absolute));
        }

        private void btnRedord_MouseEnter(object sender, MouseEventArgs e)
        {
            btnRecord.Foreground = Brushes.Black;
            imageRecord.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/MenuImages/recordActive.png", UriKind.Absolute));
        }

        private void btnRedord_MouseLeave(object sender, MouseEventArgs e)
        {
            btnRecord.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#666666"));
            imageRecord.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/MenuImages/recordInactive.png", UriKind.Absolute));
        }

        private void btnServices_MouseEnter(object sender, MouseEventArgs e)
        {
            btnServices.Foreground = Brushes.Black;
            imageService.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/MenuImages/barberScissorsActive.png", UriKind.Absolute));
        }

        private void btnServices_MouseLeave(object sender, MouseEventArgs e)
        {
            btnServices.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#666666"));
            imageService.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/MenuImages/barberScissorsInactive.png", UriKind.Absolute));
        }

        private void btnClients_MouseEnter(object sender, MouseEventArgs e)
        {
            btnClients.Foreground = Brushes.Black;
            imageClient.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/MenuImages/clientActive.png", UriKind.Absolute));
        }

        private void btnClients_MouseLeave(object sender, MouseEventArgs e)
        {
            btnClients.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#666666"));
            imageClient.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/MenuImages/clientInactive.png", UriKind.Absolute));
        }

        private void btnEmployeers_MouseEnter(object sender, MouseEventArgs e)
        {
            btnEmployeers.Foreground = Brushes.Black;
            imageEmployee.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/MenuImages/employeeActive.png", UriKind.Absolute));
        }

        private void btnEmployeers_MouseLeave(object sender, MouseEventArgs e)
        {
            btnEmployeers.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#666666"));
            imageEmployee.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/MenuImages/employeeInactive.png", UriKind.Absolute));
        }
        
        private void btnReports_MouseEnter(object sender, MouseEventArgs e)
        {
            btnReports.Foreground = Brushes.Black;
            imageReport.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/MenuImages/reportActive.png", UriKind.Absolute));
        }

        private void btnReports_MouseLeave(object sender, MouseEventArgs e)
        {
            btnReports.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#666666"));
            imageReport.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/MenuImages/reportInactive.png", UriKind.Absolute));
        }

        
        #endregion
    }
    class InverseBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !((bool)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !((bool)value);
        }
    }
}
