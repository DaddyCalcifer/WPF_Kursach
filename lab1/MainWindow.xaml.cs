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
using System.Collections.ObjectModel;
namespace lab1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int id = -1;
        bool admin = false;
        public bool logout = false;
        public PageMain pmain;
        public PageSklad psklad;
        public MainWindow(int id, bool admin=true)
        {
            InitializeComponent();
            this.id = id;
            this.admin = admin;
        }
        private void frame1_Loaded(object sender, RoutedEventArgs e)
        {
            if (admin)
            {
                pmain = new PageMain(id, this);
                this.Content = pmain;
            }
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
