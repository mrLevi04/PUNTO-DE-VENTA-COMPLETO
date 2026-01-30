using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logueo_222310072
{
    internal class Conexion
    {
        public static SqlConnection cadena()
        {
            SqlConnection con = new SqlConnection(@"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=TBDBTienda;Data Source=MRL\SQLEXPRESS;Trust Server Certificate=True");
            try
            {
                con.Open();
                return con;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
