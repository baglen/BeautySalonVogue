using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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
using BeautySalonVogue.Windows;

namespace BeautySalonVogue.Pages
{
    /// <summary>
    /// Логика взаимодействия для ReportsPage.xaml
    /// </summary>
    public partial class ReportsPage : Page
    {

        public ReportsPage()
        {
            InitializeComponent();
            comboBoxReportType.ItemsSource = new string[] { "Услуги", "Предост. услуги" };
            comboBoxReportType.SelectedIndex = 0;
        }

        private void comboBoxReportType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxReportType.SelectedIndex == 0)
            {
                txtBlockControls.Text = "Введите цену (от/до)";
                datePickerFrom.Visibility = Visibility.Collapsed;
                datePickerUntill.Visibility = Visibility.Collapsed;
                txtBoxPriceFrom.Visibility = Visibility.Visible;
                txtBoxPriceUntill.Visibility = Visibility.Visible;
                scrollService.Visibility = Visibility.Visible;
                scrollReport.Visibility = Visibility.Hidden;
                btnResetDate.Visibility = Visibility.Collapsed;
                txtBlockSumService.Visibility = Visibility.Visible;
                txtBlockSumRecord.Visibility = Visibility.Collapsed;
            }
            else
            {
                txtBlockControls.Text = "Выберите период (от/до)";
                datePickerFrom.Visibility = Visibility.Visible;
                datePickerUntill.Visibility = Visibility.Visible;
                txtBoxPriceFrom.Visibility = Visibility.Collapsed;
                txtBoxPriceUntill.Visibility = Visibility.Collapsed;
                scrollReport.Visibility = Visibility.Visible;
                scrollService.Visibility = Visibility.Hidden;
                btnResetDate.Visibility = Visibility.Visible;
                txtBlockSumService.Visibility = Visibility.Collapsed;
                txtBlockSumRecord.Visibility = Visibility.Visible;
            }
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            decimal sumService = 0;
            decimal sumRecord = 0;
            lblNothingFound.Visibility = Visibility.Collapsed;
            try 
            {
                if (comboBoxReportType.SelectedIndex == 1)
                {
                    if (datePickerFrom.SelectedDate != null && datePickerUntill.SelectedDate != null)
                    {
                        if(datePickerFrom.SelectedDate < datePickerUntill.SelectedDate)
                        {
                            lblInitializePage.Visibility = Visibility.Collapsed;
                            lblNothingFound.Visibility = Visibility.Collapsed;
                            txtBlockRecordTitle.Text = "за период: " + datePickerFrom.SelectedDate.Value.Date.ToShortDateString() + " - " + datePickerUntill.SelectedDate.Value.Date.ToShortDateString();
                            txtBlockRecordTitle.Visibility = Visibility.Visible;
                            DGridReportRecords.ItemsSource = BeautySalonBaseEntities.getContext().ClientServiceView.Where(p => p.Date > datePickerFrom.SelectedDate && p.Date < datePickerUntill.SelectedDate).ToList();
                            if(DGridReportRecords.Items.Count == 0)
                            {
                                lblNothingFound.Visibility = Visibility.Visible;
                                return;
                            }
                        }
                        else 
                        {
                            MessageBox.Show("Начальная дата не можеть быть меньше конечной", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                        }
                    }
                    else
                    {
                        lblInitializePage.Visibility = Visibility.Collapsed;
                        DGridReportRecords.ItemsSource = BeautySalonBaseEntities.getContext().ClientServiceView.ToList();
                    }
                    List<ClientServiceView> items = DGridReportRecords.ItemsSource as List<ClientServiceView>;
                    sumRecord = items.Sum(x => x.Cost);
                    txtBlockSumRecord.Text = $"Итого: {sumRecord:f2}₽";
                }
                else
                {
                    if (txtBoxPriceFrom.Text != "" && txtBoxPriceUntill.Text == "")
                    {
                        lblInitializePage.Visibility = Visibility.Collapsed;
                        decimal costFrom = Convert.ToDecimal(txtBoxPriceFrom.Text);
                        DGridReportServices.ItemsSource = BeautySalonBaseEntities.getContext().Service.Where(p => p.Cost > costFrom).ToList();
                    }
                    if (txtBoxPriceFrom.Text != "" && txtBoxPriceUntill.Text != "")
                    {
                        if(Convert.ToDecimal(txtBoxPriceFrom.Text) < Convert.ToDecimal(txtBoxPriceUntill.Text))
                        {
                            lblInitializePage.Visibility = Visibility.Collapsed;
                            decimal costFrom = Convert.ToDecimal(txtBoxPriceFrom.Text);
                            decimal costUntill = Convert.ToDecimal(txtBoxPriceUntill.Text);
                            DGridReportServices.ItemsSource = BeautySalonBaseEntities.getContext().Service.Where(p => p.Cost > costFrom && p.Cost < costUntill).ToList();
                            if (DGridReportServices.Items.Count == 0)
                            {
                                lblNothingFound.Visibility = Visibility.Visible;
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Цена 'от' не может быть больше или равно 'до'", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                        }
                    }
                    else
                    {
                        lblInitializePage.Visibility = Visibility.Collapsed;
                        DGridReportServices.ItemsSource = BeautySalonBaseEntities.getContext().Service.ToList();
                    }
                    List<Service> items = DGridReportServices.ItemsSource as List<Service>;
                    sumService = items.Sum(x => x.Cost);
                    txtBlockSumService.Text = $"Итого: {sumService:f2}₽";
                }
            }
            catch
            {
                MessageBox.Show("Ошибка обновления данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            if(comboBoxReportType.SelectedIndex == 0)
            {
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    IDocumentPaginatorSource idpSource = flowDocumentService;
                    printDialog.PrintDocument(idpSource.DocumentPaginator, $"Report_Services_From_{DateTime.Now.ToShortDateString()}");
                    Manager.baseFrame.GoBack();
                }
            }
            if(comboBoxReportType.SelectedIndex == 1)
            {
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    IDocumentPaginatorSource idpSource = flowDocumentReport;
                    printDialog.PrintDocument(idpSource.DocumentPaginator, $"Report_Records_From_{DateTime.Now.ToShortDateString()}");
                    Manager.baseFrame.GoBack();
                } 
            }
        }

        private void txtBoxPriceFrom_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[0-9]\d?\d?$");
            e.Handled = !regex.IsMatch(e.Text);
        }

        private void txtBoxPriceUntill_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[0-9]\d?\d?$");
            e.Handled = !regex.IsMatch(e.Text);
        }

        private void btnResetDate_Click(object sender, RoutedEventArgs e)
        {
            datePickerFrom.SelectedDate = null;
            datePickerUntill.SelectedDate = null;
        }
    }
}