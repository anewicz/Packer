using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace Packer_v2.Models
{
    public class PackageViewModel
    {

        public int IdEps { get; set; }
        public int? IdProject { get; set; }
        public int? IdSolution { get; set; }
        public int? IdDtbase { get; set; }
        public bool HasSqlScripts { get; set; }
        public int? IdSqlComandType { get; set; }

        public int? IdSqlItenType { get; set; }

        public int? IdQuery { get; set; }
        public Ticket Ticket { get; set; }

        public Query Query { get; set; }

        public string NmSqlObject { get; set; }

        public List<Query> Querys { get; set; }
        public UploadFileResult File { get; set; }

        //public List<ScriptType> ScriptType { get { return GetScriptTypes(); } }

        public List<Status> StatusList { get; set; }
        public List<Eps> EpsList { get; set; }
        public List<Project> Projects { get; set; }
        public List<Solution> Solutions { get; set; }
        public List<Dtbase> Dtbases { get; set; }


        public List<SqlComandType> GetSqlComandTypes()
        {
            List<SqlComandType> _List = new List<SqlComandType>();

            _List.Add(new SqlComandType() { IdSqlComandType = 1, NmSqlComandType = "Update" });
            _List.Add(new SqlComandType() { IdSqlComandType = 2, NmSqlComandType = "Drop" });
            _List.Add(new SqlComandType() { IdSqlComandType = 3, NmSqlComandType = "Alter" });
            _List.Add(new SqlComandType() { IdSqlComandType = 4, NmSqlComandType = "Create" });

            return _List;
        }

        public List<SqlItenType> GetSqlItenTypes()
        {
            List<SqlItenType> _List = new List<SqlItenType>();

            _List.Add(new SqlItenType() { IdSqlItenType = 1, NmSqlItenType = "Table" });
            _List.Add(new SqlItenType() { IdSqlItenType = 2, NmSqlItenType = "View" });
            _List.Add(new SqlItenType() { IdSqlItenType = 3, NmSqlItenType = "Procedure" });
            _List.Add(new SqlItenType() { IdSqlItenType = 4, NmSqlItenType = "Column" });

            return _List;
        }

    }

    public class SqlComandType
    {
        public int IdSqlComandType { get; set; }
        public string NmSqlComandType { get; set; }

    }

    public class SqlItenType
    {
        public int IdSqlItenType { get; set; }
        public string NmSqlItenType { get; set; }

    }

    public class UploadFileResult
    {
        public IEnumerable<HttpPostedFileBase> File { get; set; }

    }

    public class QueryResult
    {
        public IEnumerable<HttpPostedFileBase> QueryObject { get; set; }

    }

}