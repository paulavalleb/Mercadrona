namespace SimpleExample
{
    partial class Pedidos_forms
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
            this.components = new System.ComponentModel.Container();
            this.finalizar = new System.Windows.Forms.Button();
            this.textBox_destinatario = new System.Windows.Forms.TextBox();
            this.textbox_direccion = new System.Windows.Forms.TextBox();
            this.panelMapa = new System.Windows.Forms.Panel();
            this.anadir = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.dataGrid = new System.Windows.Forms.DataGridView();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox_pedido = new System.Windows.Forms.TextBox();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.textBox11 = new System.Windows.Forms.TextBox();
            this.ejemplo_btn = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.cargar = new System.Windows.Forms.Button();
            this.eliminar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // finalizar
            // 
            this.finalizar.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.finalizar.Location = new System.Drawing.Point(77, 401);
            this.finalizar.Name = "finalizar";
            this.finalizar.Size = new System.Drawing.Size(251, 41);
            this.finalizar.TabIndex = 1;
            this.finalizar.Text = "Finalizar pedido";
            this.finalizar.UseVisualStyleBackColor = true;
            this.finalizar.Click += new System.EventHandler(this.finalizar_click);
            // 
            // textBox_destinatario
            // 
            this.textBox_destinatario.Location = new System.Drawing.Point(17, 231);
            this.textBox_destinatario.Name = "textBox_destinatario";
            this.textBox_destinatario.Size = new System.Drawing.Size(224, 26);
            this.textBox_destinatario.TabIndex = 3;
            // 
            // textbox_direccion
            // 
            this.textbox_direccion.Location = new System.Drawing.Point(17, 167);
            this.textbox_direccion.Name = "textbox_direccion";
            this.textbox_direccion.Size = new System.Drawing.Size(224, 26);
            this.textbox_direccion.TabIndex = 4;
            // 
            // panelMapa
            // 
            this.panelMapa.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.panelMapa.Location = new System.Drawing.Point(907, 101);
            this.panelMapa.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelMapa.Name = "panelMapa";
            this.panelMapa.Size = new System.Drawing.Size(376, 326);
            this.panelMapa.TabIndex = 46;
            // 
            // anadir
            // 
            this.anadir.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.anadir.Location = new System.Drawing.Point(469, 36);
            this.anadir.Name = "anadir";
            this.anadir.Size = new System.Drawing.Size(102, 41);
            this.anadir.TabIndex = 47;
            this.anadir.Text = "Añadir";
            this.anadir.UseVisualStyleBackColor = true;
            this.anadir.Click += new System.EventHandler(this.anadir_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(17, 36);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(183, 28);
            this.comboBox1.TabIndex = 48;
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(246, 39);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(183, 28);
            this.comboBox2.TabIndex = 49;
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // dataGrid
            // 
            this.dataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGrid.Location = new System.Drawing.Point(77, 478);
            this.dataGrid.Name = "dataGrid";
            this.dataGrid.RowHeadersWidth = 62;
            this.dataGrid.RowTemplate.Height = 28;
            this.dataGrid.Size = new System.Drawing.Size(1206, 385);
            this.dataGrid.TabIndex = 50;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.White;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.ForeColor = System.Drawing.Color.Green;
            this.textBox1.Location = new System.Drawing.Point(115, 33);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(294, 33);
            this.textBox1.TabIndex = 51;
            this.textBox1.Text = "Productos";
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.White;
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Font = new System.Drawing.Font("Arial Rounded MT Bold", 20F);
            this.textBox2.ForeColor = System.Drawing.Color.Green;
            this.textBox2.Location = new System.Drawing.Point(980, 21);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(323, 47);
            this.textBox2.TabIndex = 52;
            this.textBox2.Text = "MERCADRONA";
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.Color.White;
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox3.Font = new System.Drawing.Font("Arial Rounded MT Bold", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox3.ForeColor = System.Drawing.Color.Green;
            this.textBox3.Location = new System.Drawing.Point(115, 107);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(118, 26);
            this.textBox3.TabIndex = 53;
            this.textBox3.Text = "Producto:";
            // 
            // textBox4
            // 
            this.textBox4.BackColor = System.Drawing.Color.White;
            this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox4.Font = new System.Drawing.Font("Arial Rounded MT Bold", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox4.ForeColor = System.Drawing.Color.Green;
            this.textBox4.Location = new System.Drawing.Point(246, 7);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(122, 26);
            this.textBox4.TabIndex = 54;
            this.textBox4.Text = "Cantidad:";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Green;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(18, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(588, 270);
            this.panel1.TabIndex = 55;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Green;
            this.panel3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Controls.Add(this.panel1);
            this.panel3.Location = new System.Drawing.Point(77, 81);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(625, 302);
            this.panel3.TabIndex = 57;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.SystemColors.Window;
            this.panel4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.pictureBox1);
            this.panel4.Controls.Add(this.pictureBox2);
            this.panel4.Controls.Add(this.textBox8);
            this.panel4.Controls.Add(this.textBox7);
            this.panel4.Controls.Add(this.textBox5);
            this.panel4.Controls.Add(this.textBox_pedido);
            this.panel4.Controls.Add(this.comboBox1);
            this.panel4.Controls.Add(this.textBox4);
            this.panel4.Controls.Add(this.comboBox2);
            this.panel4.Controls.Add(this.textbox_direccion);
            this.panel4.Controls.Add(this.anadir);
            this.panel4.Controls.Add(this.textBox_destinatario);
            this.panel4.Location = new System.Drawing.Point(18, 17);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(588, 262);
            this.panel4.TabIndex = 57;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(258, 167);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(34, 26);
            this.pictureBox1.TabIndex = 62;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseEnter += new System.EventHandler(this.pictureBox1_MouseEnter);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(537, 103);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(34, 26);
            this.pictureBox2.TabIndex = 61;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.MouseEnter += new System.EventHandler(this.pictureBox2_MouseEnter);
            // 
            // textBox8
            // 
            this.textBox8.BackColor = System.Drawing.Color.White;
            this.textBox8.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox8.Font = new System.Drawing.Font("Arial Rounded MT Bold", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox8.ForeColor = System.Drawing.Color.Green;
            this.textBox8.Location = new System.Drawing.Point(17, 135);
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(189, 26);
            this.textBox8.TabIndex = 60;
            this.textBox8.Text = "Dirección:";
            // 
            // textBox7
            // 
            this.textBox7.BackColor = System.Drawing.Color.White;
            this.textBox7.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox7.Font = new System.Drawing.Font("Arial Rounded MT Bold", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox7.ForeColor = System.Drawing.Color.Green;
            this.textBox7.Location = new System.Drawing.Point(18, 199);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(189, 26);
            this.textBox7.TabIndex = 59;
            this.textBox7.Text = "Nombre:";
            // 
            // textBox5
            // 
            this.textBox5.BackColor = System.Drawing.Color.White;
            this.textBox5.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox5.Font = new System.Drawing.Font("Arial Rounded MT Bold", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox5.ForeColor = System.Drawing.Color.Green;
            this.textBox5.Location = new System.Drawing.Point(18, 71);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(189, 26);
            this.textBox5.TabIndex = 58;
            this.textBox5.Text = "Lista de compra:";
            // 
            // textBox_pedido
            // 
            this.textBox_pedido.Location = new System.Drawing.Point(18, 103);
            this.textBox_pedido.Name = "textBox_pedido";
            this.textBox_pedido.Size = new System.Drawing.Size(499, 26);
            this.textBox_pedido.TabIndex = 5;
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(508, 414);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(106, 26);
            this.textBox6.TabIndex = 63;
            // 
            // textBox9
            // 
            this.textBox9.Location = new System.Drawing.Point(766, 411);
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(106, 26);
            this.textBox9.TabIndex = 64;
            // 
            // textBox10
            // 
            this.textBox10.BackColor = System.Drawing.Color.White;
            this.textBox10.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox10.Font = new System.Drawing.Font("Arial Rounded MT Bold", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox10.ForeColor = System.Drawing.Color.Green;
            this.textBox10.Location = new System.Drawing.Point(355, 414);
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new System.Drawing.Size(189, 26);
            this.textBox10.TabIndex = 63;
            this.textBox10.Text = "Precio total:";
            // 
            // textBox11
            // 
            this.textBox11.BackColor = System.Drawing.Color.White;
            this.textBox11.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox11.Font = new System.Drawing.Font("Arial Rounded MT Bold", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox11.ForeColor = System.Drawing.Color.Green;
            this.textBox11.Location = new System.Drawing.Point(628, 412);
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new System.Drawing.Size(132, 26);
            this.textBox11.TabIndex = 65;
            this.textBox11.Text = "Peso total:";
            // 
            // ejemplo_btn
            // 
            this.ejemplo_btn.Location = new System.Drawing.Point(718, 81);
            this.ejemplo_btn.Name = "ejemplo_btn";
            this.ejemplo_btn.Size = new System.Drawing.Size(133, 30);
            this.ejemplo_btn.TabIndex = 66;
            this.ejemplo_btn.Text = "Ejemplo";
            this.ejemplo_btn.UseVisualStyleBackColor = true;
            this.ejemplo_btn.Click += new System.EventHandler(this.ejemplo_btn_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(718, 134);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(133, 30);
            this.button1.TabIndex = 67;
            this.button1.Text = "Modificar pedido";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cargar
            // 
            this.cargar.Location = new System.Drawing.Point(718, 183);
            this.cargar.Name = "cargar";
            this.cargar.Size = new System.Drawing.Size(133, 30);
            this.cargar.TabIndex = 68;
            this.cargar.Text = "Cargar";
            this.cargar.UseVisualStyleBackColor = true;
            this.cargar.Click += new System.EventHandler(this.cargar_Click);
            // 
            // eliminar
            // 
            this.eliminar.Location = new System.Drawing.Point(718, 235);
            this.eliminar.Name = "eliminar";
            this.eliminar.Size = new System.Drawing.Size(133, 30);
            this.eliminar.TabIndex = 69;
            this.eliminar.Text = "Eliminar pedidos";
            this.eliminar.UseVisualStyleBackColor = true;
            this.eliminar.Click += new System.EventHandler(this.eliminar_Click);
            // 
            // Pedidos_forms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1419, 875);
            this.Controls.Add(this.eliminar);
            this.Controls.Add(this.cargar);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ejemplo_btn);
            this.Controls.Add(this.textBox9);
            this.Controls.Add(this.textBox11);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.textBox10);
            this.Controls.Add(this.finalizar);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.dataGrid);
            this.Controls.Add(this.panelMapa);
            this.Controls.Add(this.panel3);
            this.Name = "Pedidos_forms";
            this.Text = "Pedidos_forms";
            this.Load += new System.EventHandler(this.Pedidos_forms_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button finalizar;
        private System.Windows.Forms.TextBox textBox_destinatario;
        private System.Windows.Forms.TextBox textbox_direccion;
        private System.Windows.Forms.Panel panelMapa;
        private System.Windows.Forms.Button anadir;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.DataGridView dataGrid;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox textBox_pedido;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.TextBox textBox10;
        private System.Windows.Forms.TextBox textBox11;
        private System.Windows.Forms.Button ejemplo_btn;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button cargar;
        private System.Windows.Forms.Button eliminar;
    }
}