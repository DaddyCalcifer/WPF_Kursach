using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Data.Entity;

namespace lab1.Model
{
    public class ListUserType : ObservableCollection<UserType>
    {
        public ListUserType()
        {
            DbSet<UserType> specifics = PageMain.DataEntitiesSKLAD.UserType;
            var querySpec = from spec in specifics select spec;
            foreach (UserType specc in querySpec)
            {
                this.Add(specc);
            }
        }
    }
}
