namespace SimpleExample
{
    partial class Inicio
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
            this.simulación = new System.Windows.Forms.Button();
            this.mercadrona = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // simulación
            // 
            this.simulación.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.simulación.Location = new System.Drawing.Point(403, 180);
            this.simulación.Name = "simulación";
            this.simulación.Size = new System.Drawing.Size(317, 90);
            this.simulación.TabIndex = 3;
            this.simulación.Text = "Simulación";
            this.simulación.UseVisualStyleBackColor = true;
            this.simulación.Click += new System.EventHandler(this.simulación_Click);
            // 
            // mercadrona
            // 
            this.mercadrona.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mercadrona.Location = new System.Drawing.Point(80, 180);
            this.mercadrona.Name = "mercadrona";
            this.mercadrona.Size = new System.Drawing.Size(317, 90);
            this.mercadrona.TabIndex = 2;
            this.mercadrona.Text = "Mercadrona";
            this.mercadrona.UseVisualStyleBackColor = true;
            this.mercadrona.Click += new System.EventHandler(this.mercadrona_Click);
            // 
            // Inicio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.simulación);
            this.Controls.Add(this.mercadrona);
            this.Name = "Inicio";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button simulación;
        private System.Windows.Forms.Button mercadrona;
    }
}