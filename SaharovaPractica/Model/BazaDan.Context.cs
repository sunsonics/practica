﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SaharovaPractica.Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TechnoParkEntities : DbContext
    {
        public TechnoParkEntities()
            : base("name=TechnoParkEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<client> client { get; set; }
        public virtual DbSet<employee> employee { get; set; }
        public virtual DbSet<inventory> inventory { get; set; }
        public virtual DbSet<orderitem> orderitem { get; set; }
        public virtual DbSet<orders> orders { get; set; }
        public virtual DbSet<payment> payment { get; set; }
        public virtual DbSet<product> product { get; set; }
        public virtual DbSet<record> record { get; set; }
        public virtual DbSet<role> role { get; set; }
        public virtual DbSet<service> service { get; set; }
        public virtual DbSet<supply> supply { get; set; }
    }
}
