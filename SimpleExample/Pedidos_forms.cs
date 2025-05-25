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
        // Mapa
        private GMapControl gmap;
        PointLatLng home;

        // Pedidos
        List<Pedido> pedidos = new List<Pedido>();
        PointLatLng direccion;
        string lista_compra;
        List<(string, int, double, double)> productos = new List<(string, int, double, double)>(); // nombre, cantidad, peso, precio
        DataTable table = new DataTable();
        funcionesPedidos f = new funcionesPedidos(); // Para poder guardar pedidos y pasarlos a formulario principal.
        int indexCantidad; // Combobox cantidad

        public Pedidos_forms(funcionesPedidos f)
        {
            this.f = f;
            InitializeComponent();
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

            foreach (var clave in productos_diccionario.Keys)
            {
                comboBox1.Items.Add(clave);
            }

            comboBox2.Items.Add("1");
            comboBox2.Items.Add("2");
            comboBox2.Items.Add("3");
            comboBox2.Items.Add("4");
            comboBox2.Items.Add("...");
            indexCantidad = 4;
        }

        private void Pedidos_forms_Load(object sender, EventArgs e)
        {
            panelMapa.Controls.Add(gmap);
            // Columnas tabla pedidos
            table.Columns.Add("Num.", typeof(int));
            table.Columns.Add("Nombre", typeof(string));
            table.Columns.Add("Dirección", typeof(PointLatLng));
            table.Columns.Add("Lista productos", typeof(string));
            table.Columns.Add("Precio", typeof(double));
            table.Columns.Add("Peso", typeof(double));
            pictureBox1.Image = Image.FromFile("icono_info.png");
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.Image = Image.FromFile("icono_info.png");
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        // Pedido:

        // Cantidad
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem != null && comboBox2.SelectedItem.ToString() == "...")
            {
                for (int i = 1; i < 5; i++)
                {
                    i = i + indexCantidad;
                    comboBox2.Items.Add(i.ToString());
                }
                indexCantidad += 4; // Incrementa el índice para la próxima vez que se añadan más elementos
            }
        }
        private void anadir_Click(object sender, EventArgs e)
        {
            string producto = comboBox1.SelectedItem.ToString();
            int cantidad = Convert.ToInt16(comboBox2.SelectedItem.ToString());
            double peso = calcular_peso(producto, cantidad);
            double precio = calcular_precio(producto, cantidad);
            productos.Add((producto, cantidad, peso, precio));
            lista_compra = lista_compra + producto + '(' + cantidad.ToString() + ") ";
            textBox_pedido.Text = lista_compra;
            comboBox1.SelectedIndex = -1; // Reinicia el comboBox
            comboBox2.SelectedIndex = -1; // Reinicia el comboBox
            indexCantidad = 4; // Reinicia el índice de cantidad para el comboBox2
            //comboBox2_SelectedIndexChanged(sender, e);
            List<double> precio_peso= peso_precio_total(productos);
            textBox6.Text = Convert.ToString(precio_peso[0]);
            textBox9.Text = Convert.ToString(precio_peso[1]);
        }

        // Dirección
        private void GMapControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // Con el doble click guarda la dirección señalada en el mapa.

            direccion = gmap.FromLocalToLatLng(e.X, e.Y);
            double lat = Math.Round(direccion.Lat, 2);
            double lng = Math.Round(direccion.Lng, 2);
            textbox_direccion.Text = lat + "," + lng;
        }

        // Finalizar pedido
        private void finalizar_click(object sender, EventArgs e)
        {
            if (textBox_destinatario.Text == "" || textbox_direccion.Text == "" || textBox_pedido.Text == "")
            {
                MessageBox.Show("Por favor, rellene todos los campos.");
            }
            else
            {
                Pedido pedido = new Pedido();

                // Crear un nuevo pedido
                double precio = Convert.ToDouble(textBox6.Text);
                double peso = Convert.ToDouble(textBox6.Text);
                pedido.crear_pedido(table.Rows.Count, productos, direccion, textBox_destinatario.Text, precio, peso);
                pedidos.Add(pedido);

                DataRow nuevaFila = table.NewRow();

                nuevaFila["Num."] = pedido.getId();
                nuevaFila["Nombre"] = pedido.getDestinatario();
                nuevaFila["Dirección"] = pedido.getDireccion();
                nuevaFila["Lista productos"] = pedido.getProductos();
                nuevaFila["Precio"] = pedido.getPrecioTotal();
                nuevaFila["Peso"] = pedido.getPesoTotal();

                table.Rows.Add(nuevaFila);

                dataGrid.DataSource = table;
                dataGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

                lista_compra = "";
                productos.Clear(); // Limpiar la lista de productos
                textBox_destinatario.Clear();
                textbox_direccion.Clear();
                textBox_pedido.Clear();
                textBox6.Clear();
                textBox9.Clear();
            }
        }

        // Comprobaciones y cálculos
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

        // Diccionario de productos
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

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            ToolTip direccion = new ToolTip();
            direccion.SetToolTip(pictureBox1, "Haz click en el mapa para indicar tu dirección.");
        }

        private void pictureBox2_MouseEnter(object sender, EventArgs e)
        {
            ToolTip lista_productos = new ToolTip();
            lista_productos.SetToolTip(pictureBox2, "Selecciona el producto y su cantidad y haz click en añadir para incluirlo en tu lista de compra.");
        }

        private void ejemplo_btn_Click(object sender, EventArgs e)
        {
            // pedido 1:
            Pedido pedido = new Pedido();

            PointLatLng direccion = new PointLatLng(41.2842564942199, 1.97371959686279);

            string producto = ("Manzanas");
            int cantidad = 2;
            double peso = calcular_peso(producto, cantidad);
            double precio = calcular_precio(producto, cantidad);
            productos.Add((producto, cantidad, peso, precio));
            string producto2 = ("Jabón");
            int cantidad2 = 3;
            double peso2 = calcular_peso(producto2, cantidad2);
            double precio2 = calcular_precio(producto2, cantidad2);
            productos.Add((producto2, cantidad2, peso2, precio2));
            List<double> precio_peso = peso_precio_total(productos);
            
            pedido.crear_pedido(1, productos, direccion, "Pol Casals", precio_peso[0], precio_peso[1]);
            pedidos.Add(pedido);

            DataRow nuevaFila = table.NewRow();

            nuevaFila["Num."] = pedido.getId();
            nuevaFila["Nombre"] = pedido.getDestinatario();
            nuevaFila["Dirección"] = pedido.getDireccion();
            nuevaFila["Lista productos"] = pedido.getProductos();
            nuevaFila["Precio"] = pedido.getPrecioTotal();
            nuevaFila["Peso"] = pedido.getPesoTotal();

            table.Rows.Add(nuevaFila);

            productos.Clear();

            // pedido 2:
            Pedido pedido2 = new Pedido();

            PointLatLng direccion2 = new PointLatLng(41.2808380593931, 1.9700288772583);

            string producto_2 = ("Arroz");
            int cantidad_2 = 1;
            double peso_2 = calcular_peso(producto_2, cantidad_2);
            double precio_2 = calcular_precio(producto_2, cantidad_2);
            productos.Add((producto_2, cantidad_2, peso_2, precio_2));
            string producto_21 = ("Café");
            int cantidad_21 = 4;
            double peso_21 = calcular_peso(producto_21, cantidad_21);
            double precio_21 = calcular_precio(producto_21, cantidad_21);
            productos.Add((producto_21, cantidad_21, peso_21, precio_21));
            List<double> precio_peso_2 = peso_precio_total(productos);
            
            pedido2.crear_pedido(2, productos, direccion2, "Adrià Martos", precio_peso_2[0], precio_peso_2[1]);
            pedidos.Add(pedido2);

            DataRow nuevaFila2 = table.NewRow();

            nuevaFila2["Num."] = pedido2.getId();
            nuevaFila2["Nombre"] = pedido2.getDestinatario();
            nuevaFila2["Dirección"] = pedido2.getDireccion();
            nuevaFila2["Lista productos"] = pedido2.getProductos();
            nuevaFila2["Precio"] = pedido2.getPrecioTotal();
            nuevaFila2["Peso"] = pedido2.getPesoTotal();

            table.Rows.Add(nuevaFila2);

            productos.Clear();

            // pedido 3:
            Pedido pedido3 = new Pedido();

            PointLatLng direccion3 = new PointLatLng(41.280999310343, 1.96994304656982);

            string producto_3 = ("Arroz");
            int cantidad_3= 1;
            double peso_3 = calcular_peso(producto_3, cantidad_3);
            double precio_3 = calcular_precio(producto_3, cantidad_3);
            productos.Add((producto_3, cantidad_3, peso_3, precio_3));
            string producto_31 = ("Sal");
            int cantidad_31 = 2;
            double peso_31 = calcular_peso(producto_31, cantidad_31);
            double precio_31 = calcular_precio(producto_31, cantidad_31);
            productos.Add((producto_31, cantidad_31, peso_31, precio_31));
            List<double> precio_peso_3 = peso_precio_total(productos);

            pedido3.crear_pedido(3, productos, direccion3, "Arnau Doménech", precio_peso_3[0], precio_peso_3[1]);
            pedidos.Add(pedido3);

            DataRow nuevaFila3 = table.NewRow();

            nuevaFila3["Num."] = pedido3.getId();
            nuevaFila3["Nombre"] = pedido3.getDestinatario();
            nuevaFila3["Dirección"] = pedido3.getDireccion();
            nuevaFila3["Lista productos"] = pedido3.getProductos();
            nuevaFila3["Precio"] = pedido3.getPrecioTotal();
            nuevaFila3["Peso"] = pedido3.getPesoTotal();

            table.Rows.Add(nuevaFila3);

            productos.Clear();


            dataGrid.DataSource = table;
            dataGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            f.setPedidos(pedidos);


        }
    }

}