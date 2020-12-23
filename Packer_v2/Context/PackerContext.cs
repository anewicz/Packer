using Packer_v2.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Packer_v2.Context
{
    public class PackerContext : DbContext
    {
        public virtual DbSet<Eps> Eps { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<Solution> Solution { get; set; }
        public virtual DbSet<TypeSolution> TypeSolution { get; set; }
        public virtual DbSet<TypeSolutionWay> TypeSolutiontWay { get; set; }
        public virtual DbSet<WayType> WayType { get; set; }
        public virtual DbSet<Folder> Folder { get; set; }
        public virtual DbSet<DefaultWayType> DefaultWayType { get; set; }
        public virtual DbSet<Ticket> Ticket { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<Dtbase> Dtbase { get; set; }
        public virtual DbSet<DbIp> DbIp { get; set; }
        public virtual DbSet<DbSolution> DbSolution { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConve‌​ntion>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}