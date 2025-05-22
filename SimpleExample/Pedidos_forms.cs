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
        List<(string, int, double, double)> productos = new List<(string, int, double, double)>(); // nombre, cantidad, peso, precio
        DataTable table = new DataTable();

        funcionesPedidos f = new funcionesPedidos();

        public Pedidos_forms(funcionesPedidos f)
        {
            this.f = f;
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

            foreach (var clave in productos_diccionario.Keys)
            {
                comboBox1.Items.Add(clave);
            }
            
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
            
            double peso_total = 0; 
            double precio_total = 0;
            foreach (var elemento in productos)
            {
                peso_total += elemento.Item3;
                precio_total += elemento.Item4;
            }
            pedido.crear_pedido(table.Rows.Count, productos, direccion, textBox_destinatario.Text, peso_total, precio_total);
            pedidos.Add(pedido);
            

            // Creamos nueva fila y actualizamos tabla de pedidos
            DataRow nuevaFila = table.NewRow();

            nuevaFila["Num."] = table.Rows.Count+1;
            nuevaFila["Nombre"] = textBox_destinatario.Text;
            nuevaFila["Dirección"] = direccion;
            nuevaFila["Lista productos"] = lista_compra;

            table.Rows.Add(nuevaFila);

            dataGrid.DataSource = table;
            dataGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            lista_compra = "";
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
            double precio = calcular_precio(producto, cantidad);
            productos.Add((producto, cantidad, peso, precio));
            lista_compra = lista_compra + producto + '(' + cantidad.ToString() + ')';
            textBox_pedido.Text = lista_compra;
            comboBox1.SelectedIndex = -1; // Reinicia el comboBox
            comboBox2.SelectedIndex = -1; // Reinicia el comboBox

        }
        
        private double calcular_peso(string producto, int cantidad)
        {
            if (productos_diccionario.TryGetValue(producto, out var info))
            {
                return info.peso * cantidad;
            }
            else
            {
                MessageBox.Show("Producto no encontrado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return 0;
            }     
        }
        private double calcular_precio(string producto, int cantidad)
        {
            if (productos_diccionario.TryGetValue(producto, out var info))
            {
                return info.precio * cantidad;
            }
            else
            {
                MessageBox.Show("Producto no encontrado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return 0;
            }
        }
        Dictionary<string, (double precio, double peso)> productos_diccionario = new Dictionary<string, (double, double)>
        {
            { "Manzanas", (2.5, 1.2) },
            { "Leche", (1.1, 1.0) },
            { "Arroz", (1.8, 1.0) },
            { "Pan", (1.3, 0.5) },
            { "Huevos", (2.2, 0.6) },
            { "Pollo", (5.5, 1.8) },
            { "Café", (3.7, 0.25) },
            { "Queso", (4.0, 0.7) },
            { "Tomates", (2.0, 1.0) },
            { "Zanahorias", (1.6, 1.2) },
            { "Pasta", (1.4, 0.5) },
            { "Armario", (30.0,  30.0) },
            { "Jabón", (1.5, 0.3) },
            { "Champú", (3.2, 0.4) },
            { "Detergente", (6.5, 2.0) },
            { "Azúcar", (1.7, 1.0) },
            { "Sal", (0.6, 0.75) },
            { "Aceite de oliva", (4.8, 1.0) },
            { "Agua embotellada", (0.5, 1.5) },
            { "Cereal", (3.9, 0.6) }
        };

        


    }
}
