using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Data.Entity;

namespace lab1.Model
{
    public class ListProvider : ObservableCollection<Account>
    {
        public ListProvider()
        {
            DbSet<Account> accs = PageMain.DataEntitiesSKLAD.Account;
            var queryAcc = (from acc in accs
                            where acc.Type == 1
                             select acc);
            foreach (Account accc in queryAcc)
            {
                this.Add(accc);
            }
        }
    }
}
