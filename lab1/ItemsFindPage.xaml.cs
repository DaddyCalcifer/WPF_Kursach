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
using lab1.Model;

namespace lab1
{
    /// <summary>
    /// Логика взаимодействия для ProviderFindPage.xaml
    /// </summary>
    public partial class ItemsFindPage : Page
    {
        PageSklad pv;
        public ItemsFindPage(PageSklad pv)
        {
            InitializeComponent();
            this.pv = pv;
            priceFromBox.Text = new Model.ListItem().Min(i => i.Price).ToString();
            priceTillBox.Text = new Model.ListItem().Max(i => i.Price).ToString();

            countBoxFrom.Text = new Model.ListItem().Min(i => i.Count).ToString();
            countBoxTo.Text = new Model.ListItem().Max(i => i.Count).ToString();
            FillSpecificBox();
        }
        FindItem fromPage()
        {
            FindItem fi = new FindItem();
            fi.Name = nameBox.Text.Trim();
            fi.Description = descBox.Text.Trim();
            if (!int.TryParse(priceFromBox.Text.Trim(), out fi.price_from))
                fi.price_from = new Model.ListItem().Min(i => i.Price);
            if (!int.TryParse(priceTillBox.Text.Trim(), out fi.price_to))
                fi.price_to = new Model.ListItem().Max(i => i.Price);
            if (!int.TryParse(countBoxFrom.Text.Trim(), out fi.count_from))
                fi.count_from = new Model.ListItem().Min(i => i.Count);
            if (!int.TryParse(countBoxTo.Text.Trim(), out fi.count_to))
                fi.count_to = new Model.ListItem().Max(i => i.Count);
            fi.specific = specififcBox.SelectedIndex;
            return fi;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var fi = fromPage();
            pv.FindByName(fi);
        }
        void FillSpecificBox()
        {
            specififcBox.Items.Add("Все");
            var ls = new ListSpecific();
            foreach(var item in ls)
            {
                specififcBox.Items.Add(item.Name);
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            nameBox.Clear();
            descBox.Clear();
            idBox.Clear();
            priceFromBox.Text = new Model.ListItem().Min(i => i.Price).ToString();
            priceTillBox.Text = new Model.ListItem().Max(i => i.Price).ToString();

            countBoxFrom.Text = new Model.ListItem().Min(i => i.Count).ToString();
            countBoxTo.Text = new Model.ListItem().Max(i => i.Count).ToString();
            specififcBox.SelectedIndex = 0;

            pv.GetItems();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            nameBox.Clear();
            var fi = fromPage();
            pv.FindByName(fi);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
           descBox.Clear();
            var fi = fromPage();
            pv.FindByName(fi);
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("closed");
            pv.fw = null;
            pv.GetItems();
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

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            priceFromBox.Text = new Model.ListItem().Min(i => i.Price).ToString();
            priceTillBox.Text = new Model.ListItem().Max(i => i.Price).ToString();
            var fi = fromPage();
            pv.FindByName(fi);
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            countBoxFrom.Text = new Model.ListItem().Min(i => i.Count).ToString();
            countBoxTo.Text = new Model.ListItem().Max(i => i.Count).ToString();
            var fi = fromPage();
            pv.FindByName(fi);
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            specififcBox.SelectedIndex = 0;
            var fi = fromPage();
            pv.FindByName(fi);
        }
    }
}
