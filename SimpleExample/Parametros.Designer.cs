namespace SimpleExample
{
    partial class Parametros
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Land_radio = new System.Windows.Forms.RadioButton();
            this.Brake_radio = new System.Windows.Forms.RadioButton();
            this.RTL_radio = new System.Windows.Forms.RadioButton();
            this.ALT_MAX_Box = new System.Windows.Forms.TextBox();
            this.enableBtn = new System.Windows.Forms.Button();
            this.RTL_ALT_lbl = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.leerBtn = new System.Windows.Forms.Button();
            this.escribirBtn = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cerrarBtn = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Land_radio);
            this.groupBox1.Controls.Add(this.Brake_radio);
            this.groupBox1.Controls.Add(this.RTL_radio);
            this.groupBox1.Controls.Add(this.ALT_MAX_Box);
            this.groupBox1.Controls.Add(this.enableBtn);
            this.groupBox1.Controls.Add(this.RTL_ALT_lbl);
            this.groupBox1.Controls.Add(this.trackBar1);
            this.groupBox1.Controls.Add(this.leerBtn);
            this.groupBox1.Controls.Add(this.escribirBtn);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 60);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(723, 503);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Parametros";
            // 
            // Land_radio
            // 
            this.Land_radio.AutoSize = true;
            this.Land_radio.Location = new System.Drawing.Point(419, 349);
            this.Land_radio.Name = "Land_radio";
            this.Land_radio.Size = new System.Drawing.Size(114, 41);
            this.Land_radio.TabIndex = 11;
            this.Land_radio.TabStop = true;
            this.Land_radio.Tag = "2";
            this.Land_radio.Text = "Land";
            this.Land_radio.UseVisualStyleBackColor = true;
            // 
            // Brake_radio
            // 
            this.Brake_radio.AutoSize = true;
            this.Brake_radio.Location = new System.Drawing.Point(564, 349);
            this.Brake_radio.Name = "Brake_radio";
            this.Brake_radio.Size = new System.Drawing.Size(125, 41);
            this.Brake_radio.TabIndex = 10;
            this.Brake_radio.TabStop = true;
            this.Brake_radio.Tag = "4";
            this.Brake_radio.Text = "Brake";
            this.Brake_radio.UseVisualStyleBackColor = true;
            // 
            // RTL_radio
            // 
            this.RTL_radio.AutoSize = true;
            this.RTL_radio.Location = new System.Drawing.Point(311, 349);
            this.RTL_radio.Name = "RTL_radio";
            this.RTL_radio.Size = new System.Drawing.Size(102, 41);
            this.RTL_radio.TabIndex = 9;
            this.RTL_radio.TabStop = true;
            this.RTL_radio.Tag = "1";
            this.RTL_radio.Text = "RTL";
            this.RTL_radio.UseVisualStyleBackColor = true;
            // 
            // ALT_MAX_Box
            // 
            this.ALT_MAX_Box.Location = new System.Drawing.Point(311, 407);
            this.ALT_MAX_Box.Name = "ALT_MAX_Box";
            this.ALT_MAX_Box.Size = new System.Drawing.Size(100, 43);
            this.ALT_MAX_Box.TabIndex = 1;
            // 
            // enableBtn
            // 
            this.enableBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.enableBtn.Location = new System.Drawing.Point(303, 283);
            this.enableBtn.Name = "enableBtn";
            this.enableBtn.Size = new System.Drawing.Size(110, 60);
            this.enableBtn.TabIndex = 6;
            this.enableBtn.Text = "NO";
            this.enableBtn.UseVisualStyleBackColor = false;
            this.enableBtn.Click += new System.EventHandler(this.enableBtn_Click);
            // 
            // RTL_ALT_lbl
            // 
            this.RTL_ALT_lbl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RTL_ALT_lbl.ForeColor = System.Drawing.Color.Red;
            this.RTL_ALT_lbl.Location = new System.Drawing.Point(485, 220);
            this.RTL_ALT_lbl.Name = "RTL_ALT_lbl";
            this.RTL_ALT_lbl.Size = new System.Drawing.Size(86, 60);
            this.RTL_ALT_lbl.TabIndex = 5;
            this.RTL_ALT_lbl.Text = "0";
            this.RTL_ALT_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(303, 223);
            this.trackBar1.Maximum = 40;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(143, 69);
            this.trackBar1.TabIndex = 4;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // leerBtn
            // 
            this.leerBtn.Location = new System.Drawing.Point(81, 74);
            this.leerBtn.Name = "leerBtn";
            this.leerBtn.Size = new System.Drawing.Size(227, 65);
            this.leerBtn.TabIndex = 2;
            this.leerBtn.Text = "Leer";
            this.leerBtn.UseVisualStyleBackColor = true;
            this.leerBtn.Click += new System.EventHandler(this.leerBtn_Click);
            // 
            // escribirBtn
            // 
            this.escribirBtn.Location = new System.Drawing.Point(362, 74);
            this.escribirBtn.Name = "escribirBtn";
            this.escribirBtn.Size = new System.Drawing.Size(209, 65);
            this.escribirBtn.TabIndex = 3;
            this.escribirBtn.Text = "Escribir";
            this.escribirBtn.UseVisualStyleBackColor = true;
            this.escribirBtn.Click += new System.EventHandler(this.escribirBtn_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 349);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(266, 37);
            this.label4.TabIndex = 3;
            this.label4.Text = "FENCE_ACTION";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 407);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(291, 37);
            this.label3.TabIndex = 2;
            this.label3.Text = "FENCE_ALT_MAX";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 295);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(271, 37);
            this.label2.TabIndex = 1;
            this.label2.Text = "FENCE_ENABLE";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(137, 232);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(155, 37);
            this.label1.TabIndex = 0;
            this.label1.Text = "RTL_ALT";
            // 
            // cerrarBtn
            // 
            this.cerrarBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cerrarBtn.Location = new System.Drawing.Point(201, 593);
            this.cerrarBtn.Name = "cerrarBtn";
            this.cerrarBtn.Size = new System.Drawing.Size(271, 43);
            this.cerrarBtn.TabIndex = 1;
            this.cerrarBtn.Text = "Cerrar";
            this.cerrarBtn.UseVisualStyleBackColor = true;
            this.cerrarBtn.Click += new System.EventHandler(this.cerrarBtn_Click);
            // 
            // Parametros
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(757, 672);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cerrarBtn);
            this.Name = "Parametros";
            this.Text = "Paramatros";
            this.Load += new System.EventHandler(this.Paramatros_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Button cerrarBtn;
        private System.Windows.Forms.Button leerBtn;
        private System.Windows.Forms.Button escribirBtn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ALT_MAX_Box;
        private System.Windows.Forms.Button enableBtn;
        private System.Windows.Forms.Label RTL_ALT_lbl;
        private System.Windows.Forms.RadioButton Land_radio;
        private System.Windows.Forms.RadioButton Brake_radio;
        private System.Windows.Forms.RadioButton RTL_radio;
    }
}