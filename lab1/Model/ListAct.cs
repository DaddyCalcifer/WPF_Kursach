using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Data.Entity;

namespace lab1.Model
{
    public class ListAct : ObservableCollection<LoadAct>
    {
        public ListAct()
        {
            DbSet<LoadAct> accs = PageMain.DataEntitiesSKLAD.LoadAct;
            var queryAcc = (from acc in accs
                             select acc);
            foreach (var accc in queryAcc)
            {
                this.Add(accc);
            }
        }
    }
}
