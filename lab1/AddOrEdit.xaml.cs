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
using System.Windows.Shapes;
using lab1.Model;
namespace lab1
{
    /// <summary>
    /// Логика взаимодействия для AddOrEdit.xaml
    /// </summary>
    public partial class AddOrEdit : Window
    {
        public string name, email, phone;
        bool edit = false;
        PageMain pm;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(edit)
            {
                Owner own = new Owner();
                own.Phone = PhoneBox.Text.Trim();
                own.Email = EmailBox.Text.Trim();
                own.Name = NameBox.Text.Trim();
                pm.AddOwner(own);
            } 
            else
            {
                Owner own = new Owner();
                own.Phone = PhoneBox.Text.Trim();
                own.Email = EmailBox.Text.Trim();
                own.Name =  NameBox.Text.Trim();
                pm.AddOwner(own);
            }
            this.Close();
        }

        public AddOrEdit(int id, PageMain pm)
        {
            InitializeComponent();

            edit = true;
            this.NameBox.Text = name;
            this.EmailBox.Text = email;
            this.PhoneBox.Text = phone;
            this.pm = pm;
        }
        public AddOrEdit(PageMain pm)
        {
            InitializeComponent();
            this.pm = pm;
        }
    }
}
