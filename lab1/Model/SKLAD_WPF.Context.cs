﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SKLAD_WPF : DbContext
    {
        public SKLAD_WPF()
            : base("name=SKLAD_WPF")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<Item> Item { get; set; }
        public virtual DbSet<Owner> Owner { get; set; }
        public virtual DbSet<Specific> Specific { get; set; }
        public virtual DbSet<Structure> Structure { get; set; }
    }
}
