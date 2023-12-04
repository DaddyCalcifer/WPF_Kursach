using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1.Model
{
    [System.Serializable]
    public class LoginData
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public bool? AutoLogin { get; set; }
        public DateTime LastLogin { get; set; }
    }
}
