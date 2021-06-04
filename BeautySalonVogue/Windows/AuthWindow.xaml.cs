using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BeautySalonVogue.Data;
using BeautySalonVogue.Windows;

namespace BeautySalonVogue
{
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        private int countErrorAuths = 0;
        public AuthWindow()
        {
            InitializeComponent();
            Style = (Style)FindResource(typeof(Window));
            DataObject.AddCopyingHandler(txtBoxPassword, this.OnCancelCommand);
        }
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            { 
                bool loginStatus = false;
                AuthHistory loginHistory = new AuthHistory();
                User currentUser = null;
                try
                {
                    currentUser = BeautySalonBaseEntities.getContext().User.Where(p => p.Login == txtBoxLogin.Text).First();
                    var result = BeautySalonBaseEntities.getContext().User.ToList().Select(p => p).Where(p => p.Login == txtBoxLogin.Text && (p.Password == txtBoxPassword.Text || p.Password == txtBoxPassword.Text));
                    if (result.Count() != 0)
                    {
                        currentUser.Lastenter = DateTime.Now;
                        BeautySalonBaseEntities.getContext().SaveChanges();
                        Manager.loginedUser = currentUser;
                        loginStatus = true;
                        MessageBox.Show("Вы успешно авторизовались", "Авторизация", MessageBoxButton.OK, MessageBoxImage.Information);
                        BaseWindow baseWindow = new BaseWindow(currentUser);
                        baseWindow.Show();
                        this.Close();
                    }
                    else
                    {
                        countErrorAuths++;
                        MessageBox.Show("Неверный пароль!", "Авторизация", MessageBoxButton.OK, MessageBoxImage.Warning);
                        if (countErrorAuths == 3)
                        {
                            countErrorAuths = 0;
                            MessageBox.Show("Превышено количество попыток входа!\nПовторите попытку через 10 секунд.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Warning);
                            Thread.Sleep(10000);
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Неверный логин!", "Авторизация", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                if(currentUser != null)
                {
                    loginHistory.DateTime = DateTime.Now;
                    loginHistory.Status = loginStatus;
                    loginHistory.UserId = currentUser.Id;
                    BeautySalonBaseEntities.getContext().AuthHistory.Add(loginHistory);
                    BeautySalonBaseEntities.getContext().SaveChanges();
                }
            }
            catch 
            {
                MessageBox.Show("Ошибка связи с базой данных!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void checkBoxShowPass_Checked(object sender, RoutedEventArgs e)
        {
            txtBoxPassword.FontFamily = new FontFamily("Roboto Light");
        }
        private void checkBoxShowPass_Unchecked(object sender, RoutedEventArgs e)
        {
            txtBoxPassword.FontFamily = new FontFamily("Password");
        }
        private void OnCancelCommand(object sender, DataObjectEventArgs e)
        {
            e.CancelCommand();
        }
    }
}
