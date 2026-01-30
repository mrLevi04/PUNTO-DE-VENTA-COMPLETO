namespace Logueo_222310072
{
    partial class wfGestionReservas
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
            label1 = new Label();
            lbNOMBRE = new Label();
            lbIDUSUARIO = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Times New Roman", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(85, 34);
            label1.TabIndex = 0;
            label1.Text = "Menu";
            // 
            // lbNOMBRE
            // 
            lbNOMBRE.AutoSize = true;
            lbNOMBRE.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbNOMBRE.Location = new Point(33, 92);
            lbNOMBRE.Name = "lbNOMBRE";
            lbNOMBRE.Size = new Size(136, 22);
            lbNOMBRE.TabIndex = 4;
            lbNOMBRE.Text = "NombreUsuario";
            // 
            // lbIDUSUARIO
            // 
            lbIDUSUARIO.AutoSize = true;
            lbIDUSUARIO.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbIDUSUARIO.Location = new Point(33, 70);
            lbIDUSUARIO.Name = "lbIDUSUARIO";
            lbIDUSUARIO.Size = new Size(88, 22);
            lbIDUSUARIO.TabIndex = 3;
            lbIDUSUARIO.Text = "idUsuario";
            // 
            // wfGestionReservas
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(609, 450);
            Controls.Add(lbNOMBRE);
            Controls.Add(lbIDUSUARIO);
            Controls.Add(label1);
            Name = "wfGestionReservas";
            Text = " ";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        public Label lbNOMBRE;
        public Label lbIDUSUARIO;
    }
}