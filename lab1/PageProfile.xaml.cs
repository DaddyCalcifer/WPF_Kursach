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
using lab1.Model;
using System.Windows.Media.Animation;

namespace lab1
{
    /// <summary>
    /// Логика взаимодействия для PageProfile.xaml
    /// </summary>
    public partial class PageProfile : Page
    {
        int acc_id = -1;
        Account acc;
        MainWindow mainWindow;
        PageMain pageMain;
        public static bool canSave = false;

        public static SKLAD_WPF DataEntitiesSKLAD { get; set; } = new SKLAD_WPF();
        public ObservableCollection<LoadAct> ListActs { get; set; }
        public PageProfile(MainWindow mw, PageMain pm, int acc_id)
        {
            InitializeComponent();
            this.mainWindow = mw;
            this.pageMain = pm;
            acc = GetAcc(acc_id);
            this.DataContext = acc;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.Content = pageMain;
        }
        Account GetAcc(int id)
        {
            var queryActs = (from act in DataEntitiesSKLAD.Account
                             where act.ID_Account == id
                             select act).ToList();
            return queryActs.FirstOrDefault();
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            DoubleAnimation btnAnim = new DoubleAnimation();
            btnAnim.From = 60;
            btnAnim.To = 130;
            btnAnim.Duration = TimeSpan.FromSeconds(0.2);
            backButton.BeginAnimation(Button.WidthProperty, btnAnim);
            backButton.Content = "Назад";
            backButton.Foreground = new System.Windows.Media.SolidColorBrush(Color.FromArgb(255,0,0,255));
        }

        private void backButton_MouseLeave(object sender, MouseEventArgs e)
        {
            DoubleAnimation btnAnim = new DoubleAnimation();
            btnAnim.From = backButton.Width;
            btnAnim.To = 60;
            btnAnim.Duration = TimeSpan.FromSeconds(0.2);
            backButton.BeginAnimation(Button.WidthProperty, btnAnim);
            backButton.Content = "<";
            backButton.Foreground = new System.Windows.Media.SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (!canSave)
            {
                MessageBox.Show("Невозможно сохранить изменения!",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            else
            {
                DataEntitiesSKLAD.SaveChanges();
                MessageBox.Show("Изменения сохранены");
            }
        }
    }
}
