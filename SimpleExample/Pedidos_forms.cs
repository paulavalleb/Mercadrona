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
using System.Data;
using System.Diagnostics;
using System.Collections;

namespace SimpleExample
{
    public partial class Pedidos_forms : Form
    {
        private GMapControl gmap;
        PointLatLng home;

        List<Pedido> pedidos = new List<Pedido>();
        PointLatLng direccion;
        string lista_compra;
        List<(string, int, double)> productos = new List<(string, int, double)>();
        DataTable table = new DataTable();

        funcionesPedidos f = new funcionesPedidos();

        public Pedidos_forms()
        {
            InitializeComponent();
            home = new PointLatLng(41.282654591229225, 1.9733365698308918);
            gmap = new GMapControl
            {
                Dock = DockStyle.Fill,
                Visible = false, // Inicialmente oculto
                CanDragMap = true, // Permite arrastrar el mapa
                DragButton = MouseButtons.Left, // Usa el clic izquierdo para arrastrar
                MouseWheelZoomType = MouseWheelZoomType.MousePositionAndCenter, // Zoom con la rueda del mouse
                IgnoreMarkerOnMouseWheel = true, // Evita interferencias con marcadores
                MinZoom = 5,
                MaxZoom = 20,
                Zoom = 15,
                ShowCenter = false
            };
            gmap.MouseDown += GMapControl_MouseDoubleClick; // capturo el evento de click en raton
            this.Controls.Add(gmap);
            gmap.MapProvider = GMapProviders.GoogleSatelliteMap;
            GMaps.Instance.Mode = AccessMode.ServerOnly;
            gmap.Position = home;
            gmap.Visible = true; // Mostrar el mapa


            comboBox1.Items.Add("Botella agua 1L");
            comboBox1.Items.Add("Leche 1L");
            comboBox1.Items.Add("Galletas");
            comboBox1.Items.Add("Sandía");
            comboBox1.Items.Add("Docena de huevos");

            comboBox2.Items.Add("1");
            comboBox2.Items.Add("2");
            comboBox2.Items.Add("3");
            comboBox2.Items.Add("4");
            comboBox2.Items.Add("...");
        }
        private void GMapControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // Con el doble click guarda la dirección señalada en el mapa.

            direccion = gmap.FromLocalToLatLng(e.X, e.Y);
            double lat = Math.Round(direccion.Lat, 2);
            double lng = Math.Round(direccion.Lng, 2);
            textbox_direccion.Text = lat + "," + lng;
        }

        private void Pedidos_forms_Load(object sender, EventArgs e)
        {
            panelMapa.Controls.Add(gmap);
            // Definir las columnas del DataTable(deben coincidir con lo que quieres mostrar)
            table.Columns.Add("Num.", typeof(int));
            table.Columns.Add("Nombre", typeof(string));
            table.Columns.Add("Dirección", typeof(PointLatLng));
            table.Columns.Add("Lista productos", typeof(string));
        }

        private void finalizar_click(object sender, EventArgs e)
        {
            if (textBox_destinatario.Text == "" || textbox_direccion.Text == "" || textBox_pedido.Text == "")
            {
                MessageBox.Show("Por favor, rellene todos los campos.");
            }
            Pedido pedido = new Pedido();

            // Crear un nuevo pedido
            pedido.crear_pedido(table.Rows.Count, productos, direccion, textBox_destinatario.Text);
            pedidos.Add(pedido);
            f.setPedidos(pedidos);

            // Creamos nueva fila y actualizamos tabla de pedidos
            DataRow nuevaFila = table.NewRow();

            nuevaFila["Num."] = table.Rows.Count+1;
            nuevaFila["Nombre"] = textBox_destinatario.Text;
            nuevaFila["Dirección"] = direccion;
            nuevaFila["Lista productos"] = lista_compra;

            table.Rows.Add(nuevaFila);

            dataGrid.DataSource = table;
            dataGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            
            textBox_destinatario.Clear();
            textbox_direccion.Clear();
            textBox_pedido.Clear();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void anadir_Click(object sender, EventArgs e)
        {
            string producto = comboBox1.SelectedItem.ToString();
            int cantidad = Convert.ToInt16(comboBox2.SelectedItem.ToString());
            double peso = calcular_peso(producto, cantidad);
            productos.Add((producto, cantidad, peso));
            lista_compra = lista_compra + producto + '(' + cantidad.ToString() + ')';
            textBox_pedido.Text = lista_compra;
            comboBox1.SelectedIndex = -1; // Reinicia el comboBox
            comboBox2.SelectedIndex = -1; // Reinicia el comboBox

        }
        
        private double calcular_peso(string producto, int cantidad)
        {
            if (producto == "Botella agua 1L")
            {
                return 1 * cantidad;
            }
            else if (producto == "Leche 1L")
            {
                return 1 * cantidad;
            }
            else if (producto == "Galletas")
            {
                return 0.5 * cantidad;
            }
            else if (producto == "Sandía")
            {
                return 5 * cantidad;
            }
            else if (producto == "Docena de huevos")
            {
                return 0.5 * cantidad;
            }
            else
            {
                return 0; // Producto no reconocido
            }
        }

        
    }
}
