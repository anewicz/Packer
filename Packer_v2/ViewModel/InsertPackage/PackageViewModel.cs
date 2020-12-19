using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Packer_v2.Models
{
    public class PackageViewModel
    {

        public int IdEps { get; set; }
        public int? IdProject { get; set; }
        public int? IdSolution { get; set; }

        public List<Eps> EpsList { get; set; }
        public List<Project> Projects { get; set; }
        public List<Solution> Solutions { get; set; }

    }
}