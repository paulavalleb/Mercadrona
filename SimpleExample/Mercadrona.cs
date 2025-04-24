using csDronLink;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET.WindowsForms.Markers;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.IO.Ports;

namespace SimpleExample
{
    public partial class Mercadrona : Form
    {
        Dron dron = new Dron();

        List<Dron> drons_list = new List<Dron>(); // lista de drons que se conectan al programa

        // para gestionar el mapa: diferentes mapas para cada capa
        private GMapControl gmap;
        private GMapOverlay overlay;
        private GMapRoute ruta;

        // Para representar el dron
        private Bitmap iconoPersonalizado; //imagen de dron
        GMarkerGoogle dronIcon; // marcador para la posición del dron
        GMapRoute dronHeading; // linea marcando el heading

        // aqui iremos guardando los waypoints de la misión
        List<(float lat, float lon)> mision;

        PointLatLng home; // Coordenadas Mercadona
        string nombreHome;

        public Mercadrona()
        {
            // No queremos que nos molesten con la excepción Cross-Threading
            CheckForIllegalCrossThreadCalls = false;

            InitializeComponent();

            // Hacemos que el formulario principal ocupe toda la pantalla
            this.WindowState = FormWindowState.Maximized;

            // Cargamos la imagen del dron (un punto rojo)
            iconoPersonalizado = new Bitmap("dron.png");
            // y ajustamos su trabajo
            iconoPersonalizado = RedimensionarImagen(iconoPersonalizado, 20, 20);

            // Configuración del formulario
            //this.Text = "Mapa Interactivo - Círculos de Clic";
            //this.Size = new System.Drawing.Size(800, 600);

            // Configurar el control del mapa
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
            gmap.MouseDown += GMapControl1_MouseDown; // capturo el evento de click en raton
            this.Controls.Add(gmap);
        }

        private void Mercadrona_Load(object sender, EventArgs e)
        {
            drons_list.Add(dron); // añado el dron a la lista de drons
            desplegable.Items.Add("1");
        }



        public Bitmap RedimensionarImagen(Bitmap img, int ancho, int alto) // antes era private
        {
            Bitmap nuevaImagen = new Bitmap(ancho, alto);
            using (Graphics g = Graphics.FromImage(nuevaImagen))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(img, 0, 0, ancho, alto);
            }
            return nuevaImagen;
        }

        public void AñadirWaypoint((float lat, float lon) point)
        {
            // Cuando estoy creando una misión, vengo aquí al clicar
            // en el mapa

            if (this.mision == null)
                // es el primer waypoint de la misión
                this.mision = new List<(float lat, float lon)>();

            PointLatLng p = new PointLatLng(point.lat, point.lon);
            // añado un marcador
            GMarkerGoogle marker = new GMarkerGoogle(p, GMarkerGoogleType.blue_dot);
            overlay.Markers.Add(marker);
            // añado el punto a la misión
            this.mision.Add(point);

            if (this.mision.Count > 1)
            {
                // Hay que dibujar la linea que va del anterior al nuevo
                PointLatLng previo = new PointLatLng(
                    this.mision[this.mision.Count - 2].lat,
                    this.mision[this.mision.Count - 2].lon);
                List<PointLatLng> points = new List<PointLatLng>
                {
                    previo,
                    p
                };
                GMapRoute route = new GMapRoute(points, "Line")
                {
                    Stroke = new System.Drawing.Pen(System.Drawing.Color.Blue, 2) // Color y grosor de la línea
                };
                overlay.Routes.Add(route);
            }

        }
        private void GMapControl1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // aqui vendre cuando pulse el boton derecho del raton

                PointLatLng point = gmap.FromLocalToLatLng(e.X, e.Y);

                // muestro el desplegable con las 3 opciones (solo las dos primeras
                // están operativas
                ContextMenuStrip contextMenu = new ContextMenuStrip();

                ToolStripMenuItem option1 = new ToolStripMenuItem("Volar aquí");
                ToolStripMenuItem option2 = new ToolStripMenuItem("Pon waypoint");
                ToolStripMenuItem option3 = new ToolStripMenuItem("Opción 3");
                // indico lo que ha que hacer según la opción elegida
                option1.Click += (s, ev) => dron.IrAlPunto((float)point.Lat, (float)point.Lng, 20, bloquear: false);
                option2.Click += (s, ev) => AñadirWaypoint(((float)point.Lat, (float)point.Lng));

                contextMenu.Items.Add(option1);
                contextMenu.Items.Add(option2);

                contextMenu.Show(gmap, e.Location);
            }
        }


        private void CMB_comport_Click(object sender, EventArgs e)
        {
            // cargo la lista de los puertos disponibles
            CMB_comport.DataSource = SerialPort.GetPortNames();
        }



        private void EnAire(object param)
        {
            // Esto es lo que haré cuando el dron haya alcanzado la altura de despegue
            despegarBtn.BackColor = Color.Green;
            despegarBtn.ForeColor = Color.White;
            despegarBtn.Text = (string)param;
        }

        private void but_takeoff_Click(object sender, EventArgs e)
        {
            // Click en boton para dspegar
            // Llamada no bloqueante para no bloquear el formulario
            dron.Despegar(int.Parse(alturaBox.Text), bloquear: false, EnAire, "Volando");

            despegarBtn.BackColor = Color.Yellow;
        }
        private void EnTierra(object mensaje)
        {
            // Aqui vendre cuando el dron esté en tierra
            // El mensaje me dice si vengo de un aterrizaje o de un RTL
            if ((string)mensaje == "Aterrizaje")
                button7.BackColor = Color.Green;
            else
                button6.BackColor = Color.Green;
        }

        private void aterrizarBtn_Click(object sender, EventArgs e)
        {
            // Click en el botón de aterrizar
            dron.Aterrizar(bloquear: false, EnTierra, "Aterrizaje");
            button7.BackColor = Color.Yellow;
        }

        private void RTLBtn_Click(object sender, EventArgs e)
        {
            // Click en el botón de RTL
            dron.RTL(bloquear: false, EnTierra, "RTL");
            button6.BackColor = Color.Yellow;
        }

        private void enviarTelemetriaBtn_Click(object sender, EventArgs e)
        {

            dron.EnviarDatosTelemetria(ProcesarTelemetria);
        }

        private void detenerTelemetriaBtn_Click(object sender, EventArgs e)
        {
            dron.DetenerDatosTelemetria();
        }

        private void ProcesarTelemetria(List<(string nombre, float valor)> telemetria)
        {
            // Aqui vendre cada vez que llegue un paquete de telemetría
            double lat = ((double)telemetria[1].valor) / 0.1E+8;
            double lon = ((double)telemetria[2].valor) / 0.1E+8;
            double heading = ((double)telemetria[3].valor) / 100;

            // Coloco los datos de telemetria en su sitio
            altitudLbl.Text = telemetria[0].valor.ToString();
            latitudLbl.Text = lat.ToString();
            longitudLbl.Text = lon.ToString();
            headLbl.Text = heading.ToString();

            // Actualizo posición y heading del dron en el mapa
            PointLatLng point = new PointLatLng(lat, lon);
            int angulo = Convert.ToInt16(telemetria[3].valor / 100);

            // Defino la linea que debe señalar el heading
            double anguloRad = angulo * Math.PI / 180.0;
            // La línea que señala el heading va desde la posición del dron
            // hasta 50 metros en la dirección del heading
            double distanciaGrados = 50 / 111139.0;
            // Calculo hasta donde debe llegar la línea
            double lat2 = lat + (distanciaGrados * Math.Cos(anguloRad));
            double lon2 = lon + (distanciaGrados * Math.Sin(anguloRad) / Math.Cos(lat * Math.PI / 180));

            // Ahora preparo la linea
            List<PointLatLng> puntosLinea = new List<PointLatLng>
            {
                new PointLatLng(lat, lon),  // Punto de inicio
                new PointLatLng(lat2, lon2) // Punto final
            };

            // Borro el marcador y linea de heading del dron si es que hay
            if (overlay.Markers.Contains(dronIcon))
            {
                overlay.Markers.Remove(dronIcon);
                overlay.Routes.Remove(dronHeading);
            }
            dronHeading = new GMapRoute(puntosLinea, "miLinea")
            {
                Stroke = new Pen(Color.Red, 2) // Color rojo, grosor 2
            };
            // Añado la linea
            overlay.Routes.Add(dronHeading);
            // y el icono del dron
            dronIcon = new GMarkerGoogle(point, iconoPersonalizado);
            // Añado un offset para que el centro del icono esté exactamente en la
            // posición del mapa en la que está el dron
            dronIcon.Offset = new Point(-iconoPersonalizado.Width / 2, -iconoPersonalizado.Height / 2);
            overlay.Markers.Add(dronIcon);

        }



        private void headingTrackBar_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void velocidadTrackBar_MouseUp(object sender, MouseEventArgs e)
        {

        }
        private void DibujarLimites(List<(float lat, float lon)> limites)
        {
            // Dibujo el poligono que define los límites del area de vuelo (geofence
            // de inclusió)
            // Convierto los puntos
            List<PointLatLng> puntos = new List<PointLatLng>();
            foreach ((float lat, float lon) p in limites)
                puntos.Add(new PointLatLng(p.lat, p.lon));
            // Dibujo en el mapa el polígono que represeta el geofence de inclusión
            GMapPolygon polygon = new GMapPolygon(puntos, "Polygon");
            // Establecer un relleno completamente transparente
            polygon.Fill = new SolidBrush(Color.FromArgb(0, 0, 0, 0)); // ARGB (0 = totalmente transparente)
            polygon.Stroke = new Pen(Color.Red, 2); // Solo borde rojo
            overlay.Polygons.Add(polygon);
        }

        private void DibujarCirculo(List<(float lat, float lon)> datos, int segments = 36)
        {
            // Dibujo el círculo que representa un obstaculo
            // Se representa mediante un polígono de tantos lados como indique 
            // segments

            double lat = datos[0].lat;
            double lon = datos[0].lon;
            double radiusMeters = datos[1].lat;

            double radiusLat = radiusMeters / 111320.0; // Aproximación de metros a grados en latitud
            double radiusLon = radiusMeters / (111320.0 * Math.Cos(lat * Math.PI / 180.0)); // Ajuste para la longitud
            List<PointLatLng> points = new List<PointLatLng>();
            for (int i = 0; i < segments; i++)
            {
                double angle = (Math.PI / 180) * (i * (360.0 / segments));
                double offsetX = radiusLon * Math.Cos(angle);
                double offsetY = radiusLat * Math.Sin(angle);

                points.Add(new PointLatLng(lat + offsetY, lon + offsetX));
            }

            GMapPolygon circle = new GMapPolygon(points, "Circle");

            // Relleno semitransparente
            circle.Fill = new SolidBrush(Color.FromArgb(50, Color.Black));
            circle.Stroke = new Pen(Color.Black, 2); // Contorno azul

            overlay.Polygons.Add(circle);
        }

        private void DibujarObstaculo(List<(float lat, float lon)> limites)
        {
            // Convierto los puntos
            List<PointLatLng> puntos = new List<PointLatLng>();
            foreach ((float lat, float lon) p in limites)
                puntos.Add(new PointLatLng(p.lat, p.lon));
            // Dibujo el polígono que representa un obstaculo
            GMapPolygon polygon = new GMapPolygon(puntos, "Polygon");
            polygon.Fill = new SolidBrush(Color.FromArgb(50, Color.Black));
            polygon.Stroke = new Pen(Color.Black, 2); // Solo borde rojo
            overlay.Polygons.Add(polygon);
        }

        private void FinMision(object param = null)
        {
            // Borro los puntos y lineas de la misión
            overlay.Markers.Clear();
            overlay.Routes.Clear();
            mision = null;
            dron.PonModoGuiado();
        }
        private void EnWaypoint(object n)
        {
            // Acabo de llegar al siguiente waypoint de la misión
            Console.WriteLine("Estoy en waypoint " + (int)n);
            (float lat, float lon) waypoint = mision[(int)n];

            PointLatLng p = new PointLatLng(waypoint.lat, waypoint.lon);
            // añado un marcador de color verde
            GMarkerGoogle marker = new GMarkerGoogle(p, GMarkerGoogleType.green_dot);
            overlay.Markers.Add(marker);
        }

        private void parámetrosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Abro el formulario para leer/escribir los parámetros
            Parametros f = new Parametros(dron);
            f.ShowDialog();
        }

        private void simRadio_CheckedChanged(object sender, EventArgs e)
        {
            CMB_comport.Visible = false;
        }

        private void prodRadio_CheckedChanged(object sender, EventArgs e)
        {
            CMB_comport.Visible = true;
        }

        private void but_connect_Click(object sender, EventArgs e)
        {
            if (!prodRadio.Checked && !simRadio.Checked)
                MessageBox.Show("No has elegido un modo de control: simulación o producción.");
            else
            {
                if (prodRadio.Checked)
                    dron.Conectar("produccion", CMB_comport.Text);
                else
                    dron.Conectar("simulacion");


                but_connect.BackColor = Color.Green;
                but_connect.ForeColor = Color.White;

                // Acabo la configuración del mapa
                gmap.MapProvider = GMapProviders.GoogleSatelliteMap;
                GMaps.Instance.Mode = AccessMode.ServerOnly;


                // Situo el mapa en el home elegido (DroneLab o Nou Camp
                gmap.Position = home;

                gmap.Visible = true; // Mostrar el mapa

                // Crear un overlay para los marcadores y rutas
                overlay = new GMapOverlay("circulos");
                gmap.Overlays.Add(overlay);
                panelMapa.Controls.Add(gmap);

                // Zona de operación Mercadrona

                List<(float lat, float lon)> fenceInclusion = new List<(float lat, float lon)> // o fer cercle
                {
                    (41.29000677730377f, 1.98301118f),
                    (41.29000677730377f, 1.9830111940801851f),
                    (41.290005f, 1.98301118f),
                    (41.290005f, 1.9830111940801851f)
                };
                List<List<(float lat, float lon)>> scenario = new List<List<(float lat, float lon)>>();
                scenario.Add(fenceInclusion);
                DibujarLimites(fenceInclusion);

                // Zonas que no se pueden sobrevolar

                // Colegios...
                /*List<(float lat, float lon)> fenceEnclusion = new List<(float lat, float lon)>
                {
                     (41.27638358037891f, 1.9884803813032008f),
                     (41.27640757922506f, 1.9887081705037886f),
                     (41.27650197460089f, 1.9888018408292638f),
                     (41.27655797179322f, 1.988935959704376f),
                     (41.27639157999527f, 1.9890317589008852f),
                     (41.27626838579467f, 1.988493154529402f)
                };
                dron.EstableceEscenario(scenario);
                scenario.Add(fenceEnclusion);
                DibujarObstaculo(fenceEnclusion);
                dron.EstableceEscenario(scenario);
                button27.BackColor = Color.Green;
                button27.ForeColor = Color.White;
                */
            }
        }

        private void trackBarSpeed_Scroll(object sender, EventArgs e)
        {
            // Recojo y muestro el valor la velocidad según se mueve 
            // la barra de desplazamiento
            int n = trackBarSpeed.Value;
            velocidadLbl.Text = n.ToString();
        }

        private void trackBarHeading_Scroll(object sender, EventArgs e)
        {
            // Recojo el valor del heading seleccionado
            int n = trackBarHeading.Value;
            headingLbl.Text = n.ToString();
        }

        private void trackBarDistancia_Scroll(object sender, EventArgs e)
        {
            // Recojo el valor de la distancia a aplicar en las operaciones
            // de movimiento
            int n = trackBarDistancia.Value;
            pasoLbl.Text = n.ToString();
        }

        private void Connectar_click(object sender, EventArgs e)
        {
            desplegable.Items.Clear();
            drons_list.Clear();
            int numDrons = Convert.ToInt16(textBoxNumDrons.Text);
            for (int i = 1; i <= numDrons; i++)
            {
                Dron dron = new Dron();
                drons_list.Add(dron);
                desplegable.Items.Add(i);
            }
        }
    }

}


