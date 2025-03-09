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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.RTL_ALT_lbl = new System.Windows.Forms.Label();
            this.enableBtn = new System.Windows.Forms.Button();
            this.ALT_MAX_Box = new System.Windows.Forms.TextBox();
            this.RTL_radio = new System.Windows.Forms.RadioButton();
            this.Brake_radio = new System.Windows.Forms.RadioButton();
            this.Land_radio = new System.Windows.Forms.RadioButton();
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
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 60);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(521, 321);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Parametros";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(101, 89);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "RTL_ALT";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 131);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(178, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = "FENCE_ENABLE";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 211);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(192, 25);
            this.label3.TabIndex = 2;
            this.label3.Text = "FENCE_ALT_MAX";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 173);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(173, 25);
            this.label4.TabIndex = 3;
            this.label4.Text = "FENCE_ACTION";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(106, 272);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(271, 43);
            this.button1.TabIndex = 1;
            this.button1.Text = "Cerrar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(70, 30);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(147, 29);
            this.button2.TabIndex = 2;
            this.button2.Text = "Leer";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(270, 30);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(161, 29);
            this.button3.TabIndex = 3;
            this.button3.Text = "Escribir";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(234, 85);
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(143, 45);
            this.trackBar1.TabIndex = 4;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // RTL_ALT_lbl
            // 
            this.RTL_ALT_lbl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RTL_ALT_lbl.ForeColor = System.Drawing.Color.Red;
            this.RTL_ALT_lbl.Location = new System.Drawing.Point(383, 85);
            this.RTL_ALT_lbl.Name = "RTL_ALT_lbl";
            this.RTL_ALT_lbl.Size = new System.Drawing.Size(48, 33);
            this.RTL_ALT_lbl.TabIndex = 5;
            this.RTL_ALT_lbl.Text = "0";
            this.RTL_ALT_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // enableBtn
            // 
            this.enableBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.enableBtn.Location = new System.Drawing.Point(234, 123);
            this.enableBtn.Name = "enableBtn";
            this.enableBtn.Size = new System.Drawing.Size(62, 41);
            this.enableBtn.TabIndex = 6;
            this.enableBtn.Text = "NO";
            this.enableBtn.UseVisualStyleBackColor = false;
            this.enableBtn.Click += new System.EventHandler(this.enableBtn_Click);
            // 
            // ALT_MAX_Box
            // 
            this.ALT_MAX_Box.Location = new System.Drawing.Point(231, 205);
            this.ALT_MAX_Box.Name = "ALT_MAX_Box";
            this.ALT_MAX_Box.Size = new System.Drawing.Size(100, 31);
            this.ALT_MAX_Box.TabIndex = 1;
            // 
            // RTL_radio
            // 
            this.RTL_radio.AutoSize = true;
            this.RTL_radio.Location = new System.Drawing.Point(231, 170);
            this.RTL_radio.Name = "RTL_radio";
            this.RTL_radio.Size = new System.Drawing.Size(70, 29);
            this.RTL_radio.TabIndex = 9;
            this.RTL_radio.TabStop = true;
            this.RTL_radio.Tag = "1";
            this.RTL_radio.Text = "RTL";
            this.RTL_radio.UseVisualStyleBackColor = true;
            // 
            // Brake_radio
            // 
            this.Brake_radio.AutoSize = true;
            this.Brake_radio.Location = new System.Drawing.Point(395, 170);
            this.Brake_radio.Name = "Brake_radio";
            this.Brake_radio.Size = new System.Drawing.Size(86, 29);
            this.Brake_radio.TabIndex = 10;
            this.Brake_radio.TabStop = true;
            this.Brake_radio.Tag = "4";
            this.Brake_radio.Text = "Brake";
            this.Brake_radio.UseVisualStyleBackColor = true;
            // 
            // Land_radio
            // 
            this.Land_radio.AutoSize = true;
            this.Land_radio.Location = new System.Drawing.Point(311, 170);
            this.Land_radio.Name = "Land_radio";
            this.Land_radio.Size = new System.Drawing.Size(78, 29);
            this.Land_radio.TabIndex = 11;
            this.Land_radio.TabStop = true;
            this.Land_radio.Tag = "2";
            this.Land_radio.Text = "Land";
            this.Land_radio.UseVisualStyleBackColor = true;
            // 
            // Parametros
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(543, 387);
            this.Controls.Add(this.groupBox1);
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
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
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