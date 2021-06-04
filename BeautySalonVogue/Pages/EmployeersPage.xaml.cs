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
    /// Логика взаимодействия для EmployeersPage.xaml
    /// </summary>
    public partial class EmployeersPage : Page
    {
        private Employee addNewEmployee;
        public EmployeersPage()
        {
            InitializeComponent();
            addNewEmployee = new Employee();
            addNewEmployee.FirstName = "Добавить";
            addNewEmployee.LastName = "сотрудника";
            try
            {
                addNewEmployee.PhotoPath = Manager.BitmapToByteArray(new BitmapImage(new Uri("pack://application:,,,/Resources/MenuImages/addEmployee.png", UriKind.Absolute)));
            }
            catch
            {
                MessageBox.Show("Ошибка загрузки изображения роли", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            try 
            {
                if (Visibility == Visibility.Visible)
                {
                    BeautySalonBaseEntities.getContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                    List<Employee> employees = BeautySalonBaseEntities.getContext().Employee.ToList();
                    employees.Add(addNewEmployee);
                    ListViewEmployeers.ItemsSource = employees;
                }
            }
            catch
            {
                MessageBox.Show("Ошибка обновления данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

        private void ListViewEmployeers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Employee choosenEmployee = (Employee)ListViewEmployeers.SelectedItem;
            if (choosenEmployee.FirstName == "Добавить")
            {
                Manager.baseFrame.Navigate(new AddEditEmployeePage(null));
            }
            else
            { 
                Manager.baseFrame.Navigate(new AddEditEmployeePage(choosenEmployee));
            }
        }
    }
}
