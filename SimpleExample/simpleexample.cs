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
    public partial class simpleexample : Form
    {
        MAVLink.MavlinkParse mavlink = new MAVLink.MavlinkParse();

        //private System.Windows.Forms.Timer timer;
        Dron dron = new Dron();
        private GMapControl gmap;
        private GMapOverlay overlay;
        private Bitmap iconoPersonalizado;


        private GMapRoute ruta;
        GMarkerGoogle dronIcon;
        GMapRoute dronHeading;
        List<(float lat, float lon)> mision;

        PointLatLng home; // coordenadas del DroneLab o del Nou Camp (a elegir)
        string nombreHome;
        public simpleexample()
        {

            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;

            // Botones con imagenes de flechas
            button26.Text = "\u2196";
            button25.Text = "\u25B2";
            button24.Text = "\u2197";

            button19.Text = "\u25C4";
            button8.Text = "Stop";
            button5.Text = "\u25BA";

            button4.Text = "\u2199";
            button3.Text = "\u25BC";
            button2.Text = "\u2198";


            try
            {

                iconoPersonalizado = new Bitmap("dron.png"); // Asegúrate de que el archivo esté en la carpeta del ejecutable
                iconoPersonalizado = RedimensionarImagen(iconoPersonalizado, 20, 20); // Reducimos el tamaño del icono            
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la imagen: " + ex.Message);
                iconoPersonalizado = null;
            }


            // Configuración del formulario
            this.Text = "Mapa Interactivo - Círculos de Clic";
            this.Size = new System.Drawing.Size(800, 600);



            // Configurar el control del mapa
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
                Zoom = 18,
                ShowCenter = false
            };
            gmap.MouseDown += GMapControl1_MouseDown; // capturo el evento de click en raton
            this.Controls.Add(gmap);


        }
        private void AñadirWaypoint((float lat, float lon) point)
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
                PointLatLng previo = new PointLatLng(
                    this.mision[this.mision.Count - 2].lat,
                    this.mision[this.mision.Count - 2].lon);
                // si no es el primero trazo una linea que lo une al anterior
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
                option1.Click += (s, ev) => dron.IrAlPunto((float)point.Lat, (float)point.Lng, 20);
                option2.Click += (s, ev) => AñadirWaypoint(((float)point.Lat, (float)point.Lng));

                contextMenu.Items.Add(option1);
                contextMenu.Items.Add(option2);

                contextMenu.Show(gmap, e.Location);
            }
        }


        private void but_connect_Click(object sender, EventArgs e)
        {
            dron.Conectar("sim", CMB_comport.Text);
            but_connect.BackColor = Color.Green;
            but_connect.ForeColor = Color.White;
            // Configurar GMap.NET
            gmap.MapProvider = GMapProviders.GoogleSatelliteMap;
            GMaps.Instance.Mode = AccessMode.ServerOnly;

            gmap.Position = home;
            gmap.MinZoom = 5;
            gmap.MaxZoom = 22;
            gmap.Zoom = 19;
            gmap.ShowCenter = true;
            gmap.Visible = true; // Mostrar el mapa

            gmap.CanDragMap = true;           // Permite arrastrar con el mouse
            gmap.DragButton = MouseButtons.Left; // Se usa el clic izquierdo para arrastrar
            gmap.MouseWheelZoomType = MouseWheelZoomType.MousePositionAndCenter; // Zoom en la posición del cursor
            gmap.IgnoreMarkerOnMouseWheel = true;

            // Crear un overlay para los marcadores y rutas
            overlay = new GMapOverlay("circulos");
            gmap.Overlays.Add(overlay);

            //ruta = new GMapRoute(new List<PointLatLng>(), "Ruta");
            //ruta.Stroke = new Pen(Color.Blue, 3); // Línea azul de grosor 3
            //overlay.Routes.Add(ruta);
            panelMapa.Controls.Add(gmap);
        }


        private void CMB_comport_Click(object sender, EventArgs e)
        {
            // cargo la lista de los puertos disponibles
            CMB_comport.DataSource = SerialPort.GetPortNames();
        }


        private void simpleexample_Load(object sender, EventArgs e)
        {

        }



        private void EnAire(object param)
        {
            // Esto es lo que haré cuando el dron haya alcanzado la altura de despegue
            despegarBtn.BackColor = Color.Green;
            despegarBtn.ForeColor = Color.White;
            despegarBtn.Text = (string) param;
        }

        private void but_takeoff_Click(object sender, EventArgs e)
        {
            // Click en boton para dspegar
            // Llamada no bloqueante para no bloquear el formulario
            dron.Despegar(int.Parse(alturaBox.Text), bloquear: false, EnAire, "Volando");
            despegarBtn.BackColor = Color.Yellow;
        }




        private void navButton_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            string tag = b.Tag.ToString();
            dron.Navegar(tag);

        }
        private void movButton_Click(object sender, EventArgs e)
        {
            // obtenco el boton que he clicado
            Button b = (Button)sender;
            // El tag me indica la dirección en la que debo moverme
            string direccion = b.Tag.ToString();
            // recupero la distancia que debe recorrer el dron
            int distancia = Convert.ToInt32(pasoLbl.Text);
            dron.Mover(direccion, distancia, bloquear: false);

           

        }
        private void EnTierra(object mensaje)
        {
            // Aqui vendre cuando el dron esté en tierra
            // El mensaje me dice si vengo de un aterrizaje o de un RTL
            if ((string) mensaje == "Aterrizaje")
                button7.BackColor = Color.Green;
            else
                button6.BackColor = Color.Green;
        }



        private void button7_Click(object sender, EventArgs e)
        {
            // Click en el botón de aterrizar
            dron.Aterrizar(bloquear: false, EnTierra, "Aterrizaje");
            button7.BackColor = Color.Yellow;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Click en el botón de RTL
            dron.RTL(bloquear: false, EnTierra, "RTL");
            button6.BackColor = Color.Yellow;
        }

        private void button23_Click(object sender, EventArgs e)
        {

            dron.EnviarDatosTelemetria(ProcesarTelemetria);
        }

        private void button22_Click(object sender, EventArgs e)
        {
            dron.DetenerDatosTelemetria();
        }

        private void ProcesarTelemetria(List<(string nombre, float valor)> telemetria)
        {
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
            double anguloRad = angulo * Math.PI / 180.0;
            // La línea que señala el heading va desde la posición del dron
            // hasta 50 metros en la dirección del heading
            double distanciaGrados = 50 / 111139.0;
            // Calculo hasta donde debe llegar la línea
            double lat2 = lat + (distanciaGrados * Math.Cos(anguloRad));
            double lon2 = lon + (distanciaGrados * Math.Sin(anguloRad) / Math.Cos(lat * Math.PI / 180));

            // Ahora dibujo la linea
            List<PointLatLng> puntosLinea = new List<PointLatLng>
            {
                new PointLatLng(lat, lon),  // Punto de inicio
                new PointLatLng(lat2, lon2) // Punto final
            };
            // Borro el marcador y linea de heading del dron si es que hay
            if (dronIcon != null)
                overlay.Markers.Remove(dronIcon);
            if (dronHeading != null)
                overlay.Routes.Remove(dronHeading);
            // Dibujo la linea y añado el nuevo marcador
            dronHeading = new GMapRoute(puntosLinea, "miLinea")
            {
                Stroke = new Pen(Color.Red, 2) // Color rojo, grosor 2
            };

            overlay.Routes.Add(dronHeading);
            dronIcon = new GMarkerGoogle(point, iconoPersonalizado);
            // Añado un offset para que el centro del icono esté exactamente en la
            // posición del mapa en la que está el dron
            dronIcon.Offset = new Point(-iconoPersonalizado.Width / 2, -iconoPersonalizado.Height / 2);
            overlay.Markers.Add(dronIcon);

        }

        private Bitmap RedimensionarImagen(Bitmap img, int ancho, int alto)
        {
            Bitmap nuevaImagen = new Bitmap(ancho, alto);
            using (Graphics g = Graphics.FromImage(nuevaImagen))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(img, 0, 0, ancho, alto);
            }
            return nuevaImagen;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            // Recojo el valor de la distancia a aplicar en las operaciones
            // de movimiento
            int n = trackBar1.Value;
            pasoLbl.Text = n.ToString();
        }
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            // Recojo el valor de la distancia a aplicar en las operaciones
            // de movimiento
            int n = trackBar2.Value;
            headingLbl.Text = n.ToString();
        }


        private void trackBar2_MouseUp(object sender, MouseEventArgs e)
        {
            // Cuando se libera la barra de desplazamiento recojo el valor
            // definitivo para el heading y lo envío al dron
            float valorSeleccionado = trackBar2.Value;
            dron.CambiarHeading(valorSeleccionado);
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            // Recojo y muestro el valor la velocidad según se mueve 
            // la barra de desplazamiento
            int n = trackBar3.Value;
            velocidadLbl.Text = n.ToString();

        }

        private void trackBar3_MouseUp(object sender, MouseEventArgs e)
        {
            // Cuando se libera la barra de desplazamiento recojo el valor
            // definitivo para la velocidad y lo envío al dron
            int valorSeleccionado = trackBar3.Value;
            dron.CambiaVelocidad(valorSeleccionado);
        }
        private void DibujarLimites(List<(float lat, float lon)> limites)
        {
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

        private void button27_Click(object sender, EventArgs e)
        {
            if (nombreHome == "Nou Camp")
            {
                // Envio un escenario predefinido formado por el rectangulo
                // de juego del Nou Camp como limites
                List<(float lat, float lon)> fenceInclusion = new List<(float lat, float lon)>
                {
                   (41.381231903161684f, 2.1222157786711398f),  // Punto 1
                    (41.38144678751476f, 2.1229592490702984f),  // Punto 2
                    (41.380573014924174f, 2.1234296078942556f),   // Punto 3
                    (41.38035516016044f, 2.122686439511463f)   // Punto 3
                };

                List<List<(float lat, float lon)>> scenario = new List<List<(float lat, float lon)>>();
                scenario.Add(fenceInclusion);
                DibujarLimites(fenceInclusion);

                // Las dos áreas pequeñas como obstáculos
                List<(float lat, float lon)> fenceEnclusion = new List<(float lat, float lon)>
                {
                     (41.3812630831125f, 2.122367454083514f),  // Punto 1
                     (41.38140016195083f, 2.1228174183374695f),  // Punto 2
                     (41.38126985244463f, 2.1228873375949764f),   // Punto 3
                     (41.381135311836346f, 2.1224407565309003f)   // Punto 3
                };
                scenario.Add(fenceEnclusion);
                DibujarObstaculo(fenceEnclusion);
                fenceEnclusion = new List<(float lat, float lon)>
                {
                     (41.380528000007885f, 2.1227596575244694f),  // Punto 1
                     (41.38066592656998f, 2.1232051108585854f),  // Punto 2
                     (41.38053646176858f, 2.123277285576012f),   // Punto 3
                     (41.380406996709425f, 2.122828449052016f)   // Punto 3
                };
                scenario.Add(fenceEnclusion);
                DibujarObstaculo(fenceEnclusion);

                // Y el circulo central también como obstaculo
                fenceEnclusion = new List<(float lat, float lon)>
                {
                     (41.38089947817193f, 2.122819453881491f),  // Centro
                     (10f, 0),  // Radio
                };
                scenario.Add(fenceEnclusion);
                DibujarCirculo(fenceEnclusion);

                dron.EstableceEscenario(scenario);
        
                button27.BackColor = Color.Green;
                button27.ForeColor = Color.White;
            } else
            {
                // Envio un escenario predefinido formado por el rectangulo
                // con los límites del droneLab
                List<(float lat, float lon)> fenceInclusion = new List<(float lat, float lon)>
                {
                    (41.27643157806237f, 1.9882249167791768f),  
                    (41.27660756926631f, 1.9890211145457177f),  
                    (41.276378780608624f, 1.9891254292263607f),   
                    (41.276194789148306f, 1.9883313603308532f)  
                };

                List<List<(float lat, float lon)>> scenario = new List<List<(float lat, float lon)>>();
                scenario.Add(fenceInclusion);
                DibujarLimites(fenceInclusion);

                // Y un poligono como obstaculo
                List<(float lat, float lon)> fenceEnclusion = new List<(float lat, float lon)>
                {
                     (41.27638358037891f, 1.9884803813032008f), 
                     (41.27640757922506f, 1.9887081705037886f),  
                     (41.27650197460089f, 1.9888018408292638f),  
                     (41.27655797179322f, 1.988935959704376f),
                     (41.27639157999527f, 1.9890317589008852f),  
                     (41.27626838579467f, 1.988493154529402f)   
                };
                scenario.Add(fenceEnclusion);
                DibujarObstaculo(fenceEnclusion);
                dron.EstableceEscenario(scenario);
                button27.BackColor = Color.Green;
                button27.ForeColor = Color.White;
            }


        }
        private void FinMision (object param=null)
        {
            dron.PonModoGuiado();
        }

        private void button28_Click(object sender, EventArgs e)
        {
            dron.EjecutarMision(bloquear: false, FinMision, null);
            button28.BackColor = Color.Green;
            button28.ForeColor = Color.White;
        }

        private void button29_Click(object sender, EventArgs e)
        {
            // En mision están los waypoints que se han ido recogiendo
            // cuando el usuario clicado sobre el mapa
            dron.CargarMision(mision);
            button29.BackColor = Color.Green;
            button29.ForeColor = Color.White;
        }

        private void parámetrosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Abro el formulario para leer/escribir los parámetros
            Parametros f = new Parametros(dron);
            f.ShowDialog();
        }

        private void button18_Click(object sender, EventArgs e)
        {
         
            nombreHome = "DroneLab";
            home = new PointLatLng(41.27641528631191, 1.988679188034754);
            button18.BackColor = Color.Green;
            button18.ForeColor = Color.White;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            nombreHome = "Nou Camp";
            home = new PointLatLng(41.38090032929851, 2.12283331445973);
            button1.BackColor = Color.Green;
            button1.ForeColor = Color.White;

        }

        private void simRadio_Click(object sender, EventArgs e)
        {

        }

        private void simRadio_CheckedChanged(object sender, EventArgs e)
        {
        
            CMB_comport.Visible = false;

        }

        private void prodRadio_CheckedChanged(object sender, EventArgs e)
        {
     
            CMB_comport.Visible = true;
      
        }
    }

}

