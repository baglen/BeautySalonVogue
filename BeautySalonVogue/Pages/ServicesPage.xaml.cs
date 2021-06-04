using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для ServicesPage.xaml
    /// </summary>
    public partial class ServicesPage : Page
    {
        private Category category;
        public ServicesPage(Category choosenCategory)
        {
            InitializeComponent();
            if (choosenCategory != null)
                category = choosenCategory;
            else
                category = new Category();
        }

        private void btnAddService_Click(object sender, RoutedEventArgs e)
        {
            Manager.baseFrame.Navigate(new AddEditServicePage(null, category));
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            ServiceCategory serviceCategory = (ServiceCategory)DGridServices.SelectedItem;
            Manager.baseFrame.Navigate(new AddEditServicePage((sender as Button).DataContext as ServiceCategory, BeautySalonBaseEntities.getContext().Category.Where(p => p.Id == serviceCategory.CategoryId).FirstOrDefault()));
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if(category.Id != 0)
                { 
                    if (Visibility == Visibility.Visible)
                    {
                        DGridServices.ItemsSource = BeautySalonBaseEntities.getContext().ServiceCategory.Where(p=>p.CategoryId == category.Id).ToList();
                    }
                }
                else
                {
                    DGridServices.ItemsSource = BeautySalonBaseEntities.getContext().ServiceCategory.ToList();
                }
            }
            catch
            {
                MessageBox.Show("Ошибка обновления данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDeleteService_Click(object sender, RoutedEventArgs e)
        {
            if(DGridServices.SelectedItems.Count >0)
            { 
            var serviceCategoryForRemoving = DGridServices.SelectedItems.Cast<ServiceCategory>().ToList();
            var serviceForRemoving = new List<Service>();
            foreach(var c in serviceCategoryForRemoving)
            {
                serviceForRemoving.Add(BeautySalonBaseEntities.getContext().Service.Select(p=>p).Where(p=>p.Id == c.ServiceId).First());
            }
                if (MessageBox.Show($"Вы точно хотите удалить следующие {serviceCategoryForRemoving.Count()} элементов?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        BeautySalonBaseEntities.getContext().Service.RemoveRange(serviceForRemoving);
                        BeautySalonBaseEntities.getContext().ServiceCategory.RemoveRange(serviceCategoryForRemoving);
                        BeautySalonBaseEntities.getContext().SaveChanges();
                        MessageBox.Show("Данные удалены!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                        DGridServices.ItemsSource = BeautySalonBaseEntities.getContext().ServiceCategory.Where(p=>p.CategoryId == category.Id).ToList();
                    }
                    catch
                    {
                        MessageBox.Show("Ошибка удаления записей", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Вы не выбрали ни одну запись для удаления!", "Информация", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private decimal costOut = 0;
        private decimal discountOut = 0;
        private void txtBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Decimal.TryParse(txtBoxSearch.Text, out costOut);
            Decimal.TryParse(txtBoxSearch.Text, out discountOut);
            try
            {
                List<ServiceCategory> currentService = BeautySalonBaseEntities.getContext().ServiceCategory.ToList();
                if (category.Id != 0)
                {
                    currentService = currentService.Where(p => p.CategoryId == category.Id && (p.Service.Name.ToLower().Contains(txtBoxSearch.Text.ToLower()) ||
                    p.Service.Description.ToLower().Contains(txtBoxSearch.Text.ToLower()) || p.Service.Cost.Equals(costOut))).ToList();
                    DGridServices.ItemsSource = currentService.ToList();
                }
                else
                {
                    currentService = currentService.Where(p => p.Service.Name.ToLower().Contains(txtBoxSearch.Text.ToLower()) ||
                    p.Service.Description.ToLower().Contains(txtBoxSearch.Text.ToLower()) || p.Service.Cost.Equals(costOut)).ToList();
                    DGridServices.ItemsSource = currentService.ToList();
                }
                if (currentService.Count == 0)
                {
                    lblNothingFound.Visibility = Visibility.Visible;
                }
                else
                {
                    lblNothingFound.Visibility = Visibility.Collapsed;
                }
            }
            catch
            {
                MessageBox.Show("Ошибка получения данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
