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
using Microsoft.Win32;

namespace BeautySalonVogue.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditEmployeePage.xaml
    /// </summary>
    public partial class AddEditEmployeePage : Page
    {
        private BitmapImage btmpPhotoImage;
        private Employee currentEmployee;
        public AddEditEmployeePage(Employee choosenEmployee)
        {
            InitializeComponent();
            currentEmployee = new Employee();
            comboBoxPost.ItemsSource = new string[] { "Парикмахер", "Визажист", "Мастер маникюра" };
            comboBoxPost.SelectedIndex = 0;
            if (choosenEmployee != null)
            {
                currentEmployee = choosenEmployee;
                comboBoxPost.SelectedItem = currentEmployee.Post;
            }
            DataContext = currentEmployee;
            
        }

        private void btnPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Все картинки|*.jpg;*.jpeg;*.png|" +
            "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
            "PNG (*.png)|*.png";
            openFileDialog.Title = "Выберите фотографию сотрудника";
            if(openFileDialog.ShowDialog() == true)
            {
                btmpPhotoImage = new BitmapImage(new Uri(openFileDialog.FileName));
                if(btmpPhotoImage != null)
                {
                    employeePhoto.ImageSource = btmpPhotoImage;
                    currentEmployee.PhotoPath = Manager.BitmapToByteArray(btmpPhotoImage);
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (string.IsNullOrEmpty(currentEmployee.FirstName))
            {
                stringBuilder.AppendLine("Не введено имя");
            }
            if (string.IsNullOrEmpty(currentEmployee.LastName))
            {
                stringBuilder.AppendLine("Не введена фамилия");
            }
            if (comboBoxPost.SelectedItem == null)
            {
                stringBuilder.AppendLine("Не выбрана должность");
            }
            if (stringBuilder.Length != 0)
            {
                MessageBox.Show(stringBuilder.ToString(), "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (btmpPhotoImage == null && currentEmployee.Id == 0)
            { 
                btmpPhotoImage = new BitmapImage(new Uri("pack://application:,,,/Resources/MenuImages/defaultEmployee.png", UriKind.Absolute));
                currentEmployee.PhotoPath = Manager.BitmapToByteArray(btmpPhotoImage);
            }
            try
            {
                currentEmployee.Post = comboBoxPost.SelectedItem.ToString();
                if (currentEmployee.Id == 0)
                {
                    BeautySalonBaseEntities.getContext().Employee.Add(currentEmployee);
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

        private void btnDeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Вы точно хотите удалить сотрудника из базы данных?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    BeautySalonBaseEntities.getContext().Employee.Remove(currentEmployee);
                    BeautySalonBaseEntities.getContext().SaveChanges();
                    MessageBox.Show("Данные успешно удалены!");
                    Manager.baseFrame.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }
    }
}
