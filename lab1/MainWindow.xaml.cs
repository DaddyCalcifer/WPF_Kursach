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
using System.Data.Entity;
using lab1.Model;
using System.Collections.ObjectModel;
namespace lab1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int id = -1;
        bool admin = false;
        public bool logout = false;
        public Page page_;
        public PageSklad psklad;
        Account account;
        public MainWindow(int id)
        {
            InitializeComponent();
            this.id = id;
            account = PageProfile.GetAcc(id);
        }
        private void frame1_Loaded(object sender, RoutedEventArgs e)
        {
            switch(account.Type)
            {
                case 1:
                    this.Hide();
                    MessageBox.Show(
                        "Аккаунт заблокирован!",
                        "Ошибка", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    logout = true;
                    this.Close();
                    return;
                case 2:
                    page_ = new PageMain(id, this);
                    break;
                case 3:
                    page_ = new AdminPage(this, account.ID_Account);
                    break;
                default:
                    MessageBox.Show("Ошибка авторизации!");
                    return;
            }
            this.Content = page_;
        }

        private void frame1_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (logout == false)
                Application.Current.Shutdown();
        }
    }
}
