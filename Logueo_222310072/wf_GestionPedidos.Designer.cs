namespace Logueo_222310072
{
    partial class wf_GestionPedidos
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
            lbNOMBRE = new Label();
            lbIDUSUARIO = new Label();
            label1 = new Label();
            dataGridView1 = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // lbNOMBRE
            // 
            lbNOMBRE.AutoSize = true;
            lbNOMBRE.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbNOMBRE.Location = new Point(50, 104);
            lbNOMBRE.Name = "lbNOMBRE";
            lbNOMBRE.Size = new Size(136, 22);
            lbNOMBRE.TabIndex = 7;
            lbNOMBRE.Text = "NombreUsuario";
            // 
            // lbIDUSUARIO
            // 
            lbIDUSUARIO.AutoSize = true;
            lbIDUSUARIO.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbIDUSUARIO.Location = new Point(50, 82);
            lbIDUSUARIO.Name = "lbIDUSUARIO";
            lbIDUSUARIO.Size = new Size(88, 22);
            lbIDUSUARIO.TabIndex = 6;
            lbIDUSUARIO.Text = "idUsuario";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Times New Roman", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(29, 21);
            label1.Name = "label1";
            label1.Size = new Size(218, 34);
            label1.TabIndex = 5;
            label1.Text = "Pedidos en Linea";
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(76, 154);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(518, 229);
            dataGridView1.TabIndex = 8;
            // 
            // wf_GestionPedidos
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(dataGridView1);
            Controls.Add(lbNOMBRE);
            Controls.Add(lbIDUSUARIO);
            Controls.Add(label1);
            Name = "wf_GestionPedidos";
            Text = "wf_GestionPedidos";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        public Label lbNOMBRE;
        public Label lbIDUSUARIO;
        private Label label1;
        private DataGridView dataGridView1;
    }
}