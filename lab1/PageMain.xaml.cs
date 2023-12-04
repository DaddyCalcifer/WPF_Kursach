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
    public partial class PageMain : Page
    {
        public static SKLAD_WPF DataEntitiesSKLAD { get; set; } = new SKLAD_WPF();
        public ObservableCollection<LoadAct> ListActs { get; set; }
        public MainWindow mw;
        int role;
        int owner_id = -1;
        public static int ActSum(LoadAct act)
        {
            int sum = 0;
            var queryActs = (from ite in DataEntitiesSKLAD.Item
                             orderby ite.ID_Item
                             where ite.ID_Act == act.ID_Pocket
                             select ite).ToList();
            foreach (Item itt in queryActs)
            {
                sum += itt.Price;
            }
            return sum;
        }
        public static int ActCount(LoadAct act)
        {
            var queryActs = (from ite in DataEntitiesSKLAD.Item
                             orderby ite.ID_Item
                             where ite.ID_Act == act.ID_Pocket
                             select ite).ToList();
            return queryActs.Count();
        }
        public PageMain(int owner, MainWindow mw)
        {
            InitializeComponent();
            DataEntitiesSKLAD = new SKLAD_WPF();
            ListActs = new ObservableCollection<LoadAct>();
            owner_id = owner;
            this.mw = mw;
            role = (int)PageProfile.GetAcc(owner).Type;
        }
        public bool isDirty = true;
        public static bool canSave = true;

        public void GetActs()
        {
            ListActs.Clear();

            DataGridItem.Columns[6].Visibility = Visibility.Collapsed;
            AdminSep.Visibility = Visibility.Collapsed;
            ToAdminMenu.Visibility = Visibility.Collapsed;
            List<LoadAct> queryActs = (from act in DataEntitiesSKLAD.LoadAct
                             where act.AddedBy == owner_id
                               orderby act.ID_Pocket
                               select act).ToList();
            if (role == 3)
            {
                DataGridItem.Columns[6].Visibility = Visibility.Visible;
                AdminSep.Visibility = Visibility.Visible;
                ToAdminMenu.Visibility = Visibility.Visible;
                queryActs = (from act in DataEntitiesSKLAD.LoadAct
                             orderby act.ID_Pocket
                             select act).ToList();
            }
            foreach (LoadAct act1 in queryActs)
            {
                ListActs.Add(act1);
            }
            DataGridItem.ItemsSource = ListActs;
            foreach (var item in ListActs)
            {
                Console.WriteLine(item.LoadDate + " " + item.Provider);
            }
        }
        private void RewriteAct()
        {
            DataEntitiesSKLAD = new SKLAD_WPF();
            ListActs.Clear();
            GetActs();
        }

        private void UndoCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            RewriteAct();
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
            GetActs();
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
                bool admin = true;
                if(role == 2)
                    admin = false;
                var fp = new FindActsPage(this, owner_id, admin);
                fw.Content = fp;
                fw.Show();
            }
            isDirty = true;
        }
        public void FindByID(int id)
        {
            DataEntitiesSKLAD = new SKLAD_WPF();
            ListActs.Clear();
            var queryOwner = (from owner in DataEntitiesSKLAD.LoadAct
                              where owner.ID_Pocket == id
                              select owner).ToList();
            foreach (var ow in queryOwner)
            {
                ListActs.Add(ow);
            }
            if (ListActs.Count > 0)
            {
                DataGridItem.ItemsSource = ListActs;
            }
            else
            {
                MessageBox.Show(
                    "Накладные с заданным фильтром не найдены!",
                    "Внимание!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                GetActs();
            }
        }
        public void FindByName(FindAct args)
        {
            DataEntitiesSKLAD = new SKLAD_WPF();
            ListActs.Clear();
            //нет фильтров
            List<LoadAct> queryOwner = (from owner in DataEntitiesSKLAD.LoadAct
                              where owner.LoadDate >= args.from_ && owner.LoadDate <= args.till
                                select owner).ToList();
            if (args.provider != 0 && args.addedby != 0 && args.structure != 0)
                //Все
            queryOwner = (from owner in DataEntitiesSKLAD.LoadAct
                              where owner.LoadDate >= args.from_ && owner.LoadDate <= args.till
                              where owner.AddedBy==args.addedby
                              where owner.Provider==args.provider
                              where owner.ID_Structure==args.structure
                                select owner).ToList();
            //Поставщик + принял
            if (args.provider != 0 && args.addedby != 0 && args.structure == 0)
                queryOwner = (from owner in DataEntitiesSKLAD.LoadAct
                              where owner.LoadDate >= args.from_ && owner.LoadDate <= args.till
                              where owner.AddedBy == args.addedby
                              where owner.Provider == args.provider
                              select owner).ToList();
            //поставщик + склад
            if (args.provider != 0 && args.structure != 0 && args.addedby == 0)
                queryOwner = (from owner in DataEntitiesSKLAD.LoadAct
                              where owner.LoadDate >= args.from_ && owner.LoadDate <= args.till
                              where owner.Provider == args.provider
                              where owner.ID_Structure == args.structure
                              select owner).ToList();
            //поставщик
            if (args.provider != 0 && args.addedby == 0 && args.structure == 0)
                queryOwner = (from owner in DataEntitiesSKLAD.LoadAct
                              where owner.LoadDate >= args.from_ && owner.LoadDate <= args.till
                              where owner.Provider == args.provider
                              select owner).ToList();
            //Склад
            if (args.structure != 0 && args.addedby == 0 && args.provider == 0)
                queryOwner = (from owner in DataEntitiesSKLAD.LoadAct
                              where owner.LoadDate >= args.from_ && owner.LoadDate <= args.till
                              where owner.ID_Structure == args.structure
                              select owner).ToList();
            //Принял
            if (args.addedby != 0 && args.provider == 0 && args.structure == 0)
                queryOwner = (from owner in DataEntitiesSKLAD.LoadAct
                              where owner.LoadDate >= args.from_ && owner.LoadDate <= args.till
                              where owner.AddedBy == args.addedby
                              select owner).ToList();
            //Принял + склад
            if (args.addedby != 0 && args.structure != 0 && args.provider == 0)
                queryOwner = (from owner in DataEntitiesSKLAD.LoadAct
                              where owner.LoadDate >= args.from_ && owner.LoadDate <= args.till
                              where owner.AddedBy == args.addedby
                              where owner.ID_Structure == args.structure
                              select owner).ToList();
            foreach (var ow in queryOwner)
            {
                ListActs.Add(ow);
            }
            if (ListActs.Count > 0)
            {
                DataGridItem.ItemsSource = ListActs;
            }
            else
            {
                MessageBox.Show(
                    "Накладные с заданным фильтром не найдены!",
                    "Внимание!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                GetActs();
            }
            Console.WriteLine("xd" + args.provider);
        }

        private void FindCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = isDirty;

        }
        private void AddCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //MessageBox.Show("Создание");
            LoadAct act = new LoadAct();
            act.ID_Owner = 1;
            act.LoadDate = DateTime.Now;
            act.Provider = 1;
            act.ID_Structure = 1;
            act.AddedBy = owner_id;
            AddAct(act);
        }
        public void AddAct(LoadAct act)
        {
            try
            {
                DataEntitiesSKLAD.LoadAct.Add(act);
                ListActs.Add(act);
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
            LoadAct emp = DataGridItem.SelectedItem as LoadAct;
            if (emp != null)
            {
                MessageBoxResult result = MessageBox.Show("Удалить данные ",
                "Предупреждение", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    DataEntitiesSKLAD.LoadAct.Remove(emp);
                    DataGridItem.SelectedIndex =
                    DataGridItem.SelectedIndex == 0 ? 1 : DataGridItem.SelectedIndex - 1;
                    ListActs.Remove(emp);
                    //DataEntitiesSKLAD.SaveChanges();
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
        private void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(System.DateTime))
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy";
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GetActs();
            DataGridItem.SelectedIndex = 0;
        }

        private void DataGridItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataGridItem.IsReadOnly)
            {
                var index = ((LoadAct)DataGridItem.CurrentCell.Item).ID_Pocket;
                Console.WriteLine("index: " + index);
                mw.Content = new PageSklad(index, mw);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var prof_page = new PageProfile(mw, this, owner_id);
            mw.Content = prof_page;
        }

        //Сменить аккаунт
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            mw.logout = true;
            mw.Close();
        }

        private void ToAdminMenu_Click(object sender, RoutedEventArgs e)
        {
            var am = new AdminPage(mw, owner_id);
            mw.Content = am;
        }
    }
}
