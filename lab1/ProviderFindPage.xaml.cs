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

namespace lab1
{
    /// <summary>
    /// Логика взаимодействия для ProviderFindPage.xaml
    /// </summary>
    public partial class ProviderFindPage : Page
    {
        PageProviders pv;
        public ProviderFindPage(PageProviders pv)
        {
            InitializeComponent();
            this.pv = pv;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            pv.FindByName(nameBox.Text.Trim(), phoneBox.Text.Trim());
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            nameBox.Clear();
            phoneBox.Clear();
            pv.GetStructs();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            nameBox.Clear();
            pv.FindByName(nameBox.Text.Trim(), phoneBox.Text.Trim());
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            phoneBox.Clear();
            pv.FindByName(nameBox.Text.Trim(), phoneBox.Text.Trim());
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("closed");
            pv.fw = null;
            pv.GetStructs();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            int id = 0;
            if (int.TryParse(idBox.Text.Trim(), out id))
            {
                idBox.Clear();
                pv.FindByID(id);
            }
            else MessageBox.Show("Некорректно введён идентификатор!");
        }
    }
}
