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
    public partial class PageWorkers : Page
    {
        public static SKLAD_WPF DataEntitiesSKLAD { get; set; } = new SKLAD_WPF();
        public ObservableCollection<Account> ListAccs { get; set; }
        public MainWindow mw;
        int role;
        int owner_id = -1;

        public PageWorkers(int owner, MainWindow mw)
        {
            InitializeComponent();
            DataEntitiesSKLAD = new SKLAD_WPF();
            ListAccs = new ObservableCollection<Account>();
            owner_id = owner;
            this.mw = mw;
            role = (int)PageProfile.GetAcc(owner).Type;
        }
        public bool isDirty = true;
        public static bool canSave = true;

        public void GetActs()
        {
            ListAccs.Clear();
            List<Account> queryActs = (from act in DataEntitiesSKLAD.Account
                               orderby act.Type descending
                               select act).ToList();
      
            foreach (Account act1 in queryActs)
            {
                ListAccs.Add(act1);
            }
            DataGridItem.ItemsSource = ListAccs;
            foreach (var item in ListAccs)
            {
                Console.WriteLine(item.Login + " " + item.ID_Account);
            }
        }
        private void RewriteAct()
        {
            DataEntitiesSKLAD = new SKLAD_WPF();
            ListAccs.Clear();
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
            e.CanExecute = false;

        }
        public FIndWindow fw;
        private void FindCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (fw == null)
            {
                fw = new FIndWindow(this);
                fw.Show();
            }
            isDirty = true;
        }
        public void FindByName(string login, string name, string phone, string email, int type =0)
        {
            DataEntitiesSKLAD = new SKLAD_WPF();
            ListAccs.Clear();
            List<Account> queryOwner;
            if(type != 0)
            queryOwner = (from owner in DataEntitiesSKLAD.Account
                              where owner.Name.Contains(name)
                              where owner.Phone.Contains(phone)
                              where owner.Email.Contains(email)
                              where owner.Login.Contains(login)
                              where owner.Type == type
                                select owner).ToList();
            else
                queryOwner = (from owner in DataEntitiesSKLAD.Account
                              where owner.Name.Contains(name)
                              where owner.Phone.Contains(phone)
                              where owner.Email.Contains(email)
                              where owner.Login.Contains(login)
                              select owner).ToList();
            foreach (var ow in queryOwner)
            {
                ListAccs.Add(ow);
            }
            if (ListAccs.Count > 0)
            {
                DataGridItem.ItemsSource = ListAccs;
            }
            else
            {
                MessageBox.Show(
                    "Работники с заданным фильтром не найдены!",
                    "Внимание!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                GetActs();
            }
        }

        private void FindCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = isDirty;

        }
        private void AddCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var act = new Account();
            AddAct(act);
        }
        public void AddAct(Account act)
        {
            try
            {
                DataEntitiesSKLAD.Account.Add(act);
                ListAccs.Add(act);
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
            e.CanExecute = false;

        }
        private void DeleteCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Account emp = DataGridItem.SelectedItem as Account;
            if (emp != null)
            {
                MessageBoxResult result = MessageBox.Show("Удалить данные ",
                "Предупреждение", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    emp.Type = 1;
                    DataGridItem.SelectedIndex =
                    DataGridItem.SelectedIndex == 0 ? 1 : DataGridItem.SelectedIndex - 1;
                    //ListAccs.Remove(emp);
                    isDirty = true;
                    GetActs();
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

        private void Unban_Click(object sender, RoutedEventArgs e)
        {
            Account emp = DataGridItem.SelectedItem as Account;
            if (emp.Type == 2)
            {
                MessageBox.Show("Действие невозможно");
                return;
            }
            if (emp != null)
            {
                MessageBoxResult result = MessageBox.Show("Разблокировать пользователя?",
                "Предупреждение", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    emp.Type = 2;
                    DataGridItem.SelectedIndex =
                    DataGridItem.SelectedIndex == 0 ? 1 : DataGridItem.SelectedIndex - 1;
                    isDirty = true;
                    GetActs();
                }
            }
            else
            {
                MessageBox.Show("Выберите пользователя для разблокировки");
            }
        }

        private void MakeAdmin_Click(object sender, RoutedEventArgs e)
        {
            Account emp = DataGridItem.SelectedItem as Account;
            if(emp.Type == 1 || emp.Type==3)
            {
                MessageBox.Show("Действие невозможно");
                return;
            }
            if (emp != null)
            {
                MessageBoxResult result = MessageBox.Show($"Назначить пользователя({emp.Login}) администратором?",
                "Предупреждение", MessageBoxButton.OKCancel); ;
                if (result == MessageBoxResult.OK)
                {
                    emp.Type = 3;
                    DataGridItem.SelectedIndex =
                    DataGridItem.SelectedIndex == 0 ? 1 : DataGridItem.SelectedIndex - 1;
                    isDirty = true;
                    GetActs();
                }
            }
            else
            {
                MessageBox.Show("Выберите пользователя");
            }
        }
    }
}
