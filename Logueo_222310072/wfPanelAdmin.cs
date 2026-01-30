using AForge.Video;
using AForge.Video.DirectShow;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using iTextFont = iTextSharp.text.Font;
using iTextImage = iTextSharp.text.Image;


namespace Logueo_222310072
{
    public partial class wfPanelAdmin : Form
    {
        Dictionary<string, int> productoCounts = new Dictionary<string, int>();
        Dictionary<string, int> productoPrecios = new Dictionary<string, int>();

        private string ticketContent = "";
        int idVenta = 0;
        public string ruta, path;
        private bool ExistenDispositivos = false;
        private FilterInfoCollection DispositivosDeVideo;
        public VideoCaptureDevice FuenteDeVideo = null;

        private bool expandiendo = false; // Indica si se está expandiendo
        private int alturaExpandida = 785; // Altura del formulario expandido
        private int alturaContraida = 516; // Altura del formulario contraído
        private int velocidadAnimacion = 60; // Cantidad de píxeles por paso
        private string tipoPagoSeleccionado = "EFECTIVO"; // por defecto

        public wfPanelAdmin()
        {
            InitializeComponent();
            // Configura el temporizador
            timActual.Interval = 1000; // 1 segundo
            timActual.Tick += timActual_Tick;
            timActual.Start();
            // Configurar el evento Tick del timer ya agregado
            timer1.Tick += timer1_Tick;

            // Configurar el tamaño inicial del formulario
            this.Height = alturaContraida;
        }

        private void wfPanelAdmin_Load(object sender, EventArgs e)
        {
            CargarHorarios();
            CargarBotonesDesdeBD();
            CargaDatosDGV();
            CargaDatosDGVP();
            BuscarDispositivosVideo();
            Form1 form1 = new Form1();
            form1.Close();
        }
        public void BuscarDispositivosVideo()
        {
            DispositivosDeVideo = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (DispositivosDeVideo.Count == 0)
            {
                ExistenDispositivos = false;
            }
            else
            {
                ExistenDispositivos = true;
                CargaDispositivos(DispositivosDeVideo);
            }
        }

        public void CargaDispositivos(FilterInfoCollection Dispositivos)
        {


        }
        public void CargaDatosDGV()
        {
            SqlConnection con = Conexion.cadena();
            if (con != null)
            {
                try
                {
                    string query = @"
                SELECT 
                    u.idUsuario,
                    u.NombreUsuario,
                    u.ContraUsuario,
                    u.Tipo,
                    u.Foto,
                    rh.Fecha,
                    rh.HoraEntrada,
                    rh.HoraSalida
                FROM Usuarios u
                LEFT JOIN (
                    SELECT IdUsuario, Fecha, HoraEntrada, HoraSalida
                    FROM RegistroHorario
                    WHERE Fecha = CAST(GETDATE() AS DATE)
                ) rh ON u.idUsuario = rh.IdUsuario
                ORDER BY u.idUsuario ASC";

                    SqlDataAdapter daCarga = new SqlDataAdapter(query, con);
                    DataSet dsCarga = new DataSet();
                    daCarga.Fill(dsCarga, "UsuariosConHorario");

                    dataGridView1.DataSource = dsCarga;
                    dataGridView1.DataMember = "UsuariosConHorario";

                    // Cambiar encabezados
                    dataGridView1.Columns["idUsuario"].HeaderText = "ID";
                    dataGridView1.Columns["NombreUsuario"].HeaderText = "Usuario";
                    dataGridView1.Columns["ContraUsuario"].HeaderText = "Contraseña";
                    dataGridView1.Columns["Tipo"].HeaderText = "Tipo Usuario";
                    dataGridView1.Columns["Foto"].HeaderText = "Foto";
                    dataGridView1.Columns["Fecha"].HeaderText = "Fecha";
                    dataGridView1.Columns["HoraEntrada"].HeaderText = "Hora Entrada";
                    dataGridView1.Columns["HoraSalida"].HeaderText = "Hora Salida";

                    // Aplicar formato HH:mm a las columnas de hora
                    dataGridView1.Columns["HoraEntrada"].DefaultCellStyle.Format = @"hh\:mm";
                    dataGridView1.Columns["HoraSalida"].DefaultCellStyle.Format = @"hh\:mm";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    con.Close();
                }
            }
            else
            {
                MessageBox.Show("No se pudo conectar a la base de datos.", "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void CargaDatosDGVP()
        {
            SqlConnection con = Conexion.cadena();
            if (con != null)
            {
                SqlDataAdapter daCarga = new SqlDataAdapter("SELECT * FROM tbProd", con);
                DataSet dsCarga = new DataSet();
                daCarga.Fill(dsCarga, "tbProd");
                dgvProductos.DataSource = dsCarga;
                dgvProductos.DataMember = "tbProd";

                con.Close();
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            picUsuario.Image = System.Drawing.Image.FromFile(Application.StartupPath + "\\Usuarios\\user.jpg");
            try
            {
                int id = (int)dataGridView1.CurrentRow.Cells["idUsuario"].Value;
                poneTexto(id);
            }
            catch (Exception)
            {
                return;
            }
        }

        public void poneTexto(int id)
        {
            SqlConnection con = Conexion.cadena();
            if (con != null)
            {
                try
                {
                    // Cargar datos de la tabla Usuarios
                    SqlCommand com = new SqlCommand("SELECT * FROM Usuarios WHERE idUsuario = @id", con);
                    com.Parameters.AddWithValue("@id", id);
                    SqlDataReader dr = com.ExecuteReader();

                    if (dr.Read())
                    {
                        txtID.Text = dr.GetInt32(0).ToString();
                        txtNombre.Text = dr.GetString(1);
                        txtContra.Text = dr.GetString(2);
                        txtTipoU.Text = dr.GetInt32(3).ToString();

                        string rutaImagen = Application.StartupPath + "\\Usuarios" + dr.GetString(4);
                        if (File.Exists(rutaImagen))
                        {
                            picUsuario.Image = System.Drawing.Image.FromFile(rutaImagen);
                        }
                        else
                        {
                            picUsuario.Image = null;
                        }
                    }
                    dr.Close();

                    // Cargar horarios en dgvEMPLE desde RegistroHorario
                    SqlCommand horarioCmd = new SqlCommand(@"
                SELECT Fecha, HoraEntrada, HoraSalida 
                FROM RegistroHorario 
                WHERE IdUsuario = @id 
                ORDER BY Fecha DESC", con);

                    horarioCmd.Parameters.AddWithValue("@id", id);

                    SqlDataAdapter da = new SqlDataAdapter(horarioCmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvEMPLE.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void poneTextoP(int id)
        {
            SqlConnection con = Conexion.cadena();
            if (con != null)
            {
                try
                {
                    SqlCommand com = new SqlCommand("SELECT * FROM tbProd WHERE IdProducto = @id", con);
                    com.Parameters.AddWithValue("@id", id);

                    SqlDataReader dr = com.ExecuteReader();
                    if (dr.Read())
                    {
                        txtID_P.Text = dr.GetInt32(0).ToString();     // IdProducto
                        txtNombreP.Text = dr.GetString(1);           // NombreProd
                        txtCostoP.Text = dr.GetInt32(2).ToString();  // Costo
                        txtCantP.Text = dr.GetInt32(3).ToString();   // Cantidad

                        string rutaImagen = Application.StartupPath + "\\Productos" + dr.GetString(4);
                        if (File.Exists(rutaImagen))
                        {
                            picImgProd.Image = System.Drawing.Image.FromFile(rutaImagen);
                        }
                        else
                        {
                            picImgProd.Image = null;
                        }
                    }
                    dr.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar producto: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Error en la conexión", "Intentar de nuevo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnFoto_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    ruta = openFileDialog1.FileName;
                    picUsuario.Image = System.Drawing.Image.FromFile(ruta);
                    picUsuario.Image.Save(Application.StartupPath + ("\\Usuarios\\") + txtNombre.Text + ".jpg", ImageFormat.Jpeg);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.None);
            }
        }

        private void btnCamaraON_Click(object sender, EventArgs e)
        {

        }

        private void video_NuevoFrameCapturando(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap Imagen = (Bitmap)eventArgs.Frame.Clone();
            picCamara.Image = Imagen;
        }

        public void TerminarFuenteDeVideo()
        {
            if (!(FuenteDeVideo == null))
            {
                try
                {

                    if (FuenteDeVideo.IsRunning)
                    {
                        FuenteDeVideo.SignalToStop();
                        FuenteDeVideo.Stop();
                        FuenteDeVideo = null;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.None);
                }
            }
        }

        private void btnTomarFoto_Click(object sender, EventArgs e)
        {
            try
            {
                if (!(FuenteDeVideo == null))
                {
                    if (FuenteDeVideo.IsRunning)
                    {
                        picUsuario.Image = picCamara.Image;
                        picUsuario.Image.Save(Application.StartupPath + ("\\Usuarios\\") + txtNombre.Text + ".jpg", ImageFormat.Jpeg);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.None);
            }
        }

        private void btnAltaUsuario_Click(object sender, EventArgs e)
        {
            TerminarFuenteDeVideo();
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
            string.IsNullOrWhiteSpace(txtContra.Text) ||
            string.IsNullOrWhiteSpace(txtTipoU.Text))
            {
                MessageBox.Show("Todos los campos son obligatorios", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int idUsuarioResponsable = Convert.ToInt32(lbIDUSUARIO.Text);
            SqlConnection con = Conexion.cadena();
            if (con != null)
            {
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = "spAltaUsuarios";
                com.Parameters.Clear();
                try
                {
                    ruta = Application.StartupPath + @"\Usuarios\" + txtNombre.Text + ".jpg";
                    path = Path.GetFileName(ruta);

                    com.Parameters.AddWithValue("@idResponsable", idUsuarioResponsable);
                    com.Parameters.AddWithValue("@nombreUsuario", txtNombre.Text);
                    com.Parameters.AddWithValue("@contraUsuario", txtContra.Text);
                    com.Parameters.AddWithValue("@tipoUsuario", txtTipoU.Text);
                    com.Parameters.AddWithValue("@Foto", SqlDbType.Text).Value = "\\" + path;

                    int a = com.ExecuteNonQuery();
                    con.Close();

                    if (a > 0)
                    {
                        MessageBox.Show("Usuario dado de alta exitosamente", "Proceso Completado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargaDatosDGV();
                    }
                    else
                    {
                        MessageBox.Show("El usuario ya existe en la base de datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Question);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Question);
                }
            }
            else
            {
                MessageBox.Show("Error al intentar conectar con la base de datos", "Intenta nuevamente", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnBaja_Click(object sender, EventArgs e)
        {
            try
            {
                TerminarFuenteDeVideo();
                int idUsuarioResponsable;
                idUsuarioResponsable = Convert.ToInt32(lbIDUSUARIO.Text);
                int idBaja;
                idBaja = Convert.ToInt32(txtID.Text);

                SqlConnection con = Conexion.cadena();
                if (con != null)
                {
                    SqlCommand com = new SqlCommand();
                    com.Connection = con;
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "spEliminaUsuario";

                    com.Parameters.Clear();
                    try
                    {
                        com.Parameters.AddWithValue("@idResponsable", idUsuarioResponsable);
                        com.Parameters.AddWithValue("@idUsuarioBaja", idBaja);
                        int renglones = com.ExecuteNonQuery();
                        con.Close();
                        if (renglones > 0)
                        {
                            MessageBox.Show("Usuario dado de baja exitosamente", "Proceso Completado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CargaDatosDGV();
                        }
                        else
                        {
                            MessageBox.Show("El usuario que pretendes dar de baja no existe en la base de datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Question);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.None);
                    }
                }
                else
                {
                    MessageBox.Show("Error al abrir la base de datos", "Vuelva a intentarlo más tarde");
                }
            }
            catch
            {
                MessageBox.Show("El valor ingresado no está en el formato correcto", "Vuelva a intentarlo más tarde");
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                TerminarFuenteDeVideo();
                if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
            string.IsNullOrWhiteSpace(txtContra.Text) ||
            string.IsNullOrWhiteSpace(txtTipoU.Text))
                {
                    MessageBox.Show("Todos los campos son obligatorios", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                int idUsuario;
                idUsuario = Convert.ToInt32(txtID.Text);
                string NombreUsuario;
                NombreUsuario = txtNombre.Text;
                string ContraUsuario;
                ContraUsuario = txtContra.Text;
                int TipoUsuario;
                TipoUsuario = Convert.ToInt32(txtTipoU.Text);
                string Foto = ""; // Aquí puedes agregar lógica para obtener la imagen

                SqlConnection con = Conexion.cadena();
                if (con != null)
                {
                    SqlCommand com = new SqlCommand();
                    com.Connection = con;
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "spEditarUser";

                    com.Parameters.Clear();
                    try
                    {
                        ruta = Application.StartupPath + @"\Usuarios\" + txtNombre.Text + ".jpg";
                        path = Path.GetFileName(ruta);

                        com.Parameters.AddWithValue("@idUsuario", idUsuario);
                        com.Parameters.AddWithValue("@NombreUsuario", NombreUsuario);
                        com.Parameters.AddWithValue("@ContraUsuario", ContraUsuario);
                        com.Parameters.AddWithValue("@Tipo", TipoUsuario);
                        com.Parameters.AddWithValue("@Foto", SqlDbType.Text).Value = "\\" + path;

                        int filasAfectadas = com.ExecuteNonQuery();
                        con.Close();

                        if (filasAfectadas > 0)
                        {
                            MessageBox.Show("Usuario actualizado exitosamente", "Proceso Completado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CargaDatosDGV();
                        }
                        else
                        {
                            MessageBox.Show("El usuario que pretendes actualizar no existe en la base de datos");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.None);
                    }
                }
                else
                {
                    MessageBox.Show("Error al abrir la base de datos", "Vuelva a intentarlo más tarde");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.None);
            }
        }
        private void AbrirFormularioNuevo()
        {
            string nombreUsuario = txtNombre.Text; // Capturar el texto del TextBox en el formulario actual
            wfTomarFoto nuevoFormulario = new wfTomarFoto(nombreUsuario, this);
            nuevoFormulario.Show();
        }
        private void AbrirMenu()
        {
            // Establecer el tamaño fijo
            this.Width = 1287;
            this.Height = 822;

            // Obtener el tamaño de la pantalla
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;

            // Calcular la posición centrada
            int posX = (screenWidth - this.Width) / 2;
            int posY = (screenHeight - this.Height) / 2;

            // Asignar la posición centrada manualmente
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(posX, posY);

            // Cambiar al tab del menú
            tbcAdmin.SelectedTab = tabMenu;
        }
        private void AbrirGestionProd()
        {
            // Establecer el tamaño fijo
            this.Width = 780;
            this.Height = 712;

            // Obtener el tamaño de la pantalla
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;

            // Calcular la posición centrada
            int posX = (screenWidth - this.Width) / 2;
            int posY = (screenHeight - this.Height) / 2;

            // Asignar la posición centrada manualmente
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(posX, posY);

            // Cambiar al tab del menú
            tbcAdmin.SelectedTab = tabG_Prod;

        }
        private void AbrirGestionUsers()
        {
            this.Width = 777;
            this.Height = alturaContraida;

            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;

            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(
                (screenWidth - this.Width) / 2,
                (screenHeight - this.Height) / 2
            );

            tbcAdmin.SelectedTab = tabUsuarios;

        }
        private void AbrirReportes()
        {
            this.Width = 771;
            this.Height = 697;
            // Obtener el tamaño de la pantalla
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;

            // Calcular la posición centrada
            int posX = (screenWidth - this.Width) / 2;
            int posY = (screenHeight - this.Height) / 2;

            // Asignar la posición centrada manualmente
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(posX, posY);

            // Cambiar al tab del menú
            tbcAdmin.SelectedTab = tabReport;

        }
        private void AbrirADMIN()
        {
            this.Width = 890;
            this.Height = 460;


            // Obtener el tamaño de la pantalla
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;

            // Calcular la posición centrada
            int posX = (screenWidth - this.Width) / 2;
            int posY = (screenHeight - this.Height) / 2;

            // Asignar la posición centrada manualmente
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(posX, posY);

            // Cambiar al tab del menú
            tbcAdmin.SelectedTab = tabADMIN;

        }
        private void btnAgregaImg_Click(object sender, EventArgs e)
        {
            AbrirFormularioNuevo();
        }

        private void btnRegistra_Click(object sender, EventArgs e)
        {
            expandiendo = !expandiendo; // Cambiar entre expandir y contraer
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;

            if (expandiendo)
            {
                if (this.Height < alturaExpandida)
                {
                    this.Height += velocidadAnimacion;
                    // Centrar en cada paso
                    this.Location = new Point(
                        (screenWidth - this.Width) / 2,
                        (screenHeight - this.Height) / 2
                    );
                }
                else
                {
                    timer1.Stop();
                    this.Height = alturaExpandida;
                    this.Location = new Point(
                        (screenWidth - this.Width) / 2,
                        (screenHeight - this.Height) / 2
                    );
                }
            }
            else
            {
                if (this.Height > alturaContraida)
                {
                    this.Height -= velocidadAnimacion;
                    this.Location = new Point(
                        (screenWidth - this.Width) / 2,
                        (screenHeight - this.Height) / 2
                    );
                }
                else
                {
                    timer1.Stop();
                    this.Height = alturaContraida;
                    this.Location = new Point(
                        (screenWidth - this.Width) / 2,
                        (screenHeight - this.Height) / 2
                    );
                }
            }
        }
        // Método público para actualizar la imagen del PictureBox
        public void ActualizarImagenUsuario(System.Drawing.Image imagen)
        {
            picUsuario.Image = imagen;
        }
        private void RemoveProducto()
        {
            if (lbProductos.SelectedItem != null)
            {
                string selectedItem = lbProductos.SelectedItem.ToString().Trim();

                // Evitar borrar cabecera o separadores
                if (selectedItem.StartsWith("Cant.") || selectedItem.StartsWith("---"))
                    return;

                // Separar por espacios, ignorando múltiples
                string[] partes = selectedItem.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (partes.Length >= 3)
                {
                    // La primera es la cantidad, la última es el precio, el resto es el nombre
                    string nombreProducto = string.Join(" ", partes.Skip(1).Take(partes.Length - 2));

                    if (productoCounts.ContainsKey(nombreProducto))
                    {
                        productoCounts[nombreProducto]--;
                        if (productoCounts[nombreProducto] <= 0)
                        {
                            productoCounts.Remove(nombreProducto);
                        }

                        UpdateListBox();
                        UpdateTotal();
                        UpdateItemCount();
                    }
                }
            }
        }
        private void UpdateListBox()
        {
            lbProductos.Items.Clear();

            // Encabezado más ajustado
            string encabezado = string.Format("{0,-6} {1,-13} {2,8}", "Cant.", "Producto", "Precio");
            lbProductos.Items.Add(encabezado);
            lbProductos.Items.Add(new string('-', 32)); // línea separadora

            foreach (var prod in productoCounts)
            {
                string nombre = prod.Key;
                int cantidad = prod.Value;
                int precioUnitario = productoPrecios.ContainsKey(nombre) ? productoPrecios[nombre] : 0;
                int subtotal = cantidad * precioUnitario;

                // Alineación compacta
                string fila = string.Format("{0,-6} {1,-13} {2,8:C0}", cantidad, nombre.ToUpper(), subtotal);
                lbProductos.Items.Add(fila);
            }
            lbVenta.Items.Clear();

            // Encabezado como en lbProductos
            string encabezado2 = string.Format("{0,-6} {1,-13} {2,8}", "Cant.", "Producto", "Precio");
            lbVenta.Items.Add(encabezado2);
            lbVenta.Items.Add(new string('-', 32));

            foreach (var prod in productoCounts)
            {
                string nombre = prod.Key;
                int cantidad = prod.Value;
                int precioUnitario = productoPrecios.ContainsKey(nombre) ? productoPrecios[nombre] : 0;
                int subtotal = cantidad * precioUnitario;

                string fila = string.Format("{0,-6} {1,-13} {2,8:C0}", cantidad, nombre.ToUpper(), subtotal);
                lbVenta.Items.Add(fila);
            }
        }
        private void UpdateItemCount()
        {
            int totalItems = 0;
            foreach (var prod in productoCounts)
            {
                totalItems += prod.Value;
            }

            lbItems.Text = $"{totalItems}";
        }
        private void UpdateTotal()
        {
            decimal total = 0;

            foreach (var prod in productoCounts)
            {
                if (productoPrecios.ContainsKey(prod.Key))
                {
                    total += prod.Value * productoPrecios[prod.Key];
                }
            }

            lblTotalVenta.Text = total.ToString("C");
        }





        private void AgregarBotonDinamico(string nombre, int costo, int posX, int posY, string rutaImagen)
        {
            Button btn = new Button();

            // Tamaño más grande
            btn.Width = 200;
            btn.Height = 200;

            // Solo mostrar nombre del producto
            btn.Text = nombre;

            // Posición (si la manejas manualmente)
            btn.Location = new Point(posX, posY);

            // Estilo visual
            btn.FlatStyle = FlatStyle.Standard;
            btn.BackgroundImage = Properties.Resources.bgAurora;
            btn.TextAlign = ContentAlignment.BottomCenter;
            btn.TextImageRelation = TextImageRelation.ImageAboveText;

            // Fuente personalizada
            btn.Font = new System.Drawing.Font("Zrnic", 14, FontStyle.Bold); // Cambia la fuente si gustas

            // Cargar imagen si existe
            if (File.Exists(rutaImagen))
            {
                System.Drawing.Image img = System.Drawing.Image.FromFile(rutaImagen);
                btn.Image = new Bitmap(img, new Size(120, 120)); // Imagen más grande
            }

            // Evento Click
            btn.Click += (s, e) =>
            {
                // Agregar precio al diccionario si no existe
                if (!productoPrecios.ContainsKey(nombre))
                {
                    productoPrecios.Add(nombre, costo);
                }

                // Incrementar contador del producto
                if (!productoCounts.ContainsKey(nombre))
                {
                    productoCounts[nombre] = 0;
                }
                productoCounts[nombre]++;

                // Actualizar interfaz
                UpdateListBox();
                UpdateTotal();
                UpdateItemCount();
            };

            flpProductos.Controls.Add(btn);
        }


        public void CargarBotonesDesdeBD()
        {
            flpProductos.Controls.Clear(); // Limpiar los botones existentes

            using (SqlConnection con = Conexion.cadena())
            {
                try
                {

                    using (SqlCommand cmd = new SqlCommand("SELECT NombreProducto, Costo, PosX, PosY, Imagen FROM tbBotonesProductos", con))

                    {
                        cmd.Connection = con; // ← Agregado para asegurar conexión

                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            string nombre = reader["NombreProducto"].ToString();
                            int costo = Convert.ToInt32(reader["Costo"]);
                            int posX = Convert.ToInt32(reader["PosX"]);
                            int posY = Convert.ToInt32(reader["PosY"]);

                            string rutaImagen = reader["Imagen"].ToString();
                            AgregarBotonDinamico(nombre, costo, posX, posY, rutaImagen);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar los botones: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void btnAltaProducto_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombreP.Text) ||
       string.IsNullOrWhiteSpace(txtCostoP.Text) ||
       string.IsNullOrWhiteSpace(txtCantP.Text) ||
       cbCat.SelectedIndex == -1)
            {
                MessageBox.Show("Todos los campos son obligatorios", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtCostoP.Text, out int costo) || !int.TryParse(txtCantP.Text, out int cantidad))
            {
                MessageBox.Show("El costo y la cantidad deben ser valores numéricos", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string categoria = cbCat.SelectedItem.ToString();
            string nombreArchivo = txtNombreP.Text + ".jpg";
            string rutaDirectorio = Path.Combine(Application.StartupPath, "Productos");
            Directory.CreateDirectory(rutaDirectorio);
            string rutaFinal = Path.Combine(rutaDirectorio, nombreArchivo);

            try
            {
                if (picImgProd.Image != null)
                {
                    picImgProd.Image.Save(rutaFinal, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                else
                {
                    MessageBox.Show("Por favor seleccione una imagen antes de añadir el producto.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SqlConnection con = Conexion.cadena())
                {
                    using (SqlCommand com = new SqlCommand("spAltaProducto", con))
                    {
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@NombreProd", txtNombreP.Text);
                        com.Parameters.AddWithValue("@Costo", costo);
                        com.Parameters.AddWithValue("@Cantidad", cantidad);
                        com.Parameters.AddWithValue("@Imagen", rutaFinal);
                        com.Parameters.AddWithValue("@Categoria", categoria); // Nuevo parámetro

                        int resultado = com.ExecuteNonQuery();
                        con.Close();

                        if (resultado > 0)
                        {
                            CargarBotonesDesdeBD();
                            CargaDatosDGVP();
                            MessageBox.Show("Producto agregado exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("El producto ya existe en la base de datos", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar imagen o insertar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvProductos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            picUsuario.Image = System.Drawing.Image.FromFile(Application.StartupPath + "\\Usuarios\\user.jpg");
            try
            {
                int id = (int)dgvProductos.CurrentRow.Cells["IdProducto"].Value;
                poneTextoP(id);
            }
            catch (Exception)
            {
                return;
            }
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {

        }

        private void EliminarBotonDelPanel(string nombreProducto)
        {
            foreach (Control control in flpProductos.Controls)
            {
                if (control is Button btn && btn.Text.StartsWith(nombreProducto))
                {
                    flpProductos.Controls.Remove(btn);
                    btn.Dispose();
                    break; // Salimos del bucle después de eliminar el botón
                }
            }
        }

        private void btnElimina_P_Click(object sender, EventArgs e)
        {
            // Verificar que hay una fila seleccionada en el DataGridView
            if (dgvProductos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un producto para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Obtener el ID del producto seleccionado
            int idProducto = Convert.ToInt32(dgvProductos.SelectedRows[0].Cells["IdProducto"].Value);
            string nombreProducto = dgvProductos.SelectedRows[0].Cells["NombreProd"].Value.ToString();

            // Confirmación de eliminación
            DialogResult result = MessageBox.Show($"¿Está seguro de eliminar el producto {nombreProducto}?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.No)
                return;

            // Conectar a la base de datos y eliminar el producto
            using (SqlConnection con = Conexion.cadena())
            {
                try
                {
                    using (SqlCommand com = new SqlCommand("spEliminaProducto", con))
                    {
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@IdProducto", idProducto);

                        int filasAfectadas = com.ExecuteNonQuery();

                        if (filasAfectadas > 0)
                        {
                            MessageBox.Show("Producto eliminado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CargaDatosDGVP();
                            EliminarBotonDelPanel(nombreProducto); // Eliminar el botón correspondiente en el panel
                        }
                        else
                        {
                            MessageBox.Show("El producto no se encontró en la base de datos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void icbMenu_Click(object sender, EventArgs e)
        {
            AbrirADMIN();
        }

        private void icbG_Prod_Click(object sender, EventArgs e)
        {
            this.Height = alturaExpandida;
            this.Height += velocidadAnimacion;
            AbrirGestionProd();
        }

        private void icbG_Users_Click(object sender, EventArgs e)
        {
            this.Height = alturaExpandida;
            this.Height += velocidadAnimacion;
            AbrirGestionUsers();
        }

        private bool imagenSeleccionada = false;
        private void btnActualizarP_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombreP.Text) ||
        string.IsNullOrWhiteSpace(txtCostoP.Text) ||
        string.IsNullOrWhiteSpace(txtCantP.Text) ||
        cbCat.SelectedIndex == -1)
            {
                MessageBox.Show("Todos los campos son obligatorios", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idProducto = Convert.ToInt32(txtID_P.Text);
            string nombreProducto = txtNombreP.Text.Trim();
            string categoria = cbCat.SelectedItem.ToString();

            if (!int.TryParse(txtCostoP.Text, out int costo) || !int.TryParse(txtCantP.Text, out int cantidad))
            {
                MessageBox.Show("El costo y la cantidad deben ser valores numéricos.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult result = MessageBox.Show($"¿Desea actualizar el producto {nombreProducto}?", "Confirmar Actualización", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
                return;

            string carpetaImagenes = Path.Combine(Application.StartupPath, "Productos");
            Directory.CreateDirectory(carpetaImagenes);
            string rutaImagen = Path.Combine(carpetaImagenes, nombreProducto + ".jpg");

            if (imagenSeleccionada && picImgProd.Image != null)
            {
                picImgProd.Image.Save(rutaImagen, ImageFormat.Jpeg);
            }

            using (SqlConnection con = Conexion.cadena())
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("spEditarProducto", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdProducto", idProducto);
                    cmd.Parameters.AddWithValue("@NombreProd", nombreProducto);
                    cmd.Parameters.AddWithValue("@Costo", costo);
                    cmd.Parameters.AddWithValue("@Cantidad", cantidad);
                    cmd.Parameters.AddWithValue("@Imagen", rutaImagen);
                    cmd.Parameters.AddWithValue("@Categoria", categoria); // Nuevo parámetro

                    cmd.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("Producto actualizado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarBotonesDesdeBD();
                    CargaDatosDGVP();
                    imagenSeleccionada = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void icbEliminar_P_Click(object sender, EventArgs e)
        {
            RemoveProducto();
        }
        private int CalcularAltoTicket()
        {
            int lineCount = ticketContent.Split('\n').Length;
            int logoExtra = 120; // espacio para logo + margen
            int lineHeight = 20;
            return (lineCount * lineHeight) + logoExtra + 60; // + espacio para corte
        }
        public static byte[] GetImageEscPosData(Bitmap bitmap)
        {
            int width = bitmap.Width;
            int height = bitmap.Height;
            MemoryStream ms = new MemoryStream();

            for (int y = 0; y < height; y += 24)
            {
                ms.WriteByte(0x1B); ms.WriteByte((byte)'*'); ms.WriteByte(33);
                ms.WriteByte((byte)(width % 256));
                ms.WriteByte((byte)(width / 256));

                for (int x = 0; x < width; x++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        byte slice = 0;
                        for (int b = 0; b < 8; b++)
                        {
                            int yy = y + (k * 8) + b;
                            if (yy >= height) continue;

                            Color pixel = bitmap.GetPixel(x, yy);
                            bool isBlack = (pixel.R + pixel.G + pixel.B) < 384;
                            slice |= (byte)((isBlack ? 1 : 0) << (7 - b));
                        }
                        ms.WriteByte(slice);
                    }
                }
            }

            return ms.ToArray();
        }

        private byte[] CentrarImagenEscPos(Bitmap imagen, int anchoPapel)
        {
            int anchoImagen = imagen.Width;
            int margenIzquierdo = (anchoPapel - anchoImagen) / 2;

            if (margenIzquierdo < 0) margenIzquierdo = 0;

            // GS L nL nH  → margen izquierdo
            byte[] margen = new byte[]
            {
        0x1D, 0x4C,
        (byte)(margenIzquierdo & 0xFF),
        (byte)((margenIzquierdo >> 8) & 0xFF)
            };

            byte[] imgBytes = GetImageEscPosData(imagen);

            byte[] resultado = new byte[margen.Length + imgBytes.Length];
            Buffer.BlockCopy(margen, 0, resultado, 0, margen.Length);
            Buffer.BlockCopy(imgBytes, 0, resultado, margen.Length, imgBytes.Length);

            return resultado;
        }


        public Bitmap ConvertirBmpABlancoYNegro(string rutaBmp)
        {
            Bitmap original = new Bitmap(rutaBmp);
            Bitmap bn = new Bitmap(original.Width, original.Height);

            for (int y = 0; y < original.Height; y++)
            {
                for (int x = 0; x < original.Width; x++)
                {
                    Color pixel = original.GetPixel(x, y);
                    int gris = (pixel.R + pixel.G + pixel.B) / 3;
                    bn.SetPixel(x, y, gris < 140 ? Color.Black : Color.White);
                }
            }

            return bn;
        }

        private void Ticket(int idVenta, string nombreUsuario, List<string> productos, int totalItems, decimal totalVenta, decimal montoPago, decimal cambio, string tipoPago)
        {
            string printerName = "MR-LT58 Printer";
            Bitmap logo = ConvertirBmpABlancoYNegro("logoPOS.bmp");
            byte[] logoBytes = GetImageEscPosData(logo);
          
            byte[] logoCentrado = CentrarImagenEscPos(logo, 384);

            StringBuilder sb = new StringBuilder();
            sb.Append("\x1B\x40"); // Inicializar
            sb.Append("\x1B\x61\x01"); // Centrar

            // Encabezado
            sb.Append("SA DE C.V\n");
            sb.Append("Av.Tecnologico #1555-Km. 14.5,\n");
            sb.Append("Placido Domingo, 35150.\n");
            sb.Append("Lerdo,Durango\n");

            sb.Append("******************************\n");
            sb.Append("FECHA: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "\n");
            sb.Append("FOLIO: " + idVenta.ToString() + "\n");
            sb.Append("CON GUSTO LE ATENDIO: " + nombreUsuario + "\n");
            sb.Append("******************************\n");

            foreach (var item in productos)
                sb.Append(item + "\n");

            sb.Append("--------------------------------\n");
            sb.Append("ITEMS: " + totalItems.ToString().PadRight(5) + "\n");
            sb.Append("PAGO: " + tipoPago.ToUpper() + "\n");
            sb.Append("TOTAL: ".PadRight(14) + totalVenta.ToString("C").PadRight(6) + "\n");
            sb.Append("MONTO PAGO: ".PadRight(14) + montoPago.ToString("C").PadRight(5) + "\n");
            sb.Append("CAMBIO: ".PadRight(14) + cambio.ToString("C").PadRight(5) + "\n");

            sb.Append("\nGRACIAS POR SU COMPRA!\n");
            sb.Append("ESTE NO ES UN COMPROBANTE FISCAL\n");
            sb.Append("\"MR-L posystem v1.0\"\n\n\n");
            sb.Append("\n");

            byte[] textoBytes = Encoding.ASCII.GetBytes(sb.ToString());
            byte[] cortar = new byte[] { 0x1D, 0x56, 0x01 };

            byte[] finalData = new byte[logoBytes.Length + textoBytes.Length + cortar.Length];
            int offset = 0;
            Buffer.BlockCopy(logoBytes, 0, finalData, offset, logoBytes.Length);
            offset += logoBytes.Length;
            Buffer.BlockCopy(textoBytes, 0, finalData, offset, textoBytes.Length);
            offset += textoBytes.Length;
            Buffer.BlockCopy(cortar, 0, finalData, offset, cortar.Length);

            RawPrinterHelper.SendBytesToPrinter(printerName, finalData);
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            System.Drawing.Font fuente = new System.Drawing.Font("Consolas", 10);
            float y = 10;
            int pageWidth = e.PageBounds.Width;

            // Cargar logo y centrarlo
            string logoPath = "logoPOS.bmp";
            if (System.IO.File.Exists(logoPath))
            {
                System.Drawing.Image logo = System.Drawing.Image.FromFile(logoPath);
                int logoWidth = 100;
                int logoHeight = 100;
                int centerX = (pageWidth - logoWidth) / 2;
                e.Graphics.DrawImage(logo, new System.Drawing.Rectangle(centerX, (int)y, logoWidth, logoHeight));
                y += logoHeight + 10;
            }

            // Imprimir el texto, centrado línea por línea
            string[] lineas = ticketContent.Split('\n');
            foreach (string linea in lineas)
            {
                SizeF textoSize = e.Graphics.MeasureString(linea.TrimEnd(), fuente);
                float x = (pageWidth - textoSize.Width) / 2;
                e.Graphics.DrawString(linea.TrimEnd(), fuente, Brushes.Black, new PointF(x, y));
                y += fuente.GetHeight();
            }

            e.HasMorePages = false;
        }

        private void icbOFF_Click(object sender, EventArgs e)
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

        private void icbGUsers_Click(object sender, EventArgs e)
        {

            AbrirGestionUsers();
        }

        private void icbProd_Click(object sender, EventArgs e)
        {

            AbrirGestionProd();
        }

        private void icbM_Click(object sender, EventArgs e)
        {

            AbrirMenu();
        }

        private void icbMenu2_Click(object sender, EventArgs e)
        {
            // Centrar manualmente el formulario
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;

            int formWidth = this.Width;
            int formHeight = this.Height;

            this.Location = new Point(
                (screenWidth - formWidth) / 2,
                (screenHeight - formHeight) / 2
            );
            AbrirADMIN();
        }

        private void icbMenu_Rp_Click(object sender, EventArgs e)
        {

        }

        private void icbReportes_Click(object sender, EventArgs e)
        {
            // Centrar manualmente el formulario
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;

            int formWidth = this.Width;
            int formHeight = this.Height;

            this.Location = new Point(
                (screenWidth - formWidth) / 2,
                (screenHeight - formHeight) / 2
            );
            AbrirReportes();
            CargarVentasEnReporte();
        }

        private void icbBackupBD_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {

                try
                {
                    // Ruta fija donde se guardará el backup
                    string carpeta = @"C:\RespaldoTBDBTienda\";

                    // Asegurarse de que la carpeta exista
                    if (!Directory.Exists(carpeta))
                        Directory.CreateDirectory(carpeta);

                    // Generar nombre único para el archivo
                    string nombreArchivo = "TBDBTienda_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".bak";
                    string rutaCompleta = Path.Combine(carpeta, nombreArchivo);

                    using (SqlConnection con = Conexion.cadena())
                    {

                        using (SqlCommand comando = new SqlCommand("sp_RespaldarBaseDeDatos", con))
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.Parameters.AddWithValue("@ruta", rutaCompleta);
                            comando.ExecuteNonQuery();

                            MessageBox.Show("Respaldo realizado con éxito en:\n" + rutaCompleta, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            con.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al realizar el respaldo:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }


        private void CalcularTotales()
        {
            try
            {
                decimal total = 0;
                int totalArticulos = 0;

                foreach (var prod in productoCounts)
                {
                    int cantidad = prod.Value;
                    int precioUnitario = productoPrecios.ContainsKey(prod.Key) ? productoPrecios[prod.Key] : 0;

                    total += cantidad * precioUnitario;
                    totalArticulos += cantidad;
                }

                lbItemsConfirm.Text = totalArticulos.ToString();
                lblMontoVenta.Text = "$" + total.ToString("N2");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al calcular totales: " + ex.Message);
            }
        }


        private void txtMontoPago_TextChanged(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtMontoPago.Text, out decimal montoPago))
            {
                lblMontoPago.Text = "$" + montoPago.ToString("N2");

                if (decimal.TryParse(lblMontoVenta.Text.Replace("$", ""), out decimal montoVenta))
                {
                    decimal cambio = montoPago - montoVenta;
                    lblCambio.Text = "$" + cambio.ToString("N2");
                }
            }
        }

        private void RegistrarVenta()
        {
            decimal total = decimal.Parse(lblMontoVenta.Text.Replace("$", ""));
            decimal pago = decimal.Parse(lblMontoPago.Text.Replace("$", ""));
            decimal cambio = decimal.Parse(lblCambio.Text.Replace("$", ""));

            if (!int.TryParse(lbIDUser_Menu.Text, out int idUsuarioActual))
            {
                MessageBox.Show("ID de usuario inválido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SqlConnection con = Conexion.cadena())
            {
                try
                {
                    if (con.State != ConnectionState.Open)
                        con.Open(); // Solo si no está abierta

                    SqlTransaction trans = con.BeginTransaction();

                    SqlCommand cmdVenta = new SqlCommand(
                        @"INSERT INTO tbVentas(FechaVenta, MontoTotal, MontoPagado, Cambio, idUsuario) 
                  VALUES(GETDATE(), @total, @pago, @cambio, @idUsuario); 
                  SELECT SCOPE_IDENTITY();", con, trans);
                    cmdVenta.CommandTimeout = 10;
                    cmdVenta.Parameters.AddWithValue("@total", total);
                    cmdVenta.Parameters.AddWithValue("@pago", pago);
                    cmdVenta.Parameters.AddWithValue("@cambio", cambio);
                    cmdVenta.Parameters.AddWithValue("@idUsuario", idUsuarioActual);

                    int idVenta = Convert.ToInt32(cmdVenta.ExecuteScalar());

                    Dictionary<string, int> productosAgrupados = new Dictionary<string, int>();

                    foreach (string item in lbVenta.Items)
                    {
                        string[] partes = item.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (partes.Length >= 3 && int.TryParse(partes[0], out int cantidad))
                        {
                            string nombreProducto = string.Join(" ", partes.Skip(1).Take(partes.Length - 2)).Trim();
                            if (productosAgrupados.ContainsKey(nombreProducto))
                                productosAgrupados[nombreProducto] += cantidad;
                            else
                                productosAgrupados[nombreProducto] = cantidad;
                        }
                    }

                    foreach (var kvp in productosAgrupados)
                    {
                        string nombre = kvp.Key;
                        int cantidad = kvp.Value;

                        SqlCommand cmdPrecio = new SqlCommand("SELECT Costo FROM tbProd WHERE NombreProd = @nombre", con, trans);
                        cmdPrecio.CommandTimeout = 10;
                        cmdPrecio.Parameters.AddWithValue("@nombre", nombre);
                        decimal precio = Convert.ToDecimal(cmdPrecio.ExecuteScalar());

                        SqlCommand cmdDetalle = new SqlCommand(
                            "INSERT INTO DetVentas(IdVenta, NombreProducto, Cantidad, PrecioUnitario) VALUES(@idVenta, @nombre, @cantidad, @precio)", con, trans);
                        cmdDetalle.CommandTimeout = 10;
                        cmdDetalle.Parameters.AddWithValue("@idVenta", idVenta);
                        cmdDetalle.Parameters.AddWithValue("@nombre", nombre);
                        cmdDetalle.Parameters.AddWithValue("@cantidad", cantidad);
                        cmdDetalle.Parameters.AddWithValue("@precio", precio);
                        cmdDetalle.ExecuteNonQuery();

                        SqlCommand cmdExistencia = new SqlCommand(
                            @"UPDATE tbProd 
                      SET Cantidad = Cantidad - @cantidad 
                      WHERE NombreProd = @nombre AND Cantidad >= @cantidad", con, trans);
                        cmdExistencia.CommandTimeout = 10;
                        cmdExistencia.Parameters.AddWithValue("@cantidad", cantidad);
                        cmdExistencia.Parameters.AddWithValue("@nombre", nombre);
                        int filasAfectadas = cmdExistencia.ExecuteNonQuery();

                        if (filasAfectadas == 0)
                        {
                            trans.Rollback();
                            MessageBox.Show($"¡No hay suficiente existencia del producto \"{nombre}\"!", "Sin stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        SqlCommand cmdValidar = new SqlCommand("SELECT Cantidad FROM tbProd WHERE NombreProd = @nombre", con, trans);
                        cmdValidar.CommandTimeout = 10;
                        cmdValidar.Parameters.AddWithValue("@nombre", nombre);
                        int cantidadRestante = Convert.ToInt32(cmdValidar.ExecuteScalar());

                        if (cantidadRestante == 0)
                            MessageBox.Show($"⚠ El producto \"{nombre}\" se ha agotado. ¡No hay más en existencia!", "Producto agotado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    trans.Commit();
                    List<string> productosTicket = new List<string>();
                    foreach (var item in lbProductos.Items)
                        productosTicket.Add(item.ToString());

                    this.idVenta = idVenta; // opcional si lo usas en otro lado

                    Ticket
                    (
                        idVenta,
                        lbNOMBRE.Text,
                        productosTicket,
                        int.Parse(lbItems.Text),
                        decimal.Parse(lblTotalVenta.Text.Replace("$", "")),
                        decimal.Parse(lblMontoPago.Text.Replace("$", "")),
                        decimal.Parse(lblCambio.Text.Replace("$", "")),
                       tipoPagoSeleccionado // ✅ aquí pasas el valor actual
                    );

                    MessageBox.Show("¡Venta registrada con éxito!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Refrescar vista
                    CargaDatosDGVP();
                    CargarVentasEnReporte();
                    tbcAdmin.SelectedTab = tabMenu;
                    this.Width = 1287;
                    this.Height = 822;
                    // Centrar manualmente el formulario
                    int screenWidth = Screen.PrimaryScreen.Bounds.Width;
                    int screenHeight = Screen.PrimaryScreen.Bounds.Height;

                    int formWidth = this.Width;
                    int formHeight = this.Height;

                    this.Location = new Point(
                        (screenWidth - formWidth) / 2,
                        (screenHeight - formHeight) / 2
                    );
                    lbProductos.Items.Clear();
                    lbVenta.Items.Clear();
                    UpdateTotal();
                    UpdateItemCount();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al registrar la venta:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void CargarVentasEnReporte()
        {
            try
            {
                using (SqlConnection con = Conexion.cadena())
                {
                    string query = @"
                SELECT 
                    MAX(v.FechaVenta) AS [Fecha de Compra],
                    d.NombreProducto AS Producto,
                    p.Categoria AS Descripción,
                    SUM(d.Cantidad) AS [Cantidad Vendida],
                    d.PrecioUnitario AS [Costo Unitario],
                    SUM(d.Cantidad * d.PrecioUnitario) AS [Venta Total],
                    u.NombreUsuario AS [Responsable de Venta]
                FROM DetVentas d
                INNER JOIN tbVentas v ON d.IdVenta = v.IdVenta
                INNER JOIN tbProd p ON d.NombreProducto = p.NombreProd
                INNER JOIN Usuarios u ON v.idUsuario = u.idUsuario
                GROUP BY 
                    d.NombreProducto, p.Categoria, d.PrecioUnitario, u.NombreUsuario
                ORDER BY [Fecha de Compra] DESC;
            ";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dgvReporte.DataSource = null;
                        dgvReporte.DataSource = dt;
                    }
                }
                CalcularTotalVentasDelDia();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar ventas: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void picVolver_Users_Click(object sender, EventArgs e)
        {
            AbrirADMIN();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void pic_RegistrarUser_Click(object sender, EventArgs e)
        {
            expandiendo = !expandiendo; // Cambiar entre expandir y contraer
            timer1.Start();
        }

        private void pic_AddImage_Click(object sender, EventArgs e)
        {
            AbrirFormularioNuevo();
        }

        private void pic_saveUser_Click(object sender, EventArgs e)
        {
            TerminarFuenteDeVideo();
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
            string.IsNullOrWhiteSpace(txtContra.Text) ||
            string.IsNullOrWhiteSpace(txtTipoU.Text))
            {
                MessageBox.Show("Todos los campos son obligatorios", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int idUsuarioResponsable = Convert.ToInt32(lbIDUSUARIO.Text);
            SqlConnection con = Conexion.cadena();
            if (con != null)
            {
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = "spAltaUsuarios";
                com.Parameters.Clear();
                try
                {
                    ruta = Application.StartupPath + @"\Usuarios\" + txtNombre.Text + ".jpg";
                    path = Path.GetFileName(ruta);

                    com.Parameters.AddWithValue("@idResponsable", idUsuarioResponsable);
                    com.Parameters.AddWithValue("@nombreUsuario", txtNombre.Text);
                    com.Parameters.AddWithValue("@contraUsuario", txtContra.Text);
                    com.Parameters.AddWithValue("@tipoUsuario", txtTipoU.Text);
                    com.Parameters.AddWithValue("@Foto", SqlDbType.Text).Value = "\\" + path;

                    int a = com.ExecuteNonQuery();
                    con.Close();

                    if (a > 0)
                    {
                        MessageBox.Show("Usuario dado de alta exitosamente", "Proceso Completado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargaDatosDGV();
                    }
                    else
                    {
                        MessageBox.Show("El usuario ya existe en la base de datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Question);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Question);
                }
            }
            else
            {
                MessageBox.Show("Error al intentar conectar con la base de datos", "Intenta nuevamente", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pic_deleteUser_Click(object sender, EventArgs e)
        {
            try
            {
                TerminarFuenteDeVideo();
                int idUsuarioResponsable;
                idUsuarioResponsable = Convert.ToInt32(lbIDUSUARIO.Text);
                int idBaja;
                idBaja = Convert.ToInt32(txtID.Text);

                SqlConnection con = Conexion.cadena();
                if (con != null)
                {
                    SqlCommand com = new SqlCommand();
                    com.Connection = con;
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "spEliminaUsuario";

                    com.Parameters.Clear();
                    try
                    {
                        com.Parameters.AddWithValue("@idResponsable", idUsuarioResponsable);
                        com.Parameters.AddWithValue("@idUsuarioBaja", idBaja);
                        int renglones = com.ExecuteNonQuery();
                        con.Close();
                        if (renglones > 0)
                        {
                            MessageBox.Show("Usuario dado de baja exitosamente", "Proceso Completado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CargaDatosDGV();
                        }
                        else
                        {
                            MessageBox.Show("El usuario que pretendes dar de baja no existe en la base de datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Question);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.None);
                    }
                }
                else
                {
                    MessageBox.Show("Error al abrir la base de datos", "Vuelva a intentarlo más tarde");
                }
            }
            catch
            {
                MessageBox.Show("El valor ingresado no está en el formato correcto", "Vuelva a intentarlo más tarde");
            }
        }

        private void pic_updateUser_Click(object sender, EventArgs e)
        {
            try
            {
                TerminarFuenteDeVideo();

                if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                    string.IsNullOrWhiteSpace(txtContra.Text) ||
                    string.IsNullOrWhiteSpace(txtTipoU.Text))
                {
                    MessageBox.Show("Todos los campos son obligatorios", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int idUsuario = Convert.ToInt32(txtID.Text);
                string nombreUsuario = txtNombre.Text;
                string contraUsuario = txtContra.Text;
                int tipoUsuario = Convert.ToInt32(txtTipoU.Text);
                string path = "";
                DateTime fecha = DateTime.Today;

                TimeSpan? horaEntrada = null;
                TimeSpan? horaSalida = null;

                // Buscar en dgvEMPLE por idUsuario y fecha actual
                foreach (DataGridViewRow row in dgvEMPLE.Rows)
                {
                    if (row.Cells["Fecha"].Value != null &&
                        row.Cells["Fecha"].Value.ToString() == fecha.ToShortDateString())
                    {
                        if (row.Cells["HoraEntrada"].Value != DBNull.Value && row.Cells["HoraEntrada"].Value != null)
                            horaEntrada = TimeSpan.Parse(row.Cells["HoraEntrada"].Value.ToString());

                        if (row.Cells["HoraSalida"].Value != DBNull.Value &&
                            row.Cells["HoraSalida"].Value != null &&
                            !string.IsNullOrWhiteSpace(row.Cells["HoraSalida"].Value.ToString()))
                            horaSalida = TimeSpan.Parse(row.Cells["HoraSalida"].Value.ToString());

                        break;
                    }
                }

                using (SqlConnection con = Conexion.cadena())
                {
                    // Actualizar usuario
                    SqlCommand com = new SqlCommand("spEditarUser", con);
                    com.CommandType = CommandType.StoredProcedure;

                    ruta = Application.StartupPath + @"\Usuarios\" + nombreUsuario + ".jpg";
                    path = Path.GetFileName(ruta);

                    com.Parameters.AddWithValue("@idUsuario", idUsuario);
                    com.Parameters.AddWithValue("@NombreUsuario", nombreUsuario);
                    com.Parameters.AddWithValue("@ContraUsuario", contraUsuario);
                    com.Parameters.AddWithValue("@Tipo", tipoUsuario);
                    com.Parameters.AddWithValue("@Foto", "\\" + path);

                    com.ExecuteNonQuery();

                    // Actualizar horario
                    SqlCommand horarioCmd = new SqlCommand(@"
UPDATE RegistroHorario 
SET 
    HoraSalida = @HoraSalida 
WHERE IdUsuario = @IdUsuario AND Fecha = @Fecha", con);

                    horarioCmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    horarioCmd.Parameters.AddWithValue("@Fecha", fecha);
                    horarioCmd.Parameters.AddWithValue("@HoraEntrada", horaEntrada.HasValue ? (object)horaEntrada.Value : DBNull.Value);
                    horarioCmd.Parameters.AddWithValue("@HoraSalida", horaSalida.HasValue ? (object)horaSalida.Value : DBNull.Value);

                    horarioCmd.ExecuteNonQuery();

                    MessageBox.Show("Usuario y horario actualizados correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargaDatosDGV(); // refrescar grid
                    CargarHorarios();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void timActual_Tick(object sender, EventArgs e)
        {
            string hora = DateTime.Now.ToString("hh:mm");
            string meridiano = DateTime.Now.ToString("tt", new System.Globalization.CultureInfo("es-MX")).ToLower();

            if (meridiano == "am")
            {
                lbHora_User.Text = $"{hora} p.m.";
                lbHoraAct_Menu.Text = $"{hora} p.m.";
                lbHoraProd.Text = $"{hora} p.m.";
                lbHoraEmployee.Text = $"{hora} p.m.";
                lbHoraRP.Text = $"{hora} p.m.";
            }
            else
            {
                lbHora_User.Text = $"{hora} a.m.";
                lbHoraAct_Menu.Text = $"{hora} a.m.";
                lbHoraProd.Text = $"{hora} a.m.";
                lbHoraEmployee.Text = $"{hora} a.m.";
                lbHoraRP.Text = $"{hora} a.m.";
            }
        }

        private void picClear_Click(object sender, EventArgs e)
        {
            txtID.Clear();
            txtNombre.Clear();
            txtContra.Clear();
            txtTipoU.Clear();
        }

        private void pic_RegistrarUser_MouseEnter(object sender, EventArgs e)
        {
            pic_RegistrarUser.Image = Properties.Resources.newUser2;
        }

        private void pic_RegistrarUser_MouseLeave(object sender, EventArgs e)
        {
            pic_RegistrarUser.Image = Properties.Resources.newUser;
        }

        private void picVolver_Users_MouseEnter(object sender, EventArgs e)
        {
            picVolver_Users.Image = Properties.Resources.volver2;
        }

        private void picVolver_Users_MouseLeave(object sender, EventArgs e)
        {
            picVolver_Users.Image = Properties.Resources.volver1;
        }

        private void picClear_MouseEnter(object sender, EventArgs e)
        {
            picClear.Image = Properties.Resources.powerON2;
        }

        private void picClear_MouseLeave(object sender, EventArgs e)
        {
            picClear.Image = Properties.Resources.powerON;
        }

        private void pic_AddImage_MouseEnter(object sender, EventArgs e)
        {
            pic_AddImage.Image = Properties.Resources.addImage2;
        }

        private void pic_AddImage_MouseLeave(object sender, EventArgs e)
        {
            pic_AddImage.Image = Properties.Resources.addImage;
        }

        private void pic_updateUser_MouseEnter(object sender, EventArgs e)
        {
            pic_updateUser.Image = Properties.Resources.update2;
        }

        private void pic_updateUser_MouseLeave(object sender, EventArgs e)
        {
            pic_updateUser.Image = Properties.Resources.update;
        }

        private void pic_saveUser_MouseEnter(object sender, EventArgs e)
        {
            pic_saveUser.Image = Properties.Resources.save2;
        }

        private void pic_saveUser_MouseLeave(object sender, EventArgs e)
        {
            pic_saveUser.Image = Properties.Resources.save1;
        }

        private void pic_deleteUser_MouseEnter(object sender, EventArgs e)
        {
            pic_deleteUser.Image = Properties.Resources.delete2;
        }

        private void pic_deleteUser_MouseLeave(object sender, EventArgs e)
        {
            pic_deleteUser.Image = Properties.Resources.delete;
        }

        private void picRegistrar_Click(object sender, EventArgs e)
        {
            lbVenta.Items.Clear();
            // Verificar si hay productos agregados (ignorando encabezados)
            if (lbProductos.Items.Count <= 2)
            {
                MessageBox.Show("Debes agregar al menos un producto para registrar la venta.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            foreach (var item in lbProductos.Items)
            {
                lbVenta.Items.Add(item);
            }
            UpdateListBox();
            CalcularTotales();

            tbcAdmin.SelectedTab = tabConfirmOrden;
            this.Width = 839;
            this.Height = 651;
            // Centrar manualmente el formulario
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;

            int formWidth = this.Width;
            int formHeight = this.Height;

            this.Location = new Point(
                (screenWidth - formWidth) / 2,
                (screenHeight - formHeight) / 2
            );
        }


        private void pictureBox11_Click(object sender, EventArgs e)
        {
            RemoveProducto();
        }

        private void picRegistrar_MouseEnter(object sender, EventArgs e)
        {
            picRegistrar.Image = Properties.Resources.registrar2;
        }

        private void picRegistrar_MouseLeave(object sender, EventArgs e)
        {
            picRegistrar.Image = Properties.Resources.registrar1;
        }

        private void picSOUV_MouseEnter(object sender, EventArgs e)
        {
            picSOUV.Image = Properties.Resources.souv2;
        }

        private void picSOUV_MouseLeave(object sender, EventArgs e)
        {
            picSOUV.Image = Properties.Resources.souv;
        }

        private void picDRINK_MouseEnter(object sender, EventArgs e)
        {
            picDRINK.Image = Properties.Resources.drink2;
        }

        private void picDRINK_MouseLeave(object sender, EventArgs e)
        {
            picDRINK.Image = Properties.Resources.drink;
        }

        private void picSNACK_MouseEnter(object sender, EventArgs e)
        {
            picSNACK.Image = Properties.Resources.snak2;
        }

        private void picSNACK_MouseLeave(object sender, EventArgs e)
        {
            picSNACK.Image = Properties.Resources.snak;
        }

        private void picMYP_MouseEnter(object sender, EventArgs e)
        {
            picMYP.Image = Properties.Resources.mypanel2;
        }

        private void picMYP_MouseLeave(object sender, EventArgs e)
        {
            picMYP.Image = Properties.Resources.mypanel;
        }

        private void picVolverMenu_MouseEnter(object sender, EventArgs e)
        {
            picVolverMenu.Image = Properties.Resources.volver2;
        }

        private void picVolverMenu_MouseLeave(object sender, EventArgs e)
        {
            picVolverMenu.Image = Properties.Resources.volver1;
        }

        private void picDeleteItem_MouseEnter(object sender, EventArgs e)
        {
            picDeleteItem.Image = Properties.Resources.error2;
        }

        private void picDeleteItem_MouseLeave(object sender, EventArgs e)
        {
            picDeleteItem.Image = Properties.Resources.error;
        }

        private void picVolverMenu_Click(object sender, EventArgs e)
        {
            tbcAdmin.SelectedTab = tabADMIN;
            this.Width = 892;
            this.Height = 465;
            // Centrar manualmente el formulario
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;

            int formWidth = this.Width;
            int formHeight = this.Height;

            this.Location = new Point(
                (screenWidth - formWidth) / 2,
                (screenHeight - formHeight) / 2
            );
        }

        private void CargarProductosPorCategoria(string categoria)
        {
            flpProductos.Controls.Clear(); // Limpiar los botones existentes

            using (SqlConnection con = Conexion.cadena())
            {
                try
                {

                    string query = "SELECT b.NombreProducto, b.Costo, b.PosX, b.PosY, p.Imagen AS Imagen FROM tbBotonesProductos b INNER JOIN tbProd p ON p.NombreProd = b.NombreProducto WHERE p.Categoria = @categoria";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@categoria", categoria);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string nombre = reader["NombreProducto"].ToString();
                                int costo = Convert.ToInt32(reader["Costo"]);
                                int posX = Convert.ToInt32(reader["PosX"]);
                                int posY = Convert.ToInt32(reader["PosY"]);
                                string rutaImagen = reader["Imagen"].ToString();

                                AgregarBotonDinamico(nombre, costo, posX, posY, rutaImagen);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar los productos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void picImagenProd_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string ruta = openFileDialog1.FileName;
                    string nombreProducto = txtNombreP.Text.Trim(); // Asegúrate de tener este textbox en el formulario
                    string carpeta = Path.Combine(Application.StartupPath, "Productos");
                    Directory.CreateDirectory(carpeta); // crea carpeta si no existe

                    string rutaFinal = Path.Combine(carpeta, nombreProducto + ".png");

                    // Mostrar en PictureBox (si tienes uno)
                    picImgProd.Image = System.Drawing.Image.FromFile(ruta);

                    // Guardar físicamente la imagen
                    picImgProd.Image.Save(rutaFinal, ImageFormat.Png);

                    // Guardar la ruta en ambas tablas (tbProd y tbBotonesProductos)
                    using (SqlConnection con = Conexion.cadena())
                    {

                        // tbProd
                        string query1 = "UPDATE tbProd SET Imagen = @ruta WHERE NombreProd = @nombre";
                        using (SqlCommand cmd1 = new SqlCommand(query1, con))
                        {
                            cmd1.Parameters.AddWithValue("@ruta", rutaFinal);
                            cmd1.Parameters.AddWithValue("@nombre", nombreProducto);
                            cmd1.ExecuteNonQuery();
                        }

                        // tbBotonesProductos
                        string query2 = "UPDATE tbBotonesProductos SET Imagen = @ruta WHERE NombreProducto = @nombre";
                        using (SqlCommand cmd2 = new SqlCommand(query2, con))
                        {
                            cmd2.Parameters.AddWithValue("@ruta", rutaFinal);
                            cmd2.Parameters.AddWithValue("@nombre", nombreProducto);
                            cmd2.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
            }
        }

        private void picSOUV_Click(object sender, EventArgs e)
        {
            flpProductos.BackgroundImage = Properties.Resources.flow;
            CargarProductosPorCategoria("Souvenirs");
        }

        private void picDRINK_Click(object sender, EventArgs e)
        {
            flpProductos.BackgroundImage = Properties.Resources.JHONNY;
            CargarProductosPorCategoria("Bebidas");
        }

        private void picSNACK_Click(object sender, EventArgs e)
        {
            flpProductos.BackgroundImage = Properties.Resources.CHENI;
            CargarProductosPorCategoria("Snacks");
        }

        private void tbcAdmin_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tbcAdmin.SelectedTab == tabMenu)
            {
                flpProductos.Controls.Clear(); // Limpiar botones
            }
        }

        private void picVolverMenu_Prod_Click(object sender, EventArgs e)
        {
            AbrirADMIN();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombreP.Text) ||
       string.IsNullOrWhiteSpace(txtCostoP.Text) ||
       string.IsNullOrWhiteSpace(txtCantP.Text) ||
       cbCat.SelectedIndex == -1)
            {
                MessageBox.Show("Todos los campos son obligatorios", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtCostoP.Text, out int costo) || !int.TryParse(txtCantP.Text, out int cantidad))
            {
                MessageBox.Show("El costo y la cantidad deben ser valores numéricos", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string categoria = cbCat.SelectedItem.ToString();
            string nombreArchivo = txtNombreP.Text + ".png";
            string rutaDirectorio = Path.Combine(Application.StartupPath, "Productos");
            Directory.CreateDirectory(rutaDirectorio);
            string rutaFinal = Path.Combine(rutaDirectorio, nombreArchivo);

            try
            {
                if (picImgProd.Image != null)
                {
                    picImgProd.Image.Save(rutaFinal, System.Drawing.Imaging.ImageFormat.Png);
                }
                else
                {
                    MessageBox.Show("Por favor seleccione una imagen antes de añadir el producto.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SqlConnection con = Conexion.cadena())
                {
                    using (SqlCommand com = new SqlCommand("spAltaProducto", con))
                    {
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@NombreProd", txtNombreP.Text);
                        com.Parameters.AddWithValue("@Costo", costo);
                        com.Parameters.AddWithValue("@Cantidad", cantidad);
                        com.Parameters.AddWithValue("@Imagen", rutaFinal);
                        com.Parameters.AddWithValue("@Categoria", categoria); // Nuevo parámetro

                        int resultado = com.ExecuteNonQuery();
                        con.Close();

                        if (resultado > 0)
                        {
                            CargarBotonesDesdeBD();
                            CargaDatosDGVP();
                            MessageBox.Show("Producto agregado exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            CargarBotonesDesdeBD();
                            CargaDatosDGVP();
                            MessageBox.Show("El producto ya existe en la base de datos", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar imagen o insertar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void picDeleteProd_Click(object sender, EventArgs e)
        {
            // Verificar que hay una fila seleccionada en el DataGridView
            if (dgvProductos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un producto para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Obtener el ID del producto seleccionado
            int idProducto = Convert.ToInt32(dgvProductos.SelectedRows[0].Cells["IdProducto"].Value);
            string nombreProducto = dgvProductos.SelectedRows[0].Cells["NombreProd"].Value.ToString();

            // Confirmación de eliminación
            DialogResult result = MessageBox.Show($"¿Está seguro de eliminar el producto {nombreProducto}?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.No)
                return;

            // Conectar a la base de datos y eliminar el producto
            using (SqlConnection con = Conexion.cadena())
            {
                try
                {
                    using (SqlCommand com = new SqlCommand("spEliminaProducto", con))
                    {
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@IdProducto", idProducto);

                        int filasAfectadas = com.ExecuteNonQuery();

                        if (filasAfectadas > 0)
                        {
                            MessageBox.Show("Producto eliminado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CargaDatosDGVP();
                            EliminarBotonDelPanel(nombreProducto); // Eliminar el botón correspondiente en el panel
                        }
                        else
                        {
                            MessageBox.Show("El producto no se encontró en la base de datos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void picUpdateProd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombreP.Text) ||
        string.IsNullOrWhiteSpace(txtCostoP.Text) ||
        string.IsNullOrWhiteSpace(txtCantP.Text) ||
        cbCat.SelectedIndex == -1)
            {
                MessageBox.Show("Todos los campos son obligatorios", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idProducto = Convert.ToInt32(txtID_P.Text);
            string nombreProducto = txtNombreP.Text.Trim();
            string categoria = cbCat.SelectedItem.ToString();

            if (!int.TryParse(txtCostoP.Text, out int costo) || !int.TryParse(txtCantP.Text, out int cantidad))
            {
                MessageBox.Show("El costo y la cantidad deben ser valores numéricos.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult result = MessageBox.Show($"¿Desea actualizar el producto {nombreProducto}?", "Confirmar Actualización", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
                return;

            string carpetaImagenes = Path.Combine(Application.StartupPath, "Productos");
            Directory.CreateDirectory(carpetaImagenes);
            string rutaImagen = Path.Combine(carpetaImagenes, nombreProducto + ".png");

            if (imagenSeleccionada && picImgProd.Image != null)
            {
                picImgProd.Image.Save(rutaImagen, ImageFormat.Png);
            }

            using (SqlConnection con = Conexion.cadena())
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("spEditarProducto", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdProducto", idProducto);
                    cmd.Parameters.AddWithValue("@NombreProd", nombreProducto);
                    cmd.Parameters.AddWithValue("@Costo", costo);
                    cmd.Parameters.AddWithValue("@Cantidad", cantidad);
                    cmd.Parameters.AddWithValue("@Imagen", rutaImagen);
                    cmd.Parameters.AddWithValue("@Categoria", categoria); // Nuevo parámetro

                    cmd.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("Producto actualizado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarBotonesDesdeBD();
                    CargaDatosDGVP();
                    imagenSeleccionada = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void picRegresarMenu_Click(object sender, EventArgs e)
        {
            tbcAdmin.SelectedTab = tabMenu;
            this.Width = 1280;
            this.Height = 805;
            // Centrar manualmente el formulario
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;

            int formWidth = this.Width;
            int formHeight = this.Height;

            this.Location = new Point(
                (screenWidth - formWidth) / 2,
                (screenHeight - formHeight) / 2
            );
        }

        private void picMYP_Click(object sender, EventArgs e)
        {
            tbcAdmin.SelectedTab = tabEMPLEADO;
            this.Width = 706;
            this.Height = 503;
            // Centrar manualmente el formulario
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;

            int formWidth = this.Width;
            int formHeight = this.Height;

            this.Location = new Point(
                (screenWidth - formWidth) / 2,
                (screenHeight - formHeight) / 2
            );
        }

        private void btnHORAENT_Click(object sender, EventArgs e)
        {

        }
        private void CargarHorarios()
        {
            int idUsuario = int.Parse(lbIDUSUARIO.Text); // o lbIDUser_Menu.Text si lo usas

            using (SqlConnection con = Conexion.cadena())
            {
                string query = @"
            SELECT 
                Fecha,
                CASE 
                    WHEN HoraEntrada IS NOT NULL THEN CONVERT(varchar(5), HoraEntrada)
                    ELSE ''
                END AS [Hora Entrada],
                CASE 
                    WHEN HoraSalida IS NOT NULL THEN CONVERT(varchar(5), HoraSalida)
                    ELSE ''
                END AS [Hora Salida]
            FROM RegistroHorario
            WHERE IdUsuario = @id
            ORDER BY Fecha DESC";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", idUsuario);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvEMPLE.DataSource = dt;
            }
        }

        private void btnHORASALE_Click(object sender, EventArgs e)
        {

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
                this.Hide(); // Oculta el formulario actual en lugar de cerrarlo inmediatamente

                Form1 log = new Form1
                {
                    StartPosition = FormStartPosition.CenterScreen,
                    TopMost = true // Hace que esté al frente de todo
                };

                log.Show(); // Mostrar como ventana modal
                this.Close(); // Cierra el formulario actual después
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = Conexion.cadena())
                {
                    SqlCommand cmd = new SqlCommand("spEnviarArchivoMuerto", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Registros enviados a archivo muerto correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    CargarVentasEnReporte(); // Refresca el grid actual
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al transferir a archivo muerto:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dtpReportes_ValueChanged(object sender, EventArgs e)
        {
            DateTime fechaSeleccionada = dtpReportes.Value.Date;

            try
            {
                using (SqlConnection con = Conexion.cadena())
                {
                    SqlCommand cmd = new SqlCommand("spConsultarArchivoPorFecha", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fecha", fechaSeleccionada);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvReporte.DataSource = null;
                    dgvReporte.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al consultar archivo muerto:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void icbClient_Click(object sender, EventArgs e)
        {
            tbcAdmin.SelectedTab = tabEMPLEADO;
            this.Width = 706;
            this.Height = 503;
            // Centrar manualmente el formulario
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;

            int formWidth = this.Width;
            int formHeight = this.Height;

            this.Location = new Point(
                (screenWidth - formWidth) / 2,
                (screenHeight - formHeight) / 2
            );
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvReporte.Rows.Count == 0)
                {
                    MessageBox.Show("No hay datos para generar el reporte.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.Filter = "Archivos PDF|*.pdf";
                saveFile.Title = "Guardar Reporte de Ventas";
                saveFile.FileName = "Reporte_Ventas_Diarias.pdf";

                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    using (FileStream stream = new FileStream(saveFile.FileName, FileMode.Create))
                    {
                        Document pdfDoc = new Document(PageSize.A4, 25, 25, 30, 30);
                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                        pdfDoc.Open();

                        iTextFont tituloFont = new iTextFont(iTextFont.FontFamily.HELVETICA, 18, iTextFont.BOLD);
                        Paragraph titulo = new Paragraph("Reporte Diario de Ventas", tituloFont);
                        titulo.Alignment = Element.ALIGN_CENTER;
                        titulo.SpacingAfter = 20;
                        pdfDoc.Add(titulo);

                        PdfPTable tabla = new PdfPTable(dgvReporte.Columns.Count);
                        tabla.WidthPercentage = 100;

                        // Encabezados
                        foreach (DataGridViewColumn column in dgvReporte.Columns)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText));
                            cell.BackgroundColor = new BaseColor(240, 240, 240);
                            tabla.AddCell(cell);
                        }

                        // Datos
                        foreach (DataGridViewRow row in dgvReporte.Rows)
                        {
                            if (row.IsNewRow) continue;

                            foreach (DataGridViewCell cell in row.Cells)
                            {
                                tabla.AddCell(cell.Value?.ToString());
                            }
                        }

                        pdfDoc.Add(tabla);
                        pdfDoc.Close();
                        writer.Close();
                        stream.Close();
                    }

                    MessageBox.Show("Reporte generado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar el PDF: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void picVolverAdmin_rp_Click(object sender, EventArgs e)
        {
            // Centrar manualmente el formulario
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;

            int formWidth = this.Width;
            int formHeight = this.Height;

            this.Location = new Point(
                (screenWidth - formWidth) / 2,
                (screenHeight - formHeight) / 2
            );
            AbrirADMIN();
        }

        private void picArchivoMuerto_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = Conexion.cadena())
                {
                    SqlCommand cmd = new SqlCommand("spEnviarArchivoMuerto", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Registros enviados a archivo muerto correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    CargarVentasEnReporte(); // Refresca el grid actual
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al transferir a archivo muerto:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void picPDF_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvReporte.Rows.Count == 0)
                {
                    MessageBox.Show("No hay datos para generar el reporte.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SaveFileDialog saveFile = new SaveFileDialog
                {
                    Filter = "Archivos PDF|*.pdf",
                    Title = "Guardar Reporte de Ventas",
                    FileName = "Reporte_Ventas_Diarias.pdf"
                };

                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    using (FileStream stream = new FileStream(saveFile.FileName, FileMode.Create))
                    {
                        Document pdfDoc = new Document(PageSize.A4, 25, 25, 30, 30);
                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                        pdfDoc.Open();

                        // LOGO
                        string logoPath = "logoPOS.bmp";
                        if (File.Exists(logoPath))
                        {
                            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(logoPath);
                            logo.ScaleToFit(80, 80);
                            logo.Alignment = Element.ALIGN_CENTER;
                            pdfDoc.Add(logo);
                        }

                        // TÍTULO
                        iTextSharp.text.Font tituloFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                        Paragraph titulo = new Paragraph("Reporte Diario de Ventas", tituloFont)
                        {
                            Alignment = Element.ALIGN_CENTER,
                            SpacingAfter = 20f
                        };
                        pdfDoc.Add(titulo);

                        // TABLA
                        PdfPTable tabla = new PdfPTable(dgvReporte.Columns.Count)
                        {
                            WidthPercentage = 100
                        };

                        foreach (DataGridViewColumn col in dgvReporte.Columns)
                        {
                            PdfPCell celdaEncabezado = new PdfPCell(new Phrase(col.HeaderText, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9)))
                            {
                                BackgroundColor = new BaseColor(230, 230, 230),
                                HorizontalAlignment = Element.ALIGN_CENTER
                            };
                            tabla.AddCell(celdaEncabezado);
                        }

                        foreach (DataGridViewRow row in dgvReporte.Rows)
                        {
                            if (row.IsNewRow) continue;

                            foreach (DataGridViewCell cell in row.Cells)
                            {
                                string texto = cell.Value?.ToString() ?? "";
                                tabla.AddCell(new Phrase(texto, FontFactory.GetFont(FontFactory.HELVETICA, 9)));
                            }
                        }

                        pdfDoc.Add(tabla);
                        pdfDoc.Add(Chunk.NEWLINE);

                        // TOTAL (tomado directamente del label)
                        iTextSharp.text.Font negrita = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                        Paragraph total = new Paragraph("Total Ganancias del Día: $" + totalGananciasGlobal.ToString("0.00"), negrita)
                        {
                            Alignment = Element.ALIGN_RIGHT
                        };
                        pdfDoc.Add(total);

                        // CIERRE
                        pdfDoc.Close();
                        writer.Close();
                        stream.Close();

                        MessageBox.Show("Reporte generado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar el PDF: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        decimal totalGananciasGlobal = 0; // fuera del método, a nivel de clase

        private void CalcularTotalVentasDelDia()
        {
            totalGananciasGlobal = 0;

            foreach (DataGridViewRow row in dgvReporte.Rows)
            {
                if (row.IsNewRow) continue;

                if (decimal.TryParse(row.Cells["Cantidad Vendida"].Value?.ToString(), out decimal cantidad) &&
                    decimal.TryParse(row.Cells["Costo Unitario"].Value?.ToString(), out decimal costo))
                {
                    totalGananciasGlobal += cantidad * costo;
                }
            }

            lblTotalVentasDia.Text = $"{totalGananciasGlobal:0.00}";
        }

        private void picConfirmar_Click(object sender, EventArgs e)
        {
            if (lbProductos.Items.Count <= 2)
            {
                MessageBox.Show("Debes agregar al menos un producto para registrar la venta.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtMontoPago.Text, out decimal montoPago))
            {
                MessageBox.Show("Ingresa un monto válido.");
                return;
            }

            decimal montoVenta = decimal.Parse(lblMontoVenta.Text.Replace("$", ""));
            if (montoPago < montoVenta)
            {
                MessageBox.Show("El monto pagado es menor al monto de venta.");
                return;
            }

            RegistrarVenta();


        }

        private void picVolverMenu_Conf_Click(object sender, EventArgs e)
        {
            tbcAdmin.SelectedTab = tabMenu;
            this.Width = 1287;
            this.Height = 822;
            // Centrar manualmente el formulario
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;

            int formWidth = this.Width;
            int formHeight = this.Height;

            this.Location = new Point(
                (screenWidth - formWidth) / 2,
                (screenHeight - formHeight) / 2
            );
        }

        private void AgregarMonto(decimal monto)
        {
            decimal montoActual = 0;

            if (!string.IsNullOrWhiteSpace(txtMontoPago.Text))
            {
                decimal.TryParse(txtMontoPago.Text, out montoActual);
            }

            montoActual += monto;
            txtMontoPago.Text = montoActual.ToString("0.00");
        }
        private void btn20_Click(object sender, EventArgs e)
        {
            AgregarMonto(20);
        }

        private void btn50_Click(object sender, EventArgs e)
        {
            AgregarMonto(50);
        }

        private void btn100_Click(object sender, EventArgs e)
        {
            AgregarMonto(100);
        }

        private void btn200_Click(object sender, EventArgs e)
        {
            AgregarMonto(200);
        }

        private void btn500_Click(object sender, EventArgs e)
        {
            AgregarMonto(500);
        }

        private void btn1000_Click(object sender, EventArgs e)
        {
            AgregarMonto(1000);
        }

        private void picEntrada_Click(object sender, EventArgs e)
        {
            int idUsuario = int.Parse(lbIDUSUARIO.Text);
            DateTime fechaHoy = DateTime.Today;
            TimeSpan horaActual = DateTime.Now.TimeOfDay;

            using (SqlConnection con = Conexion.cadena())
            {
                // ¿Ya existe entrada hoy?
                SqlCommand checkEntrada = new SqlCommand(
                    "SELECT HoraEntrada FROM RegistroHorario WHERE IdUsuario = @id AND Fecha = @fecha", con);
                checkEntrada.Parameters.AddWithValue("@id", idUsuario);
                checkEntrada.Parameters.AddWithValue("@fecha", fechaHoy);

                object resultado = checkEntrada.ExecuteScalar();

                if (resultado == null || resultado == DBNull.Value)
                {
                    // No hay entrada, registrar
                    SqlCommand insertCmd = new SqlCommand(
                        "INSERT INTO RegistroHorario (IdUsuario, Fecha, HoraEntrada) VALUES (@id, @fecha, @entrada)", con);
                    insertCmd.Parameters.AddWithValue("@id", idUsuario);
                    insertCmd.Parameters.AddWithValue("@fecha", fechaHoy);
                    insertCmd.Parameters.AddWithValue("@entrada", horaActual);
                    insertCmd.ExecuteNonQuery();

                    MessageBox.Show("Hora de entrada registrada.");
                }
                else
                {
                    MessageBox.Show("Ya registraste tu hora de entrada para hoy.");
                }

                // Actualizar el grid
                CargarHorarios();
            }
        }

        private void picSalida_Click(object sender, EventArgs e)
        {
            int idUsuario = int.Parse(lbIDUSUARIO.Text);
            DateTime fechaHoy = DateTime.Today;
            TimeSpan horaActual = DateTime.Now.TimeOfDay;

            using (SqlConnection con = Conexion.cadena())
            {
                // Verificar si existe registro de hoy
                SqlCommand checkRegistro = new SqlCommand(
                    "SELECT HoraEntrada, HoraSalida FROM RegistroHorario WHERE IdUsuario = @id AND Fecha = @fecha", con);
                checkRegistro.Parameters.AddWithValue("@id", idUsuario);
                checkRegistro.Parameters.AddWithValue("@fecha", fechaHoy);

                using (SqlDataReader reader = checkRegistro.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        object horaEntrada = reader["HoraEntrada"];
                        object horaSalida = reader["HoraSalida"];

                        if (horaEntrada == DBNull.Value)
                        {
                            MessageBox.Show("Primero debes registrar la hora de entrada.");
                            return;
                        }

                        if (horaSalida != DBNull.Value)
                        {
                            MessageBox.Show("Ya registraste tu hora de salida para hoy.");
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Primero debes registrar la hora de entrada.");
                        return;
                    }
                }

                // Registrar hora de salida
                SqlCommand update = new SqlCommand(
                    "UPDATE RegistroHorario SET HoraSalida = @salida WHERE IdUsuario = @id AND Fecha = @fecha", con);
                update.Parameters.AddWithValue("@salida", horaActual);
                update.Parameters.AddWithValue("@id", idUsuario);
                update.Parameters.AddWithValue("@fecha", fechaHoy);
                update.ExecuteNonQuery();

                MessageBox.Show("Hora de salida registrada correctamente.");
                CargaDatosDGV();
                CargarHorarios();
            }
        }

        private void picArchivoMuerto_MouseEnter(object sender, EventArgs e)
        {
            picArchivoMuerto.Image = Properties.Resources.delete2;
        }

        private void picArchivoMuerto_MouseLeave(object sender, EventArgs e)
        {
            picArchivoMuerto.Image = Properties.Resources.delete;
        }

        private void picPDF_MouseEnter(object sender, EventArgs e)
        {
            picPDF.Image = Properties.Resources.addImage2;
        }

        private void picPDF_MouseLeave(object sender, EventArgs e)
        {
            picPDF.Image = Properties.Resources.addImage;
        }

        private void picVolverAdmin_rp_MouseEnter(object sender, EventArgs e)
        {
            picVolverAdmin_rp.Image = Properties.Resources.volver2;
        }

        private void picVolverAdmin_rp_MouseLeave(object sender, EventArgs e)
        {
            picVolverAdmin_rp.Image = Properties.Resources.volver1;
        }

        private void picAddProd_MouseEnter(object sender, EventArgs e)
        {
            picAddProd.Image = Properties.Resources.save2;
        }

        private void picAddProd_MouseLeave(object sender, EventArgs e)
        {
            picAddProd.Image = Properties.Resources.save1;
        }

        private void picUpdateProd_MouseEnter(object sender, EventArgs e)
        {
            picUpdateProd.Image = Properties.Resources.update2;
        }

        private void picUpdateProd_MouseLeave(object sender, EventArgs e)
        {
            picUpdateProd.Image = Properties.Resources.update;
        }

        private void picImagenProd_MouseEnter(object sender, EventArgs e)
        {
            picImagenProd.Image = Properties.Resources.addImage2;
        }

        private void picImagenProd_MouseLeave(object sender, EventArgs e)
        {
            picImagenProd.Image = Properties.Resources.addImage;
        }

        private void picDeleteProd_MouseEnter(object sender, EventArgs e)
        {
            picDeleteProd.Image = Properties.Resources.delete2;
        }

        private void picDeleteProd_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void picDeleteProd_MouseLeave(object sender, EventArgs e)
        {
            picDeleteProd.Image = Properties.Resources.delete;
        }

        private void picVolverMenu_Prod_MouseEnter(object sender, EventArgs e)
        {
            picVolverMenu_Prod.Image = Properties.Resources.volver2;
        }

        private void picVolverMenu_Prod_MouseLeave(object sender, EventArgs e)
        {
            picVolverMenu_Prod.Image = Properties.Resources.volver1;
        }

        private void picEFECT_MouseEnter(object sender, EventArgs e)
        {

        }

        private void picEFECT_MouseLeave(object sender, EventArgs e)
        {

        }

        private void picTPV_MouseEnter(object sender, EventArgs e)
        {

        }

        private void picTPV_MouseLeave(object sender, EventArgs e)
        {

        }

        private void picTRANSFE_MouseEnter(object sender, EventArgs e)
        {

        }

        private void picTRANSFE_MouseLeave(object sender, EventArgs e)
        {

        }

        private void picVolverMenu_Conf_MouseEnter(object sender, EventArgs e)
        {
            picVolverMenu_Conf.Image = Properties.Resources.volver2;
        }

        private void picVolverMenu_Conf_MouseLeave(object sender, EventArgs e)
        {
            picVolverMenu_Conf.Image = Properties.Resources.volver1;
        }

        private void picConfirmar_MouseEnter(object sender, EventArgs e)
        {
            picConfirmar.Image = Properties.Resources.registrar2;
        }

        private void picConfirmar_MouseLeave(object sender, EventArgs e)
        {
            picConfirmar.Image = Properties.Resources.registrar1;
        }

        private void picEntrada_MouseEnter(object sender, EventArgs e)
        {
            picEntrada.Image = Properties.Resources.entrada2;
        }

        private void picEntrada_MouseLeave(object sender, EventArgs e)
        {
            picEntrada.Image = Properties.Resources.entrada;
        }

        private void picSalida_MouseEnter(object sender, EventArgs e)
        {
            picSalida.Image = Properties.Resources.salida2;
        }

        private void picSalida_MouseLeave(object sender, EventArgs e)
        {
            picSalida.Image = Properties.Resources.salida;
        }

        private void picRegresarMenu_MouseEnter(object sender, EventArgs e)
        {
            picRegresarMenu.Image = Properties.Resources.SHOP2;
        }

        private void picRegresarMenu_MouseLeave(object sender, EventArgs e)
        {
            picRegresarMenu.Image = Properties.Resources.SHOP;
        }

        private void picEFECT_Click(object sender, EventArgs e)
        {
            tipoPagoSeleccionado = "EFECTIVO";
            picEFECT.Image = Properties.Resources.bills2;

            picTPV.Image = Properties.Resources.TPV;
            picTRANSFE.Image = Properties.Resources.TRANS;
        }

        private void picTPV_Click(object sender, EventArgs e)
        {
            tipoPagoSeleccionado = "TERMINAL";
            picEFECT.Image = Properties.Resources.bills;
            picTPV.Image = Properties.Resources.TPV2;
            picTRANSFE.Image = Properties.Resources.TRANS;
        }

        private void picTRANSFE_Click(object sender, EventArgs e)
        {
            tipoPagoSeleccionado = "TRANSFERENCIA";
            picEFECT.Image = Properties.Resources.bills;
            picTPV.Image = Properties.Resources.TPV;
            picTRANSFE.Image = Properties.Resources.TRANSFE2;
        }

        private void picOFF_MouseEnter(object sender, EventArgs e)
        {
            picOFF.Image = Properties.Resources.powerOff2;
        }

        private void picOFF_MouseLeave(object sender, EventArgs e)
        {
            picOFF.Image = Properties.Resources.powerOff;
        }

        private void picRefresh_Click(object sender, EventArgs e)
        {
            txtID_P.Clear();
            txtNombreP.Clear();
            txtCostoP.Clear();
            txtCantP.Clear();
            cbCat.Text = null;
        }
    }
}
