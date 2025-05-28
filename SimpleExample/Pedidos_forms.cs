using GMap.NET;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Text;
using System.Windows.Forms;
using static MAVLink; // Ten cuidado con los static using si no son estrictamente necesarios globalmente
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using csDronLink;
using System.Data;
using System.Diagnostics;
using System.Collections;
using System.Linq; // Añadido para .Select y .ToList

namespace SimpleExample
{
    public partial class Pedidos_forms : Form
    {
        // Mapa
        private GMapControl gmap;
        PointLatLng home;

        // Pedidos
        private List<Pedido> pedidos = new List<Pedido>();
        PointLatLng direccion;
        string lista_compra;
        private List<(string, int, double, double)> productos_list = new List<(string, int, double, double)>(); // nombre, cantidad, peso, precio
        DataTable table = new DataTable();
        funcionesPedidos f; // Para poder guardar pedidos y pasarlos a formulario principal.
        int indexCantidad; // Combobox cantidad

        // Diccionario de productos (movido aquí para que esté disponible en el constructor)
        Dictionary<string, (double precio, double peso)> productos_diccionario = new Dictionary<string, (double, double)>
        {
            { "Manzanas", (2.5, 1.2) }, { "Leche", (1.1, 1.0) }, { "Arroz", (1.8, 1.0) },
            { "Pan", (1.3, 0.5) }, { "Huevos", (2.2, 0.6) }, { "Pollo", (5.5, 1.8) },
            { "Café", (3.7, 0.25) }, { "Queso", (4.0, 0.7) }, { "Tomates", (2.0, 1.0) },
            { "Zanahorias", (1.6, 1.2) }, { "Pasta", (1.4, 0.5) }, { "Armario", (30.0,  30.0) },
            { "Jabón", (1.5, 0.3) }, { "Champú", (3.2, 0.4) }, { "Detergente", (6.5, 2.0) },
            { "Azúcar", (1.7, 1.0) }, { "Sal", (0.6, 0.75) }, { "Aceite de oliva", (4.8, 1.0) },
            { "Agua embotellada", (0.5, 1.5) }, { "Cereal", (3.9, 0.6) }
        };

        #region Definición de Zonas Prohibidas (Fence Enclusions)
        // Colegios...
        List<(float lat, float lon)> fenceEnclusion1 = new List<(float lat, float lon)>
            { (41.27099943353775f,1.977631560058872f), (41.27027621209532f,1.97767676002818f), (41.27030454020156f,1.979152792360499f), (41.27104327071283f,1.979154935020309f) };
        List<(float lat, float lon)> fenceEnclusion2 = new List<(float lat, float lon)>
            { (41.27283798891926f,1.988586504728218f), (41.27147830210465f,1.988507551032976f), (41.27160855559626f,1.990522529649097f), (41.27334509840505f,1.990535784068159f) };
        List<(float lat, float lon)> fenceEnclusion3 = new List<(float lat, float lon)>
            { (41.2770882556513f,1.980939593902604f), (41.27809273912374f,1.983876271785934f), (41.27946942271178f,1.98302120145837f), (41.27860920655556f,1.979998984963587f) };
        List<(float lat, float lon)> fenceEnclusion4 = new List<(float lat, float lon)>
            { (41.27463243683545f,1.976336649473214f), (41.27545515341081f,1.977646093108056f), (41.2764290286723f,1.976632835194603f), (41.27550160648772f,1.975356767951471f) };
        List<(float lon, float lat)> fenceEnclusion5 = new List<(float lon, float lat)>
            { (41.2741639050083f, 1.967764949657134f), (41.27440508502002f, 1.969788531763554f), (41.2757897553886f, 1.969574400775693f), (41.27551718621686f, 1.967610846031294f) };
        List<(float lon, float lat)> fenceEnclusion6 = new List<(float lon, float lat)>
            { (41.27704673951776f, 1.969464373460454f), (41.27715599299962f, 1.970586151283267f), (41.27800611896174f, 1.97051349514578f), (41.27790609839828f, 1.969290429840806f) };
        List<(float lon, float lat)> fenceEnclusion7 = new List<(float lon, float lat)>
            { (41.27767293756999f, 1.971114672959771f), (41.2776723586971f, 1.971938476524795f), (41.27846503158283f, 1.972043622484534f), (41.27845996926608f, 1.97121952282644f) };
        List<(float lon, float lat)> fenceEnclusion8 = new List<(float lon, float lat)>
            { (41.28097767218927f, 1.970712858695269f), (41.28114914950467f, 1.971760842925125f), (41.28201519901487f, 1.971724957251053f), (41.28180506424749f, 1.970623027175686f) };
        List<(float lon, float lat)> fenceEnclusion9 = new List<(float lon, float lat)>
            { (41.28255617778812f, 1.971794836784413f), (41.28314309908107f, 1.974440002356612f), (41.28444264632859f, 1.974062963453653f), (41.28387145460871f, 1.971409068803733f) };
        List<(float lon, float lat)> fenceEnclusion10 = new List<(float lon, float lat)>
            { (41.28811777147848f, 1.972859760110355f), (41.28697659676988f, 1.973151605557883f), (41.28719281173127f, 1.974647846628526f), (41.28836973015334f, 1.974360220181166f) };
        List<(float lon, float lat)> fenceEnclusion11 = new List<(float lon, float lat)>
            { (41.28721961665448f, 1.980898818698691f), (41.28621053921606f, 1.980963075418516f), (41.28625954833255f, 1.982195232795525f), (41.28725778896633f, 1.982098479461161f) };
        List<(float lon, float lat)> fenceEnclusion12 = new List<(float lon, float lat)>
            { (41.28856366187149f, 1.981545723049238f), (41.28777363537311f, 1.981659168083165f), (41.28781966231267f, 1.982643414438363f), (41.28859877900759f, 1.982541961489621f) };
        List<(float lon, float lat)> fenceEnclusion13 = new List<(float lon, float lat)>
            { (41.28896657919925f, 1.983493688840789f), (41.28810331729189f, 1.983385431599158f), (41.28815457623424f, 1.984655131702415f), (41.2889747038954f, 1.984764767514045f) };
        List<(float lon, float lat)> fenceEnclusion14 = new List<(float lon, float lat)>
            { (41.2909775748782f, 1.986669906670917f), (41.28926594689915f, 1.986640771799881f), (41.28940572342928f, 1.988783092985051f), (41.29104037324556f, 1.988935905247915f) };
        #endregion

        // Variables para las zonas prohibidas en el mapa
        private List<GMapPolygon> restrictedPolygons = new List<GMapPolygon>();
        private GMapOverlay polygonsOverlay;

        public Pedidos_forms(funcionesPedidos f)
        {
            this.f = f;
            InitializeComponent();
            home = new PointLatLng(41.282654591229225, 1.9733365698308918); // Mercadona
            gmap = new GMapControl
            {
                Dock = DockStyle.Fill,
                Visible = false, // Se hará visible en Form_Load después de configurar todo
                CanDragMap = true,
                DragButton = MouseButtons.Left,
                MouseWheelZoomType = MouseWheelZoomType.MousePositionAndCenter,
                IgnoreMarkerOnMouseWheel = true,
                MinZoom = 5,
                MaxZoom = 20,
                Zoom = 15,
                ShowCenter = false
            };
            // La adición a Controls (panelMapa) se hace en Form_Load
            gmap.MapProvider = GMapProviders.GoogleSatelliteMap;
            GMaps.Instance.Mode = AccessMode.ServerOnly;
            gmap.Position = home;

            gmap.MouseDown += GMapControl_Combined_MouseDown; // Nuevo manejador combinado

            // Configuración de ComboBoxes
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
            if (panelMapa != null) // Asegurarse de que panelMapa no es null
            {
                panelMapa.Controls.Add(gmap);
            }
            else
            {
                this.Controls.Add(gmap); // Fallback si panelMapa no existe (raro)
            }

            // Inicializar la capa de polígonos y cargar las zonas prohibidas
            polygonsOverlay = new GMapOverlay("restrictedZonesOverlay");
            gmap.Overlays.Add(polygonsOverlay);
            LoadAllRestrictedZones();

            // Columnas tabla pedidos
            table.Columns.Add("Num.", typeof(int));
            table.Columns.Add("Nombre", typeof(string));
            table.Columns.Add("Dirección", typeof(PointLatLng));
            table.Columns.Add("Lista productos", typeof(string));
            table.Columns.Add("Precio", typeof(double));
            table.Columns.Add("Peso", typeof(double));

            // Configuración de PictureBox (asegúrate que las imágenes existen en la ruta)
            try
            {
                pictureBox1.Image = Image.FromFile("icono_info.png");
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox2.Image = Image.FromFile("icono_info.png");
                pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error cargando imágenes para PictureBox: " + ex.Message);
                // Considera mostrar un MessageBox o loggear este error de forma más formal.
            }

            gmap.Visible = true; // Mostrar el mapa ahora que todo está configurado
        }

        #region Lógica de Zonas Prohibidas
        private List<PointLatLng> ConvertLatLonTuplesToPoints(List<(float lat, float lon)> coords)
        {
            return coords.Select(p => new PointLatLng(p.lat, p.lon)).ToList();
        }

        private List<PointLatLng> ConvertLonLatTuplesToPoints(List<(float lon, float lat)> coords)
        {
            // Asume que p.lon es Latitud y p.lat es Longitud para estas tuplas,
            // basado en los valores numéricos (41.xxx, 1.xxx).
            return coords.Select(p => new PointLatLng(p.lon, p.lat)).ToList();
        }

        private void AddRestrictedPolygonToMap(List<PointLatLng> points, string polygonName)
        {
            if (points == null || !points.Any()) return;

            var polygon = new GMapPolygon(points, polygonName)
            {
                Fill = new SolidBrush(Color.FromArgb(70, Color.Red)),
                Stroke = new Pen(Color.Red, 2)
            };
            polygonsOverlay.Polygons.Add(polygon);
            restrictedPolygons.Add(polygon);
        }

        private void LoadAllRestrictedZones()
        {
            AddRestrictedPolygonToMap(ConvertLatLonTuplesToPoints(fenceEnclusion1), "ZonaProhibida1");
            AddRestrictedPolygonToMap(ConvertLatLonTuplesToPoints(fenceEnclusion2), "ZonaProhibida2");
            AddRestrictedPolygonToMap(ConvertLatLonTuplesToPoints(fenceEnclusion3), "ZonaProhibida3");
            AddRestrictedPolygonToMap(ConvertLatLonTuplesToPoints(fenceEnclusion4), "ZonaProhibida4");
            AddRestrictedPolygonToMap(ConvertLonLatTuplesToPoints(fenceEnclusion5), "ZonaProhibida5");
            AddRestrictedPolygonToMap(ConvertLonLatTuplesToPoints(fenceEnclusion6), "ZonaProhibida6");
            AddRestrictedPolygonToMap(ConvertLonLatTuplesToPoints(fenceEnclusion7), "ZonaProhibida7");
            AddRestrictedPolygonToMap(ConvertLonLatTuplesToPoints(fenceEnclusion8), "ZonaProhibida8");
            AddRestrictedPolygonToMap(ConvertLonLatTuplesToPoints(fenceEnclusion9), "ZonaProhibida9");
            AddRestrictedPolygonToMap(ConvertLonLatTuplesToPoints(fenceEnclusion10), "ZonaProhibida10");
            AddRestrictedPolygonToMap(ConvertLonLatTuplesToPoints(fenceEnclusion11), "ZonaProhibida11");
            AddRestrictedPolygonToMap(ConvertLonLatTuplesToPoints(fenceEnclusion12), "ZonaProhibida12");
            AddRestrictedPolygonToMap(ConvertLonLatTuplesToPoints(fenceEnclusion13), "ZonaProhibida13");
            AddRestrictedPolygonToMap(ConvertLonLatTuplesToPoints(fenceEnclusion14), "ZonaProhibida14");
        }
        #endregion

        // Pedido:

        // Cantidad
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem != null && comboBox2.SelectedItem.ToString() == "...")
            {
                // Guardar el item "..." para poder reinsertarlo
                object moreItem = comboBox2.Items[comboBox2.Items.Count - 1];
                comboBox2.Items.RemoveAt(comboBox2.Items.Count - 1); // Remover "..." temporalmente

                for (int i = 1; i <= 4; i++) // Añadir 4 más cada vez
                {
                    comboBox2.Items.Add((indexCantidad + i).ToString());
                }
                indexCantidad += 4;
                comboBox2.Items.Add(moreItem); // Reinsertar "..." al final
            }
        }
        private void anadir_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null || comboBox2.SelectedItem == null || comboBox2.SelectedItem.ToString() == "...")
            {
                MessageBox.Show("Por favor, seleccione un producto y una cantidad válida.", "Información Incompleta", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string producto = comboBox1.SelectedItem.ToString();
            int cantidad = Convert.ToInt16(comboBox2.SelectedItem.ToString()); // Asegúrate que esto no falle si "..." está seleccionado.
            double peso = calcular_peso(producto, cantidad);
            double precio = calcular_precio(producto, cantidad);
            productos_list.Add((producto, cantidad, peso, precio));
            lista_compra = lista_compra + producto + '(' + cantidad.ToString() + ") ";
            textBox_pedido.Text = lista_compra;
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;

            // Resetear comboBox2 a su estado inicial (1, 2, 3, 4, ...)
            comboBox2.Items.Clear();
            comboBox2.Items.Add("1");
            comboBox2.Items.Add("2");
            comboBox2.Items.Add("3");
            comboBox2.Items.Add("4");
            comboBox2.Items.Add("...");
            indexCantidad = 4; // Reinicia el contador para la próxima vez que se pulse "..."

            List<double> precio_peso = peso_precio_total(productos_list);
            textBox6.Text = Convert.ToString(precio_peso[1]); // precio_peso[0] es peso, precio_peso[1] es precio
            textBox9.Text = Convert.ToString(precio_peso[0]);
        }

        // Dirección
        private void GMapControl_Combined_MouseDown(object sender, MouseEventArgs e)
        {
            // 1. Comprobar si el clic fue en una zona prohibida (solo clic izquierdo)
            if (e.Button == MouseButtons.Left)
            {
                PointLatLng clickedPoint = gmap.FromLocalToLatLng(e.X, e.Y);
                foreach (var restrictedPolygon in restrictedPolygons)
                {
                    if (restrictedPolygon.IsInside(clickedPoint))
                    {
                        MessageBox.Show("No se puede pulsar en esta zona porque está prohibida.", "Zona Prohibida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return; // Detener el procesamiento si está en zona prohibida
                    }
                }
            }

            // 2. Si no está en zona prohibida, y es el clic deseado para la dirección, procesar.
            // Asumimos que la selección de dirección también es con clic izquierdo.
            // Si tu intención original era un DOBLE clic para la dirección, necesitarás manejar el evento gmap.MouseDoubleClick por separado.
            // Por ahora, la lógica de dirección se ejecutará en cualquier clic izquierdo que NO esté en zona prohibida.
            if (e.Button == MouseButtons.Left) // O el botón que corresponda para seleccionar dirección
            {
                SelectAddressFromMapClick(e.X, e.Y);
            }
        }

        private void SelectAddressFromMapClick(int mouseX, int mouseY)
        {
            // Lógica original de GMapControl_MouseDoubleClick para seleccionar dirección
            direccion = gmap.FromLocalToLatLng(mouseX, mouseY);
            double lat = Math.Round(direccion.Lat, 7); // Aumentada precisión
            double lng = Math.Round(direccion.Lng, 7); // Aumentada precisión
            textbox_direccion.Text = $"{lat},{lng}"; // Usando interpolación de cadenas
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

                double precioTotal = Convert.ToDouble(textBox6.Text); // Precio total
                double pesoTotal = Convert.ToDouble(textBox9.Text);   // Peso total
                pedido.crear_pedido(table.Rows.Count + 1, productos_list, direccion, textBox_destinatario.Text, precioTotal, pesoTotal);
                pedidos.Add(pedido);

                DataRow nuevaFila = table.NewRow();
                nuevaFila["Num."] = pedido.getId();
                nuevaFila["Nombre"] = pedido.getDestinatario();
                nuevaFila["Dirección"] = pedido.getDireccion();

                // Formatear la lista de productos para el DataGrid
                var sbProductos = new StringBuilder();
                foreach (var prod in pedido.getProductos()) // Usar la lista del objeto pedido
                {
                    sbProductos.Append($"{prod.nombre}({prod.cantidad}) ");
                }
                nuevaFila["Lista productos"] = sbProductos.ToString().Trim();

                nuevaFila["Precio"] = pedido.getPrecioTotal();
                nuevaFila["Peso"] = pedido.getPesoTotal();
                table.Rows.Add(nuevaFila);

                dataGrid.DataSource = table; // Reasignar DataSource
                dataGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

                // Limpiar campos
                lista_compra = "";
                productos_list = new List<(string, int, double, double)>(); // Crear nueva lista
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
                MessageBox.Show("Producto no encontrado en diccionario para calcular peso.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                MessageBox.Show("Producto no encontrado en diccionario para calcular precio.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return 0;
            }
        }

        private List<double> peso_precio_total(List<(string, int, double, double)> productos)
        {
            double pesoTotal = 0;
            double precioTotal = 0;
            foreach (var p in productos)
            {
                // p.Item3 es peso, p.Item4 es precio
                pesoTotal += p.Item3;
                precioTotal += p.Item4;
            }
            return new List<double> { pesoTotal, precioTotal };
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            ToolTip direccionTooltip = new ToolTip(); // Renombrada variable local
            direccionTooltip.SetToolTip(pictureBox1, "Haz click en el mapa para indicar tu dirección.");
        }

        private void pictureBox2_MouseEnter(object sender, EventArgs e)
        {
            ToolTip lista_productosTooltip = new ToolTip(); // Renombrada variable local
            lista_productosTooltip.SetToolTip(pictureBox2, "Selecciona el producto y su cantidad y haz click en añadir para incluirlo en tu lista de compra.");
        }

        private void ejemplo_btn_Click(object sender, EventArgs e)
        {
            // Limpiar datos previos para el ejemplo
            productos_list.Clear();
            lista_compra = "";

            // pedido 1:
            Pedido pedido1 = new Pedido(); // Renombrada variable local
            PointLatLng direccion1 = new PointLatLng(41.2842564942199, 1.97371959686279); // Renombrada variable local

            string producto_p1_1 = ("Manzanas");
            int cantidad_p1_1 = 2;
            double peso_p1_1 = calcular_peso(producto_p1_1, cantidad_p1_1);
            double precio_p1_1 = calcular_precio(producto_p1_1, cantidad_p1_1);
            productos_list.Add((producto_p1_1, cantidad_p1_1, peso_p1_1, precio_p1_1));

            string producto_p1_2 = ("Jabón");
            int cantidad_p1_2 = 3;
            double peso_p1_2 = calcular_peso(producto_p1_2, cantidad_p1_2);
            double precio_p1_2 = calcular_precio(producto_p1_2, cantidad_p1_2);
            productos_list.Add((producto_p1_2, cantidad_p1_2, peso_p1_2, precio_p1_2));

            List<double> precio_peso_p1 = peso_precio_total(productos_list);
            // Asignar un ID único si la tabla ya tiene filas
            pedido1.crear_pedido(table.Rows.Count + 1, new List<(string, int, double, double)>(productos_list), direccion1, "Pol Casals", precio_peso_p1[1], precio_peso_p1[0]);
            pedidos.Add(pedido1);

            DataRow nuevaFila1 = table.NewRow(); // Renombrada variable local
            nuevaFila1["Num."] = pedido1.getId();
            nuevaFila1["Nombre"] = pedido1.getDestinatario();
            nuevaFila1["Dirección"] = pedido1.getDireccion();
            var sbP1 = new StringBuilder(); foreach (var prod in pedido1.getProductos()) { sbP1.Append($"{prod.nombre}({prod.cantidad}) "); }
            nuevaFila1["Lista productos"] = sbP1.ToString().Trim();
            nuevaFila1["Precio"] = pedido1.getPrecioTotal();
            nuevaFila1["Peso"] = pedido1.getPesoTotal();
            table.Rows.Add(nuevaFila1);
            productos_list.Clear();

            // pedido 2:
            Pedido pedido2_obj = new Pedido(); // Renombrada variable local
            PointLatLng direccion2_val = new PointLatLng(41.2808380593931, 1.9700288772583); // Renombrada variable local

            string producto_p2_1 = ("Arroz");
            int cantidad_p2_1 = 1;
            double peso_p2_1 = calcular_peso(producto_p2_1, cantidad_p2_1);
            double precio_p2_1 = calcular_precio(producto_p2_1, cantidad_p2_1);
            productos_list.Add((producto_p2_1, cantidad_p2_1, peso_p2_1, precio_p2_1));

            string producto_p2_2 = ("Café");
            int cantidad_p2_2 = 4;
            double peso_p2_2 = calcular_peso(producto_p2_2, cantidad_p2_2);
            double precio_p2_2 = calcular_precio(producto_p2_2, cantidad_p2_2);
            productos_list.Add((producto_p2_2, cantidad_p2_2, peso_p2_2, precio_p2_2));

            List<double> precio_peso_p2 = peso_precio_total(productos_list);
            pedido2_obj.crear_pedido(table.Rows.Count + 1, new List<(string, int, double, double)>(productos_list), direccion2_val, "Adrià Martos", precio_peso_p2[1], precio_peso_p2[0]);
            pedidos.Add(pedido2_obj);

            DataRow nuevaFila2_val = table.NewRow(); // Renombrada variable local
            nuevaFila2_val["Num."] = pedido2_obj.getId();
            nuevaFila2_val["Nombre"] = pedido2_obj.getDestinatario();
            nuevaFila2_val["Dirección"] = pedido2_obj.getDireccion();
            var sbP2 = new StringBuilder(); foreach (var prod in pedido2_obj.getProductos()) { sbP2.Append($"{prod.nombre}({prod.cantidad}) "); }
            nuevaFila2_val["Lista productos"] = sbP2.ToString().Trim();
            nuevaFila2_val["Precio"] = pedido2_obj.getPrecioTotal();
            nuevaFila2_val["Peso"] = pedido2_obj.getPesoTotal();
            table.Rows.Add(nuevaFila2_val);
            productos_list.Clear();

            // pedido 3:
            Pedido pedido3_obj = new Pedido(); // Renombrada variable local
            PointLatLng direccion3_val = new PointLatLng(41.280999310343, 1.96994304656982); // Renombrada variable local

            string producto_p3_1 = ("Arroz"); // Corregido nombre de variable
            int cantidad_p3_1 = 1;    // Corregido nombre de variable
            double peso_p3_1 = calcular_peso(producto_p3_1, cantidad_p3_1);
            double precio_p3_1 = calcular_precio(producto_p3_1, cantidad_p3_1);
            productos_list.Add((producto_p3_1, cantidad_p3_1, peso_p3_1, precio_p3_1));

            string producto_p3_2 = ("Sal"); // Corregido nombre de variable
            int cantidad_p3_2 = 2;   // Corregido nombre de variable
            double peso_p3_2 = calcular_peso(producto_p3_2, cantidad_p3_2);
            double precio_p3_2 = calcular_precio(producto_p3_2, cantidad_p3_2);
            productos_list.Add((producto_p3_2, cantidad_p3_2, peso_p3_2, precio_p3_2));

            List<double> precio_peso_p3 = peso_precio_total(productos_list);
            pedido3_obj.crear_pedido(table.Rows.Count + 1, new List<(string, int, double, double)>(productos_list), direccion3_val, "Arnau Doménech", precio_peso_p3[1], precio_peso_p3[0]);
            pedidos.Add(pedido3_obj);

            DataRow nuevaFila3_val = table.NewRow(); // Renombrada variable local
            nuevaFila3_val["Num."] = pedido3_obj.getId();
            nuevaFila3_val["Nombre"] = pedido3_obj.getDestinatario();
            nuevaFila3_val["Dirección"] = pedido3_obj.getDireccion();
            var sbP3 = new StringBuilder(); foreach (var prod in pedido3_obj.getProductos()) { sbP3.Append($"{prod.nombre}({prod.cantidad}) "); }
            nuevaFila3_val["Lista productos"] = sbP3.ToString().Trim();
            nuevaFila3_val["Precio"] = pedido3_obj.getPrecioTotal();
            nuevaFila3_val["Peso"] = pedido3_obj.getPesoTotal();
            table.Rows.Add(nuevaFila3_val);
            productos_list.Clear();

            dataGrid.DataSource = table;
            dataGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            f.setPedidos(new List<Pedido>(pedidos)); // Pasar una copia por si acaso
            f.setTable(table.Copy()); // Pasar una copia de la tabla
        }

        private void button1_Click(object sender, EventArgs e)
        {
            f.setTable(table.Copy()); // Pasar copia
            editar_pedido edit_pedido = new editar_pedido(productos_diccionario, f);
            edit_pedido.ShowDialog(); // ShowDialog puede ser mejor para modales
            // Si se hicieron cambios en editar_pedido y se guardaron en 'f', recargar:
            cargar_Click(sender, e);
        }

        private void cargar_Click(object sender, EventArgs e)
        {
            table = f.GetTable()?.Copy() ?? new DataTable(); // Obtener copia o nueva tabla si es null
            pedidos = f.GetPedidos() != null ? new List<Pedido>(f.GetPedidos()) : new List<Pedido>(); // Obtener copia o nueva lista

            dataGrid.DataSource = null; // Limpiar DataSource antes de reasignar
            dataGrid.DataSource = table;
            dataGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            // Actualizar columna "Lista productos" si es necesario (ya debería estar bien formateada)
            // El bucle foreach que tenías aquí para reformatear "Lista productos" puede no ser necesario
            // si la tabla de 'f' ya está correctamente formateada.
        }

        private void eliminar_Click(object sender, EventArgs e)
        {
            table.Clear();
            // f.setTable(table); // No es necesario pasar una tabla vacía si la intención es limpiar en 'f' también
            pedidos.Clear();
            // f.setPedidos(pedidos); // Similarmente

            // Si 'f' debe reflejar la limpieza:
            f.setTable(new DataTable()); // O pasar 'table' si esa es la instancia compartida que se limpia
            f.setPedidos(new List<Pedido>());

            dataGrid.DataSource = null;
            dataGrid.DataSource = table; // Mostrar la tabla vacía
        }

        private void panelMapa_Paint(object sender, PaintEventArgs e)
        {
            // Usualmente no se necesita código aquí para GMapControl, ya que se redibuja solo.
        }
    }
}