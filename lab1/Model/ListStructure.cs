using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Data.Entity;

namespace lab1.Model
{
    public class ListStructure : ObservableCollection<Structure>
    {
        public ListStructure()
        {
            DbSet<Structure> structs = lab1.Logic.WorkerManager.DataEntitiesSKLAD.Structure;
            var querySpec = from spec in structs select spec;
            foreach (Structure specc in querySpec)
            {
                this.Add(specc);
            }
        }
    }
}
