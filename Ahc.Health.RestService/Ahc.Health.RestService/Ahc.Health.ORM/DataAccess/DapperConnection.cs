using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Ahc.Health.ORM.DataAccess
{
    public class DapperConnection
    {
        public IDbConnection DapperCon
        {
            get
            {
                return new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());

            }
        }
    }
}
