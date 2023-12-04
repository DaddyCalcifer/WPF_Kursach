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
using System.Windows.Shapes;

namespace lab1
{
    /// <summary>
    /// Логика взаимодействия для FIndWindow.xaml
    /// </summary>
    public partial class FIndWindow : Window
    {
        public PageWorkers pm;
        public FIndWindow(PageWorkers pm = null)
        {
            InitializeComponent();
            this.pm = pm;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (pm != null) {
                pm.fw = null;
                pm.GetActs();
            }
        }
        void find_act()
        {
            pm.FindByName(
                loginBox.Text.Trim(),
                nameBox.Text.Trim(),
                phoneBox.Text.Trim(),
                emailBox.Text.Trim(),
                userTypeBox.SelectedIndex);
            Console.WriteLine("Выбрана роль: " + (userTypeBox.SelectedIndex).ToString());
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            find_act();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            nameBox.Clear();
            find_act();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            phoneBox.Clear();
            find_act();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            emailBox.Clear();
            find_act();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            phoneBox.Clear();
            emailBox.Clear();
            nameBox.Clear();
            pm.GetActs();
        }

        private void userTypeBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            loginBox.Clear();
            find_act();
        }
    }
}
