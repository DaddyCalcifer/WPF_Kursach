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
        public int act_id = -1;
        MainWindow mainWindow;
        public static SKLAD_WPF DataEntitiesSKLAD { get; set; } = new SKLAD_WPF();
        public ObservableCollection<Item> ListItem { get; set; }
        bool adminVision = false;
        public PageSklad(int act_id, MainWindow mainWindow, bool adminVision=false)
        {
            InitializeComponent();
            DataEntitiesSKLAD = new SKLAD_WPF();
            ListItem = new ObservableCollection<Item>();
            this.act_id = act_id;

            var queryAccs = (from item in DataEntitiesSKLAD.Item
                               where item.ID_Act == act_id
                               select item).ToList();

            Console.WriteLine(act_id);
            this.mainWindow = mainWindow;
            this.adminVision = adminVision;
        }
        public bool isDirty = true;
        public static bool canSave = true;

        public void GetItems()
        {
            ListItem.Clear();
            var queryOwners = new List<Item>();
            if (adminVision)
            {
                DataGridItem.Columns[1].Visibility = Visibility.Visible;
                ToAdminMenu.Visibility = Visibility.Visible;
                queryOwners = (from item in DataEntitiesSKLAD.Item
                               orderby item.ID_Item
                               select item).ToList();
            }
            else
            {
                DataGridItem.Columns[1].Visibility = Visibility.Collapsed;
                ToAdminMenu.Visibility = Visibility.Collapsed;
                queryOwners = (from item in DataEntitiesSKLAD.Item
                               where item.ID_Act == act_id
                               orderby item.ID_Item
                               select item).ToList();
            }
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
                fw = new FIndWindow();
                var ip = new ItemsFindPage(this);
                fw.Content = ip;
                fw.Show();
            }
            else
            {
                fw.Close();
            }
            isDirty = true;
        }
        public void FindByID(int id)
        {
            DataEntitiesSKLAD = new SKLAD_WPF();
            ListItem.Clear();
            List<Item> queryOwner;
            if (adminVision)
                queryOwner = (from owner in DataEntitiesSKLAD.Item
                              where owner.ID_Item == id
                              select owner).ToList();
            else
                queryOwner = (from owner in DataEntitiesSKLAD.Item
                              where owner.ID_Item == id
                              where owner.ID_Act == act_id
                              select owner).ToList();
            foreach (var ow in queryOwner)
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
                    "Поставщики с заданным фильтром не найдены!",
                    "Внимание!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                GetItems();
            }
        }
        public void FindByName(FindItem args)
        {
            DataEntitiesSKLAD = new SKLAD_WPF();
            ListItem.Clear();
            List<Item> queryOwner;
            if (adminVision)
            {
                if (args.specific > 0)
                    queryOwner = (from owner in DataEntitiesSKLAD.Item
                                  where owner.Name.Contains(args.Name)
                                  where owner.Description.Contains(args.Description)
                                  where owner.Count >= args.count_from && owner.Count <= args.count_to
                                  where owner.Specific == args.specific
                                  where owner.Price >= args.price_from && owner.Price <= args.price_to
                                  select owner).ToList();
                else
                    queryOwner = (from owner in DataEntitiesSKLAD.Item
                                  where owner.Name.Contains(args.Name)
                                  where owner.Description.Contains(args.Description)
                                  where owner.Count >= args.count_from && owner.Count <= args.count_to
                                  where owner.Price >= args.price_from && owner.Price <= args.price_to
                                  select owner).ToList();
            }
            else
            {
                if (args.specific > 0)
                    queryOwner = (from owner in DataEntitiesSKLAD.Item
                                  where owner.Name.Contains(args.Name)
                                  where owner.Description.Contains(args.Description)
                                  where owner.Count >= args.count_from && owner.Count <= args.count_to
                                  where owner.Specific == args.specific
                                  where owner.Price >= args.price_from && owner.Price <= args.price_to
                                  where owner.ID_Act == act_id
                                  select owner).ToList();
                else
                    queryOwner = (from owner in DataEntitiesSKLAD.Item
                                  where owner.Name.Contains(args.Name)
                                  where owner.Description.Contains(args.Description)
                                  where owner.Count >= args.count_from && owner.Count <= args.count_to
                                  where owner.Price >= args.price_from && owner.Price <= args.price_to
                                  where owner.ID_Act == act_id
                                  select owner).ToList();
            }
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
            ite.Specific = 0;
            ite.Count = 0;
            ite.Description = "не задано";
            ite.ID_Act = act_id;
            ite.Price = 0;
            ite.Unit = 0;
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
            e.CanExecute = !adminVision;

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
            mainWindow.Content = new PageMain(mainWindow.id, mainWindow);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GetItems();
            DataGridItem.SelectedIndex = 0;
        }

        private void ToAdminMenu_Click(object sender, RoutedEventArgs e)
        {
            var am = new AdminPage(mainWindow, mainWindow.id);
            mainWindow.Content = am;
        }
    }
}
