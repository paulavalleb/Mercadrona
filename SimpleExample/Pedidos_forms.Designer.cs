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
            this.finalizar = new System.Windows.Forms.Button();
            this.textBox_destinatario = new System.Windows.Forms.TextBox();
            this.textbox_direccion = new System.Windows.Forms.TextBox();
            this.textBox_pedido = new System.Windows.Forms.TextBox();
            this.panelMapa = new System.Windows.Forms.Panel();
            this.anadir = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.dataGrid = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // finalizar
            // 
            this.finalizar.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.finalizar.Location = new System.Drawing.Point(115, 284);
            this.finalizar.Name = "finalizar";
            this.finalizar.Size = new System.Drawing.Size(251, 41);
            this.finalizar.TabIndex = 1;
            this.finalizar.Text = "Finalizar pedido";
            this.finalizar.UseVisualStyleBackColor = true;
            this.finalizar.Click += new System.EventHandler(this.finalizar_click);
            // 
            // textBox_destinatario
            // 
            this.textBox_destinatario.Location = new System.Drawing.Point(115, 220);
            this.textBox_destinatario.Name = "textBox_destinatario";
            this.textBox_destinatario.Size = new System.Drawing.Size(224, 26);
            this.textBox_destinatario.TabIndex = 3;
            // 
            // textbox_direccion
            // 
            this.textbox_direccion.Location = new System.Drawing.Point(115, 164);
            this.textbox_direccion.Name = "textbox_direccion";
            this.textbox_direccion.Size = new System.Drawing.Size(224, 26);
            this.textbox_direccion.TabIndex = 4;
            // 
            // textBox_pedido
            // 
            this.textBox_pedido.Location = new System.Drawing.Point(115, 112);
            this.textBox_pedido.Name = "textBox_pedido";
            this.textBox_pedido.Size = new System.Drawing.Size(499, 26);
            this.textBox_pedido.TabIndex = 5;
            // 
            // panelMapa
            // 
            this.panelMapa.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.panelMapa.Location = new System.Drawing.Point(907, 33);
            this.panelMapa.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelMapa.Name = "panelMapa";
            this.panelMapa.Size = new System.Drawing.Size(376, 326);
            this.panelMapa.TabIndex = 46;
            // 
            // anadir
            // 
            this.anadir.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.anadir.Location = new System.Drawing.Point(639, 52);
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
            this.comboBox1.Location = new System.Drawing.Point(115, 52);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(183, 28);
            this.comboBox1.TabIndex = 48;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(356, 52);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(183, 28);
            this.comboBox2.TabIndex = 49;
            // 
            // dataGrid
            // 
            this.dataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGrid.Location = new System.Drawing.Point(115, 393);
            this.dataGrid.Name = "dataGrid";
            this.dataGrid.RowHeadersWidth = 62;
            this.dataGrid.RowTemplate.Height = 28;
            this.dataGrid.Size = new System.Drawing.Size(1168, 394);
            this.dataGrid.TabIndex = 50;
            // 
            // Pedidos_forms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1419, 843);
            this.Controls.Add(this.dataGrid);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.anadir);
            this.Controls.Add(this.panelMapa);
            this.Controls.Add(this.textBox_pedido);
            this.Controls.Add(this.textbox_direccion);
            this.Controls.Add(this.textBox_destinatario);
            this.Controls.Add(this.finalizar);
            this.Name = "Pedidos_forms";
            this.Text = "Pedidos_forms";
            this.Load += new System.EventHandler(this.Pedidos_forms_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button finalizar;
        private System.Windows.Forms.TextBox textBox_destinatario;
        private System.Windows.Forms.TextBox textbox_direccion;
        private System.Windows.Forms.TextBox textBox_pedido;
        private System.Windows.Forms.Panel panelMapa;
        private System.Windows.Forms.Button anadir;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.DataGridView dataGrid;
    }
}