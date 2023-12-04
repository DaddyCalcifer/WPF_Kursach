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

namespace lab1
{
    /// <summary>
    /// Логика взаимодействия для ProviderFindPage.xaml
    /// </summary>
    public partial class FindActsPage : Page
    {
        PageMain pv;
        int id;
        bool admin = true;
        public FindActsPage(PageMain pv, int id,bool admin=true)
        {
            InitializeComponent();
            this.pv = pv;
            fromDate.SelectedDate = new ListAct().Min(i => i.LoadDate);
            tillDate.SelectedDate = new ListAct().Max(i => i.LoadDate);

            this.admin = admin;
            this.id = id;

            FillOwnerBox();
            FillProviderBox();
            FillStructureBox();

            addedByBox.SelectedIndex = 0;
            structBox.SelectedIndex = 0;
            providerBox.SelectedIndex = 0;

            addedButton.IsEnabled = addedByBox.IsEnabled = admin;
        }
        FindAct fromPage()
        {
            FindAct fi = new FindAct();
            fi.from_ = (DateTime)fromDate.SelectedDate;
            fi.till = (DateTime)tillDate.SelectedDate;
            fi.structure = ((Structure)structBox.SelectedItem).ID_Structure;
            if (admin)
                fi.addedby = ((Account)addedByBox.SelectedItem).ID_Account;
            else
                fi.addedby = id;
            fi.provider = ((Provider)providerBox.SelectedItem).ID_Producer;
            Console.WriteLine(fi.provider);
            return fi;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var fi = fromPage();
            pv.FindByName(fi);
        }
        void FillOwnerBox()
        {
            var all = new Account();

            all.Name = "Вы";
            if (admin)  all.Name = "Все";
            all.ID_Account = 0;
            addedByBox.Items.Add(all);
            foreach(var item in new ListAcc())
            {
                addedByBox.Items.Add(item);
            }
        }
        void FillStructureBox()
        {
            var all = new Structure();
            all.Name = "Все"; all.ID_Structure = 0;
            structBox.Items.Add(all);
            foreach (var item in new ListStructure())
            {
                structBox.Items.Add(item);
            }
        }
        void FillProviderBox()
        {
            var all = new Provider();
            all.Name = "Все"; all.ID_Producer = 0;
            providerBox.Items.Add(all);
            foreach (var item in new ListProvider())
            {
                providerBox.Items.Add(item);
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            addedByBox.SelectedIndex = 0;
            structBox.SelectedIndex = 0;
            providerBox.SelectedIndex = 0;
            fromDate.SelectedDate = new ListAct().Min(i => i.LoadDate);
            tillDate.SelectedDate = new ListAct().Max(i => i.LoadDate);

            pv.GetActs();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("closed");
            pv.fw = null;
            pv.GetActs();
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
            fromDate.SelectedDate = new ListAct().Min(i => i.LoadDate);
            tillDate.SelectedDate = new ListAct().Max(i => i.LoadDate);
            var fi = fromPage();
            pv.FindByName(fi);
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            providerBox.SelectedIndex = 0;
            var fi = fromPage();
            pv.FindByName(fi);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            structBox.SelectedIndex = 0;
            var fi = fromPage();
            pv.FindByName(fi);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            addedByBox.SelectedIndex = 0;
            var fi = fromPage();
            pv.FindByName(fi);
        }

        private void providerBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var prov = ((Provider)((ComboBox)sender).SelectedItem);
            Console.WriteLine($"{prov.ID_Producer} - {prov.Name}");
        }
    }
}
