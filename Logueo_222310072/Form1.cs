using Microsoft.Data.SqlClient;
using System.Data;

namespace Logueo_222310072
{
    public partial class Form1 : Form
    {
        int idUsuario = 0;
        int TipoU = 0;
        string nombreAutorizado = "";
        bool Status = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_Aceptar_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void icbLogueo_Click(object sender, EventArgs e)
        {
            try
            {
                string message;
                string caption;
                MessageBoxButtons buttons;
                MessageBoxIcon icon;
                DialogResult result;

                SqlConnection con = Conexion.cadena();
                if (con != null)
                {
                    MessageBox.Show("Conexión Exitosa", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    SqlCommand com = new SqlCommand();
                    com.Connection = con;
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "spLogueo";

                    com.Parameters.Clear();

                    com.Parameters.Add("@nombreUsuario", SqlDbType.VarChar, 50).Value = txtUsuario.Text;
                    com.Parameters.Add("@contraUsuario", SqlDbType.VarChar, 50).Value = txtContraseña.Text;

                    com.Parameters.Add("@idUsuarioLogueado", SqlDbType.Int).Value = 0;
                    com.Parameters["@idUsuarioLogueado"].Direction = ParameterDirection.Output;

                    com.Parameters.Add("@TipoU", SqlDbType.Int).Value = 0;
                    com.Parameters["@TipoU"].Direction = ParameterDirection.Output;

                    com.Parameters.Add("@Status", SqlDbType.Bit).Value = 0;
                    com.Parameters["@Status"].Direction = ParameterDirection.Output;

                    com.ExecuteNonQuery();

                    Status = bool.Parse(com.Parameters["@Status"].Value.ToString());

                    if (Status == true)
                    {
                        nombreAutorizado = txtUsuario.Text;
                        idUsuario = int.Parse(com.Parameters["@idUsuarioLogueado"].Value.ToString());
                        TipoU = int.Parse(com.Parameters["@TipoU"].Value.ToString());



                        if (TipoU == 1)
                        {
                            message = "Bienvenido " + nombreAutorizado + Environment.NewLine + " del Area: Administrador ";
                            caption = "Datos Validados";
                            buttons = MessageBoxButtons.OK;
                            icon = MessageBoxIcon.Information;
                            result = MessageBox.Show(message, caption, buttons, icon);

                            if (result == DialogResult.OK)
                            {
                                wfPanelAdmin m = new wfPanelAdmin()
                                {
                                    StartPosition = FormStartPosition.CenterScreen,
                                    TopMost = true // Hace que esté al frente de todo
                                };
                                m.Width = 892;
                                m.Height = 465;
                                m.lbIDUSUARIO.Text = idUsuario.ToString();
                                m.lbIDUser_Menu.Text = idUsuario.ToString();
                                m.lbNombreMenu.Text = nombreAutorizado;
                                m.lbNOMBRE.Text = nombreAutorizado;
                                m.lbIDUser_P.Text = idUsuario.ToString();
                                m.lbNombreUser_P.Text = nombreAutorizado;
                                m.lbName_tbADM.Text = nombreAutorizado;
                                m.lbIDEMP.Text = idUsuario.ToString();
                                m.lbNameEMP.Text = nombreAutorizado;
                                m.lbAreaEMP.Text = "Administrador";
                                m.lbNameRP.Text = nombreAutorizado;
                                m.lbIdRP.Text = idUsuario.ToString();
                                m.tbcAdmin.SelectedTab = m.tabADMIN;

                                // === CARGAR IMAGEN DEL USUARIO ===
                                try
                                {
                                    SqlCommand comImg = new SqlCommand("SELECT Foto FROM Usuarios WHERE idUsuario = @id", con);
                                    comImg.Parameters.AddWithValue("@id", idUsuario);
                                    object resultImg = comImg.ExecuteScalar();

                                    if (resultImg != DBNull.Value && resultImg != null)
                                    {
                                        string nombreImagen = resultImg.ToString().Trim().TrimStart('\\'); // quitar barra inicial
                                        string rutaImagen = Path.Combine(Application.StartupPath, "Usuarios", nombreImagen);

                                        if (File.Exists(rutaImagen))
                                        {
                                            using (FileStream fs = new FileStream(rutaImagen, FileMode.Open, FileAccess.Read))
                                            {
                                                m.pic_usReg_Users.Image = Image.FromStream(fs);
                                                m.pic_UserMenu.Image = Image.FromStream(fs);
                                                m.picUserProd.Image = Image.FromStream(fs);
                                                m.picImgEMP.Image = Image.FromStream(fs);
                                                m.picImgRP.Image = Image.FromStream(fs);
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Imagen no encontrada:\n" + rutaImagen);
                                            m.pic_usReg_Users.Image = null;
                                            m.pic_UserMenu.Image = null;
                                            m.picUserProd.Image = null;
                                            m.picImgEMP.Image = null;
                                            m.picImgRP.Image = null;
                                        }
                                    }
                                    else
                                    {
                                        m.pic_usReg_Users.Image = null;
                                        m.pic_UserMenu.Image = null;
                                        m.picUserProd.Image = null;
                                        m.picImgEMP.Image = null;
                                        m.picImgRP.Image = null;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Error al cargar la imagen del usuario:\n" + ex.Message);
                                    m.pic_usReg_Users.Image = null;
                                    m.pic_UserMenu.Image = null;
                                    m.picUserProd.Image = null;
                                    m.picImgEMP.Image = null;
                                    m.picImgRP.Image = null;
                                }

                                m.Show();
                                m.Width = 890;
                                m.Height = 460;

                                txtUsuario.Clear();
                                txtContraseña.Clear();
                                this.Hide();
                            }
                        }

                        if (TipoU == 2)
                        {
                            message = "Bienvenido " + nombreAutorizado + Environment.NewLine + " del Area: Cajas ";
                            caption = "Datos Validados";
                            buttons = MessageBoxButtons.OK;
                            icon = MessageBoxIcon.Exclamation;
                            result = MessageBox.Show(message, caption, buttons, icon);

                            if (result == DialogResult.OK)
                            {
                                wfPanelAdmin m = new wfPanelAdmin();

                                this.Width = 706;
                                this.Height = 503;
                                m.lbIDUSUARIO.Text = idUsuario.ToString();
                                m.lbIDUser_Menu.Text = idUsuario.ToString();
                                m.lbNombreMenu.Text = nombreAutorizado;
                                m.lbNOMBRE.Text = nombreAutorizado;
                                m.lbIDUser_P.Text = idUsuario.ToString();
                                m.lbNombreUser_P.Text = nombreAutorizado;
                                m.lbName_tbADM.Text = nombreAutorizado;
                                m.lbIDEMP.Text = idUsuario.ToString();
                                m.lbNameEMP.Text = nombreAutorizado;
                                m.lbAreaEMP.Text = "Cajero";

                                m.picVolverMenu.Enabled = false;
                                m.picVolverMenu.Visible = false;

                                m.tbcAdmin.SelectedTab = m.tabEMPLEADO;
                                // CENTRAR FORMULARIO MANUALMENTE EN PANTALLA
                                int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
                                int screenHeight = Screen.PrimaryScreen.WorkingArea.Height;
                                m.StartPosition = FormStartPosition.Manual;
                                m.Location = new Point((screenWidth - m.Width) / 2, (screenHeight - m.Height) / 2);

                                // === CARGAR IMAGEN DEL USUARIO ===
                                try
                                {
                                    SqlCommand comImg = new SqlCommand("SELECT Foto FROM Usuarios WHERE idUsuario = @id", con);
                                    comImg.Parameters.AddWithValue("@id", idUsuario);
                                    object resultImg = comImg.ExecuteScalar();

                                    if (resultImg != DBNull.Value && resultImg != null)
                                    {
                                        string nombreImagen = resultImg.ToString().Trim().TrimStart('\\'); // quitar barra inicial
                                        string rutaImagen = Path.Combine(Application.StartupPath, "Usuarios", nombreImagen);

                                        if (File.Exists(rutaImagen))
                                        {
                                            using (FileStream fs = new FileStream(rutaImagen, FileMode.Open, FileAccess.Read))
                                            {
                                                m.pic_usReg_Users.Image = Image.FromStream(fs);
                                                m.pic_UserMenu.Image = Image.FromStream(fs);
                                                m.picUserProd.Image = Image.FromStream(fs);
                                                m.picImgEMP.Image = Image.FromStream(fs);
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Imagen no encontrada:\n" + rutaImagen);
                                            m.pic_usReg_Users.Image = null;
                                            m.pic_UserMenu.Image = null;
                                            m.picUserProd.Image = null;
                                            m.picImgEMP.Image = null;
                                        }
                                    }
                                    else
                                    {
                                        m.pic_usReg_Users.Image = null;
                                        m.pic_UserMenu.Image = null;
                                        m.picUserProd.Image = null;
                                        m.picImgEMP.Image = null;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Error al cargar la imagen del usuario:\n" + ex.Message);
                                    m.pic_usReg_Users.Image = null;
                                    m.pic_UserMenu.Image = null;
                                    m.picUserProd.Image = null;
                                    m.picImgEMP.Image = null;
                                }

                                m.Show();
                                txtUsuario.Clear();
                                txtContraseña.Clear();
                                this.Hide();
                            }
                        }
                        if (TipoU == 3)
                        {
                            message = "Bienvenido " + nombreAutorizado + Environment.NewLine + " del Area: Cocina ";
                            caption = "Datos Validados";
                            buttons = MessageBoxButtons.OK;
                            icon = MessageBoxIcon.Exclamation;
                            result = MessageBox.Show(message, caption, buttons, icon);

                            if (result == DialogResult.OK)
                            {
                                wf_GestionPedidos m = new wf_GestionPedidos();
                                m.lbIDUSUARIO.Text = idUsuario.ToString();
                                m.lbNOMBRE.Text = nombreAutorizado;
                                m.Show();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Usuario no encontrado en la base de datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.None);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            DialogResult resultado = MessageBox.Show(
                       "¿Quieres cerrar el programa?",
                       "Confirmación",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                                                 );

            if (resultado == DialogResult.Yes)
            {
                Application.Exit(); // O this.Close() si solo quieres cerrar el formulario actual
            }
        }

        private void picOFF_Click(object sender, EventArgs e)
        {
            DialogResult resultado = MessageBox.Show(
                      "¿Quieres cerrar el programa?",
                      "Confirmación",
                       MessageBoxButtons.YesNo,
                       MessageBoxIcon.Question
                                                );

            if (resultado == DialogResult.Yes)
            {
                Application.Exit(); // O this.Close() si solo quieres cerrar el formulario actual
            }
        }

        private void picOFF_MouseEnter(object sender, EventArgs e)
        {
            picOFF.Image = Properties.Resources.powerOff2;
        }

        private void picOFF_MouseLeave(object sender, EventArgs e)
        {
            picOFF.Image = Properties.Resources.powerOff;
        }
    }
}
