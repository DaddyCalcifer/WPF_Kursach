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
        public PageMain pm;
        public FIndWindow(PageMain pm)
        {
            InitializeComponent();
            this.pm = pm;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            pm.fw = null;
            pm.GetOwners();
        }
        void find_act()
        {
            pm.FindByName(
                nameBox.Text.Trim(),
                phoneBox.Text.Trim(),
                emailBox.Text.Trim());
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
            pm.GetOwners();
        }
    }
}
