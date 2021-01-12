using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Packer_v2.Models
{
    public class Package
    {
        
        public Ticket ticket { get; set; }
        public List<Query> querys { get; set; }
        public List<Dtbase> dtbase { get; set; }



    }
}