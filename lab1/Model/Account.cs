//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace lab1.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Account
    {
        public int ID_Account { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int TypeID { get; set; }
        public string Password { get; set; }
        public Nullable<int> ID_Owner { get; set; }
    }
}
