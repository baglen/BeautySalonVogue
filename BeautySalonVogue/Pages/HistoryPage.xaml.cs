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
    /// Логика взаимодействия для HistoryPage.xaml
    /// </summary>
    public partial class HistoryPage : Page
    {
        public HistoryPage()
        {
            InitializeComponent();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            try
            { 
                if (Visibility == Visibility.Visible)
                {
                    BeautySalonBaseEntities.getContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                    DGridHistory.ItemsSource = BeautySalonBaseEntities.getContext().AuthHistory.ToList();
                }
            }
            catch
            {
                MessageBox.Show("Ошибка обновления записей!\nПопробуйте снова обновить записи", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
