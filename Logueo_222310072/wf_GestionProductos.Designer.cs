namespace Logueo_222310072
{
    partial class wf_GestionProductos
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
            label8 = new Label();
            lbIDUSUARIO = new Label();
            label1 = new Label();
            label9 = new Label();
            lbNOMBRE = new Label();
            txtID_P = new TextBox();
            label2 = new Label();
            label5 = new Label();
            label3 = new Label();
            label4 = new Label();
            label6 = new Label();
            dgvProductos = new DataGridView();
            txtNombreP = new TextBox();
            txtCostoP = new TextBox();
            txtCantP = new TextBox();
            btnAltaProducto = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvProductos).BeginInit();
            SuspendLayout();
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Zrnic Rg", 12F);
            label8.ForeColor = Color.White;
            label8.Location = new Point(298, 9);
            label8.Name = "label8";
            label8.Size = new Size(156, 24);
            label8.TabIndex = 45;
            label8.Text = "Usuario Logueado:";
            // 
            // lbIDUSUARIO
            // 
            lbIDUSUARIO.AutoSize = true;
            lbIDUSUARIO.Font = new Font("Zrnic Rg", 12F);
            lbIDUSUARIO.ForeColor = Color.White;
            lbIDUSUARIO.Location = new Point(132, 9);
            lbIDUSUARIO.Name = "lbIDUSUARIO";
            lbIDUSUARIO.Size = new Size(84, 24);
            lbIDUSUARIO.TabIndex = 28;
            lbIDUSUARIO.Text = "idUsuario";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Zrnic Rg", 17.9999981F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(12, 47);
            label1.Name = "label1";
            label1.Size = new Size(303, 35);
            label1.TabIndex = 27;
            label1.Text = "Productos Disponibles";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Zrnic Rg", 12F);
            label9.ForeColor = Color.White;
            label9.Location = new Point(12, 9);
            label9.Name = "label9";
            label9.Size = new Size(122, 24);
            label9.TabIndex = 44;
            label9.Text = "ID del Usuario:";
            // 
            // lbNOMBRE
            // 
            lbNOMBRE.AutoSize = true;
            lbNOMBRE.Font = new Font("Zrnic Rg", 12F);
            lbNOMBRE.ForeColor = Color.White;
            lbNOMBRE.Location = new Point(464, 9);
            lbNOMBRE.Name = "lbNOMBRE";
            lbNOMBRE.Size = new Size(132, 24);
            lbNOMBRE.TabIndex = 29;
            lbNOMBRE.Text = "NombreUsuario";
            // 
            // txtID_P
            // 
            txtID_P.Location = new Point(178, 426);
            txtID_P.Name = "txtID_P";
            txtID_P.Size = new Size(49, 27);
            txtID_P.TabIndex = 36;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Zrnic Rg", 12F);
            label2.ForeColor = Color.White;
            label2.Location = new Point(32, 429);
            label2.Name = "label2";
            label2.Size = new Size(103, 24);
            label2.TabIndex = 32;
            label2.Text = "Id Producto:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Zrnic Rg", 12F);
            label5.ForeColor = Color.White;
            label5.Location = new Point(32, 531);
            label5.Name = "label5";
            label5.Size = new Size(82, 24);
            label5.TabIndex = 35;
            label5.Text = "Cantidad:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Zrnic Rg", 12F);
            label3.ForeColor = Color.White;
            label3.Location = new Point(32, 460);
            label3.Name = "label3";
            label3.Size = new Size(76, 24);
            label3.TabIndex = 33;
            label3.Text = "Nombre:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Zrnic Rg", 12F);
            label4.ForeColor = Color.White;
            label4.Location = new Point(32, 496);
            label4.Name = "label4";
            label4.Size = new Size(57, 24);
            label4.TabIndex = 34;
            label4.Text = "Costo:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Zrnic Rg", 17.9999981F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.ForeColor = Color.White;
            label6.Location = new Point(19, 379);
            label6.Name = "label6";
            label6.Size = new Size(285, 35);
            label6.TabIndex = 46;
            label6.Text = "Gestionar Productos:";
            // 
            // dgvProductos
            // 
            dgvProductos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProductos.Location = new Point(15, 94);
            dgvProductos.Name = "dgvProductos";
            dgvProductos.RowHeadersWidth = 51;
            dgvProductos.Size = new Size(577, 267);
            dgvProductos.TabIndex = 47;
            // 
            // txtNombreP
            // 
            txtNombreP.Location = new Point(178, 460);
            txtNombreP.Name = "txtNombreP";
            txtNombreP.Size = new Size(171, 27);
            txtNombreP.TabIndex = 48;
            // 
            // txtCostoP
            // 
            txtCostoP.Location = new Point(178, 496);
            txtCostoP.Name = "txtCostoP";
            txtCostoP.Size = new Size(81, 27);
            txtCostoP.TabIndex = 49;
            // 
            // txtCantP
            // 
            txtCantP.Location = new Point(178, 531);
            txtCantP.Name = "txtCantP";
            txtCantP.Size = new Size(54, 27);
            txtCantP.TabIndex = 50;
            // 
            // btnAltaProducto
            // 
            btnAltaProducto.BackgroundImage = Properties.Resources.button;
            btnAltaProducto.BackgroundImageLayout = ImageLayout.Stretch;
            btnAltaProducto.FlatStyle = FlatStyle.Popup;
            btnAltaProducto.Font = new Font("Zrnic Rg", 12F);
            btnAltaProducto.ForeColor = Color.White;
            btnAltaProducto.Location = new Point(32, 585);
            btnAltaProducto.Name = "btnAltaProducto";
            btnAltaProducto.Size = new Size(115, 35);
            btnAltaProducto.TabIndex = 51;
            btnAltaProducto.Text = "Añadir";
            btnAltaProducto.UseVisualStyleBackColor = true;
            btnAltaProducto.Click += btnAltaProducto_Click;
            // 
            // wf_GestionProductos
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaptionText;
            ClientSize = new Size(604, 683);
            Controls.Add(btnAltaProducto);
            Controls.Add(txtCantP);
            Controls.Add(txtCostoP);
            Controls.Add(txtNombreP);
            Controls.Add(dgvProductos);
            Controls.Add(label6);
            Controls.Add(label8);
            Controls.Add(lbIDUSUARIO);
            Controls.Add(label1);
            Controls.Add(label9);
            Controls.Add(lbNOMBRE);
            Controls.Add(txtID_P);
            Controls.Add(label2);
            Controls.Add(label5);
            Controls.Add(label3);
            Controls.Add(label4);
            Name = "wf_GestionProductos";
            Text = "wf_GestionProductos";
            ((System.ComponentModel.ISupportInitialize)dgvProductos).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnAgregaImg;
        private Button btnRegistra;
        private DataGridView dataGridView1;
        public Label label8;
        public Label lbIDUSUARIO;
        private Label label1;
        private TextBox txtTipoU;
        public Label label9;
        private TextBox txtContra;
        private Button btnReset;
        public Label lbNOMBRE;
        private TextBox txtNombre;
        private Button btnBaja;
        private Button btnAltaProducto;
        private TextBox txtID_P;
        public Label label2;
        public Label label5;
        public Label label3;
        public Label label4;
        private Label label6;
        private DataGridView dgvProductos;
        private TextBox txtNombreP;
        private TextBox txtCostoP;
        private TextBox txtCantP;
    }
}