using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Logueo_222310072
{
    public partial class wf_GestionProductos : Form
    {
        public wf_GestionProductos()
        {
            InitializeComponent();
            CargaDatosDGVP();
        }
        public void CargaDatosDGVP()
        {
            SqlConnection con = Conexion.cadena();
            if (con != null)
            {
                SqlDataAdapter daCarga = new SqlDataAdapter("select * from Productos", con);
                DataSet dsCarga = new DataSet();
                daCarga.Fill(dsCarga, "Productos");
                dgvProductos.DataSource = dsCarga;
                dgvProductos.DataMember = "Productos";

                con.Close();
            }
        }
        private void btnAltaProducto_Click(object sender, EventArgs e)
        {

        }
    }
}
