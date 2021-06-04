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
    /// Логика взаимодействия для AddEditUserPage.xaml
    /// </summary>
    public partial class AddEditUserPage : Page
    {
        private User currentUser;
        public AddEditUserPage(User user)
        {
            InitializeComponent();
            currentUser = new User();
            if (user != null)
                currentUser = user;
            else
                btnDropPassword.Visibility = Visibility.Collapsed;
            DataContext = currentUser;
            try
            {
                comboBoxRole.ItemsSource = BeautySalonBaseEntities.getContext().Role.ToList();
            }
            catch
            {
                MessageBox.Show("Ошибка обновления данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (string.IsNullOrEmpty(currentUser.FirstName))
            {
                stringBuilder.AppendLine("Не введено имя");
            }
            if (string.IsNullOrEmpty(currentUser.LastName))
            {
                stringBuilder.AppendLine("Не введена фамилия");
            }
            if (string.IsNullOrEmpty(currentUser.Login))
            {
                stringBuilder.AppendLine("Не введен логин");
            }
            if (comboBoxRole.SelectedItem == null)
            {
                stringBuilder.AppendLine("Не введена роль");
            }
            if (stringBuilder.Length != 0)
            {
                MessageBox.Show(stringBuilder.ToString(), "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                string newPassword = "";
                if (currentUser.Id == 0)
                {
                    currentUser.Password = "password";
                    newPassword = "\nУстановлен пароль по умолчанию: 'password'";
                    BeautySalonBaseEntities.getContext().User.Add(currentUser);
                }
                BeautySalonBaseEntities.getContext().SaveChanges();
                MessageBox.Show("Данные успешно сохранены!"+newPassword, "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                Manager.baseFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка" + ex.Message.ToString());
            }
        }

        private void btnDropPassword_Click(object sender, RoutedEventArgs e)
        {
            currentUser.Password = "password";
            try
            {
                BeautySalonBaseEntities.getContext().SaveChanges();
                MessageBox.Show("Пароль успешно сброшен!\nНовый пароль: 'password'", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка" + ex.Message.ToString());
            }
        }
    }
}
