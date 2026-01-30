using AForge.Video.DirectShow;
using AForge.Video;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using Microsoft.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Logueo_222310072
{
    public partial class wfTomarFoto : Form
    {
        public string ruta, path;
        private bool ExistenDispositivos = false;
        private FilterInfoCollection DispositivosDeVideo;
        public VideoCaptureDevice FuenteDeVideo = null;
        private string nombreUsuario; 
        private wfPanelAdmin panelAdmin; // Referencia al formulario de administración
        public wfTomarFoto(string nombre, wfPanelAdmin panel)
        {
            InitializeComponent();
            nombreUsuario = nombre; // Guardar el nombre recibido en una variable local
            panelAdmin = panel; // Guardar la referencia al formulario
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
            for (int i = 0; i < Dispositivos.Count; i++)
            {
                cmbCamara.Items.Add(Dispositivos[i].Name.ToString());
            }
            cmbCamara.Text = cmbCamara.Items[0].ToString();

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
        private void btnCamaraON_Click(object sender, EventArgs e)
        {
            if (ExistenDispositivos == true)
            {
                TerminarFuenteDeVideo();
                int i = cmbCamara.SelectedIndex;
                string NombreVideo = DispositivosDeVideo[i].MonikerString;
                FuenteDeVideo = new VideoCaptureDevice(NombreVideo);
                FuenteDeVideo.NewFrame += new NewFrameEventHandler(video_NuevoFrameCapturando);
                FuenteDeVideo.Start();
            }
            else
            {
                MessageBox.Show("No existen dispositivos conectados", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnFoto_Click(object sender, EventArgs e)
        {

            try
            {
                if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    ruta = openFileDialog1.FileName;
                    picUsuario.Image = Image.FromFile(ruta);
                    picUsuario.Image.Save(Application.StartupPath + ("\\Usuarios\\") + nombreUsuario + ".jpg", ImageFormat.Jpeg);
                    // Actualizar la imagen en el formulario principal (Panel Admin)
                    panelAdmin.ActualizarImagenUsuario(picUsuario.Image);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.None);
            }
        }

        private void btnTomarFoto_Click(object sender, EventArgs e)
        {
            try
            {
                if (!(FuenteDeVideo == null) && FuenteDeVideo.IsRunning)
                {
                    picUsuario.Image = picCamara.Image;

                    // Guardar la imagen en la carpeta de usuarios
                    string rutaImagen = Application.StartupPath + "\\Usuarios\\" + nombreUsuario + ".jpg";
                    picUsuario.Image.Save(rutaImagen, ImageFormat.Jpeg);

                    // Actualizar la imagen en el formulario principal (Panel Admin)
                    panelAdmin.ActualizarImagenUsuario(picUsuario.Image);

                    MessageBox.Show("Imagen capturada y asignada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.None);
            }
        }

        private void wfTomarFoto_Load(object sender, EventArgs e)
        {
            BuscarDispositivosVideo();
        }


        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
