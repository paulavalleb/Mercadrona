using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MAVLink;
using System.Threading;
using System.Collections.Concurrent;
using System.Net.Sockets;

// MIRAR ESTO. SI LO QUITO NO RECONOCE LA CLASE WaitingRequest
using static csDronLink.MessageHandler;
using System.IO;
using System.Data.Entity.Core.Metadata.Edm;
using System.Runtime.InteropServices.ComTypes;



namespace csDronLink
{

    public class MessageHandler
    {
        // Esta es la clase para gestionar los mensajes que se reciben del autopiloto.
        // La idea es que haya un solo punto de recepción de los mensajes (en el método MessageLoop)
        // y de ahí se reenvía a los que haya solicitado ese tipo de mensaje.
        // Se consideran dos tipos de peticiones. Las síncronas son peticiones de un mensaje
        // concreto que el solicitante necesita en ese instante. Por tanto el solicitante quedará
        // bloqueado hasta que se reciba el mensaje esperado.
        // Las peticiones asíncronas no dejan bloqueado al solicitante. Simplemente se registra
        // el solicitante para que cada vez que llegue un mensaje del tipo indicado se le envíe.

        private MavlinkParse mavlink;
        string modo;
        SerialPort puertoSerie;
        NetworkStream puertoTCP;

        private bool running = true;
        Thread messageThread;
        private readonly object lockObj = new object();

        // Lista para las peticiones asíncronas
        private ConcurrentDictionary<string, List<Action<MAVLinkMessage>>> handlers = new ConcurrentDictionary<string, List<Action<MAVLinkMessage>>>();

        // Esta es la estructura que representa a una petición síncrona
        public class WaitingRequest
        {
            public string MsgType { get; }
            public Func<MAVLinkMessage, object, bool> Condition { get; }
            public object Params { get; }
            public BlockingCollection<MAVLinkMessage> Queue { get; }

            public WaitingRequest(string msgType, Func<MAVLinkMessage, object, bool> condition, object parameters, BlockingCollection<MAVLinkMessage> queue)
            {
                // Al registrar un petición síncrona guardamos:
                // el tipo de mensaje que se solicita
                MsgType = msgType;
                // la función que verificará si el mensaje cumple una cierta condición
                Condition = condition;
                // parámetros que puede necesitar la función que verifica la condición
                Params = parameters;
                // la cola en la que se quedará bloqueado el solicitante hasta que el manager
                // encole en ella el mensaje que cumpla las condiciones establecidas
                Queue = queue;

                /// LOS 3 CASOS TIPICOS:
                /// 1. Petición de mensaje que no requiere condición: petición del valor de un parametro
                /// 2. Petición con condición: petición de la altitud del dron, pero condicionado a que esa
                /// altitud sea inferior a 0.5 metros. Haremos eso cuando queramos saber si el dron acaba 
                /// de aterrizar
                /// 3. Petición con condición y parámetro: petición de la altitud pero condicionado a que 
                /// sea mayor que el valor recibido como parámetro. Usaremos eso cuando queramos saber si
                /// el dron ha alcanzado la altura de despegue establecida.
            }
        }

        // Lista para las peticiones síncronas
        private List<WaitingRequest> waitingThreads = new List<WaitingRequest>();

        private MAVLinkMessage LeerMensaje ()
        {
            if (modo == "producción")
                return mavlink.ReadPacket(puertoSerie.BaseStream);
            else
                return mavlink.ReadPacket(puertoTCP);
        }
        public MessageHandler(string modo, object canal)
        {
            this.modo = modo;
            if (modo == "producción")
                this.puertoSerie = (SerialPort) canal;
            else
                this.puertoTCP = (NetworkStream) canal;

            this.mavlink = new MavlinkParse();
            // Pongo en marcha el thread que estará continuamente recibiendo y repartiendo los 
            // mensajes
            this.messageThread = new Thread(MessageLoop) { IsBackground = true };
            this.messageThread.Start();
        }
        private void MessageLoop()
        {
            while (running)
            {
            
                var packet = LeerMensaje();
                if (packet == null) continue;
                // Obtengo el Id del mensaje
                string msgType = packet.msgid.ToString();
        

                lock (lockObj)
                {
                    // Procesar peticiones síncronas
                    foreach (var waiting in waitingThreads.ToList())
                    {
                        // Recorro la lista de peticiones para ver cuáles hay esperando 
                        // un mensaje de ese tipo
                    
                        if (waiting.MsgType == msgType && (waiting.Condition == null || waiting.Condition(packet, waiting.Params)))
                        {
                            // He comprobado que el mensaje es de ese tipo y si se había
                            // puesto una condición, he llamado a la función corresponiente 
                            // para verificar que se cumple la condición pasandole los parámetros 
                            // correspondientes
                            // Como se han satisfecho las condiciones pongo em mensaje en la cola
                            // para que le llege al solicitante, que quedará desbloqueado y podrá continuar
                     
                            waiting.Queue.Add(packet);
                            // Elimino la petición de la cola porque ya se ha satisfecho
                            waitingThreads.Remove(waiting);
                            break;
                        }
                    }
                }
                // Procesar handlers asíncronos
                // Buscar en la lista de handlers todos los del tipo de mensaje recibido
                // y obtener los callbacks asociados
                if (handlers.TryGetValue(msgType, out var callbacks))
                {
                    // Ahora llamo a cada uno de los callbacks pasandole el mensaje recibido
                    foreach (var callback in callbacks)
                    {
                        Task.Run(() => callback(packet));
                    }
                }
            }
        }

        public void RegisterHandler(string msgType, Action<MAVLinkMessage> callback)
        {   
            // Registro una petición asíncrona indicando el tipo de mensaje que interesa y el 
            // callback que hay que llamar (pasandole el mensaje) cada vez que se reciba un mensaje 
            // de ese tipo
            //LO CIERTO ES QUE NO ENTIENDO MUCHO LO QUE HACE LA FUNCIÓN. VER SI HAY UNA ALTERNATIVA
            // MAS CLARA
            handlers.AddOrUpdate(msgType, new List<Action<MAVLinkMessage>> { callback }, (key, list) =>
            {
                list.Add(callback);
                return list;
            });
        }

        public void UnregisterHandler(string msgType, Action<MAVLinkMessage> callback)
        {
            if (handlers.ContainsKey(msgType))
            {
                handlers[msgType].Remove(callback);
                if (!handlers[msgType].Any()) handlers.TryRemove(msgType, out _);
            }
        }

        public WaitingRequest WaitForMessageNoBlock(string msgType, Func<MAVLinkMessage, object, bool> condition = null, object parameters = null)
        {
            // Esta es una petición síncrona pero NO BLOQUEANTE
            // El solicitante tendrá que bloquearse después haciendo un WaitNow
            // preparo la cola en la que voy a hacer esperar al solicitante
            var msgQueue = new BlockingCollection<MAVLinkMessage>();
            // creo un objeto con el tipo de mensaja, la condición, los parámetros 
            // para la condición y a cola

            var waiting = new WaitingRequest(msgType, condition, parameters, msgQueue);
            // Añado a la lista de solicitudes
            lock (lockObj) waitingThreads.Add(waiting);
            // Retorno el objeto para que se pueda hacer el WaitNow
            return waiting;
        }
        public MAVLinkMessage WaitForMessageBlock(string msgType, Func<MAVLinkMessage, object, bool> condition = null, object parameters = null, int timeout = 3000)
        {
            // Esta es una petición síncrona BLOQUEANTE
            // preparo la cola en la que voy a hacer esperar al solicitante
            var msgQueue = new BlockingCollection<MAVLinkMessage>();
            // creo un objeto con el tipo de mensaja, la condición, los parámetros 
            // para la condición y a cola
            var waiting = new WaitingRequest(msgType, condition, parameters, msgQueue);
            // coloco el objeto en la cola de peticiones

            lock (lockObj) waitingThreads.Add(waiting);
            // hago que el solicitante espere
            return WaitNow(waiting, timeout);
        }

        public MAVLinkMessage WaitNow(WaitingRequest waiting, int timeout)
        {   
            // el timeout está en MILISEGUNDOS
            // Espero el mensaje
            if (waiting.Queue.TryTake(out var msg, timeout))
            {   
                lock (lockObj) waitingThreads.Remove(waiting);
                return msg;
            } else
            {
                // ha habido timeout
                lock (lockObj) waitingThreads.Remove(waiting);
                return null;
            }
           
        }

        public void Stop()
        {
            running = false;
            messageThread.Join();
        }

    }

    public class Dron
    {
        MAVLink.MavlinkParse mavlink = new MAVLink.MavlinkParse();
        string modo;
        // Para la comunicación en producción
        SerialPort puertoSerie = new SerialPort();
        // Para la comunicación en simulación
        NetworkStream puertoTCP;

        // Ultimo comando de navegación, que usará el bucle de navegación para recordarle
        // al dron hacia dónde debe navegar
        byte[] navPacket;
        Boolean enviandoTelemetria = false;

        // Los datos de telemetría que me interesan 
        float relative_alt;
        float lat;
        float lon;
        float heading;

        //Thread hilo;

        // Aquí guardaré la referencia a la función que tengo que ejecutar
        // si el cliente me pide que le envíe los datos de telemetría.
        Action<List<(string nombre, float valor)>> ProcesarTelemetria = null;

        Boolean navegando = false;

        // Velocidad para las operaciones de navegación
        int velocidad = 1;


        MessageHandler messageHandler;
        public Dron()
        {
        }
        private void EnviarMensaje (byte[] packet)
        {
            if (modo == "producción")
                puertoSerie.Write(packet, 0, packet.Length);
            else
                puertoTCP.Write(packet, 0, packet.Length);
        }
        private void RegistrarTelemetria (MAVLinkMessage msg)
        {   
            // Cada vez que se recibe un mensaje con datos de telemetria me los guardo
            // De momento solo me interesan la altitud, latitud, longitud y heading, pero se pueden
            // recoger muchos otros (nivel de bateria, etc.)
            MAVLink.mavlink_global_position_int_t position = (MAVLink.mavlink_global_position_int_t)msg.data;
            this.relative_alt = position.relative_alt / 1000.0f; // Convertir mm a metros
            this.lat = position.lat;
            this.lon = position.lon;
            this.heading = position.hdg;
            // Si me han pedido que envíe los datos de telemetría al cliente lo hago
            if (this.ProcesarTelemetria != null)
            {
                List<(string nombre, float valor)> telemetria = new List<(string nombre, float valor)>();
                telemetria.Add(("Alt",this.relative_alt));
                telemetria.Add(("Lat", this.lat));
                telemetria.Add(("Lon",this.lon));
                telemetria.Add(("Heading",this.heading));
                // Los envío a la función que me indicó el cliente
                this.ProcesarTelemetria(telemetria);
            }
        }
        public void Conectar(string modo, string conector = null)
        {
            this.modo = modo;
            if (modo == "producción")
            {
                // Configuro el puerto serie
                puertoSerie.PortName = conector;
                puertoSerie.BaudRate = 115200;
                puertoSerie.Open();
                // Pongo en marcha en message handler
                messageHandler = new MessageHandler(modo, puertoSerie);
            } else
            {
                string ip = "127.0.0.1";
                int port = 5763;
                TcpClient client = new TcpClient(ip, port);
                puertoTCP = client.GetStream();
                messageHandler = new MessageHandler(modo, puertoTCP);

            }


            // Cuando quiera leer del puerto (cosa que hará el MessageHandler) esperaré
            // dos segundos antes de volver a intentarlo
            //serialPort1.ReadTimeout = 2000;


            // Hago una petición asíncrona al handler para que me envíe todos los mensajes
            // del tipo indicado, que son los que contienen los datos de telemetria que me interesan
            // Le indico que cuando llegue un mensaje de ese tipo ejecute la función RegistrarTelemetria
            string msgType = ((int)MAVLink.MAVLINK_MSG_ID.GLOBAL_POSITION_INT).ToString();
            messageHandler.RegisterHandler(msgType, RegistrarTelemetria);
            
            // Ahora le pido al autopiloto que me envíe mensajes del tipo indicado (los que contienen
            // los datos de telemetría, cada 2 segundos
            MAVLink.mavlink_command_long_t req = new MAVLink.mavlink_command_long_t
            {
                target_system = 1,
                target_component = 1,
                command = (ushort)MAVLink.MAV_CMD.SET_MESSAGE_INTERVAL,
                param1 = (float)MAVLink.MAVLINK_MSG_ID.GLOBAL_POSITION_INT, // ID del mensaje que queremos recibir
                param2 = 200000, // Intervalo en microsegundos (1 Hz = 1,000,000 µs)
            };

            byte[] packet = mavlink.GenerateMAVLinkPacket10(MAVLink.MAVLINK_MSG_ID.COMMAND_LONG, req);
            EnviarMensaje(packet);
        }
        public void EnviarDatosTelemetria(Action<List<(string nombre, float valor)>> f)
        {   
            // El cliente me pide que ejecute la función f cada vez que reciba un mensaje
            // con datos de telemetría
            this.ProcesarTelemetria = f;

        }
        public void DetenerDatosTelemetria()
        {
            // El cliente ya no quiere datos de telemetría
            this.ProcesarTelemetria = null;
        }
     
        public bool ComprobarEnAire(MAVLink.MAVLinkMessage msg, object targetAlt)
        {
            // es la función que comprobará que la altitud del dron es la indicada como 
            // parámetro. Se usará para ver si el dron ha alcanzado la altura indicada en el despegue
            // Recupero la altitud a partir del mensaje recibido
            var position = (MAVLink.mavlink_global_position_int_t)msg.data;
            float altitud = position.relative_alt / 1000.0f;
            // retorno el resultado de realizar la comprobacion (con un margen del 10%)
            return altitud > (int)targetAlt * 0.90;
        }
        
        public void PonModoGuiado ()
        {
            // Primero ponemos el dron en modo GUIDED
            MAVLink.mavlink_set_mode_t setMode = new MAVLink.mavlink_set_mode_t
            {
                target_system = 1,
                base_mode = (byte)MAVLink.MAV_MODE_FLAG.CUSTOM_MODE_ENABLED,
                custom_mode = 4 // GUIDED Mode en ArduPilot
            };

            byte[] packet = mavlink.GenerateMAVLinkPacket10(MAVLink.MAVLINK_MSG_ID.SET_MODE, setMode);
            //serialPort1.Write(packet, 0, packet.Length);
            EnviarMensaje(packet);
        }
        
        private void _Despegar(int altitud, Action<object> f = null, object param = null)
        {
            // Primero ponemos el dron en modo GUIDED
            PonModoGuiado();

            // Despues armamos armamos
            MAVLink.mavlink_command_long_t req = new MAVLink.mavlink_command_long_t();
            req.target_system = 1;
            req.target_component = 1;
            req.command = (ushort)MAVLink.MAV_CMD.COMPONENT_ARM_DISARM;
            req.param1 = 1;
            byte [] packet = mavlink.GenerateMAVLinkPacket10(MAVLink.MAVLINK_MSG_ID.COMMAND_LONG, req);
            //serialPort1.Write(packet, 0, packet.Length);
            EnviarMensaje(packet);

            // Y ahora despegamos



            req = new MAVLink.mavlink_command_long_t
            {
                target_system = 1,
                target_component = 1,
                command = (ushort)MAVLink.MAV_CMD.TAKEOFF,
                param7 = altitud // Altura deseada en metros
            };

            // Generar paquete MAVLink
            packet = mavlink.GenerateMAVLinkPacket10(MAVLink.MAVLINK_MSG_ID.COMMAND_LONG, req);
            // Enviar comando al dron
            //serialPort1.Write(packet, 0, packet.Length);
            EnviarMensaje(packet);

            // Aqui espero el mensaje que indique que ya ha alcanzado la altura de despeque
            string msgType = ((int)MAVLink.MAVLINK_MSG_ID.GLOBAL_POSITION_INT).ToString();
            MAVLink.MAVLinkMessage message = messageHandler.WaitForMessageBlock(
                msgType, // GLOBAL_POSITION_INT
                condition: ComprobarEnAire,
                parameters: altitud,
                timeout: 20000000    // Esperemos 20 segundos
            );
         

            if (message != null)
                // He obtenido el mensaje que esperaba (se ha alcanzado la altura de despegue)
                // Ejecuto el callback si es que había, pasandole los parámetros (si es que hay)
                if (f != null)
                    f(param);
            else
                // ha habido un timeout
                //HABRIA QUE RETORNAR ALGO PARA QUE SEA EL CLIENTE QUIEN INFORME AL USUARIO
                Console.WriteLine(" Time out en el despegue");
        }
        public void Despegar(int altitud, Boolean bloquear = true, Action<object> f = null, object param = null)
        {
            // Si la llamada es bloqueante llamo a la función 
            if (bloquear)
            {
                this._Despegar(altitud);
            }
            // Si no es bloqueante pongo en marcha un thread para que se ocupe y no bloqueo al cliente
            else
            {
                Thread t = new Thread(() => _Despegar(altitud, f, param));
                t.Start();
            }

        }
        public List<float> LeerParametros(List<string> parametros)
        {
            // En la lista resultante dejaré los valores de los parámetros en el mismo
            // orden en el que aparecen en la lista de parámetros

            List<float> resultado = new List<float>();

            // Crear un vector de bytes con un tamaño fijo para el nombre del parámetro
            byte[] paramIdBytes = new byte[16];

            foreach (string param in parametros)
            {
                // Primero hago una petición síncrona pero no bloqueante. 
                // Lo hago así porque si primero pido y luego hago la petición
                // pudiera pasar que el mensaje llegase antes de que la petición se haya 
                // registrado. Así que primero registro, luego pido y luego espero
                string msgType = ((int)MAVLink.MAVLINK_MSG_ID.PARAM_VALUE).ToString();
                WaitingRequest waiting = messageHandler.WaitForMessageNoBlock(msgType);

                // Convertir el nombre del parámetro 
                Array.Copy(Encoding.ASCII.GetBytes(param), paramIdBytes, param.Length);

                // Crear la solicitud para obtener el valor del parámetro RTL_ALT
                MAVLink.mavlink_param_request_read_t req = new MAVLink.mavlink_param_request_read_t
                {
                    target_system = 1,  // ID del sistema (1 es el sistema principal)
                    target_component = 1,  // ID del componente (1 es el autopiloto)
                    param_index = -1,  // Índice del parámetro (no se usa para leer por nombre)
                    param_id = paramIdBytes,  // El parámetro debe ser un arreglo de bytes de longitud fija
                };

                // Generar el paquete MAVLink para solicitar el parámetro
                byte[] packet = mavlink.GenerateMAVLinkPacket10(MAVLink.MAVLINK_MSG_ID.PARAM_REQUEST_READ, req);
                //serialPort1.Write(packet, 0, packet.Length);
                EnviarMensaje(packet);

                // ahora espero la respuesta
                var response = messageHandler.WaitNow(waiting, timeout: 5000);

                if (response != null)
                {
                    // aqui tengo la respuesta
                    var paramResponse = (MAVLink.mavlink_param_value_t)response.data;
                    string res = Encoding.ASCII.GetString(paramResponse.param_id).Trim();
                    // Verificamos si el parámetro es el correcto (NO ESTOY SEGURO DE QUE HAGA FALTA)
                    res = res.Split('\0')[0];
                    if (res == param)
                        resultado.Add(paramResponse.param_value);
                }

            }
            return resultado;
        }
        public void EscribirParametros(List<(string parametro, float valor)> parametros)
        {
            byte[] paramIdBytes = new byte[16];

            for (int i=0; i< parametros.Count; i++ )
            {
                // datos del siguiente parámetro a escribir
                string parametro = parametros[i].parametro;
                float valor = parametros[i].valor;

                // Convertir el nombre del parámetro en un vector de bytes
                Array.Copy(Encoding.ASCII.GetBytes(parametro), paramIdBytes, parametro.Length);

                // Crear la solicitud para establecer el valor del parámetro 
                MAVLink.mavlink_param_set_t peticion = new MAVLink.mavlink_param_set_t
                {
                    target_system = 1,      // ID del sistema (1 es el sistema principal)
                    target_component = 1,   // ID del componente (1 es el autopiloto)
                    param_value = valor,  // Nuevo valor del parámetro 
                    param_id = paramIdBytes,    // Nombre del parámetro en un array de bytes
                    param_type = (byte)MAVLink.MAV_PARAM_TYPE.REAL32  // Tipo de dato (float de 32 bits)
                };

                // Generar el paquete MAVLink para enviar el nuevo valor del parámetro
                byte[] paquete = mavlink.GenerateMAVLinkPacket10(MAVLink.MAVLINK_MSG_ID.PARAM_SET, peticion);

                // Enviar la solicitud de actualización del parámetro al dron
                //serialPort1.Write(paquete, 0, paquete.Length);
                EnviarMensaje(paquete);

            }
        }
        private void _BucleNavegacion ( )
        {
            // En este bucle repetimos cada segundo el ultimo comando de navegación
            // porque el dron solo navega pocos segundos si no se le insiste periódicamente.
            this.navegando = true;
            while (this.navegando)
            {
                // vuelvo a enviar el último comando de navegación
                //serialPort1.Write(this.navPacket, 0, this.navPacket.Length);
                EnviarMensaje(this.navPacket);
                Thread.Sleep(1000);
            } 
        }
        public void _Navegar(string direccion, Action<object> f = null, object param = null)
        {
            int vx = 0, vy = 0, vz = 0;
            int velocidad = this.velocidad;

            if (direccion == "NorthWest")
            {
                vx = velocidad;
                vy = -velocidad;
            }

            if (direccion == "North")
            {
                vx = velocidad;
                vy = 0;
            }

            if (direccion == "NorthEast")
            {
                vx = velocidad;
                vy = velocidad;
            }

            if (direccion == "West")
            {
                vx = 0;
                vy = -velocidad;
            }

            if (direccion == "Stop")
            {
                vx = 0;
                vy = 0;
            }

            if (direccion == "East")
            {
                vx = 0;
                vy = velocidad;
            }
            if (direccion == "SouthWest")
            {
                vx = -velocidad;
                vy = -velocidad;
            }

            if (direccion == "South")
            {
                vx = -velocidad;
                vy = 0;
            }

            if (direccion == "SouthEast")
            {
                vx = -velocidad;
                vy = velocidad;
            }
            if (direccion == "Up")
            {
                vz = -velocidad;
            }
            if (direccion == "Down")
            {
                vz = velocidad;
            }

            // Crear el mensaje SET_POSITION_TARGET_LOCAL_NED para navegar en la dirección indicada
            MAVLink.mavlink_set_position_target_local_ned_t moveCmd = new MAVLink.mavlink_set_position_target_local_ned_t
            {
                target_system = 1,        // ID del sistema (autopiloto)
                target_component = 1,     // ID del componente (controlador de vuelo)
                coordinate_frame = (byte)MAVLink.MAV_FRAME.LOCAL_NED,  // Sistema de coordenadas local NED
                type_mask = 0b_0000111111000111, // Ignorar posición, usar velocidad
                x = 0,                    // No especificamos una posición en X
                y = 0,                    // No especificamos una posición en Y
                z = 0,                  // Mantener altura a 10 metros (-10 en NED)
                vx = vx,               // Velocidad hacia el norte (metros/segundo)
                vy = vy,                   // No moverse en la dirección este/oeste
                vz = vz,                   // Mantener la altura
                afx = 0,                  // Sin aceleración específica
                afy = 0,                  // Sin aceleración específica
                afz = 0,                  // Sin aceleración específica
                yaw = float.NaN,          // No cambiar la orientación
                yaw_rate = 0              // No rotar
            };

            // Generar el paquete MAVLink
            this.navPacket = mavlink.GenerateMAVLinkPacket10(MAVLink.MAVLINK_MSG_ID.SET_POSITION_TARGET_LOCAL_NED, moveCmd);
            //serialPort1.Write(this.navPacket, 0, this.navPacket.Length);
            EnviarMensaje(this.navPacket);
            // Pongo en marcha el bucle de navegación si es necesario
            if (!this.navegando)
            {
                Thread t = new Thread(() => _BucleNavegacion());
                t.Start();
            }
            
            if (f != null)
                f(param);

        }
        public void Navegar(string direccion, Boolean bloquear = true, Action<object> f = null, object param = null)
        {
            if (bloquear)
            {
                this._Navegar(direccion);
            }
            else
            {
                Thread t = new Thread(() => _Navegar(direccion));
                t.Start();
            }
        }

        public bool ComprobarParado (MAVLink.MAVLinkMessage msg, object param = null)
        {
            // verifica si el mensaje indica que la velocidad del dron es cero
            // Servirá para detectar que el dron ha llegado al destino
            var position = (MAVLink.mavlink_global_position_int_t)msg.data;
            float vx = position.vx;
            float vy = position.vy;
            float vz = position.vz;
            double velocidad =  Math.Sqrt (vx*vx + vy*vy + vz*vz);
            return velocidad/100 < 0.1;
        }
  

        public void _Mover(string direccion, int distancia, Action<object> f = null, object param = null)
        {
            // paro el bucle de navegación si es necesario
            // Las operaciones de movimiento indican la distancia. No es necesario recordarle al autopiloto
            // el comando periódicamente

            // ATENCIÓN: EN LOS MOVIMIENTOS EN DIAGONAL HABRIA QUE RECTIFICAR LA DISTANCIA PORQUE SI NO 
            // LA DISTANCIA RECORRIDA ES MAYOR QUE LA INDICADA, DE ACUERDO CON EL TEOREMA DE PITAGORAS

            if (this.navegando)
                this.navegando = false;

            int dx = 0, dy = 0, dz = 0;

            if (direccion == "ForwardLeft")
            {
                dx = distancia;
                dy = -distancia;
            }

            if (direccion == "Forward")
            {
                dx = distancia;
                dy = 0;
            }

            if (direccion == "ForwardRight")
            {
                dx = distancia;
                dy = distancia;
            }

            if (direccion == "Left")
            {
                dx = 0;
                dy = -distancia;
            }

            if (direccion == "Stop")
            {
                dx = 0;
                dy = 0;
            }

            if (direccion == "Right")
            {
                dx = 0;
                dy = distancia;
            }
            if (direccion == "BackLeft")
            {
                dx = -distancia;
                dy = -distancia;
            }

            if (direccion == "Back")
            {
                dx = -distancia;
                dy = 0;
            }

            if (direccion == "BackRight")
            {
                dx = -distancia;
                dy = distancia;
            }
            if (direccion == "Up")
            {
                dz = -distancia;
            }
            if (direccion == "Down")
            {
                dz = distancia;
            }

            // Crear el mensaje de movimiento en coordenadas NED (Norte-Este-Abajo)
            MAVLink.mavlink_set_position_target_local_ned_t moveCmd = new MAVLink.mavlink_set_position_target_local_ned_t
            {
                target_system = 1,
                target_component = 1,
                coordinate_frame = (byte)MAVLink.MAV_FRAME.BODY_OFFSET_NED, // Sistema de coordenadas local NED
                type_mask = 0b_0000110111111000, 
                x = dx, 
                y = dy, 
                z = dz, 
                vx = 0, 
                vy = 0,
                vz = 0,
                afy = 0,
                afz = 0,
                yaw =0,
                yaw_rate = 0
            };

            // Generar paquete MAVLink
            byte[] packet = mavlink.GenerateMAVLinkPacket10(MAVLink.MAVLINK_MSG_ID.SET_POSITION_TARGET_LOCAL_NED, moveCmd);

            // Enviar comando al dron
            //serialPort1.Write(packet, 0, packet.Length);
            EnviarMensaje(packet);
            // Espero 2 segundos a que el dron coja velocidad
            // Porque inmediatamente esperaré a que la velocidad vuelva a ser 0
            Thread.Sleep(2000);
            

            // Aqui espero el mensaje que indique que ya ha llegado al destino 
            // Lo sabre porque la velocidad será cero
            string msgType = ((int)MAVLink.MAVLINK_MSG_ID.GLOBAL_POSITION_INT).ToString();
            MAVLink.MAVLinkMessage message = messageHandler.WaitForMessageBlock(
                msgType, // GLOBAL_POSITION_INT
                condition: ComprobarParado,
                timeout: 20000000    // Esperemos 20 segundos
            );

            if (f != null)
                f(param);
            

        }
        public void Mover(string direccion, int distancia, Boolean bloquear= true, Action<object> f = null,  object param = null)
        {
            if (bloquear)
            {
                this._Mover(direccion, distancia);
            }
            else
            {
                Thread t = new Thread(() => _Mover(direccion, distancia, f, param));
                t.Start();
            }
        }
        public bool ComprobarEnTierra(MAVLink.MAVLinkMessage msg, object param = null)
        {
            // es la función que comprobará que la altitud del dron es menor de 50 cm. Se usará para 
            // detectar el fin de la operación de aterrizaje o RTL
            var position = (MAVLink.mavlink_global_position_int_t)msg.data;
            float altitud = position.relative_alt / 1000.0f;
            return altitud <  0.50;
        }
        public void _RTL(Action<object> f = null, object param = null)
        {
            MAVLink.mavlink_set_mode_t setMode = new MAVLink.mavlink_set_mode_t
            {
                target_system = 1,
                base_mode = (byte)MAVLink.MAV_MODE_FLAG.CUSTOM_MODE_ENABLED,
                custom_mode = 6 // Modo RTL en ArduPilot
            };

            byte[] packet = mavlink.GenerateMAVLinkPacket10(MAVLink.MAVLINK_MSG_ID.SET_MODE, setMode);
            // Enviar comando al dron
            //serialPort1.Write(packet, 0, packet.Length);
            EnviarMensaje(packet);

            // Pedimos un mensaje de telemetria que indique que ya está en tierra
            string msgType = ((int)MAVLink.MAVLINK_MSG_ID.GLOBAL_POSITION_INT).ToString();
            var mensaje = messageHandler.WaitForMessageBlock(
                msgType, // GLOBAL_POSITION_INT
                condition: ComprobarEnTierra,
                timeout: 20000000
            );
            if (mensaje != null)
                // He obtenido el mensaje que esperaba. El dron está en tierra
                // Ejecuto el callback si es que había
                if (f != null)
                    f(param);
                else
                    // ha habido un timeout
                    Console.WriteLine(" Time out en el RTL");
        }
        public void _Aterrizar(Action<object> f = null, object param = null)
        {

            // Crear el paquete para el comando de aterrizaje
            MAVLink.mavlink_command_long_t req = new MAVLink.mavlink_command_long_t
            {
                target_system = 1,  // ID del sistema (1 es el sistema principal)
                target_component = 1,  // ID del componente (1 es el autopiloto)
                command = (ushort)MAVLink.MAV_CMD.LAND,  // Comando de aterrizaje
                param1 = 0,  // No se usa (0 es valor por defecto)
                param2 = 0,  // No se usa (0 es valor por defecto)
                param3 = 0,  // No se usa (0 es valor por defecto)
                param4 = 0,  // No se usa (0 es valor por defecto)
                param5 = 0,  // No se usa (0 es valor por defecto)
                param6 = 0,  // No se usa (0 es valor por defecto)
                param7 = 0   // No se usa (0 es valor por defecto)
            };

            // Generar el paquete MAVLink para el comando de aterrizaje
            byte[] packet = mavlink.GenerateMAVLinkPacket10(MAVLink.MAVLINK_MSG_ID.COMMAND_LONG, req);
            // Enviar comando al dron
            //serialPort1.Write(packet, 0, packet.Length);
            EnviarMensaje(packet);

            string msgType = ((int)MAVLink.MAVLINK_MSG_ID.GLOBAL_POSITION_INT).ToString();
            var mensaje = messageHandler.WaitForMessageBlock(
                msgType, // GLOBAL_POSITION_INT
                condition: ComprobarEnTierra,
                 timeout: 2000000
            );
         
            // Ahora es cuando me bloqueo a la espera del mensaje 
            if (mensaje != null)
                // He obtenido el mensaje que esperaba. El dron está en tierra
                // Ejecuto el callback si es que había
                if (f != null)
                    f(param);
                else
                    // ha habido un timeout
                    Console.WriteLine(" Time out en el aterrizaje");
        }
        public void RTL(Boolean bloquear = true, Action<object> f = null, object param = null)
        {
            if (bloquear)
            {
                this._RTL();
            }
            else
            {
                Thread t = new Thread(() => _RTL(f, param));
                t.Start();
            }
        }
        public void Aterrizar( Boolean bloquear = true, Action<object> f = null, object param = null)
        {
            if (bloquear)
            {
                this._Aterrizar();
            }
            else
            {
                Thread t = new Thread(() => _Aterrizar(f, param));
                t.Start();
            }
        }
        public void CambiaVelocidad (int velocidad)
        {
            this.velocidad = velocidad;
            // Crear el mensaje COMMAND_LONG
            MAVLink.mavlink_command_long_t speedCommand = new MAVLink.mavlink_command_long_t();
            speedCommand.target_system = 1;      // ID del sistema (drone)
            speedCommand.target_component = 1; // ID del componente
            speedCommand.command = (ushort)MAVLink.MAV_CMD.DO_CHANGE_SPEED;
            speedCommand.param1 = 1;   // Tipo de velocidad (1 = Aérea, 0 = Terrestre, 2 = Vertical)
            speedCommand.param2 = velocidad;   // Velocidad en m/s (Aquí ponemos 7 m/s)
            speedCommand.param3 = -1;  // Sin aceleración específica (-1 usa la por defecto)
            speedCommand.param4 = 0;   // No se usa
            speedCommand.param5 = 0;   // No se usa
            speedCommand.param6 = 0;   // No se usa
            speedCommand.param7 = 0;   // No se usa

            // Generar el paquete MAVLink
            byte[] packet = mavlink.GenerateMAVLinkPacket10(MAVLink.MAVLINK_MSG_ID.COMMAND_LONG, speedCommand);

            // Enviar el paquete al dron
            //serialPort1.Write(packet, 0, packet.Length);
            EnviarMensaje(packet);

        }


        private bool ComprobarOrientacion (MAVLink.MAVLinkMessage msg, object grados)
        {
            // es la función que comprobará que la orientación es la indicada
            // Se usa para determiar cuándo ha acabado la operación de cambio de heading
            var position = (MAVLink.mavlink_global_position_int_t)msg.data;
            float heading = position.hdg / 100.0f;
            return Math.Abs (heading - (float) grados) < 5;
        }
   
        public void CambiarHeading(float nuevoHeading, float velocidad = 30, bool horario = true)
        {
            byte direction = (byte)(horario ? 1 : -1); // 1 = horario, -1 = antihorario
            byte relative = 0; // 0 = absoluto, 1 = relativo al heading actual

            var req = new MAVLink.mavlink_command_long_t
            {
                target_system = 1,       // ID del dron
                target_component = 1, // ID del componente (normalmente autopiloto)
                command = (ushort)MAVLink.MAV_CMD.CONDITION_YAW,
                param1 = nuevoHeading,  // Ángulo de heading (en grados)
                param2 = velocidad,     // Velocidad de giro (grados por segundo)
                param3 = direction,             // No usado
                param4 = relative,     // Dirección de giro (1 = horario, -1 = antihorario)
                param5 = 0,             // No usado
                param6 = 0,             // No usado
                param7 = 0       // 0 = Absoluto, 1 = Relativo
            };
            // Generar el paquete MAVLink
            byte[] packet = mavlink.GenerateMAVLinkPacket10(MAVLink.MAVLINK_MSG_ID.COMMAND_LONG, req);

            // Enviar el paquete al dron
            //serialPort1.Write(packet, 0, packet.Length);
            EnviarMensaje(packet);

            // Espero a que se complete la operación
            string msgType = ((int)MAVLink.MAVLINK_MSG_ID.GLOBAL_POSITION_INT).ToString();
            var mensaje = messageHandler.WaitForMessageBlock(
                msgType, // GLOBAL_POSITION_INT
                condition: ComprobarOrientacion,
                parameters: nuevoHeading, 
                timeout: 2000000
            );
        }
        private void _IrAlPunto(float lat, float lon, float alt, Action<object> f = null, object param = null)
        {
            var msg = new MAVLink.mavlink_set_position_target_global_int_t
            {
                target_system = 1, // ID del dron
                target_component = 1, // Componente (generalmente 1)
                coordinate_frame = (byte)MAV_FRAME.GLOBAL_RELATIVE_ALT_INT,
                type_mask = (ushort)(
               POSITION_TARGET_TYPEMASK.VX_IGNORE |
               POSITION_TARGET_TYPEMASK.VY_IGNORE |
               POSITION_TARGET_TYPEMASK.VZ_IGNORE |
               POSITION_TARGET_TYPEMASK.AX_IGNORE |
               POSITION_TARGET_TYPEMASK.AY_IGNORE |
               POSITION_TARGET_TYPEMASK.AZ_IGNORE |
               POSITION_TARGET_TYPEMASK.YAW_IGNORE |
               POSITION_TARGET_TYPEMASK.YAW_RATE_IGNORE
           ), // Solo usamos lat, lon y alt
                lat_int = (int)(lat * 1e7), // Convertimos a formato entero
                lon_int = (int)(lon * 1e7),
                alt = alt,
                yaw = 0, // Mantener el rumbo actual
                yaw_rate = 0
            };

            // Generar el paquete MAVLink
            byte[] packet = mavlink.GenerateMAVLinkPacket10(MAVLINK_MSG_ID.SET_POSITION_TARGET_GLOBAL_INT, msg);

            // Enviar el paquete al dron
            EnviarMensaje(packet);

            // espero 2 segundos a que el dron empiece a moverse
            Thread.Sleep(2000);

            // Aqui espero el mensaje que indique que ya ha llegado al destino 
            // Lo sabre porque la velocidad será cero
            string msgType = ((int)MAVLink.MAVLINK_MSG_ID.GLOBAL_POSITION_INT).ToString();
            MAVLink.MAVLinkMessage message = messageHandler.WaitForMessageBlock(
                msgType, // GLOBAL_POSITION_INT
                condition: ComprobarParado,
                timeout: 20000000    // Esperemos 20 segundos
            );

            if (f != null)
                f(param);
        }
        public void IrAlPunto(float lat, float lon, float alt, Boolean bloquear = true, Action<object> f = null, object param = null)
        {
            if (bloquear)
            {
                this._IrAlPunto (lat, lon, alt);
            }
            else
            {
                Thread t = new Thread(() => _IrAlPunto(lat, lon, alt, f, param));
                t.Start();
            }
        }
        public void EstableceEscenario(List<List<(float lat, float lon)>> scenario)
        {
            /* Un escenario es una lista de fences. El primero siempre es el de inclusión que define
             * los límites del area de vuelo. Los restantes son fences de exclusión, que representan los obstaculos
             * del área de vuelo.
             * Cada fence es una lista de PointLatLng. 
             * Si la lista tiene 3 PointLatLng o más es un fence de tipo polígono. Si tiene solo dos entonces
             * es un fence de tipo círculo. En ese caso, el primer PointLatLng es el centro del círculo y la latitud 
             * del segundo PointLatLng es en realidad el radio del círculo.
             * El primer fence (el de inclusión) debe ser de tipo polígono
             * */

            // Tomo el fence de inclusión
            List<(float lat, float lon)> waypoints = scenario[0];
            // En esta lista prepararé los comandos para fijar los waypoints
            List<MAVLink.mavlink_mission_item_int_t> wploader = new List<MAVLink.mavlink_mission_item_int_t>();
            int seq = 0;

            // preparo los comandos 
            foreach (var wp in waypoints)
            {
                    wploader.Add(new MAVLink.mavlink_mission_item_int_t()
                    {
                        target_system = 1,
                        target_component = 1,
                        seq = (ushort)seq,
                        frame = (byte)MAVLink.MAV_FRAME.GLOBAL,
                        command = (ushort)MAVLink.MAV_CMD.FENCE_POLYGON_VERTEX_INCLUSION,
                        param1 = waypoints.Count,
                        x = (int)(wp.lat * 1e7),
                        y = (int)(wp.lon * 1e7),
                        mission_type = (byte)MAVLink.MAV_MISSION_TYPE.FENCE
                    });
                    seq++;
             }
            
          
            // ahora preparamos los obstáculos
            for (int i = 1; i < scenario.Count; i++)
            {
                waypoints = scenario[i];
                if (waypoints.Count == 2)
                // es un circulo
                {
                    wploader.Add(new MAVLink.mavlink_mission_item_int_t()
                    {
                        target_system = 1,
                        target_component = 1,
                        seq = (ushort)seq,
                        frame = (byte)MAVLink.MAV_FRAME.GLOBAL,
                        command = (ushort)MAVLink.MAV_CMD.FENCE_CIRCLE_EXCLUSION,
                        param1 = Convert.ToSingle(waypoints[1].lat), // en realidad es el radio
                        x = (int)(waypoints[0].lat * 1e7),
                        y = (int)(waypoints[0].lon * 1e7),
                        mission_type = (byte)MAVLink.MAV_MISSION_TYPE.FENCE
                    });
                    seq++;
                }
                else
                {
                    // es un polígono

                    foreach (var wp in waypoints)
                    {
                        wploader.Add(new MAVLink.mavlink_mission_item_int_t()
                        {
                            target_system = 1,
                            target_component = 1,
                            seq = (ushort)seq,
                            frame = (byte)MAVLink.MAV_FRAME.GLOBAL,
                            command = (ushort)MAVLink.MAV_CMD.FENCE_POLYGON_VERTEX_EXCLUSION,
                            param1 = waypoints.Count,
                            x = (int)(wp.lat * 1e7),
                            y = (int)(wp.lon * 1e7),
                            mission_type = (byte)MAVLink.MAV_MISSION_TYPE.FENCE
                        });
                        seq++;
                    }
                }
            }
            // Envío el número de waypoints
            var msg = new MAVLink.mavlink_mission_count_t
            {
                target_system = 1,
                target_component = 1,
                count = (ushort)wploader.Count,
                mission_type = (byte)MAVLink.MAV_MISSION_TYPE.FENCE
            };

            byte[] packet = mavlink.GenerateMAVLinkPacket10(MAVLink.MAVLINK_MSG_ID.MISSION_COUNT, msg);
            //serialPort1.Write(packet, 0, packet.Length);
            EnviarMensaje(packet);


            // Ahora espero a que el autopiloto me vaya pidiendo los waypoints uno a uno
            string msgType;
            while (true)
            {
                msgType = ((int)MAVLink.MAVLINK_MSG_ID.MISSION_REQUEST).ToString();
                MAVLink.MAVLinkMessage request = messageHandler.WaitForMessageBlock(
                    msgType,
                    timeout: 10000
                );
                // El mensaje contiene el número de waypoint que está pidiendo el autopiloto
                int next = ((MAVLink.mavlink_mission_request_t)request.data).seq;
                // envío el comando que me pide
                MAVLink.mavlink_mission_item_int_t msg2 = wploader[next];
                packet = mavlink.GenerateMAVLinkPacket10(MAVLink.MAVLINK_MSG_ID.MISSION_ITEM_INT, msg2);
                //serialPort1.Write(packet, 0, packet.Length);
                EnviarMensaje(packet );
                if (next == wploader.Count - 1) break; // Ya los he enviado todos
            }
            // Espero a que me confirme que ha recibido todo bien
            msgType = ((int)MAVLink.MAVLINK_MSG_ID.MISSION_ACK).ToString();
            MAVLink.MAVLinkMessage response = messageHandler.WaitForMessageBlock(
                msgType,
                timeout: 10000
            );

        }

        public void CargarMision(List<(float lat, float lon)> mision)
        {

            // Por alguna razón que no controlo tengo que enviar dos veces el primer waypoint de la misión
            (float lat, float lon) primero = mision[0];
            mision.Insert(0, primero);

            // Lista con los comandos
            List<MAVLink.mavlink_mission_item_int_t> wploader = new List<MAVLink.mavlink_mission_item_int_t>();
            int seq = 0;
            foreach (var wp in mision)
            {
                wploader.Add(new MAVLink.mavlink_mission_item_int_t()
                {
                    target_system = 1,
                    target_component = 1,
                    seq = (ushort) seq,
                    frame = (byte)MAVLink.MAV_FRAME.GLOBAL_RELATIVE_ALT,
                    command = (ushort)MAVLink.MAV_CMD.WAYPOINT,
                    current = (byte)(seq == 0 ? 1 : 0), // El primer waypoint es el actual
                    autocontinue = 1,
                    //param1 = mision.Count,
                    x = (int)(wp.lat * 1e7),  // Latitud en formato entero
                    y = (int)(wp.lon * 1e7),  // Longitud en formato entero
                    z = 20,               // Altitud en metros. VER COMO HACER PARA QUE ESTO PUEDA CAMBIARSE
                    mission_type = (byte)MAVLink.MAV_MISSION_TYPE.MISSION
                });
                seq++;
            }
            // Envio el número total de waypoints
            var countMsg = new MAVLink.mavlink_mission_count_t
            {
                target_system = 1,
                target_component = 1,
                count = (ushort)mision.Count,
                mission_type = (byte)MAVLink.MAV_MISSION_TYPE.MISSION
            };

            byte[] packet = mavlink.GenerateMAVLinkPacket10(MAVLink.MAVLINK_MSG_ID.MISSION_COUNT, countMsg);
            //serialPort1.Write(packet, 0, packet.Length);
            EnviarMensaje(packet);
            string msgType;
            // Ahora espero que el autopiloto me vaya pidiendo los waypoints uno a uno
            while (true)
            {
                msgType = ((int)MAVLink.MAVLINK_MSG_ID.MISSION_REQUEST).ToString();
                MAVLink.MAVLinkMessage request = messageHandler.WaitForMessageBlock(
                    msgType,
                    timeout: 10000
                );
                int next = ((MAVLink.mavlink_mission_request_t)request.data).seq;
                MAVLink.mavlink_mission_item_int_t msg1 = wploader[next];
                packet = mavlink.GenerateMAVLinkPacket10(MAVLink.MAVLINK_MSG_ID.MISSION_ITEM_INT, msg1);
                //serialPort1.Write(packet, 0, packet.Length);
                EnviarMensaje(packet);
                if (next == wploader.Count - 1) break; // Ya estan todos
            }

            // Espero la confirmación final
            msgType = ((int)MAVLink.MAVLINK_MSG_ID.MISSION_ACK).ToString();
            MAVLink.MAVLinkMessage response2 = messageHandler.WaitForMessageBlock(
                msgType,
                timeout: 10000
            );

          
        }
        private void _EjecutarMision( Action<object> f = null, object param = null)
        {
           
            MAVLink.mavlink_command_long_t cmd = new MAVLink.mavlink_command_long_t
            {
                target_system = 1,
                target_component = 1,
                command = (ushort)MAVLink.MAV_CMD.MISSION_START,
                confirmation = 0,
                param1 = 0,  // Primera misión (waypoint 0)
                param2 = 0,  // Modo de transición (no se usa aquí)
                param3 = 0,
                param4 = 0,
                param5 = 0,
                param6 = 0,
                param7 = 0
            };

            // Enviar el comando al autopiloto
            byte[] packet = mavlink.GenerateMAVLinkPacket10(MAVLink.MAVLINK_MSG_ID.COMMAND_LONG, cmd);
            //serialPort1.Write(packet, 0, packet.Length);
            EnviarMensaje(packet);
            // Ahora espero a que acabe la misión, cuando a velocidad del 
            // dron sea 0. Pero primero espero 5 segundos a que empiece a moverse
            Thread.Sleep(5000);
            string msgType = ((int)MAVLink.MAVLINK_MSG_ID.GLOBAL_POSITION_INT).ToString();
            var mensaje = messageHandler.WaitForMessageBlock(
                msgType, // GLOBAL_POSITION_INT
                condition: ComprobarParado,
                 timeout: -1 // tiempo indefinido
            );
            if (f != null)
                f(param);
        }

        public void EjecutarMision(Boolean bloquear = true, Action<object> f = null, object param = null)
        {
            if (bloquear)
            {
                this._EjecutarMision();
            }
            else
            {
                Thread t = new Thread(() => _EjecutarMision(f, param));
                t.Start();
            }
        }

    }
}
