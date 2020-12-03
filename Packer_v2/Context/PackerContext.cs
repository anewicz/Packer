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
        public virtual DbSet<Solution> ProjectTypeIntegration { get; set; }
        public virtual DbSet<TypeSolution> TypeSolution { get; set; }
        public virtual DbSet<Folder> Folder { get; set; }
        public virtual DbSet<WayType> WayType { get; set; }
        public virtual DbSet<DefaultWayType> DefaultWayType { get; set; }
        public virtual DbSet<TypeSolutionWay> TypeSolutionDefaultWay { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }
}