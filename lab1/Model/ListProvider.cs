using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Data.Entity;

namespace lab1.Model
{
    public class ListProvider : ObservableCollection<Provider>
    {
        public ListProvider()
        {
            DbSet<Provider> accs = lab1.Logic.WorkerManager.DataEntitiesSKLAD.Provider;
            var queryAcc = (from acc in accs
                             select acc);
            foreach (Provider accc in queryAcc)
            {
                this.Add(accc);
            }
        }
    }
}
