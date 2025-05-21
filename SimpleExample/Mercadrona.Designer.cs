namespace SimpleExample
{
    partial class Mercadrona
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
            this.CMB_comport = new System.Windows.Forms.ComboBox();
            this.despegarBtn = new System.Windows.Forms.Button();
            this.RTLBtn = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.aterrizarBtn = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.headLbl = new System.Windows.Forms.Label();
            this.longitudLbl = new System.Windows.Forms.Label();
            this.latitudLbl = new System.Windows.Forms.Label();
            this.altitudLbl = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.detenerTelemetria = new System.Windows.Forms.Button();
            this.IniciaTelemetría = new System.Windows.Forms.Button();
            this.ponGuiadoBtn = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button5 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button19 = new System.Windows.Forms.Button();
            this.button24 = new System.Windows.Forms.Button();
            this.button25 = new System.Windows.Forms.Button();
            this.button26 = new System.Windows.Forms.Button();
            this.button21 = new System.Windows.Forms.Button();
            this.button20 = new System.Windows.Forms.Button();
            this.button17 = new System.Windows.Forms.Button();
            this.button16 = new System.Windows.Forms.Button();
            this.button15 = new System.Windows.Forms.Button();
            this.button14 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.velocidadLbl = new System.Windows.Forms.Label();
            this.trackBarSpeed = new System.Windows.Forms.TrackBar();
            this.label4 = new System.Windows.Forms.Label();
            this.trackBarHeading = new System.Windows.Forms.TrackBar();
            this.headingLbl = new System.Windows.Forms.Label();
            this.trackBarDistancia = new System.Windows.Forms.TrackBar();
            this.pasoLbl = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.alturaBox = new System.Windows.Forms.TextBox();
            this.panelMapa = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxNumDrons = new System.Windows.Forms.TextBox();
            this.Connectar_button = new System.Windows.Forms.Button();
            this.desplegable = new System.Windows.Forms.ComboBox();
            this.prodRadio = new System.Windows.Forms.RadioButton();
            this.simRadio = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.parámetrosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pedidos = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.cargar_pedidos = new System.Windows.Forms.Button();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarHeading)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarDistancia)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // CMB_comport
            // 
            this.CMB_comport.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CMB_comport.FormattingEnabled = true;
            this.CMB_comport.Location = new System.Drawing.Point(160, 62);
            this.CMB_comport.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.CMB_comport.Name = "CMB_comport";
            this.CMB_comport.Size = new System.Drawing.Size(146, 37);
            this.CMB_comport.TabIndex = 0;
            this.CMB_comport.Text = "Elije COM";
            this.CMB_comport.Visible = false;
            this.CMB_comport.Click += new System.EventHandler(this.CMB_comport_Click);
            // 
            // despegarBtn
            // 
            this.despegarBtn.BackColor = System.Drawing.Color.DarkOrange;
            this.despegarBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.despegarBtn.Location = new System.Drawing.Point(182, 162);
            this.despegarBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.despegarBtn.Name = "despegarBtn";
            this.despegarBtn.Size = new System.Drawing.Size(132, 48);
            this.despegarBtn.TabIndex = 5;
            this.despegarBtn.Text = "Despegar";
            this.despegarBtn.UseVisualStyleBackColor = false;
            this.despegarBtn.Click += new System.EventHandler(this.despegarBtn_Click);
            // 
            // RTLBtn
            // 
            this.RTLBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.RTLBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RTLBtn.Location = new System.Drawing.Point(322, 162);
            this.RTLBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.RTLBtn.Name = "RTLBtn";
            this.RTLBtn.Size = new System.Drawing.Size(105, 48);
            this.RTLBtn.TabIndex = 10;
            this.RTLBtn.Text = "RTL";
            this.RTLBtn.UseVisualStyleBackColor = false;
            this.RTLBtn.Click += new System.EventHandler(this.RTLBtn_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(172, 75);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(0, 37);
            this.label6.TabIndex = 18;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.button2.Location = new System.Drawing.Point(478, 275);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(80, 74);
            this.button2.TabIndex = 21;
            this.button2.Tag = "BackRight";
            this.button2.Text = "SE";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.movButton_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.button3.Location = new System.Drawing.Point(388, 275);
            this.button3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(82, 74);
            this.button3.TabIndex = 20;
            this.button3.Tag = "Back";
            this.button3.Text = "S";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.movButton_Click);
            // 
            // aterrizarBtn
            // 
            this.aterrizarBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.aterrizarBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.aterrizarBtn.Location = new System.Drawing.Point(435, 162);
            this.aterrizarBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.aterrizarBtn.Name = "aterrizarBtn";
            this.aterrizarBtn.Size = new System.Drawing.Size(124, 48);
            this.aterrizarBtn.TabIndex = 13;
            this.aterrizarBtn.Text = "Aterrizar";
            this.aterrizarBtn.UseVisualStyleBackColor = false;
            this.aterrizarBtn.Click += new System.EventHandler(this.aterrizarBtn_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(206, 146);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(104, 29);
            this.label7.TabIndex = 11;
            this.label7.Text = "Heading";
            // 
            // headLbl
            // 
            this.headLbl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.headLbl.Location = new System.Drawing.Point(315, 142);
            this.headLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.headLbl.Name = "headLbl";
            this.headLbl.Size = new System.Drawing.Size(94, 44);
            this.headLbl.TabIndex = 10;
            // 
            // longitudLbl
            // 
            this.longitudLbl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.longitudLbl.Location = new System.Drawing.Point(117, 145);
            this.longitudLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.longitudLbl.Name = "longitudLbl";
            this.longitudLbl.Size = new System.Drawing.Size(83, 47);
            this.longitudLbl.TabIndex = 8;
            // 
            // latitudLbl
            // 
            this.latitudLbl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.latitudLbl.Location = new System.Drawing.Point(117, 82);
            this.latitudLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.latitudLbl.Name = "latitudLbl";
            this.latitudLbl.Size = new System.Drawing.Size(83, 47);
            this.latitudLbl.TabIndex = 7;
            // 
            // altitudLbl
            // 
            this.altitudLbl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.altitudLbl.Location = new System.Drawing.Point(315, 82);
            this.altitudLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.altitudLbl.Name = "altitudLbl";
            this.altitudLbl.Size = new System.Drawing.Size(94, 44);
            this.altitudLbl.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(218, 85);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 29);
            this.label2.TabIndex = 4;
            this.label2.Text = "Altitud";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 85);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 29);
            this.label3.TabIndex = 3;
            this.label3.Text = "Latitud";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(0, 145);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(106, 29);
            this.label5.TabIndex = 2;
            this.label5.Text = "Longitud";
            // 
            // detenerTelemetria
            // 
            this.detenerTelemetria.Location = new System.Drawing.Point(112, 28);
            this.detenerTelemetria.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.detenerTelemetria.Name = "detenerTelemetria";
            this.detenerTelemetria.Size = new System.Drawing.Size(81, 40);
            this.detenerTelemetria.TabIndex = 1;
            this.detenerTelemetria.Text = "Parar";
            this.detenerTelemetria.UseVisualStyleBackColor = true;
            this.detenerTelemetria.Click += new System.EventHandler(this.detenerTelemetria_Click);
            // 
            // IniciaTelemetría
            // 
            this.IniciaTelemetría.Location = new System.Drawing.Point(22, 28);
            this.IniciaTelemetría.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.IniciaTelemetría.Name = "IniciaTelemetría";
            this.IniciaTelemetría.Size = new System.Drawing.Size(81, 40);
            this.IniciaTelemetría.TabIndex = 0;
            this.IniciaTelemetría.Text = "Iniciar";
            this.IniciaTelemetría.UseVisualStyleBackColor = true;
            this.IniciaTelemetría.Click += new System.EventHandler(this.enviarTelemetria);
            // 
            // ponGuiadoBtn
            // 
            this.ponGuiadoBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.ponGuiadoBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ponGuiadoBtn.Location = new System.Drawing.Point(1137, 119);
            this.ponGuiadoBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ponGuiadoBtn.Name = "ponGuiadoBtn";
            this.ponGuiadoBtn.Size = new System.Drawing.Size(262, 138);
            this.ponGuiadoBtn.TabIndex = 56;
            this.ponGuiadoBtn.Text = "Modo Guiado";
            this.ponGuiadoBtn.UseVisualStyleBackColor = false;
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.button4.Location = new System.Drawing.Point(298, 275);
            this.button4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(82, 74);
            this.button4.TabIndex = 19;
            this.button4.Tag = "BackLeft";
            this.button4.Text = "SW";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.movButton_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.headLbl);
            this.groupBox4.Controls.Add(this.longitudLbl);
            this.groupBox4.Controls.Add(this.latitudLbl);
            this.groupBox4.Controls.Add(this.altitudLbl);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.detenerTelemetria);
            this.groupBox4.Controls.Add(this.IniciaTelemetría);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(15, 735);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox4.Size = new System.Drawing.Size(416, 202);
            this.groupBox4.TabIndex = 47;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Telemetría";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.button4);
            this.groupBox2.Controls.Add(this.button5);
            this.groupBox2.Controls.Add(this.button8);
            this.groupBox2.Controls.Add(this.button19);
            this.groupBox2.Controls.Add(this.button24);
            this.groupBox2.Controls.Add(this.button25);
            this.groupBox2.Controls.Add(this.button26);
            this.groupBox2.Controls.Add(this.button21);
            this.groupBox2.Controls.Add(this.button20);
            this.groupBox2.Controls.Add(this.button17);
            this.groupBox2.Controls.Add(this.button16);
            this.groupBox2.Controls.Add(this.button15);
            this.groupBox2.Controls.Add(this.button14);
            this.groupBox2.Controls.Add(this.button13);
            this.groupBox2.Controls.Add(this.button12);
            this.groupBox2.Controls.Add(this.button11);
            this.groupBox2.Controls.Add(this.button10);
            this.groupBox2.Controls.Add(this.button9);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(15, 290);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Size = new System.Drawing.Size(577, 435);
            this.groupBox2.TabIndex = 46;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Movimiento";
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.button5.Location = new System.Drawing.Point(478, 195);
            this.button5.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(80, 71);
            this.button5.TabIndex = 18;
            this.button5.Tag = "Right";
            this.button5.Text = "E";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.movButton_Click);
            // 
            // button8
            // 
            this.button8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.button8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button8.Location = new System.Drawing.Point(388, 198);
            this.button8.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(82, 69);
            this.button8.TabIndex = 17;
            this.button8.Tag = "Stop";
            this.button8.Text = "Stop";
            this.button8.UseVisualStyleBackColor = false;
            this.button8.Click += new System.EventHandler(this.movButton_Click);
            // 
            // button19
            // 
            this.button19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.button19.Location = new System.Drawing.Point(298, 195);
            this.button19.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button19.Name = "button19";
            this.button19.Size = new System.Drawing.Size(82, 71);
            this.button19.TabIndex = 16;
            this.button19.Tag = "Left";
            this.button19.Text = "W";
            this.button19.UseVisualStyleBackColor = false;
            this.button19.Click += new System.EventHandler(this.movButton_Click);
            // 
            // button24
            // 
            this.button24.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.button24.Location = new System.Drawing.Point(478, 112);
            this.button24.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button24.Name = "button24";
            this.button24.Size = new System.Drawing.Size(80, 74);
            this.button24.TabIndex = 15;
            this.button24.Tag = "ForwardRight";
            this.button24.Text = "NE";
            this.button24.UseVisualStyleBackColor = false;
            this.button24.Click += new System.EventHandler(this.movButton_Click);
            // 
            // button25
            // 
            this.button25.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.button25.Location = new System.Drawing.Point(388, 112);
            this.button25.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button25.Name = "button25";
            this.button25.Size = new System.Drawing.Size(82, 74);
            this.button25.TabIndex = 14;
            this.button25.Tag = "Forward";
            this.button25.Text = "N";
            this.button25.UseVisualStyleBackColor = false;
            this.button25.Click += new System.EventHandler(this.movButton_Click);
            // 
            // button26
            // 
            this.button26.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.button26.Location = new System.Drawing.Point(298, 112);
            this.button26.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button26.Name = "button26";
            this.button26.Size = new System.Drawing.Size(82, 74);
            this.button26.TabIndex = 13;
            this.button26.Tag = "ForwardLeft";
            this.button26.Text = "NW";
            this.button26.UseVisualStyleBackColor = false;
            this.button26.Click += new System.EventHandler(this.movButton_Click);
            // 
            // button21
            // 
            this.button21.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.button21.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button21.Location = new System.Drawing.Point(96, 46);
            this.button21.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button21.Name = "button21";
            this.button21.Size = new System.Drawing.Size(378, 46);
            this.button21.TabIndex = 10;
            this.button21.Tag = "Up";
            this.button21.Text = "ARRIBA";
            this.button21.UseVisualStyleBackColor = false;
            this.button21.Click += new System.EventHandler(this.movButton_Click);
            // 
            // button20
            // 
            this.button20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.button20.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button20.Location = new System.Drawing.Point(96, 360);
            this.button20.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button20.Name = "button20";
            this.button20.Size = new System.Drawing.Size(378, 46);
            this.button20.TabIndex = 9;
            this.button20.Tag = "Down";
            this.button20.Text = "ABAJO";
            this.button20.UseVisualStyleBackColor = false;
            this.button20.Click += new System.EventHandler(this.movButton_Click);
            // 
            // button17
            // 
            this.button17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.button17.Location = new System.Drawing.Point(184, 278);
            this.button17.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size(82, 74);
            this.button17.TabIndex = 8;
            this.button17.Tag = "SouthEast";
            this.button17.Text = "SE";
            this.button17.UseVisualStyleBackColor = false;
            this.button17.Click += new System.EventHandler(this.navButton_Click);
            // 
            // button16
            // 
            this.button16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.button16.Location = new System.Drawing.Point(96, 278);
            this.button16.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(80, 74);
            this.button16.TabIndex = 7;
            this.button16.Tag = "South";
            this.button16.Text = "S";
            this.button16.UseVisualStyleBackColor = false;
            this.button16.Click += new System.EventHandler(this.navButton_Click);
            // 
            // button15
            // 
            this.button15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.button15.Location = new System.Drawing.Point(4, 278);
            this.button15.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(82, 74);
            this.button15.TabIndex = 6;
            this.button15.Tag = "SouthWest";
            this.button15.Text = "SW";
            this.button15.UseVisualStyleBackColor = false;
            this.button15.Click += new System.EventHandler(this.navButton_Click);
            // 
            // button14
            // 
            this.button14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.button14.Location = new System.Drawing.Point(188, 198);
            this.button14.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(80, 69);
            this.button14.TabIndex = 5;
            this.button14.Tag = "East";
            this.button14.Text = "E";
            this.button14.UseVisualStyleBackColor = false;
            this.button14.Click += new System.EventHandler(this.navButton_Click);
            // 
            // button13
            // 
            this.button13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.button13.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button13.Location = new System.Drawing.Point(96, 198);
            this.button13.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(82, 71);
            this.button13.TabIndex = 4;
            this.button13.Tag = "Stop";
            this.button13.Text = "Stop";
            this.button13.UseVisualStyleBackColor = false;
            this.button13.Click += new System.EventHandler(this.navButton_Click);
            // 
            // button12
            // 
            this.button12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.button12.Location = new System.Drawing.Point(4, 198);
            this.button12.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(82, 71);
            this.button12.TabIndex = 3;
            this.button12.Tag = "West";
            this.button12.Text = "W";
            this.button12.UseVisualStyleBackColor = false;
            this.button12.Click += new System.EventHandler(this.navButton_Click);
            // 
            // button11
            // 
            this.button11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.button11.Location = new System.Drawing.Point(188, 112);
            this.button11.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(80, 75);
            this.button11.TabIndex = 2;
            this.button11.Tag = "NorthEast";
            this.button11.Text = "NE";
            this.button11.UseVisualStyleBackColor = false;
            this.button11.Click += new System.EventHandler(this.navButton_Click);
            // 
            // button10
            // 
            this.button10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.button10.Location = new System.Drawing.Point(96, 112);
            this.button10.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(82, 75);
            this.button10.TabIndex = 1;
            this.button10.Tag = "North";
            this.button10.Text = "N";
            this.button10.UseVisualStyleBackColor = false;
            this.button10.Click += new System.EventHandler(this.navButton_Click);
            // 
            // button9
            // 
            this.button9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.button9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button9.Location = new System.Drawing.Point(4, 112);
            this.button9.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(82, 74);
            this.button9.TabIndex = 0;
            this.button9.Tag = "NorthWest";
            this.button9.Text = "NW";
            this.button9.UseVisualStyleBackColor = false;
            this.button9.Click += new System.EventHandler(this.navButton_Click);
            // 
            // velocidadLbl
            // 
            this.velocidadLbl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.velocidadLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.velocidadLbl.ForeColor = System.Drawing.Color.Red;
            this.velocidadLbl.Location = new System.Drawing.Point(19, 974);
            this.velocidadLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.velocidadLbl.Name = "velocidadLbl";
            this.velocidadLbl.Size = new System.Drawing.Size(72, 36);
            this.velocidadLbl.TabIndex = 53;
            this.velocidadLbl.Text = "0";
            this.velocidadLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackBarSpeed
            // 
            this.trackBarSpeed.Location = new System.Drawing.Point(29, 1023);
            this.trackBarSpeed.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.trackBarSpeed.Name = "trackBarSpeed";
            this.trackBarSpeed.Size = new System.Drawing.Size(164, 69);
            this.trackBarSpeed.TabIndex = 52;
            this.trackBarSpeed.Scroll += new System.EventHandler(this.trackBarSpeed_Scroll);
            this.trackBarSpeed.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trackBarSpeed_MouseUp);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(325, 984);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 25);
            this.label4.TabIndex = 55;
            this.label4.Text = "Heading";
            // 
            // trackBarHeading
            // 
            this.trackBarHeading.Location = new System.Drawing.Point(238, 1019);
            this.trackBarHeading.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.trackBarHeading.Maximum = 360;
            this.trackBarHeading.Name = "trackBarHeading";
            this.trackBarHeading.Size = new System.Drawing.Size(147, 69);
            this.trackBarHeading.TabIndex = 50;
            this.trackBarHeading.Scroll += new System.EventHandler(this.trackBarHeading_Scroll);
            this.trackBarHeading.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trackBarHeading_MouseUp);
            // 
            // headingLbl
            // 
            this.headingLbl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.headingLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.headingLbl.ForeColor = System.Drawing.Color.Red;
            this.headingLbl.Location = new System.Drawing.Point(238, 980);
            this.headingLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.headingLbl.Name = "headingLbl";
            this.headingLbl.Size = new System.Drawing.Size(72, 30);
            this.headingLbl.TabIndex = 51;
            this.headingLbl.Text = "0";
            this.headingLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackBarDistancia
            // 
            this.trackBarDistancia.Location = new System.Drawing.Point(450, 1019);
            this.trackBarDistancia.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.trackBarDistancia.Maximum = 100;
            this.trackBarDistancia.Name = "trackBarDistancia";
            this.trackBarDistancia.Size = new System.Drawing.Size(148, 69);
            this.trackBarDistancia.TabIndex = 48;
            this.trackBarDistancia.Scroll += new System.EventHandler(this.trackBarDistancia_Scroll);
            // 
            // pasoLbl
            // 
            this.pasoLbl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pasoLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pasoLbl.ForeColor = System.Drawing.Color.Red;
            this.pasoLbl.Location = new System.Drawing.Point(450, 977);
            this.pasoLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.pasoLbl.Name = "pasoLbl";
            this.pasoLbl.Size = new System.Drawing.Size(72, 30);
            this.pasoLbl.TabIndex = 49;
            this.pasoLbl.Text = "0";
            this.pasoLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(106, 980);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(99, 25);
            this.label9.TabIndex = 54;
            this.label9.Text = "Velocidad";
            // 
            // alturaBox
            // 
            this.alturaBox.Location = new System.Drawing.Point(8, 162);
            this.alturaBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.alturaBox.Name = "alturaBox";
            this.alturaBox.Size = new System.Drawing.Size(80, 43);
            this.alturaBox.TabIndex = 6;
            // 
            // panelMapa
            // 
            this.panelMapa.Location = new System.Drawing.Point(663, 365);
            this.panelMapa.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelMapa.Name = "panelMapa";
            this.panelMapa.Size = new System.Drawing.Size(736, 615);
            this.panelMapa.TabIndex = 45;
            // 
            // groupBox1
            // 
            this.groupBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.groupBox1.Controls.Add(this.textBoxNumDrons);
            this.groupBox1.Controls.Add(this.Connectar_button);
            this.groupBox1.Controls.Add(this.desplegable);
            this.groupBox1.Controls.Add(this.prodRadio);
            this.groupBox1.Controls.Add(this.simRadio);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.alturaBox);
            this.groupBox1.Controls.Add(this.aterrizarBtn);
            this.groupBox1.Controls.Add(this.CMB_comport);
            this.groupBox1.Controls.Add(this.despegarBtn);
            this.groupBox1.Controls.Add(this.RTLBtn);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(15, 65);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(596, 215);
            this.groupBox1.TabIndex = 44;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Control";
            // 
            // textBoxNumDrons
            // 
            this.textBoxNumDrons.Location = new System.Drawing.Point(179, 110);
            this.textBoxNumDrons.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxNumDrons.Name = "textBoxNumDrons";
            this.textBoxNumDrons.Size = new System.Drawing.Size(118, 43);
            this.textBoxNumDrons.TabIndex = 44;
            // 
            // Connectar_button
            // 
            this.Connectar_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.Connectar_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Connectar_button.Location = new System.Drawing.Point(8, 108);
            this.Connectar_button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Connectar_button.Name = "Connectar_button";
            this.Connectar_button.Size = new System.Drawing.Size(148, 45);
            this.Connectar_button.TabIndex = 17;
            this.Connectar_button.Text = "Connectar";
            this.Connectar_button.UseVisualStyleBackColor = false;
            this.Connectar_button.Click += new System.EventHandler(this.Connectar_button_Click);
            // 
            // desplegable
            // 
            this.desplegable.FormattingEnabled = true;
            this.desplegable.Location = new System.Drawing.Point(315, 108);
            this.desplegable.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.desplegable.Name = "desplegable";
            this.desplegable.Size = new System.Drawing.Size(121, 45);
            this.desplegable.TabIndex = 16;
            this.desplegable.SelectedIndexChanged += new System.EventHandler(this.desplegable_SelectedIndexChanged);
            // 
            // prodRadio
            // 
            this.prodRadio.AutoSize = true;
            this.prodRadio.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.prodRadio.Location = new System.Drawing.Point(20, 69);
            this.prodRadio.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.prodRadio.Name = "prodRadio";
            this.prodRadio.Size = new System.Drawing.Size(133, 29);
            this.prodRadio.TabIndex = 15;
            this.prodRadio.TabStop = true;
            this.prodRadio.Text = "producción";
            this.prodRadio.UseVisualStyleBackColor = true;
            this.prodRadio.CheckedChanged += new System.EventHandler(this.prodRadio_CheckedChanged);
            // 
            // simRadio
            // 
            this.simRadio.AutoSize = true;
            this.simRadio.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.simRadio.Location = new System.Drawing.Point(20, 42);
            this.simRadio.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.simRadio.Name = "simRadio";
            this.simRadio.Size = new System.Drawing.Size(129, 29);
            this.simRadio.TabIndex = 14;
            this.simRadio.TabStop = true;
            this.simRadio.Text = "simulación";
            this.simRadio.UseVisualStyleBackColor = true;
            this.simRadio.CheckedChanged += new System.EventHandler(this.simRadio_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(98, 176);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 29);
            this.label1.TabIndex = 7;
            this.label1.Text = "metros";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(539, 982);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(92, 25);
            this.label8.TabIndex = 57;
            this.label8.Text = "Distancia";
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.parámetrosToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1452, 53);
            this.menuStrip1.TabIndex = 58;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // parámetrosToolStripMenuItem
            // 
            this.parámetrosToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.parámetrosToolStripMenuItem.Name = "parámetrosToolStripMenuItem";
            this.parámetrosToolStripMenuItem.Size = new System.Drawing.Size(196, 49);
            this.parámetrosToolStripMenuItem.Text = "Parámetros";
            this.parámetrosToolStripMenuItem.Click += new System.EventHandler(this.parámetrosToolStripMenuItem_Click);
            // 
            // pedidos
            // 
            this.pedidos.Location = new System.Drawing.Point(663, 87);
            this.pedidos.Name = "pedidos";
            this.pedidos.Size = new System.Drawing.Size(156, 49);
            this.pedidos.TabIndex = 59;
            this.pedidos.Text = "Pedidos";
            this.pedidos.UseVisualStyleBackColor = true;
            this.pedidos.Click += new System.EventHandler(this.pedidos_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Location = new System.Drawing.Point(1022, 256);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(200, 100);
            this.flowLayoutPanel1.TabIndex = 60;
            // 
            // cargar_pedidos
            // 
            this.cargar_pedidos.Location = new System.Drawing.Point(663, 142);
            this.cargar_pedidos.Name = "cargar_pedidos";
            this.cargar_pedidos.Size = new System.Drawing.Size(156, 49);
            this.cargar_pedidos.TabIndex = 61;
            this.cargar_pedidos.Text = "Cargar pedidos";
            this.cargar_pedidos.UseVisualStyleBackColor = true;
            this.cargar_pedidos.Click += new System.EventHandler(this.cargar_pedidos_Click);
            // 
            // Mercadrona
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1452, 1050);
            this.Controls.Add(this.cargar_pedidos);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.pedidos);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.pasoLbl);
            this.Controls.Add(this.trackBarSpeed);
            this.Controls.Add(this.ponGuiadoBtn);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.velocidadLbl);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.trackBarHeading);
            this.Controls.Add(this.headingLbl);
            this.Controls.Add(this.trackBarDistancia);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.panelMapa);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Mercadrona";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.Mercadrona_Load);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarHeading)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarDistancia)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox CMB_comport;
        private System.Windows.Forms.Button despegarBtn;
        private System.Windows.Forms.Button RTLBtn;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button aterrizarBtn;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label headLbl;
        private System.Windows.Forms.Label longitudLbl;
        private System.Windows.Forms.Label latitudLbl;
        private System.Windows.Forms.Label altitudLbl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button detenerTelemetria;
        private System.Windows.Forms.Button IniciaTelemetría;
        private System.Windows.Forms.Button ponGuiadoBtn;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button19;
        private System.Windows.Forms.Button button24;
        private System.Windows.Forms.Button button25;
        private System.Windows.Forms.Button button26;
        private System.Windows.Forms.Button button21;
        private System.Windows.Forms.Button button20;
        private System.Windows.Forms.Button button17;
        private System.Windows.Forms.Button button16;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Label velocidadLbl;
        private System.Windows.Forms.TrackBar trackBarSpeed;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TrackBar trackBarHeading;
        private System.Windows.Forms.Label headingLbl;
        private System.Windows.Forms.TrackBar trackBarDistancia;
        private System.Windows.Forms.Label pasoLbl;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox alturaBox;
        private System.Windows.Forms.Panel panelMapa;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBoxNumDrons;
        private System.Windows.Forms.Button Connectar_button;
        private System.Windows.Forms.ComboBox desplegable;
        private System.Windows.Forms.RadioButton prodRadio;
        private System.Windows.Forms.RadioButton simRadio;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem parámetrosToolStripMenuItem;
        private System.Windows.Forms.Button pedidos;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button cargar_pedidos;
    }
}