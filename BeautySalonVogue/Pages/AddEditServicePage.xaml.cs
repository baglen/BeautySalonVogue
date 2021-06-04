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
using BeautySalonVogue.Data;

namespace BeautySalonVogue.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditServicePage.xaml
    /// </summary>
    public partial class AddEditServicePage : Page
    {
        private Category currentCategory;
        private Service currentService;
        private ServiceCategory currentServiceCategory;
        public AddEditServicePage(ServiceCategory serviceCategory, Category category)
        {
            InitializeComponent();
            try
            { 
                currentService = new Service();
                if(category.Name != null)
                {
                    currentCategory = category;
                }
                else
                {
                    comboBoxCategory.Visibility = Visibility.Visible;
                    txtBlockCategory.Visibility = Visibility.Visible;
                    comboBoxCategory.ItemsSource = BeautySalonBaseEntities.getContext().Category.ToList();
                }
                currentServiceCategory = new ServiceCategory();
                string[] actuality = { "Актуальна", "Не актуальна" };
                if (serviceCategory != null)
                {
                    currentServiceCategory = serviceCategory;
                    currentService.Id = serviceCategory.ServiceId;
                    currentService = BeautySalonBaseEntities.getContext().Service.Where(p => p.Id == currentService.Id).First();
                    if (currentService.IsActual == true)
                        comboBoxIsActual.SelectedItem = actuality[0];
                    else
                        comboBoxIsActual.SelectedItem = actuality[1];
                }
                DataContext = currentService;
                comboBoxIsActual.ItemsSource = actuality;
                
            }
            catch
            {
                MessageBox.Show("Ошибка получения данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtBoxDiscount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (string.IsNullOrEmpty(currentService.Name))
            {
                stringBuilder.AppendLine("Не введено название услуги");
            }
            if (string.IsNullOrWhiteSpace(currentService.Cost.ToString()) || currentService.Cost == 0)
            {
                stringBuilder.AppendLine("Не введена стоимость");
            }
            if (comboBoxIsActual.SelectedItem == null)
            {
                stringBuilder.AppendLine("Не выбрана актуальность");
            }
            if (comboBoxCategory.SelectedItem == null && currentCategory == null)
            {
                stringBuilder.AppendLine("Не выбрана категория");
            }
            if (stringBuilder.Length != 0)
            {
                MessageBox.Show(stringBuilder.ToString(), "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                if (comboBoxIsActual.SelectedIndex == 0)
                    currentService.IsActual = true;
                else
                    currentService.IsActual = false;
                if (currentCategory == null)
                    currentServiceCategory.Category = (Category)comboBoxCategory.SelectedItem;
                else
                    currentServiceCategory.CategoryId = currentCategory.Id;
                if (currentService.Id == 0)
                {
                    BeautySalonBaseEntities.getContext().Service.Add(currentService);
                    BeautySalonBaseEntities.getContext().SaveChanges();
                    currentServiceCategory.ServiceId = BeautySalonBaseEntities.getContext().Service.Max(p => p.Id);
                    BeautySalonBaseEntities.getContext().ServiceCategory.Add(currentServiceCategory);
                }
                else
                {
                    currentServiceCategory.ServiceId = currentService.Id;
                }
                
                BeautySalonBaseEntities.getContext().SaveChanges();
                MessageBox.Show("Данные успешно сохранены!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                Manager.baseFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка\n" + ex.Message.ToString());
            }
        }

        private void txtBoxCost_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
