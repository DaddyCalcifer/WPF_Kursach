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
    public partial class PageProviders : Page
    {
        public static SKLAD_WPF DataEntitiesSKLAD { get; set; } = new SKLAD_WPF();
        public ObservableCollection<Provider> ListProv { get; set; }
        public MainWindow mw;
        int role;
        int owner_id = -1;

        public PageProviders(int owner, MainWindow mw)
        {
            InitializeComponent();
            DataEntitiesSKLAD = new SKLAD_WPF();
            ListProv = new ObservableCollection<Provider>();
            owner_id = owner;
            this.mw = mw;
            role = (int)PageProfile.GetAcc(owner).Type;
        }
        public bool isDirty = true;
        public static bool canSave = true;

        public void GetStructs()
        {
            ListProv.Clear();
            List<Provider> queryActs = (from act in DataEntitiesSKLAD.Provider
                               orderby act.ID_Producer
                               select act).ToList();
      
            foreach (Provider act1 in queryActs)
            {
                ListProv.Add(act1);
            }
            DataGridItem.ItemsSource = ListProv;
            foreach (var item in ListProv)
            {
                Console.WriteLine(item.Name + " " + item.Phone);
            }
        }
        private void RewriteAct()
        {
            DataEntitiesSKLAD = new SKLAD_WPF();
            ListProv.Clear();
            GetStructs();
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
            GetStructs();
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
                Page prov = new ProviderFindPage(this);
                fw.Content = prov;
                fw.Show();
            }
            isDirty = true;
        }
        public void FindByID(int id)
        {
            DataEntitiesSKLAD = new SKLAD_WPF();
            ListProv.Clear();
            var queryOwner = (from owner in DataEntitiesSKLAD.Provider
                              where owner.ID_Producer == id
                              select owner).ToList();
            foreach (var ow in queryOwner)
            {
                ListProv.Add(ow);
            }
            if (ListProv.Count > 0)
            {
                DataGridItem.ItemsSource = ListProv;
            }
            else
            {
                MessageBox.Show(
                    "Поставщики с заданным фильтром не найдены!",
                    "Внимание!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                GetStructs();
            }
        }
        public void FindByName(string name, string phone)
        {
            DataEntitiesSKLAD = new SKLAD_WPF();
            ListProv.Clear();
            var queryOwner = (from owner in DataEntitiesSKLAD.Provider
                              where owner.Name.Contains(name)
                              where owner.Phone.Contains(phone)
                                select owner).ToList();
            foreach (var ow in queryOwner)
            {
                ListProv.Add(ow);
            }
            if (ListProv.Count > 0)
            {
                DataGridItem.ItemsSource = ListProv;
            }
            else
            {
                MessageBox.Show(
                    "Поставщики с заданным фильтром не найдены!",
                    "Внимание!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                GetStructs();
            }
        }

        private void FindCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = isDirty;

        }
        private void AddCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var act = new Provider();
            AddAct(act);
        }
        public void AddAct(Provider act)
        {
            act.Name = act.Phone = "не задано";
            act.AddedBy = owner_id;
            try
            {
                DataEntitiesSKLAD.Provider.Add(act);
                ListProv.Add(act);
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
            var emp = DataGridItem.SelectedItem as Provider;
            if (emp != null)
            {
                MessageBoxResult result = MessageBox.Show("Удалить данные ",
                "Предупреждение", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    DataEntitiesSKLAD.Provider.Remove(emp);
                    DataGridItem.SelectedIndex =
                    DataGridItem.SelectedIndex == 0 ? 1 : DataGridItem.SelectedIndex - 1;
                    ListProv.Remove(emp);
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
            GetStructs();
            DataGridItem.SelectedIndex = 0;
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
