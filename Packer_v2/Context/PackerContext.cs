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
        public virtual DbSet<ProjectTypeSolution> ProjectTypeIntegrations { get; set; }
        public virtual DbSet<TypeSolution> TypeSolution { get; set; }
        public virtual DbSet<Folder> Folders { get; set; }
        public virtual DbSet<WayType> WayTypes { get; set; }
        public virtual DbSet<DefaultWayType> DefaultWayType { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }


    }
}