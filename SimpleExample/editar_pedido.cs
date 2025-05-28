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
    public partial class editar_pedido : Form
    {
        DataTable table;
        Dictionary<string, (double precio, double peso)> dictionary;
        funcionesPedidos f;
        List<Pedido> pedidos;
        int selectedIndex;
        Pedido p;
        private GMapControl gmap;
        PointLatLng home;
        PointLatLng direccion;
        List<(string, int, double, double)> productos = new List<(string, int, double, double)>(); // nombre, cantidad, peso, precio
        string lista_compra;
        double pesoTotal = 0;
        double precioTotal = 0;
        public editar_pedido(Dictionary<string, (double precio, double peso)> dictionary, funcionesPedidos f)
        {
            InitializeComponent();
            this.f = f;
            this.table = f.GetTable();
            this.dictionary = dictionary;
            pedidos = f.GetPedidos();
            home = new PointLatLng(41.282654591229225, 1.9733365698308918); // Mercadona
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
        }

        private void editar_pedido_Load(object sender, EventArgs e)
        {
            panelMapa.Controls.Add(gmap);
            
            for (int i = 0; i < pedidos.Count; i++)
            {
                comboBox1.Items.Add(i+1);
            }
            foreach (var clave in dictionary.Keys)
            {
                comboBox2.Items.Add(clave);
            }
            comboBox3.Items.Add("1");
            comboBox3.Items.Add("2");
            comboBox3.Items.Add("3");
            comboBox3.Items.Add("4");
        }
        private void GMapControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // Con el doble click guarda la dirección señalada en el mapa.

            direccion = gmap.FromLocalToLatLng(e.X, e.Y);
            double lat = Math.Round(direccion.Lat, 2);
            double lng = Math.Round(direccion.Lng, 2);
            textbox_direccion.Text = lat + "," + lng;
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            precioTotal = 0;
            pesoTotal = 0;
            selectedIndex = comboBox1.SelectedIndex;
            if (selectedIndex >= 0 && selectedIndex <= pedidos.Count)
            {
                Pedido p = pedidos[selectedIndex];
                var productosList = p.getProductos();
                var sb = new StringBuilder();
                foreach (var prod in productosList)
                {
                    sb.AppendLine($"{prod.nombre} ({prod.cantidad}) ");
                }
                textBox_pedido.Text = sb.ToString();
                productos = p.getProductos();
                direccion = p.getDireccion();
                textbox_direccion.Text = $"{direccion.Lat}, {direccion.Lng}";
                textBox2.Text = p.getDestinatario().ToString();
               
            }


        }

        private void confirmar_Click(object sender, EventArgs e)
        {
            if (p == null || selectedIndex < 0 || selectedIndex >= table.Rows.Count)
            {
                MessageBox.Show("Por favor, selecciona un pedido válido.");
            }
            else
            {
                p.setDestinatario(textBox2.Text);
                p.setDireccion(direccion);
                p.setProductos(productos);
                p.setPrecioTotal(precioTotal);
                p.setPesoTotal(pesoTotal);
                DataRow row = table.Rows[selectedIndex];
                row["Nombre"] = p.getDestinatario();
                row["Dirección"] = p.getDireccion();
                row["Lista productos"] = p.getProductos();
                row["Precio"] = p.getPrecioTotal();
                row["Peso"] = p.getPesoTotal();
                pedidos[selectedIndex - 1] = p; // Actualiza el pedido en la lista de pedidos
                f.setPedidos(pedidos);
                f.setTable(table);
                comboBox1.SelectedIndex = -1; // Reinicia el comboBox
                comboBox2.SelectedIndex = -1; // Reinicia el comboBox
                comboBox3.SelectedIndex = -1; // Reinicia el comboBox
            }
            
        }

        private void añadir_Click(object sender, EventArgs e)
        {
            string producto = comboBox2.SelectedItem.ToString();
            int cantidad = Convert.ToInt16(comboBox3.SelectedItem.ToString());
            double peso = calcular_peso(producto, cantidad);
            double precio = calcular_precio(producto, cantidad);
            productos.Add((producto, cantidad, peso, precio));
            lista_compra = lista_compra + producto + '(' + cantidad.ToString() + ") ";
            textBox_pedido.Text = lista_compra;
            comboBox2.SelectedIndex = -1; // Reinicia el comboBox
            comboBox3.SelectedIndex = -1; // Reinicia el comboBox
            List<double> precio_peso = peso_precio_total(productos);
            precioTotal = precio_peso[0];
            pesoTotal = precio_peso[1];

        }
        private double calcular_peso(string producto, int cantidad)
        {
            if (dictionary.TryGetValue(producto, out var info))
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
            if (dictionary.TryGetValue(producto, out var info))
            {
                return info.precio * cantidad;
            }
            else
            {
                MessageBox.Show("Producto no encontrado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return 0;
            }
        }
        private List<double> peso_precio_total(List<(string, int, double, double)> productos)
        {
            double peso = 0;
            double precio = 0;
            foreach (var p in productos)
            {
                peso += p.Item3;
                precio += p.Item4;
            }
            return new List<double> { peso, precio };
        }
    }
}
