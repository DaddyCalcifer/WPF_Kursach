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
    public partial class PageStructures : Page
    {
        public static SKLAD_WPF DataEntitiesSKLAD { get; set; } = new SKLAD_WPF();
        public ObservableCollection<Structure> ListStruct { get; set; }
        public MainWindow mw;
        int role;
        int owner_id = -1;

        public PageStructures(int owner, MainWindow mw)
        {
            InitializeComponent();
            DataEntitiesSKLAD = new SKLAD_WPF();
            ListStruct = new ObservableCollection<Structure>();
            owner_id = owner;
            this.mw = mw;
            role = (int)PageProfile.GetAcc(owner).Type;
        }
        public bool isDirty = true;
        public static bool canSave = true;

        public void GetStructs()
        {
            ListStruct.Clear();
            List<Structure> queryActs = (from act in DataEntitiesSKLAD.Structure
                               orderby act.ID_Structure
                               select act).ToList();
      
            foreach (Structure act1 in queryActs)
            {
                ListStruct.Add(act1);
            }
            DataGridItem.ItemsSource = ListStruct;
            foreach (var item in ListStruct)
            {
                Console.WriteLine(item.Name + " " + item.Adress);
            }
        }
        private void RewriteAct()
        {
            DataEntitiesSKLAD = new SKLAD_WPF();
            ListStruct.Clear();
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
                StructureFindPage sfp = new StructureFindPage(this);
                fw.Content = sfp;
                fw.Show();
            }
            isDirty = true;
        }
        public void FindByID(int id)
        {
            DataEntitiesSKLAD = new SKLAD_WPF();
            ListStruct.Clear();
            var queryOwner = (from owner in DataEntitiesSKLAD.Structure
                              where owner.ID_Structure == id
                              select owner).ToList();
            foreach (var ow in queryOwner)
            {
                ListStruct.Add(ow);
            }
            if (ListStruct.Count > 0)
            {
                DataGridItem.ItemsSource = ListStruct;
            }
            else
            {
                MessageBox.Show(
                    "Склады с заданным фильтром не найдены!",
                    "Внимание!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                GetStructs();
            }
        }
        public void FindByName(string name, string adress)
        {
            DataEntitiesSKLAD = new SKLAD_WPF();
            ListStruct.Clear();
            var queryOwner = (from owner in DataEntitiesSKLAD.Structure
                              where owner.Name.Contains(name)
                              where owner.Adress.Contains(adress)
                              select owner).ToList();
            foreach (var ow in queryOwner)
            {
                ListStruct.Add(ow);
            }
            if (ListStruct.Count > 0)
            {
                DataGridItem.ItemsSource = ListStruct;
            }
            else
            {
                MessageBox.Show(
                    "Склады с заданным фильтром не найдены!",
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
            var act = new Structure();
            AddAct(act);
        }
        public void AddAct(Structure act)
        {
            act.Name = act.Adress = "не задано";
            act.Square = 1;
            try
            {
                DataEntitiesSKLAD.Structure.Add(act);
                ListStruct.Add(act);
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
            Structure emp = DataGridItem.SelectedItem as Structure;
            if (emp != null)
            {
                MessageBoxResult result = MessageBox.Show("Удалить данные ",
                "Предупреждение", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    DataEntitiesSKLAD.Structure.Remove(emp);
                    DataGridItem.SelectedIndex =
                    DataGridItem.SelectedIndex == 0 ? 1 : DataGridItem.SelectedIndex - 1;
                    ListStruct.Remove(emp);
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
                    GetStructs();
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
                    GetStructs();
                }
            }
            else
            {
                MessageBox.Show("Выберите пользователя");
            }
        }
    }
}
