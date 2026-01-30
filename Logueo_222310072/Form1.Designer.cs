namespace Logueo_222310072
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            txtUsuario = new TextBox();
            label2 = new Label();
            txtContraseña = new TextBox();
            label3 = new Label();
            pictureBox1 = new PictureBox();
            icbLogueo = new FontAwesome.Sharp.IconButton();
            picOFF = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picOFF).BeginInit();
            SuspendLayout();
            // 
            // txtUsuario
            // 
            txtUsuario.Font = new Font("Zrnic Rg", 12F);
            txtUsuario.Location = new Point(145, 253);
            txtUsuario.Name = "txtUsuario";
            txtUsuario.Size = new Size(181, 31);
            txtUsuario.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Zrnic Rg", 12F);
            label2.ForeColor = Color.White;
            label2.Location = new Point(38, 303);
            label2.Name = "label2";
            label2.Size = new Size(102, 24);
            label2.TabIndex = 4;
            label2.Text = "Contraseña:";
            label2.Click += label2_Click;
            // 
            // txtContraseña
            // 
            txtContraseña.Font = new Font("Zrnic Rg", 12F);
            txtContraseña.Location = new Point(145, 301);
            txtContraseña.Name = "txtContraseña";
            txtContraseña.Size = new Size(181, 31);
            txtContraseña.TabIndex = 3;
            txtContraseña.UseSystemPasswordChar = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Zrnic Rg", 12F);
            label3.ForeColor = Color.White;
            label3.Location = new Point(66, 255);
            label3.Name = "label3";
            label3.Size = new Size(74, 24);
            label3.TabIndex = 6;
            label3.Text = "Usuario:";
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Image = Properties.Resources.LOGOMARCA;
            pictureBox1.Location = new Point(83, 27);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(243, 209);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 7;
            pictureBox1.TabStop = false;
            // 
            // icbLogueo
            // 
            icbLogueo.BackgroundImage = Properties.Resources.bgAurora;
            icbLogueo.BackgroundImageLayout = ImageLayout.Stretch;
            icbLogueo.IconChar = FontAwesome.Sharp.IconChar.House;
            icbLogueo.IconColor = Color.Black;
            icbLogueo.IconFont = FontAwesome.Sharp.IconFont.Auto;
            icbLogueo.Location = new Point(175, 362);
            icbLogueo.Name = "icbLogueo";
            icbLogueo.Size = new Size(68, 62);
            icbLogueo.TabIndex = 8;
            icbLogueo.UseVisualStyleBackColor = true;
            icbLogueo.Click += icbLogueo_Click;
            // 
            // picOFF
            // 
            picOFF.BackColor = Color.Transparent;
            picOFF.Image = Properties.Resources.powerOff;
            picOFF.Location = new Point(345, 8);
            picOFF.Name = "picOFF";
            picOFF.Size = new Size(62, 58);
            picOFF.SizeMode = PictureBoxSizeMode.StretchImage;
            picOFF.TabIndex = 10;
            picOFF.TabStop = false;
            picOFF.Click += picOFF_Click;
            picOFF.MouseEnter += picOFF_MouseEnter;
            picOFF.MouseLeave += picOFF_MouseLeave;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            BackgroundImage = Properties.Resources.bgAurora;
            ClientSize = new Size(415, 448);
            Controls.Add(picOFF);
            Controls.Add(icbLogueo);
            Controls.Add(pictureBox1);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(txtContraseña);
            Controls.Add(txtUsuario);
            Font = new Font("Zrnic Rg", 10.7999992F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Form1";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Logueo";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)picOFF).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox txtUsuario;
        private Label label2;
        private TextBox txtContraseña;
        private Label label3;
        private PictureBox pictureBox1;
        private FontAwesome.Sharp.IconButton icbLogueo;
        private PictureBox picOFF;
    }
}
