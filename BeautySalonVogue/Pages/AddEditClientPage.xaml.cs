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
    /// Логика взаимодействия для AddEditClientPage.xaml
    /// </summary>
    public partial class AddEditClientPage : Page
    {
        private Client currentClient;
        public AddEditClientPage(Client choosenClient)
        {
            InitializeComponent();
            currentClient = new Client();
            if (choosenClient != null)
                currentClient = choosenClient;
            else
                currentClient.Birthday = DateTime.Parse("2000-01-01");
            DataContext = currentClient;
            try
            {
                comboBoxGender.ItemsSource = BeautySalonBaseEntities.getContext().Gender.ToList();
            }
            catch
            {
                MessageBox.Show("Ошибка получения данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (string.IsNullOrEmpty(currentClient.FirstName))
            {
                stringBuilder.AppendLine("Не введено имя");
            }
            if (string.IsNullOrEmpty(currentClient.LastName))
            {
                stringBuilder.AppendLine("Не введена фамилия");
            }
            if (datePickerBirthday.SelectedDate == null)
            {
                stringBuilder.AppendLine("Не выбрана дата рождения");
            }
            if (string.IsNullOrEmpty(currentClient.Email))
            {
                stringBuilder.AppendLine("Не введен Email");
            }
            if (Manager.IsValidEmail(currentClient.Email) == false)
            {
                stringBuilder.AppendLine("Неверный формат Email адреса");
            }
            if (string.IsNullOrEmpty(currentClient.Phone))
            {
                stringBuilder.AppendLine("Не введен телефон");
            }
            if (comboBoxGender.SelectedItem == null)
            {
                stringBuilder.AppendLine("Не выбран пол");
            }
            if (stringBuilder.Length != 0)
            {
                MessageBox.Show(stringBuilder.ToString(), "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                currentClient.Birthday = (DateTime)datePickerBirthday.SelectedDate;
                currentClient.Phone = txtBoxPhone.Text;
                if (currentClient.Id == 0)
                {
                    currentClient.RegistrationDate = DateTime.Now;
                    BeautySalonBaseEntities.getContext().Client.Add(currentClient);
                }
                BeautySalonBaseEntities.getContext().SaveChanges();
                MessageBox.Show("Данные успешно сохранены!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                Manager.baseFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка" + ex.Message.ToString());
            }
        }
    }
}
