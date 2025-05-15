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
            this.table_pedidos = new System.Windows.Forms.TableLayoutPanel();
            this.añadir = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox_destinatario = new System.Windows.Forms.TextBox();
            this.textbox_direccion = new System.Windows.Forms.TextBox();
            this.textBox_pedido = new System.Windows.Forms.TextBox();
            this.panelMapa = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // table_pedidos
            // 
            this.table_pedidos.ColumnCount = 2;
            this.table_pedidos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.table_pedidos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.table_pedidos.Location = new System.Drawing.Point(115, 379);
            this.table_pedidos.Name = "table_pedidos";
            this.table_pedidos.RowCount = 2;
            this.table_pedidos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.table_pedidos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.table_pedidos.Size = new System.Drawing.Size(1168, 373);
            this.table_pedidos.TabIndex = 0;
            // 
            // añadir
            // 
            this.añadir.Location = new System.Drawing.Point(98, 91);
            this.añadir.Name = "añadir";
            this.añadir.Size = new System.Drawing.Size(102, 41);
            this.añadir.TabIndex = 1;
            this.añadir.Text = "Añadir";
            this.añadir.UseVisualStyleBackColor = true;
            this.añadir.Click += new System.EventHandler(this.añadir_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(98, 165);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // textBox_destinatario
            // 
            this.textBox_destinatario.Location = new System.Drawing.Point(265, 106);
            this.textBox_destinatario.Name = "textBox_destinatario";
            this.textBox_destinatario.Size = new System.Drawing.Size(142, 26);
            this.textBox_destinatario.TabIndex = 3;
            // 
            // textbox_direccion
            // 
            this.textbox_direccion.Location = new System.Drawing.Point(448, 106);
            this.textbox_direccion.Name = "textbox_direccion";
            this.textbox_direccion.Size = new System.Drawing.Size(142, 26);
            this.textbox_direccion.TabIndex = 4;
            // 
            // textBox_pedido
            // 
            this.textBox_pedido.Location = new System.Drawing.Point(625, 106);
            this.textBox_pedido.Name = "textBox_pedido";
            this.textBox_pedido.Size = new System.Drawing.Size(142, 26);
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
            // Pedidos_forms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1419, 843);
            this.Controls.Add(this.panelMapa);
            this.Controls.Add(this.textBox_pedido);
            this.Controls.Add(this.textbox_direccion);
            this.Controls.Add(this.textBox_destinatario);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.añadir);
            this.Controls.Add(this.table_pedidos);
            this.Name = "Pedidos_forms";
            this.Text = "Pedidos_forms";
            this.Load += new System.EventHandler(this.Pedidos_forms_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel table_pedidos;
        private System.Windows.Forms.Button añadir;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox_destinatario;
        private System.Windows.Forms.TextBox textbox_direccion;
        private System.Windows.Forms.TextBox textBox_pedido;
        private System.Windows.Forms.Panel panelMapa;
    }
}