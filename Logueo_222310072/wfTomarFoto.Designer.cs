namespace Logueo_222310072
{
    partial class wfTomarFoto
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label6 = new Label();
            picUsuario = new PictureBox();
            btnFoto = new Button();
            btnTomarFoto = new Button();
            label7 = new Label();
            picCamara = new PictureBox();
            cmbCamara = new ComboBox();
            btnCamaraON = new Button();
            openFileDialog1 = new OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)picUsuario).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picCamara).BeginInit();
            SuspendLayout();
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Zrnic Rg", 12F);
            label6.ForeColor = Color.White;
            label6.Location = new Point(2, 166);
            label6.Name = "label6";
            label6.Size = new Size(167, 24);
            label6.TabIndex = 21;
            label6.Text = "Imagen del Usuario:";
            // 
            // picUsuario
            // 
            picUsuario.Location = new Point(12, 205);
            picUsuario.Name = "picUsuario";
            picUsuario.Size = new Size(157, 155);
            picUsuario.SizeMode = PictureBoxSizeMode.StretchImage;
            picUsuario.TabIndex = 22;
            picUsuario.TabStop = false;
            // 
            // btnFoto
            // 
            btnFoto.BackgroundImage = Properties.Resources.button;
            btnFoto.BackgroundImageLayout = ImageLayout.Stretch;
            btnFoto.FlatStyle = FlatStyle.Popup;
            btnFoto.Font = new Font("Zrnic Rg", 12F);
            btnFoto.ForeColor = Color.White;
            btnFoto.Location = new Point(-1, 373);
            btnFoto.Name = "btnFoto";
            btnFoto.Size = new Size(180, 32);
            btnFoto.TabIndex = 23;
            btnFoto.Text = "Seleccionar Imagen";
            btnFoto.UseVisualStyleBackColor = true;
            btnFoto.Click += btnFoto_Click;
            // 
            // btnTomarFoto
            // 
            btnTomarFoto.BackgroundImage = Properties.Resources.button;
            btnTomarFoto.BackgroundImageLayout = ImageLayout.Stretch;
            btnTomarFoto.FlatStyle = FlatStyle.Popup;
            btnTomarFoto.Font = new Font("Zrnic Rg", 12F);
            btnTomarFoto.ForeColor = Color.White;
            btnTomarFoto.Location = new Point(178, 373);
            btnTomarFoto.Name = "btnTomarFoto";
            btnTomarFoto.Size = new Size(176, 32);
            btnTomarFoto.TabIndex = 28;
            btnTomarFoto.Text = "Tomar Fotografía";
            btnTomarFoto.UseVisualStyleBackColor = true;
            btnTomarFoto.Click += btnTomarFoto_Click;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Zrnic Rg", 12F);
            label7.ForeColor = Color.White;
            label7.Location = new Point(5, 6);
            label7.Name = "label7";
            label7.Size = new Size(341, 48);
            label7.TabIndex = 24;
            label7.Text = "Selecciona un dispositivo para tomar una \r\nfotografia:";
            label7.Click += label7_Click;
            // 
            // picCamara
            // 
            picCamara.Location = new Point(187, 205);
            picCamara.Name = "picCamara";
            picCamara.Size = new Size(157, 155);
            picCamara.SizeMode = PictureBoxSizeMode.StretchImage;
            picCamara.TabIndex = 27;
            picCamara.TabStop = false;
            // 
            // cmbCamara
            // 
            cmbCamara.FormattingEnabled = true;
            cmbCamara.Location = new Point(50, 68);
            cmbCamara.Name = "cmbCamara";
            cmbCamara.Size = new Size(250, 28);
            cmbCamara.TabIndex = 25;
            // 
            // btnCamaraON
            // 
            btnCamaraON.BackgroundImage = Properties.Resources.button;
            btnCamaraON.BackgroundImageLayout = ImageLayout.Stretch;
            btnCamaraON.FlatStyle = FlatStyle.Popup;
            btnCamaraON.Font = new Font("Zrnic Rg", 12F);
            btnCamaraON.ForeColor = Color.White;
            btnCamaraON.ImageAlign = ContentAlignment.TopLeft;
            btnCamaraON.Location = new Point(-1, 121);
            btnCamaraON.Name = "btnCamaraON";
            btnCamaraON.Size = new Size(355, 28);
            btnCamaraON.TabIndex = 26;
            btnCamaraON.Text = "Activar cámara";
            btnCamaraON.UseVisualStyleBackColor = true;
            btnCamaraON.Click += btnCamaraON_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // wfTomarFoto
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlText;
            ClientSize = new Size(352, 407);
            Controls.Add(label6);
            Controls.Add(picUsuario);
            Controls.Add(btnFoto);
            Controls.Add(btnTomarFoto);
            Controls.Add(label7);
            Controls.Add(picCamara);
            Controls.Add(cmbCamara);
            Controls.Add(btnCamaraON);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "wfTomarFoto";
            StartPosition = FormStartPosition.CenterParent;
            TopMost = true;
            Load += wfTomarFoto_Load;
            ((System.ComponentModel.ISupportInitialize)picUsuario).EndInit();
            ((System.ComponentModel.ISupportInitialize)picCamara).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        public Label label6;
        private PictureBox picUsuario;
        private Button btnFoto;
        private Button btnTomarFoto;
        public Label label7;
        private PictureBox picCamara;
        private ComboBox cmbCamara;
        private Button btnCamaraON;
        private OpenFileDialog openFileDialog1;
    }
}