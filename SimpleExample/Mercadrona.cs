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
    public partial class Mercadrona : Form
    {
        // Dron:
        List<Dron> drons_list = new List<Dron> (); // lista de drons que se conectan al programa
        Dron dron_selected = new Dron();

        // Diseño
        
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
        private void GMapControl1_MouseDown(object sender, MouseEventArgs e) // no seria click?
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
                option1.Click += (s, ev) => dron_selected.IrAlPunto((float)point.Lat, (float)point.Lng, 20, bloquear: false);
                option2.Click += (s, ev) => AñadirWaypoint(((float)point.Lat, (float)point.Lng));

                contextMenu.Items.Add(option1);
                contextMenu.Items.Add(option2);

                contextMenu.Show(gmap, e.Location);
            }
        }

        private void EnTierra()
        {
            dron_selected.SetFase(0);
            despegarBtn.BackColor = Color.DarkOrange;
            aterrizarBtn.BackColor = Color.Orange;
            RTLBtn.BackColor = Color.Orange;
            despegarBtn.Text = "Despegar";
        }

        private void EnAire(object param)
        {
            // Esto es lo que haré cuando el dron haya alcanzado la altura de despegue
            dron_selected.SetFase(1);
            despegarBtn.BackColor = Color.Orange;
            despegarBtn.Text = (string)param;
        }

        private void EnTierra(object mensaje)
        {
            // Aqui vendre cuando el dron esté en tierra
            // El mensaje me dice si vengo de un aterrizaje o de un RTL
            //if ((string)mensaje == "Aterrizaje")

            dron_selected.SetFase(2);
            despegarBtn.BackColor = Color.DarkOrange;
            aterrizarBtn.BackColor = Color.Orange;
            RTLBtn.BackColor = Color.Orange;
            despegarBtn.Text = "Despegar";
        }
        private void Aterrizando()
        {
            dron_selected.SetFase(3);
            despegarBtn.BackColor = Color.Orange;
            aterrizarBtn.BackColor = Color.Green;
            RTLBtn.BackColor = Color.Orange;
            despegarBtn.Text = "Despegar";
        }
        private void en_RTL()
        {
            dron_selected.SetFase(4);
            despegarBtn.BackColor = Color.Orange;
            aterrizarBtn.BackColor = Color.Orange;
            RTLBtn.BackColor = Color.Green;
            despegarBtn.Text = "Despegar";
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

        private void DibujarCirculo(List<(float lat, float lon)> datos, int segments = 18)
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
            dron_selected.PonModoGuiado();
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


        private void Connectar_button_Click(object sender, EventArgs e)
        {
            if (!prodRadio.Checked && !simRadio.Checked)
                MessageBox.Show("No has elegido un modo de control: simulación o producción.");
            else
            {
                Connectar_button.BackColor = Color.Green;
                Connectar_button.ForeColor = Color.White;

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

                List<(float lat, float lon)> castelldefels = new List<(float lat, float lon)>
                {
                    (41.26550366424082f,1.957548455354883f),
                         (41.2659333294012f,1.996936111622045f),
                         (41.28955283495678f,1.999446311437694f),
                         (41.29630656658788f,1.977017624399422f),
                         (41.28111720670424f,1.954384326190344f),
                         (41.26550366424082f,1.957548455354883f)
                };

               
                List<List<(float lat, float lon)>> scenario = new List<List<(float lat, float lon)>>();
                scenario.Add(castelldefels);
                DibujarCirculo(castelldefels);

                // Zonas que no se pueden sobrevolar
                
                // Colegios...
                List<(float lat, float lon)> fenceEnclusion1 = new List<(float lat, float lon)>
                    {
                        //Zona prohibida 1:
                        (41.27099943353775f,1.977631560058872f),
                     (41.27027621209532f,1.97767676002818f),
                     (41.27030454020156f,1.979152792360499f),
                     (41.27104327071283f,1.979154935020309f),

                    };
                
                List<(float lat, float lon)> fenceEnclusion2 = new List<(float lat, float lon)>
                    {
                        //Zona prohibida 2:
                        (41.27283798891926f,1.988586504728218f),
                        (41.27147830210465f,1.988507551032976f),
                        (41.27160855559626f,1.990522529649097f),
                        (41.27334509840505f,1.990535784068159f),
                        (41.27345449942672f,1.988582844079294f),
                        (41.27283798891926f,1.988586504728218f)
                    };
                
                List<(float lat, float lon)> fenceEnclusion3 = new List<(float lat, float lon)>
                    { 
                        //Zona prohibida 3:
                        (41.2770882556513f,1.980939593902604f),
                        (41.27809273912374f,1.983876271785934f),
                        (41.27946942271178f,1.98302120145837f),
                        (41.27860920655556f,1.979998984963587f),
                        (41.2770882556513f,1.980939593902604f)
                    };
                List<(float lat, float lon)> fenceEnclusion4 = new List<(float lat, float lon)>
                    { 
                        //Zona prohibida 4:
                        (41.27463243683545f,1.976336649473214f),
                        (41.27545515341081f,1.977646093108056f),
                        (41.2764290286723f,1.976632835194603f),
                        (41.27550160648772f,1.975356767951471f),
                        (41.27463243683545f,1.976336649473214f)
                    };
                List<(float lon, float lat)> fenceEnclusion5 = new List<(float lon, float lat)>
                    {
                        (41.2741639050083f, 1.967764949657134f),
                        (41.27440508502002f, 1.969788531763554f),
                        (41.2757897553886f, 1.969574400775693f),
                        (41.27551718621686f, 1.967610846031294f),
                        (41.2741639050083f, 1.967764949657134f)
                    };

                List<(float lon, float lat)> fenceEnclusion6 = new List<(float lon, float lat)>
                    {
                        (41.27704673951776f, 1.969464373460454f),
                        (41.27715599299962f, 1.970586151283267f),
                        (41.27800611896174f, 1.97051349514578f),
                        (41.27790609839828f, 1.969290429840806f),
                        (41.27704673951776f, 1.969464373460454f)
                    };

                List<(float lon, float lat)> fenceEnclusion7 = new List<(float lon, float lat)>
                    {
                        (41.27767293756999f, 1.971114672959771f),
                        (41.2776723586971f, 1.971938476524795f),
                        (41.27846503158283f, 1.972043622484534f),
                        (41.27845996926608f, 1.97121952282644f),
                        (41.27767293756999f, 1.971114672959771f)
                    };

                List<(float lon, float lat)> fenceEnclusion8 = new List<(float lon, float lat)>
                    {
                        (41.28097767218927f, 1.970712858695269f),
                        (41.28114914950467f, 1.971760842925125f),
                        (41.28201519901487f, 1.971724957251053f),
                        (41.28180506424749f, 1.970623027175686f),
                        (41.28097767218927f, 1.970712858695269f)
                    };

                List<(float lon, float lat)> fenceEnclusion9 = new List<(float lon, float lat)>
                    {
                        (41.28255617778812f, 1.971794836784413f),
                        (41.28314309908107f, 1.974440002356612f),
                        (41.28444264632859f, 1.974062963453653f),
                        (41.28387145460871f, 1.971409068803733f),
                        (41.28255617778812f, 1.971794836784413f)
                    };

                List<(float lon, float lat)> fenceEnclusion10 = new List<(float lon, float lat)>
                    {
                        (41.28811777147848f, 1.972859760110355f),
                        (41.28697659676988f, 1.973151605557883f),
                        (41.28719281173127f, 1.974647846628526f),
                        (41.28836973015334f, 1.974360220181166f),
                        (41.28811777147848f, 1.972859760110355f)
                    };

                List<(float lon, float lat)> fenceEnclusion11 = new List<(float lon, float lat)>
                    {
                        (41.28721961665448f, 1.980898818698691f),
                        (41.28621053921606f, 1.980963075418516f),
                        (41.28625954833255f, 1.982195232795525f),
                        (41.28725778896633f, 1.982098479461161f),
                        (41.28721961665448f, 1.980898818698691f)
                    };

                List<(float lon, float lat)> fenceEnclusion12 = new List<(float lon, float lat)>
                    {
                        (41.28856366187149f, 1.981545723049238f),
                        (41.28777363537311f, 1.981659168083165f),
                        (41.28781966231267f, 1.982643414438363f),
                        (41.28859877900759f, 1.982541961489621f),
                        (41.28856366187149f, 1.981545723049238f)
                    };

                List<(float lon, float lat)> fenceEnclusion13 = new List<(float lon, float lat)>
                    {
                        (41.28896657919925f, 1.983493688840789f),
                        (41.28810331729189f, 1.983385431599158f),
                        (41.28815457623424f, 1.984655131702415f),
                        (41.2889747038954f, 1.984764767514045f),
                        (41.28896657919925f, 1.983493688840789f)
                    };

                List<(float lon, float lat)> fenceEnclusion14 = new List<(float lon, float lat)>
                    {
                        (41.2909775748782f, 1.986669906670917f),
                        (41.28926594689915f, 1.986640771799881f),
                        (41.28940572342928f, 1.988783092985051f),
                        (41.29104037324556f, 1.988935905247915f),
                        (41.2909775748782f, 1.986669906670917f)
                    };
                //scenario.Add(fenceEnclusion1);
                //scenario.Add(fenceEnclusion2);
                //scenario.Add(fenceEnclusion3);
                //scenario.Add(fenceEnclusion4);
                //scenario.Add(fenceEnclusion5);
                //scenario.Add(fenceEnclusion6);
                //scenario.Add(fenceEnclusion7);
                //scenario.Add(fenceEnclusion8);
                //scenario.Add(fenceEnclusion9);
                //scenario.Add(fenceEnclusion10);
                //scenario.Add(fenceEnclusion11);
                //scenario.Add(fenceEnclusion12);
                //scenario.Add(fenceEnclusion13);
                //scenario.Add(fenceEnclusion14);
                //DibujarObstaculo(fenceEnclusion1);
                //DibujarObstaculo(fenceEnclusion2);
                //DibujarObstaculo(fenceEnclusion3);
                //DibujarObstaculo(fenceEnclusion4);
                //DibujarObstaculo(fenceEnclusion5);
                //DibujarObstaculo(fenceEnclusion6);
                //DibujarObstaculo(fenceEnclusion7);
                /*DibujarObstaculo(fenceEnclusion8);
                DibujarObstaculo(fenceEnclusion9);
                DibujarObstaculo(fenceEnclusion10);
                DibujarObstaculo(fenceEnclusion11);
                DibujarObstaculo(fenceEnclusion12);
                DibujarObstaculo(fenceEnclusion13);
                DibujarObstaculo(fenceEnclusion14);
                */
                if (prodRadio.Checked)
                {
                    dron_selected.Conectar("produccion", CMB_comport.Text);
                }
                else
                {
                    desplegable.Items.Clear();

                    if (textBoxNumDrons.Text == null)
                    {
                        MessageBox.Show("Porfavor, indica el número de drones a conectar");
                    }
                    else
                    {
                        int numDrons = Convert.ToInt32(textBoxNumDrons.Text);
                        for (byte i = 1; i <= numDrons; i++)
                        {
                            Dron dron = new Dron(i);

                            drons_list.Add(dron);
                            desplegable.Items.Add(i);
                            dron.Conectar("simulacion");
                            //dron.EstableceEscenario(scenario);
                        }
                    }
                }
            }
        }


        private void Mercadrona_Load(object sender, EventArgs e)
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

        private void CMB_comport_Click(object sender, EventArgs e)
        {
            // cargo la lista de los puertos disponibles
            CMB_comport.DataSource = SerialPort.GetPortNames();
        }

        

        private void despegarBtn_Click(object sender, EventArgs e)
        {
            // Click en boton para dspegar
            // Llamada no bloqueante para no bloquear el formulario
            
            dron_selected.Despegar(int.Parse(alturaBox.Text), bloquear: false, EnAire, "Volando");
            despegarBtn.BackColor = Color.Yellow;
            RTLBtn.BackColor = Color.DarkOrange;
            aterrizarBtn.BackColor = Color.DarkOrange;
        }
        
        private void RTLBtn_Click(object sender, EventArgs e)
        {
            // Click en el botón de RTL
            dron_selected.RTL(bloquear: false, EnTierra, "RTL");
            RTLBtn.BackColor = Color.Yellow;
            despegarBtn.BackColor = Color.DarkOrange;
        }

        private void aterrizarBtn_Click(object sender, EventArgs e)
        {
            // Click en el botón de aterrizar
            dron_selected.Aterrizar(bloquear: false, EnTierra, "Aterrizaje");
            aterrizarBtn.BackColor = Color.Yellow;
            despegarBtn.BackColor = Color.DarkOrange;
        }

        private void navButton_Click(object sender, EventArgs e)
        {
            // Aqui vendremos cuando se clique cualquiera de los botones de navagación
            // En el tag del boton tenemos la dirección de navegación.
            Button b = (Button)sender;
            string tag = b.Tag.ToString();
            dron_selected.Navegar(tag);

        }
        private void movButton_Click(object sender, EventArgs e)
        {
            // Aqui vendremos cuando se clique cualquiera de los botones de movimiento
            // En el tag del boton tenemos la dirección de navegación.

            Button b = (Button)sender;
            string direccion = b.Tag.ToString();
            // recupero la distancia que debe recorrer el dron
            int distancia = Convert.ToInt32(pasoLbl.Text);
            dron_selected.Mover(direccion, distancia, bloquear: false);
        }


        private void enviarTelemetria(object sender, EventArgs e)
        {
            dron_selected.EnviarDatosTelemetria(ProcesarTelemetria);
        }

        private void detenerTelemetria_Click(object sender, EventArgs e)
        {
            dron_selected.DetenerDatosTelemetria();
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

        private void trackBarHeading_MouseUp(object sender, MouseEventArgs e)
        {
            // Cuando se libera la barra de desplazamiento recojo el valor
            // definitivo para el heading y lo envío al dron
            float valorSeleccionado = trackBarHeading.Value;
            dron_selected.CambiarHeading(valorSeleccionado, bloquear: false);
        }

        private void trackBarSpeed_MouseUp(object sender, MouseEventArgs e)
        {
            // Cuando se libera la barra de desplazamiento recojo el valor
            // definitivo para la velocidad y lo envío al dron
            int valorSeleccionado = trackBarSpeed.Value;
            dron_selected.CambiaVelocidad(valorSeleccionado);
        }

        private void parámetrosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Abro el formulario para leer/escribir los parámetros
            Parametros f = new Parametros(dron_selected);
            f.ShowDialog();
        }

        private void desplegable_SelectedIndexChanged(object sender, EventArgs e)
        {
            int texto = Convert.ToInt32(desplegable.Text);
            if (desplegable.Text == "")
            {
                MessageBox.Show("Porfavor, selecciona un dron que dirigir.");
            }
            else
            {
                dron_selected = drons_list[Convert.ToInt32(desplegable.Text)-1];
                if (dron_selected.GetFase() == 0) // en tierra
                {
                    EnTierra(null);
                }
                else if (dron_selected.GetFase() == 1) // despegando
                {
                    EnTierra();
                }
                else if (dron_selected.GetFase() == 2) // en vuelo
                {
                    EnAire("Volando");
                }
                else if (dron_selected.GetFase() == 3) // aterrizando
                {
                    Aterrizando();
                }
                else if (dron_selected.GetFase() == 4) // RTL
                {
                    en_RTL();
                }
            }

        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void pedidos_Click(object sender, EventArgs e)
        {
            Pedidos_forms pedidos_f = new Pedidos_forms();
            pedidos_f.Show();
        }
    }
}


