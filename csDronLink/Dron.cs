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
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Data;
using GMap.NET.MapProviders;



namespace csDronLink
{

    public partial class Dron
    {
        // Atributos
        MAVLink.MavlinkParse mavlink = new MAVLink.MavlinkParse();
        // El modo puede ser "simulacion" o "produccion"
        string modo;

        // Para la comunicación en producción
        SerialPort puertoSerie = new SerialPort();
        // Para la comunicación en simulación
        NetworkStream puertoTCP;

        // Ultimo comando de navegación, que usará el bucle de navegación para recordarle
        // al dron hacia dónde debe navegar
        byte[] navPacket;


        // Los datos de telemetría que me interesan 
        float relative_alt;
        float lat;
        float lon;
        float heading;


        // Aquí guardaré la referencia a la función que tengo que ejecutar
        // si el cliente me pide que le envíe los datos de telemetría.
        Action<List<(string nombre, float valor)>> ProcesarTelemetria = null;

        // Cuando reciba un comando de navegación debo poner en marcha el 
        // bucle de navegación para que recuerde al autopiloto que mantenga
        // el rumbo. Con esta variable controlamos el bucle de navagación
        Boolean navegando = false;

        // Velocidad para las operaciones de navegación
        int velocidad = 1;

        MessageHandler messageHandler;

        // Constrictor, conexión, registro de telemetria y envio de mensajes
        public Dron()
        {
        }
        private void EnviarMensaje (byte[] packet)
        {
            if (modo == "produccion")
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
            if (modo == "produccion")
            {
                // Configuro el puerto serie
                puertoSerie.PortName = conector;
                puertoSerie.BaudRate = 57600;
                puertoSerie.Open();
                // Pongo en marcha en message handler
                messageHandler = new MessageHandler(modo, puertoSerie);
            } else
            {
                // Configuro la conexión con el simulador
                // ESTO HABRÁ QUE CAMBIARLO PORQUE PUEDE QUE EL CLIENTE QUIERA CONECTARSE A OTROS PUERTOS
                string ip = "127.0.0.1";
                int port = 5763;
                TcpClient client = new TcpClient(ip, port);
                puertoTCP = client.GetStream();
                messageHandler = new MessageHandler(modo, puertoTCP);

            }


            // Hago una petición asíncrona al handler para que me envíe todos los mensajes
            // del tipo indicado, que son los que contienen los datos de telemetria que me interesan
            // Le indico que cuando llegue un mensaje de ese tipo ejecute la función RegistrarTelemetria
            string msgType = ((int)MAVLink.MAVLINK_MSG_ID.GLOBAL_POSITION_INT).ToString();
            messageHandler.RegisterHandler(msgType, RegistrarTelemetria);
            
            // Ahora le pido al autopiloto que me envíe mensajes del tipo indicado (los que contienen
            // los datos de telemetría, cada 2 segundos)
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


        //// Telemetria
        //public void EnviarDatosTelemetria(Action<List<(string nombre, float valor)>> f)
        //{   
        //    // El cliente me pide que ejecute la función f cada vez que reciba un mensaje
        //    // con datos de telemetría
        //    this.ProcesarTelemetria = f;

        //}
        //public void DetenerDatosTelemetria()
        //{
        //    // El cliente ya no quiere datos de telemetría
        //    this.ProcesarTelemetria = null;
        //}
     

        // Comprobadores
        //public bool ComprobarEnAire(MAVLink.MAVLinkMessage msg, object targetAlt)
        //{
        //    // es la función que comprobará que la altitud del dron es la indicada como 
        //    // parámetro. Se usará para ver si el dron ha alcanzado la altura indicada en el despegue
        //    // Recupero la altitud a partir del mensaje recibido
        //    var position = (MAVLink.mavlink_global_position_int_t)msg.data;
        //    float altitud = position.relative_alt / 1000.0f;
        //    // retorno el resultado de realizar la comprobacion (con un margen del 10%)
        //    return altitud > (int)targetAlt * 0.90;
        //}
        //public bool ComprobarParado(MAVLink.MAVLinkMessage msg, object param = null)
        //{
        //    // verifica si el mensaje indica que la velocidad del dron es cero
        //    // Servirá para detectar que el dron ha llegado al destino
        //    var position = (MAVLink.mavlink_global_position_int_t)msg.data;
        //    float vx = position.vx;
        //    float vy = position.vy;
        //    float vz = position.vz;
        //    double velocidad = Math.Sqrt(vx * vx + vy * vy + vz * vz) / 100;
        //    Console.WriteLine("velocidad " + velocidad);
        //    return velocidad < 0.1;
        //}
        //public bool ComprobarEnTierra(MAVLink.MAVLinkMessage msg, object param = null)
        //{
        //    // es la función que comprobará que la altitud del dron es menor de 50 cm. Se usará para 
        //    // detectar el fin de la operación de aterrizaje o RTL
        //    var position = (MAVLink.mavlink_global_position_int_t)msg.data;
        //    float altitud = position.relative_alt / 1000.0f;
        //    return altitud < 0.50;
        //}
        //private bool ComprobarOrientacion(MAVLink.MAVLinkMessage msg, object grados)
        //{
        //    // es la función que comprobará que la orientación es la indicada
        //    // Se usa para determiar cuándo ha acabado la operación de cambio de heading
        //    var position = (MAVLink.mavlink_global_position_int_t)msg.data;
        //    float heading = position.hdg / 100.0f;
        //    return Math.Abs(heading - (float)grados) < 5;
        //}
        //private bool ComprobarEnWaypoint(MAVLink.MAVLinkMessage msg, object n)
        //// Se usa para comprobar que el mensaje indica que se ha llegado al  waypoint
        //// con índice n
        //{

        //    var seq = ((MAVLink.mavlink_mission_item_reached_t)msg.data).seq;
        //    return seq == (int)n;
        //}

        //// Miscelanea
        //public void PonModoGuiado ()
        //{
        //    // Primero ponemos el dron en modo GUIDED
        //    MAVLink.mavlink_set_mode_t setMode = new MAVLink.mavlink_set_mode_t
        //    {
        //        target_system = 1,
        //        base_mode = (byte)MAVLink.MAV_MODE_FLAG.CUSTOM_MODE_ENABLED,
        //        custom_mode = 4 // GUIDED Mode en ArduPilot
        //    };

        //    byte[] packet = mavlink.GenerateMAVLinkPacket10(MAVLink.MAVLINK_MSG_ID.SET_MODE, setMode);
        //    EnviarMensaje(packet);
        //}
        //public void CambiaVelocidad(int velocidad)
        //{
        //    this.velocidad = velocidad;
        //    // Crear el mensaje COMMAND_LONG
        //    MAVLink.mavlink_command_long_t speedCommand = new MAVLink.mavlink_command_long_t();
        //    speedCommand.target_system = 1;      // ID del sistema (drone)
        //    speedCommand.target_component = 1; // ID del componente
        //    speedCommand.command = (ushort)MAVLink.MAV_CMD.DO_CHANGE_SPEED;
        //    speedCommand.param1 = 1;   // Tipo de velocidad (1 = Aérea, 0 = Terrestre, 2 = Vertical)
        //    speedCommand.param2 = velocidad;   // Velocidad en m/s (Aquí ponemos 7 m/s)
        //    speedCommand.param3 = -1;  // Sin aceleración específica (-1 usa la por defecto)
        //    speedCommand.param4 = 0;   // No se usa
        //    speedCommand.param5 = 0;   // No se usa
        //    speedCommand.param6 = 0;   // No se usa
        //    speedCommand.param7 = 0;   // No se usa

        //    // Generar el paquete MAVLink
        //    byte[] packet = mavlink.GenerateMAVLinkPacket10(MAVLink.MAVLINK_MSG_ID.COMMAND_LONG, speedCommand);

        //    // Enviar el paquete al dron
        //    EnviarMensaje(packet);

        //}
        //public void _CambiarHeading(float nuevoHeading, Action<object> f = null, object param = null)
        //{

        //    var req = new MAVLink.mavlink_command_long_t
        //    {
        //        target_system = 1,       // ID del dron
        //        target_component = 1, // ID del componente (normalmente autopiloto)
        //        command = (ushort)MAVLink.MAV_CMD.CONDITION_YAW,
        //        param1 = nuevoHeading,  // Ángulo de heading (en grados)
        //        param2 = 30,     // Velocidad de giro (grados por segundo)
        //        param3 = 1,             // Sentido horario
        //        param4 = 0,     // heading absoluto
        //        param5 = 0,             // No usado
        //        param6 = 0,             // No usado
        //        param7 = 0       // 0 = Absoluto, 1 = Relativo
        //    };
        //    // Generar el paquete MAVLink
        //    byte[] packet = mavlink.GenerateMAVLinkPacket10(MAVLink.MAVLINK_MSG_ID.COMMAND_LONG, req);

        //    // Enviar el paquete al dron
        //    EnviarMensaje(packet);

        //    // Espero a que se complete la operación
        //    string msgType = ((int)MAVLink.MAVLINK_MSG_ID.GLOBAL_POSITION_INT).ToString();
        //    var mensaje = messageHandler.WaitForMessageBlock(
        //        msgType, // GLOBAL_POSITION_INT
        //        condition: ComprobarOrientacion,
        //        parameters: nuevoHeading,
        //        timeout: -1
        //    );

        //    if (f != null)
        //        f(param);
        //}
        //public void CambiarHeading(float nuevoHeading, Boolean bloquear = true, Action<object> f = null, object param = null)
        //{
        //    if (bloquear)
        //    {
        //        this._CambiarHeading(nuevoHeading);
        //    }
        //    else
        //    {
        //        Thread t = new Thread(() => _CambiarHeading(nuevoHeading, f, param));
        //        t.Start();
        //    }
        //}

        //// Despegue, aterrizaje y RTL
        //private void _Despegar(int altitud, Action<object> f = null, object param = null)
        //{
        //    // Primero ponemos el dron en modo GUIDED
        //    PonModoGuiado();

        //    // Despues armamos armamos
        //    MAVLink.mavlink_command_long_t req = new MAVLink.mavlink_command_long_t();
        //    req.target_system = 1;
        //    req.target_component = 1;
        //    req.command = (ushort)MAVLink.MAV_CMD.COMPONENT_ARM_DISARM;
        //    req.param1 = 1;
        //    byte [] packet = mavlink.GenerateMAVLinkPacket10(MAVLink.MAVLINK_MSG_ID.COMMAND_LONG, req);
        //    EnviarMensaje(packet);

        //    // Y ahora despegamos
        //    req = new MAVLink.mavlink_command_long_t
        //    {
        //        target_system = 1,
        //        target_component = 1,
        //        command = (ushort)MAVLink.MAV_CMD.TAKEOFF,
        //        param7 = altitud // Altura deseada en metros
        //    };

        //    // Generar paquete MAVLink
        //    packet = mavlink.GenerateMAVLinkPacket10(MAVLink.MAVLINK_MSG_ID.COMMAND_LONG, req);
        //    // Enviar comando al dron
        //    EnviarMensaje(packet);

        //    // Aqui espero el mensaje que indique que ya ha alcanzado la altura de despeque
        //    string msgType = ((int)MAVLink.MAVLINK_MSG_ID.GLOBAL_POSITION_INT).ToString();
        //    MAVLink.MAVLinkMessage message = messageHandler.WaitForMessageBlock(
        //        msgType, // GLOBAL_POSITION_INT
        //        condition: ComprobarEnAire,
        //        parameters: altitud,
        //        timeout: -1    
        //    );
        //    if (f != null)
        //        f(param);

        //}
        //public void Despegar(int altitud, Boolean bloquear = true, Action<object> f = null, object param = null)
        //{
        //    // Si la llamada es bloqueante llamo a la función 
        //    if (bloquear)
        //    {
        //        this._Despegar(altitud);
        //    }
        //    // Si no es bloqueante pongo en marcha un thread para que se ocupe y no bloqueo al cliente
        //    else
        //    {
        //        Thread t = new Thread(() => _Despegar(altitud, f, param));
        //        t.Start();
        //    }

        //}
        //public void _RTL(Action<object> f = null, object param = null)
        //{
        //    MAVLink.mavlink_set_mode_t setMode = new MAVLink.mavlink_set_mode_t
        //    {
        //        target_system = 1,
        //        base_mode = (byte)MAVLink.MAV_MODE_FLAG.CUSTOM_MODE_ENABLED,
        //        custom_mode = 6 // Modo RTL en ArduPilot
        //    };

        //    byte[] packet = mavlink.GenerateMAVLinkPacket10(MAVLink.MAVLINK_MSG_ID.SET_MODE, setMode);
        //    // Enviar comando al dron
        //    //serialPort1.Write(packet, 0, packet.Length);
        //    EnviarMensaje(packet);

        //    // Pedimos un mensaje de telemetria que indique que ya está en tierra
        //    string msgType = ((int)MAVLink.MAVLINK_MSG_ID.GLOBAL_POSITION_INT).ToString();
        //    var mensaje = messageHandler.WaitForMessageBlock(
        //        msgType, // GLOBAL_POSITION_INT
        //        condition: ComprobarEnTierra,
        //        timeout: -1
        //    );

        //    if (f != null)
        //        f(param);

        //}
        //public void _Aterrizar(Action<object> f = null, object param = null)
        //{

        //    // Crear el paquete para el comando de aterrizaje
        //    MAVLink.mavlink_command_long_t req = new MAVLink.mavlink_command_long_t
        //    {
        //        target_system = 1,  // ID del sistema (1 es el sistema principal)
        //        target_component = 1,  // ID del componente (1 es el autopiloto)
        //        command = (ushort)MAVLink.MAV_CMD.LAND,  // Comando de aterrizaje
        //        param1 = 0,  // No se usa (0 es valor por defecto)
        //        param2 = 0,  // No se usa (0 es valor por defecto)
        //        param3 = 0,  // No se usa (0 es valor por defecto)
        //        param4 = 0,  // No se usa (0 es valor por defecto)
        //        param5 = 0,  // No se usa (0 es valor por defecto)
        //        param6 = 0,  // No se usa (0 es valor por defecto)
        //        param7 = 0   // No se usa (0 es valor por defecto)
        //    };

        //    // Generar el paquete MAVLink para el comando de aterrizaje
        //    byte[] packet = mavlink.GenerateMAVLinkPacket10(MAVLink.MAVLINK_MSG_ID.COMMAND_LONG, req);
        //    // Enviar comando al dron
        //    //serialPort1.Write(packet, 0, packet.Length);
        //    EnviarMensaje(packet);

        //    string msgType = ((int)MAVLink.MAVLINK_MSG_ID.GLOBAL_POSITION_INT).ToString();
        //    var mensaje = messageHandler.WaitForMessageBlock(
        //        msgType, // GLOBAL_POSITION_INT
        //        condition: ComprobarEnTierra,
        //         timeout: -1
        //    );


        //    if (f != null)
        //        f(param);

        //}
        //public void RTL(Boolean bloquear = true, Action<object> f = null, object param = null)
        //{
        //    if (bloquear)
        //    {
        //        this._RTL();
        //    }
        //    else
        //    {
        //        Thread t = new Thread(() => _RTL(f, param));
        //        t.Start();
        //    }
        //}
        //public void Aterrizar(Boolean bloquear = true, Action<object> f = null, object param = null)
        //{
        //    if (bloquear)
        //    {
        //        this._Aterrizar();
        //    }
        //    else
        //    {
        //        Thread t = new Thread(() => _Aterrizar(f, param));
        //        t.Start();
        //    }
        //}

        // Parametros
        //public List<float> LeerParametros(List<string> parametros)
        //{
        //    // En la lista resultante dejaré los valores de los parámetros en el mismo
        //    // orden en el que aparecen en la lista de parámetros

        //    List<float> resultado = new List<float>();

        //    // Crear un vector de bytes con un tamaño fijo para el nombre del parámetro
        //    byte[] paramIdBytes = new byte[16];

        //    foreach (string param in parametros)
        //    {
        //        // Primero hago una petición síncrona pero no bloqueante. 
        //        // Lo hago así porque si primero pido y luego hago la petición
        //        // pudiera pasar que el mensaje llegase antes de que la petición se haya 
        //        // registrado. Así que primero registro, luego pido y luego espero
        //        string msgType = ((int)MAVLink.MAVLINK_MSG_ID.PARAM_VALUE).ToString();
        //        WaitingRequest waiting = messageHandler.WaitForMessageNoBlock(msgType);

        //        // Convertir el nombre del parámetro 
        //        Array.Copy(Encoding.ASCII.GetBytes(param), paramIdBytes, param.Length);

        //        // Crear la solicitud para obtener el valor del parámetro RTL_ALT
        //        MAVLink.mavlink_param_request_read_t req = new MAVLink.mavlink_param_request_read_t
        //        {
        //            target_system = 1,  // ID del sistema (1 es el sistema principal)
        //            target_component = 1,  // ID del componente (1 es el autopiloto)
        //            param_index = -1,  // Índice del parámetro (no se usa para leer por nombre)
        //            param_id = paramIdBytes,  // El parámetro debe ser un arreglo de bytes de longitud fija
        //        };

        //        // Generar el paquete MAVLink para solicitar el parámetro
        //        byte[] packet = mavlink.GenerateMAVLinkPacket10(MAVLink.MAVLINK_MSG_ID.PARAM_REQUEST_READ, req);
   
        //        EnviarMensaje(packet);

        //        // ahora espero la respuesta
        //        var response = messageHandler.WaitNow(waiting, timeout: 10000);

        //        if (response != null)
        //        {
        //            // aqui tengo la respuesta
        //            var paramResponse = (MAVLink.mavlink_param_value_t)response.data;
        //            string res = Encoding.ASCII.GetString(paramResponse.param_id).Trim();
        //            // Verificamos si el parámetro es el correcto (NO ESTOY SEGURO DE QUE HAGA FALTA)
        //            res = res.Split('\0')[0];
        //            if (res == param)
        //                resultado.Add(paramResponse.param_value);
        //        }
        //        // hACEMOS ALGO SI NO LLEGA EL PARAMETRO EN 10 SEGUNDOS?

        //    }
        //    return resultado;
        //}
        //public void EscribirParametros(List<(string parametro, float valor)> parametros)
        //{
        //    byte[] paramIdBytes = new byte[16];

        //    for (int i=0; i< parametros.Count; i++ )
        //    {
        //        // datos del siguiente parámetro a escribir
        //        string parametro = parametros[i].parametro;
        //        float valor = parametros[i].valor;

        //        // Convertir el nombre del parámetro en un vector de bytes
        //        Array.Copy(Encoding.ASCII.GetBytes(parametro), paramIdBytes, parametro.Length);

        //        // Crear la solicitud para establecer el valor del parámetro 
        //        MAVLink.mavlink_param_set_t peticion = new MAVLink.mavlink_param_set_t
        //        {
        //            target_system = 1,      // ID del sistema (1 es el sistema principal)
        //            target_component = 1,   // ID del componente (1 es el autopiloto)
        //            param_value = valor,  // Nuevo valor del parámetro 
        //            param_id = paramIdBytes,    // Nombre del parámetro en un array de bytes
        //            param_type = (byte)MAVLink.MAV_PARAM_TYPE.REAL32  // Tipo de dato (float de 32 bits)
        //        };

        //        // Generar el paquete MAVLink para enviar el nuevo valor del parámetro
        //        byte[] paquete = mavlink.GenerateMAVLinkPacket10(MAVLink.MAVLINK_MSG_ID.PARAM_SET, peticion);

        //        // Enviar la solicitud de actualización del parámetro al dron
        //        EnviarMensaje(paquete);
        //    }
        //}
        
        
        //// Navegacion
        //private void _BucleNavegacion ( )
        //{
        //    // En este bucle repetimos cada segundo el ultimo comando de navegación
        //    // porque el dron solo navega pocos segundos si no se le insiste periódicamente.
        //    this.navegando = true;
        //    while (this.navegando)
        //    {
        //        // vuelvo a enviar el último comando de navegación
        //        //serialPort1.Write(this.navPacket, 0, this.navPacket.Length);
        //        EnviarMensaje(this.navPacket);
        //        Thread.Sleep(1000);
        //    } 
        //}
        //public void _Navegar(string direccion, Action<object> f = null, object param = null)
        //{
        //    int vx = 0, vy = 0, vz = 0;
        //    int velocidad = this.velocidad;

        //    if (direccion == "NorthWest")
        //    {
        //        vx = velocidad;
        //        vy = -velocidad;
        //    }

        //    if (direccion == "North")
        //    {
        //        vx = velocidad;
        //        vy = 0;
        //    }

        //    if (direccion == "NorthEast")
        //    {
        //        vx = velocidad;
        //        vy = velocidad;
        //    }

        //    if (direccion == "West")
        //    {
        //        vx = 0;
        //        vy = -velocidad;
        //    }

        //    if (direccion == "Stop")
        //    {
        //        vx = 0;
        //        vy = 0;
        //    }

        //    if (direccion == "East")
        //    {
        //        vx = 0;
        //        vy = velocidad;
        //    }
        //    if (direccion == "SouthWest")
        //    {
        //        vx = -velocidad;
        //        vy = -velocidad;
        //    }

        //    if (direccion == "South")
        //    {
        //        vx = -velocidad;
        //        vy = 0;
        //    }

        //    if (direccion == "SouthEast")
        //    {
        //        vx = -velocidad;
        //        vy = velocidad;
        //    }
        //    if (direccion == "Up")
        //    {
        //        vz = -velocidad;
        //    }
        //    if (direccion == "Down")
        //    {
        //        vz = velocidad;
        //    }

        //    // Crear el mensaje SET_POSITION_TARGET_LOCAL_NED para navegar en la dirección indicada
        //    MAVLink.mavlink_set_position_target_local_ned_t moveCmd = new MAVLink.mavlink_set_position_target_local_ned_t
        //    {
        //        target_system = 1,        // ID del sistema (autopiloto)
        //        target_component = 1,     // ID del componente (controlador de vuelo)
        //        coordinate_frame = (byte)MAVLink.MAV_FRAME.LOCAL_NED,  // Sistema de coordenadas local NED
        //        type_mask = 0b_0000111111000111, // Ignorar posición, usar velocidad
        //        x = 0,                    // No especificamos una posición en X
        //        y = 0,                    // No especificamos una posición en Y
        //        z = 0,                  // Mantener altura a 10 metros (-10 en NED)
        //        vx = vx,               // Velocidad hacia el norte (metros/segundo)
        //        vy = vy,                   // No moverse en la dirección este/oeste
        //        vz = vz,                   // Mantener la altura
        //        afx = 0,                  // Sin aceleración específica
        //        afy = 0,                  // Sin aceleración específica
        //        afz = 0,                  // Sin aceleración específica
        //        yaw = float.NaN,          // No cambiar la orientación
        //        yaw_rate = 0              // No rotar
        //    };

        //    // Generar el paquete MAVLink
        //    this.navPacket = mavlink.GenerateMAVLinkPacket10(MAVLink.MAVLINK_MSG_ID.SET_POSITION_TARGET_LOCAL_NED, moveCmd);
        //    EnviarMensaje(this.navPacket);
        //    // Pongo en marcha el bucle de navegación si es necesario
        //    if (!this.navegando)
        //    {
        //        Thread t = new Thread(() => _BucleNavegacion());
        //        t.Start();
        //    }
            
        //    if (f != null)
        //        f(param);

        //}
        //public void Navegar(string direccion, Boolean bloquear = true, Action<object> f = null, object param = null)
        //{
        //    if (bloquear)
        //    {
        //        this._Navegar(direccion);
        //    }
        //    else
        //    {
        //        Thread t = new Thread(() => _Navegar(direccion));
        //        t.Start();
        //    }
        //}

        //// Movimiento
        //public void _Mover(string direccion, int distancia, Action<object> f = null, object param = null)
        //{
        //    // paro el bucle de navegación si es necesario
        //    // Las operaciones de movimiento indican la distancia. No es necesario recordarle al autopiloto
        //    // el comando periódicamente

        //    // ATENCIÓN: EN LOS MOVIMIENTOS EN DIAGONAL HABRIA QUE RECTIFICAR LA DISTANCIA PORQUE SI NO 
        //    // LA DISTANCIA RECORRIDA ES MAYOR QUE LA INDICADA, DE ACUERDO CON EL TEOREMA DE PITAGORAS

        //    if (this.navegando)
        //        this.navegando = false;

        //    int dx = 0, dy = 0, dz = 0;

        //    if (direccion == "ForwardLeft")
        //    {
        //        dx = distancia;
        //        dy = -distancia;
        //    }

        //    if (direccion == "Forward")
        //    {
        //        dx = distancia;
        //        dy = 0;
        //    }

        //    if (direccion == "ForwardRight")
        //    {
        //        dx = distancia;
        //        dy = distancia;
        //    }

        //    if (direccion == "Left")
        //    {
        //        dx = 0;
        //        dy = -distancia;
        //    }

        //    if (direccion == "Stop")
        //    {
        //        dx = 0;
        //        dy = 0;
        //    }

        //    if (direccion == "Right")
        //    {
        //        dx = 0;
        //        dy = distancia;
        //    }
        //    if (direccion == "BackLeft")
        //    {
        //        dx = -distancia;
        //        dy = -distancia;
        //    }

        //    if (direccion == "Back")
        //    {
        //        dx = -distancia;
        //        dy = 0;
        //    }

        //    if (direccion == "BackRight")
        //    {
        //        dx = -distancia;
        //        dy = distancia;
        //    }
        //    if (direccion == "Up")
        //    {
        //        dz = -distancia;
        //    }
        //    if (direccion == "Down")
        //    {
        //        dz = distancia;
        //    }

        //    // Crear el mensaje de movimiento en coordenadas NED (Norte-Este-Abajo)
        //    MAVLink.mavlink_set_position_target_local_ned_t moveCmd = new MAVLink.mavlink_set_position_target_local_ned_t
        //    {
        //        target_system = 1,
        //        target_component = 1,
        //        coordinate_frame = (byte)MAVLink.MAV_FRAME.BODY_OFFSET_NED, // Sistema de coordenadas local NED
        //        type_mask = 0b_0000110111111000, 
        //        x = dx, 
        //        y = dy, 
        //        z = dz, 
        //        vx = 0, 
        //        vy = 0,
        //        vz = 0,
        //        afy = 0,
        //        afz = 0,
        //        yaw =0,
        //        yaw_rate = 0
        //    };

        //    // Generar paquete MAVLink
        //    byte[] packet = mavlink.GenerateMAVLinkPacket10(MAVLink.MAVLINK_MSG_ID.SET_POSITION_TARGET_LOCAL_NED, moveCmd);

        //    // Enviar comando al dron
        //    EnviarMensaje(packet);
        //    // Espero 2 segundos a que el dron coja velocidad
        //    // Porque inmediatamente esperaré a que la velocidad vuelva a ser 0
        //    Thread.Sleep(2000);
            

        //    // Aqui espero el mensaje que indique que ya ha llegado al destino 
        //    // Lo sabre porque la velocidad será cero
        //    string msgType = ((int)MAVLink.MAVLINK_MSG_ID.GLOBAL_POSITION_INT).ToString();
        //    MAVLink.MAVLinkMessage message = messageHandler.WaitForMessageBlock(
        //        msgType, // GLOBAL_POSITION_INT
        //        condition: ComprobarParado,
        //        timeout: -1   
        //    );

        //    if (f != null)
        //        f(param);
            

        //}
        //public void Mover(string direccion, int distancia, Boolean bloquear= true, Action<object> f = null,  object param = null)
        //{
        //    if (bloquear)
        //    {
        //        this._Mover(direccion, distancia);
        //    }
        //    else
        //    {
        //        Thread t = new Thread(() => _Mover(direccion, distancia, f, param));
        //        t.Start();
        //    }
        //}
     
        //private void _IrAlPunto(float lat, float lon, float alt, Action<object> f = null, object param = null)
        //{
        //    // Paramos el bucle de navegación (por si estuviera en marcha)
        //    this.navegando = false;
        //    var msg = new MAVLink.mavlink_set_position_target_global_int_t
        //    {
        //        target_system = 1, // ID del dron
        //        target_component = 1, // Componente (generalmente 1)
        //        coordinate_frame = (byte)MAV_FRAME.GLOBAL_RELATIVE_ALT_INT,
        //        type_mask = (ushort)(
        //       POSITION_TARGET_TYPEMASK.VX_IGNORE |
        //       POSITION_TARGET_TYPEMASK.VY_IGNORE |
        //       POSITION_TARGET_TYPEMASK.VZ_IGNORE |
        //       POSITION_TARGET_TYPEMASK.AX_IGNORE |
        //       POSITION_TARGET_TYPEMASK.AY_IGNORE |
        //       POSITION_TARGET_TYPEMASK.AZ_IGNORE |
        //       POSITION_TARGET_TYPEMASK.YAW_IGNORE |
        //       POSITION_TARGET_TYPEMASK.YAW_RATE_IGNORE
        //   ), // Solo usamos lat, lon y alt
        //        lat_int = (int)(lat * 1e7), // Convertimos a formato entero
        //        lon_int = (int)(lon * 1e7),
        //        alt = alt,
        //        yaw = 0, // Mantener el rumbo actual
        //        yaw_rate = 0
        //    };

        //    // Generar el paquete MAVLink
        //    byte[] packet = mavlink.GenerateMAVLinkPacket10(MAVLINK_MSG_ID.SET_POSITION_TARGET_GLOBAL_INT, msg);

        //    // Enviar el paquete al dron
        //    EnviarMensaje(packet);

        //    // espero 2 segundos a que el dron empiece a moverse
        //    Thread.Sleep(2000);

        //    // Aqui espero el mensaje que indique que ya ha llegado al destino 
        //    // Lo sabre porque la velocidad será cero
        //    string msgType = ((int)MAVLink.MAVLINK_MSG_ID.GLOBAL_POSITION_INT).ToString();
        //    MAVLink.MAVLinkMessage message = messageHandler.WaitForMessageBlock(
        //        msgType, // GLOBAL_POSITION_INT
        //        condition: ComprobarParado,
        //        timeout: -1    
        //    );

        //    if (f != null)
        //        f(param);
        //}
        //public void IrAlPunto(float lat, float lon, float alt, Boolean bloquear = true, Action<object> f = null, object param = null)
        //{
        //    if (bloquear)
        //    {
        //        this._IrAlPunto (lat, lon, alt);
        //    }
        //    else
        //    {
        //        Thread t = new Thread(() => _IrAlPunto(lat, lon, alt, f, param));
        //        t.Start();
        //    }
        //}
        
        //// Escenario
        //public void EstableceEscenario(List<List<(float lat, float lon)>> scenario)
        //{
        //    /* Un escenario es una lista de fences. El primero siempre es el de inclusión que define
        //     * los límites del area de vuelo. Los restantes son fences de exclusión, que representan los obstaculos
        //     * del área de vuelo.
        //     * Cada fence es una lista de PointLatLng. 
        //     * Si la lista tiene 3 PointLatLng o más es un fence de tipo polígono. Si tiene solo dos entonces
        //     * es un fence de tipo círculo. En ese caso, el primer PointLatLng es el centro del círculo y la latitud 
        //     * del segundo PointLatLng es en realidad el radio del círculo.
        //     * El primer fence (el de inclusión) debe ser de tipo polígono
        //     * */

        //    // Tomo el fence de inclusión
        //    List<(float lat, float lon)> waypoints = scenario[0];
        //    // En esta lista prepararé los comandos para fijar los waypoints
        //    List<MAVLink.mavlink_mission_item_int_t> wploader = new List<MAVLink.mavlink_mission_item_int_t>();
        //    int seq = 0;

        //    // preparo los comandos 
        //    foreach (var wp in waypoints)
        //    {
        //            wploader.Add(new MAVLink.mavlink_mission_item_int_t()
        //            {
        //                target_system = 1,
        //                target_component = 1,
        //                seq = (ushort)seq,
        //                frame = (byte)MAVLink.MAV_FRAME.GLOBAL,
        //                command = (ushort)MAVLink.MAV_CMD.FENCE_POLYGON_VERTEX_INCLUSION,
        //                param1 = waypoints.Count,
        //                x = (int)(wp.lat * 1e7),
        //                y = (int)(wp.lon * 1e7),
        //                mission_type = (byte)MAVLink.MAV_MISSION_TYPE.FENCE
        //            });
        //            seq++;
        //     }
            
          
        //    // ahora preparamos los obstáculos
        //    for (int i = 1; i < scenario.Count; i++)
        //    {
        //        waypoints = scenario[i];
        //        if (waypoints.Count == 2)
        //        // es un circulo
        //        {
        //            wploader.Add(new MAVLink.mavlink_mission_item_int_t()
        //            {
        //                target_system = 1,
        //                target_component = 1,
        //                seq = (ushort)seq,
        //                frame = (byte)MAVLink.MAV_FRAME.GLOBAL,
        //                command = (ushort)MAVLink.MAV_CMD.FENCE_CIRCLE_EXCLUSION,
        //                param1 = Convert.ToSingle(waypoints[1].lat), // en realidad es el radio
        //                x = (int)(waypoints[0].lat * 1e7),
        //                y = (int)(waypoints[0].lon * 1e7),
        //                mission_type = (byte)MAVLink.MAV_MISSION_TYPE.FENCE
        //            });
        //            seq++;
        //        }
        //        else
        //        {
        //            // es un polígono

        //            foreach (var wp in waypoints)
        //            {
        //                wploader.Add(new MAVLink.mavlink_mission_item_int_t()
        //                {
        //                    target_system = 1,
        //                    target_component = 1,
        //                    seq = (ushort)seq,
        //                    frame = (byte)MAVLink.MAV_FRAME.GLOBAL,
        //                    command = (ushort)MAVLink.MAV_CMD.FENCE_POLYGON_VERTEX_EXCLUSION,
        //                    param1 = waypoints.Count,
        //                    x = (int)(wp.lat * 1e7),
        //                    y = (int)(wp.lon * 1e7),
        //                    mission_type = (byte)MAVLink.MAV_MISSION_TYPE.FENCE
        //                });
        //                seq++;
        //            }
        //        }
        //    }
        //    // Envío el número de waypoints
        //    var msg = new MAVLink.mavlink_mission_count_t
        //    {
        //        target_system = 1,
        //        target_component = 1,
        //        count = (ushort)wploader.Count,
        //        mission_type = (byte)MAVLink.MAV_MISSION_TYPE.FENCE
        //    };

        //    byte[] packet = mavlink.GenerateMAVLinkPacket10(MAVLink.MAVLINK_MSG_ID.MISSION_COUNT, msg);
        //    EnviarMensaje(packet);


        //    // Ahora espero a que el autopiloto me vaya pidiendo los waypoints uno a uno
        //    string msgType;
        //    while (true)
        //    {
        //        msgType = ((int)MAVLink.MAVLINK_MSG_ID.MISSION_REQUEST).ToString();
        //        MAVLink.MAVLinkMessage request = messageHandler.WaitForMessageBlock(
        //            msgType,
        //            timeout: -1
        //        );
        //        // El mensaje contiene el número de waypoint que está pidiendo el autopiloto
        //        int next = ((MAVLink.mavlink_mission_request_t)request.data).seq;
        //        // envío el comando que me pide
        //        MAVLink.mavlink_mission_item_int_t msg2 = wploader[next];
        //        packet = mavlink.GenerateMAVLinkPacket10(MAVLink.MAVLINK_MSG_ID.MISSION_ITEM_INT, msg2);
        //        //serialPort1.Write(packet, 0, packet.Length);
        //        EnviarMensaje(packet );
        //        if (next == wploader.Count - 1) break; // Ya los he enviado todos
        //    }
        //    // Espero a que me confirme que ha recibido todo bien
        //    msgType = ((int)MAVLink.MAVLINK_MSG_ID.MISSION_ACK).ToString();
        //    MAVLink.MAVLinkMessage response = messageHandler.WaitForMessageBlock(
        //        msgType,
        //        timeout: -1
        //    );

        //}

        //// Mision
        //public void CargarMision(List<(float lat, float lon)> mision)
        //{

        //    // primero borramos la misión que tenga el autopiloto
        //    // Enviar comando para borrar todas las misiones del autopiloto
        //    MAVLink.mavlink_mission_clear_all_t clearMission = new MAVLink.mavlink_mission_clear_all_t
        //    {
        //        target_system = 1,
        //        target_component = 1
        //    };
        //    byte[] packet = mavlink.GenerateMAVLinkPacket10(MAVLink.MAVLINK_MSG_ID.MISSION_COUNT, clearMission);
        //    EnviarMensaje(packet);


        //    // POR ALGUNA RAZON que no controlo tengo que enviar dos veces el primer waypoint de la misión
        //    (float lat, float lon) primero = mision[0];
        //    mision.Insert(0, primero);

        //    // Lista con los comandos
        //    List<MAVLink.mavlink_mission_item_int_t> wploader = new List<MAVLink.mavlink_mission_item_int_t>();
        //    int seq = 0;
        //    foreach (var wp in mision)
        //    {
        //        wploader.Add(new MAVLink.mavlink_mission_item_int_t()
        //        {
        //            target_system = 1,
        //            target_component = 1,
        //            seq = (ushort) seq,
        //            frame = (byte)MAVLink.MAV_FRAME.GLOBAL_RELATIVE_ALT,
        //            command = (ushort)MAVLink.MAV_CMD.WAYPOINT,
        //            current = (byte)(seq == 0 ? 1 : 0), // El primer waypoint es el actual
        //            autocontinue = 1,
        //            //param1 = mision.Count,
        //            x = (int)(wp.lat * 1e7),  // Latitud en formato entero
        //            y = (int)(wp.lon * 1e7),  // Longitud en formato entero
        //            z =20,               // Altitud en metros. VER COMO HACER PARA QUE ESTO PUEDA CAMBIARSE
        //            mission_type = (byte)MAVLink.MAV_MISSION_TYPE.MISSION
        //        });
        //        seq++;
        //    }
        //    // Envio el número total de waypoints
        //    var countMsg = new MAVLink.mavlink_mission_count_t
        //    {
        //        target_system = 1,
        //        target_component = 1,
        //        count = (ushort)mision.Count,
        //        mission_type = (byte)MAVLink.MAV_MISSION_TYPE.MISSION
        //    };

        //    packet = mavlink.GenerateMAVLinkPacket10(MAVLink.MAVLINK_MSG_ID.MISSION_COUNT, countMsg);
        //    EnviarMensaje(packet);
        //    string msgType;
        //    // Ahora espero que el autopiloto me vaya pidiendo los waypoints uno a uno
        //    while (true)
        //    {
        //        msgType = ((int)MAVLink.MAVLINK_MSG_ID.MISSION_REQUEST).ToString();
        //        MAVLink.MAVLinkMessage request = messageHandler.WaitForMessageBlock(
        //            msgType,
        //            timeout: -1
        //        );
        //        int next = ((MAVLink.mavlink_mission_request_t)request.data).seq;
        //        MAVLink.mavlink_mission_item_int_t msg1 = wploader[next];
        //        packet = mavlink.GenerateMAVLinkPacket10(MAVLink.MAVLINK_MSG_ID.MISSION_ITEM_INT, msg1);
        //        EnviarMensaje(packet);
        //        if (next == wploader.Count - 1) break; // Ya estan todos
        //    }

        //    // Espero la confirmación final
        //    msgType = ((int)MAVLink.MAVLINK_MSG_ID.MISSION_ACK).ToString();
        //    MAVLink.MAVLinkMessage response2 = messageHandler.WaitForMessageBlock(
        //        msgType,
        //        timeout: -1
        //    );

          
        //}
        //private void _EjecutarMision(Action<object> EnWaypoint = null, Action<object> f = null, object param = null)
        //{
        //    // Enviar solicitud de número de waypoints
        //    MAVLink.mavlink_mission_request_list_t requestList = new MAVLink.mavlink_mission_request_list_t
        //    {
        //        target_system = 1,
        //        target_component = 1
        //    };
        //    // Enviar el comando al autopiloto
        //    byte[] packet = mavlink.GenerateMAVLinkPacket10(MAVLink.MAVLINK_MSG_ID.MISSION_REQUEST_LIST, requestList);
        //    EnviarMensaje(packet);

        //    string msgType = ((int)MAVLink.MAVLINK_MSG_ID.MISSION_COUNT).ToString();

        //    var mensaje = messageHandler.WaitForMessageBlock(
        //        msgType, // GLOBAL_POSITION_INT
        //        timeout: -1 // tiempo indefinido
        //    );
        //    // Recojo el numero de waypoints de la misión
        //    int numWaypoints = ((MAVLink.mavlink_mission_count_t)mensaje.data).count;

        //    // Doy la orden de iniciar la misión
        //    MAVLink.mavlink_command_long_t cmd = new MAVLink.mavlink_command_long_t
        //    {
        //        target_system = 1,
        //        target_component = 1,
        //        command = (ushort)MAVLink.MAV_CMD.MISSION_START,
        //        confirmation = 0,
        //    };

        //    // Enviar el comando al autopiloto
        //    packet = mavlink.GenerateMAVLinkPacket10(MAVLink.MAVLINK_MSG_ID.COMMAND_LONG, cmd);
        //    EnviarMensaje(packet);

     
        //    // Ahora espero que se llegue a cada uno de los waypoints
        //    for (int i = 1; i < numWaypoints; i++)
        //    {
        //        msgType = ((int)MAVLink.MAVLINK_MSG_ID.MISSION_ITEM_REACHED).ToString();
        //        mensaje = messageHandler.WaitForMessageBlock(
        //            msgType,
        //            condition: ComprobarEnWaypoint,
        //            parameters: i,
        //            timeout: -1
        //        );
        //        // Llamo a la función que me piden, pasandole el índice del waypoint alcanzado
        //        if (EnWaypoint != null)
        //            EnWaypoint(i);
        //    }
        //    PonModoGuiado();
        //    if (f != null)
        //        f(param);
        //}
        //public void EjecutarMision(Boolean bloquear = true, Action<object> EnWaypoint = null, Action<object> f = null, object param = null)
        //{
        //    // EnWaypointy es la función que se activará cada vez que se alcance uno de los waypoints,
        //    // pasándole como parámetro el índice del waypoint alcanzado
        //    if (bloquear)
        //    {
        //        this._EjecutarMision();
        //    }
        //    else
        //    {
        //        Thread t = new Thread(() => _EjecutarMision(EnWaypoint, f, param));
        //        t.Start();
        //    }
        //}

    }
}