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
    public partial class AdminPage : Page
    {
        int acc_id = -1;
        Account acc;
        MainWindow mainWindow;
        public static bool canSave = false;
        Object img;
        Object img2;

        public AdminPage(MainWindow mw, int acc_id)
        {
            InitializeComponent();
            this.mainWindow = mw;
            this.acc_id = acc_id;
            img = ProfileButton.Content;
            img2= ChangeUser.Content;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PageProfile pp = new PageProfile(mainWindow, this, acc_id);
            mainWindow.Content = pp;
        }

        private void ProfileButton_MouseEnter(object sender, MouseEventArgs e)
        {
            DoubleAnimation btnAnim = new DoubleAnimation();
            btnAnim.From = 60;
            btnAnim.To = 130;
            btnAnim.Duration = TimeSpan.FromSeconds(0.2);
            ProfileButton.BeginAnimation(Button.WidthProperty, btnAnim);
            ProfileButton.Content = "Профиль";
        }

        private void ProfileButton_MouseLeave(object sender, MouseEventArgs e)
        {
            DoubleAnimation btnAnim = new DoubleAnimation();
            btnAnim.From = ProfileButton.Width;
            btnAnim.To = 60;
            btnAnim.Duration = TimeSpan.FromSeconds(0.2);
            ProfileButton.BeginAnimation(Button.WidthProperty, btnAnim);
            ProfileButton.Content = img;
        }

        private void ChangeUser_MouseEnter(object sender, MouseEventArgs e)
        {
            DoubleAnimation btnAnim = new DoubleAnimation();
            btnAnim.From = 60;
            btnAnim.To = 160;
            btnAnim.Duration = TimeSpan.FromSeconds(0.2);
            ChangeUser.BeginAnimation(Button.WidthProperty, btnAnim);
            ChangeUser.Content = "Сменить пользователя";
        }

        private void ChangeUser_MouseLeave(object sender, MouseEventArgs e)
        {
            DoubleAnimation btnAnim = new DoubleAnimation();
            btnAnim.From = ChangeUser.Width;
            btnAnim.To = 60;
            btnAnim.Duration = TimeSpan.FromSeconds(0.2);
            ChangeUser.BeginAnimation(Button.WidthProperty, btnAnim);
            ChangeUser.Content = img2;
        }

        private void ChangeUser_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.logout = true;
            mainWindow.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            mainWindow.Content = new PageMain(acc_id, mainWindow);
        }
    }
}
