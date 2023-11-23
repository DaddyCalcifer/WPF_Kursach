using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lab1.Model;
using System.Collections.ObjectModel;
using System.Data.Entity;
using BCrypt.Net;

namespace lab1.Logic
{
    public class AuthLogic
    {
        public static SKLAD_WPF DataEntitiesSKLAD { get; set; } = new SKLAD_WPF();
        public ObservableCollection<Account> ListAcc { get; set; } = new ObservableCollection<Account>();
        public void Register(Account acc)
        {
            var queryAcc = (from account in DataEntitiesSKLAD.Account
                              where account.Login.Contains(acc.Login)
                              select account).ToList();

            foreach (Account ac in queryAcc)
            {
                ListAcc.Add(ac);
            }
            
            if (ListAcc.Count < 1)
            {
                try
                {
                    acc.Password = BCrypt.Net.BCrypt.HashPassword(acc.Password);
                    DataEntitiesSKLAD.Account.Add(acc);
                    DataEntitiesSKLAD.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(
                    "Ошибка регистрации" + ex.ToString());
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Логин уже занят!");
            }
        }
        public int Login(string login, string password)
        {
            var queryAcc = (from account in DataEntitiesSKLAD.Account
                            where account.Login.Contains(login)
                            select account).ToList();
            foreach (Account ac in queryAcc)
            {
                ListAcc.Add(ac);
            }
            if (BCrypt.Net.BCrypt.Verify(password, ListAcc.Last().Password))
                return ListAcc.Last().ID_Account;
            else return -1;
        }
    }
}
