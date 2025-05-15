using GMap.NET;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Text;
using System.Windows.Forms;
using static MAVLink;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using csDronLink;

namespace SimpleExample
{
    public partial class Pedidos_forms : Form
    {
        private GMapControl gmap;
        public Pedidos_forms()
        {
            InitializeComponent();
        }

        private void Pedidos_forms_Load(object sender, EventArgs e)
        {
        List<Pedido> pedidos = new List<Pedido>();
        table_pedidos.RowCount = 3;
        table_pedidos.ColumnCount = pedidos.Count;
        panelMapa.Controls.Add(gmap);
        }

        private void añadir_Click(object sender, EventArgs e)
        {
            Pedido pedido = new Pedido();
            if (textBox_destinatario.Text == "" || textbox_direccion.Text == "" || textBox_pedido.Text == "")
            {
                MessageBox.Show("Por favor, rellene todos los campos.");
            }
           // pedido.crear_pedido(textBox_pedido , textbox_direccion, textBox_destinatario.Text);
        }

        private void panelMapa_Click(object sender, MouseEventArgs e)
        {
            var point = gmap.FromLocalToLatLng(e.X, e.Y);
            double lat = point.Lat;
            double lng = point.Lng;
            textbox_direccion.Text = lat + "," + lng;
        }
    }
}
