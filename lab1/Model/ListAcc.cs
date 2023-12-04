using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Data.Entity;

namespace lab1.Model
{
    public class ListAcc : ObservableCollection<Account>
    {
        public ListAcc()
        {
            DbSet<Account> accs = PageMain.DataEntitiesSKLAD.Account;
            var queryAcc = (from acc in accs
                             select acc);
            foreach (Account accc in queryAcc)
            {
                this.Add(accc);
            }
        }
    }
}
