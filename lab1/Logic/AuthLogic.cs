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
            var own = new Owner();
            own.Name = acc.Name;
            own.Email = acc.Email;
            own.Phone = "не задано";

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
                    DataEntitiesSKLAD.Owner.Add(own);
                    DataEntitiesSKLAD.SaveChanges();

                    var queryOwn = (from owner in DataEntitiesSKLAD.Owner
                                    where owner.Name == acc.Name
                                    where owner.Email == acc.Email
                                    select owner).ToList();
                    var own_ = queryOwn.ToList()[0];

                    acc.ID_Owner = own_.ID_Owner;
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
