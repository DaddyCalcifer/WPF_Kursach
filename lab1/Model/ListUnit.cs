using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Data.Entity;

namespace lab1.Model
{
    public class ListUnit : ObservableCollection<Unit>
    {
        public ListUnit()
        {
            DbSet<Unit> specifics = lab1.Logic.WorkerManager.DataEntitiesSKLAD.Unit;
            var querySpec = from spec in specifics select spec;
            foreach (Unit specc in querySpec)
            {
                this.Add(specc);
            }
        }
    }
}
