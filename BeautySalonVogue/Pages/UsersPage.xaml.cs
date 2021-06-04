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
using BeautySalonVogue.Data;

namespace BeautySalonVogue.Pages
{
    /// <summary>
    /// Логика взаимодействия для UsersPage.xaml
    /// </summary>
    public partial class UsersPage : Page
    {
        public UsersPage()
        {
            InitializeComponent();
        }

        private void btnHistory_Click(object sender, RoutedEventArgs e)
        {
            Manager.baseFrame.Navigate(new HistoryPage());
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Manager.baseFrame.Navigate(new AddEditUserPage((sender as Button).DataContext as User));
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                try
                { 
                    BeautySalonBaseEntities.getContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                    DGridUsers.ItemsSource = BeautySalonBaseEntities.getContext().User.ToList();
                }
                catch
                {
                    MessageBox.Show("Ошибка обновления данных!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            Manager.baseFrame.Navigate(new AddEditUserPage(null));
        }

        private void btnDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (DGridUsers.SelectedItems.Count > 0)
            {
                var usersForRemove = DGridUsers.SelectedItems.Cast<User>().ToList();
                if (MessageBox.Show($"Вы точно хотите удалить следующие {usersForRemove.Count()} элементов?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        BeautySalonBaseEntities.getContext().User.RemoveRange(usersForRemove);
                        BeautySalonBaseEntities.getContext().SaveChanges();
                        MessageBox.Show("Данные удалены!");
                        DGridUsers.ItemsSource = BeautySalonBaseEntities.getContext().User.ToList();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }
            else
            {
                MessageBox.Show("Вы не выбрали ни одну запись для удаления!", "Информация", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
