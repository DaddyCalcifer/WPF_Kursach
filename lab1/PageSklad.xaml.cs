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
using System.Collections.ObjectModel;
using System.Data.Entity;
using lab1.Model;

namespace lab1
{
    /// <summary>
    /// Логика взаимодействия для PageMain.xaml
    /// </summary>
    public partial class PageSklad : Page
    {
        public int owner_id = -1, acc_id = -1;
        MainWindow mainWindow;
        public static SKLAD_WPF DataEntitiesSKLAD { get; set; } = new SKLAD_WPF();
        public ObservableCollection<Item> ListItem { get; set; }
        public PageSklad(int acc_id, MainWindow mainWindow)
        {
            InitializeComponent();
            DataEntitiesSKLAD = new SKLAD_WPF();
            ListItem = new ObservableCollection<Item>();
            this.acc_id = acc_id;

            var queryAccs = (from item in DataEntitiesSKLAD.Account
                               where item.ID_Account == acc_id
                               select item).ToList();
            owner_id = (int)queryAccs[0].ID_Owner;

            Console.WriteLine(owner_id);
            this.mainWindow = mainWindow;
        }
        public bool isDirty = true;
        public static bool canSave = true;

        public void GetItems()
        {
            ListItem.Clear();
            var queryOwners = (from item in DataEntitiesSKLAD.Item
                               where item.ID_Owner == owner_id
                               orderby item.ID_Item
                               select item).ToList();
            foreach (Item own in queryOwners)
            {
                ListItem.Add(own);

            }
            DataGridItem.ItemsSource = ListItem;
            foreach (var item in ListItem)
            {
                Console.WriteLine(item.Name);
            }
        }
        private void RewriteOwner()
        {
            DataEntitiesSKLAD = new SKLAD_WPF();
            ListItem.Clear();
            GetItems();
        }

        private void UndoCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            RewriteOwner();
            DataGridItem.IsReadOnly = true;
            isDirty = true;
        }
        private void UndoCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = isDirty;

        }

        private void SaveCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Сохранено");
            DataEntitiesSKLAD.SaveChanges();

            canSave = true;
            DataGridItem.IsReadOnly = true;
        }

        private void SaveCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = canSave;

        }
        private void EditCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DataGridItem.IsReadOnly = false;
            DataGridItem.BeginEdit();
            isDirty = true;
        }

        private void EditCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = isDirty;

        }
        public FIndWindow fw;
        private void FindCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (fw == null)
            {
                //fw = new FIndWindow(this);
                fw.Show();
            }
            else
            {
                fw.Close();
            }
            isDirty = true;
        }
        public void FindByName(string name, string phone, string email)
        {
            DataEntitiesSKLAD = new SKLAD_WPF();
            ListItem.Clear();
            var queryOwner = (from owner in DataEntitiesSKLAD.Item
                              where owner.Name.Contains(name)
                                select owner).ToList();
            foreach (Item ow in queryOwner)
            {
                ListItem.Add(ow);
            }
            if (ListItem.Count > 0)
            {
                DataGridItem.ItemsSource = ListItem;
            }
            else
            {
                MessageBox.Show(
                    "Владельцы с заданным фильтром не найдены!",
                    "Внимание!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                GetItems();
            }
        }

        private void FindCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = isDirty;

        }
        private void AddCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Создание");
            Item ite = new Item();
            ite.Name = "не задано";
            ite.ID_Specific = 0;
            ite.Description = "не задано";
            ite.SizeH = 0;
            ite.SizeW = 0;
            ite.Mass = 0;
            ite.ID_Structure = 0;
            ite.ID_Owner = owner_id;
            AddItem(ite);
        }
        public void AddItem(Item own)
        {
            try
            {
                DataEntitiesSKLAD.Item.Add(own);
                ListItem.Add(own);
                isDirty = true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                "Ошибка добавления данных" + ex.ToString());
            }
        }

        private void AddCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = isDirty;

        }
        private void DeleteCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //MessageBox.Show("Удаление");
            Item emp = DataGridItem.SelectedItem as Item;
            if (emp != null)
            {
                MessageBoxResult result = MessageBox.Show("Удалить данные ",
                "Предупреждение", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    DataEntitiesSKLAD.Item.Remove(emp);
                    DataGridItem.SelectedIndex =
                    DataGridItem.SelectedIndex == 0 ? 1 : DataGridItem.SelectedIndex - 1;
                    ListItem.Remove(emp);
                    DataEntitiesSKLAD.SaveChanges();
                    isDirty = true;
                }
            }
            else
            {
                MessageBox.Show("Выберите строку для удаления");
            }
        }

        private void DeleteCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;

        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.logout = true;
            mainWindow.Close();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GetItems();
            DataGridItem.SelectedIndex = 0;
        }
    }
}
