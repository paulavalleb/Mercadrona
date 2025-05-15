using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static MAVLink;

namespace csDronLink
{
    public partial class Dron
    {

        // Movimiento
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
                yaw = 0,
                yaw_rate = 0
            };

            // Generar paquete MAVLink
            byte[] packet = mavlink.GenerateMAVLinkPacket10(MAVLink.MAVLINK_MSG_ID.SET_POSITION_TARGET_LOCAL_NED, moveCmd);

            // Enviar comando al dron
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
                timeout: -1
            );

            if (f != null)
                f(param);


        }
        public void Mover(string direccion, int distancia, Boolean bloquear = true, Action<object> f = null, object param = null)
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



        private void Ir_Vertiport_desde_Origen(Dron dron)
        {
            int Verti_destino = dron.GetVertiport();
            if (Verti_destino == 1)
            {
                dron.IrAlPunto(1.97442042224494f, 41.28227584647858f, 30f, false, null, null);
                dron.IrAlPunto(1.977886141612211f, 41.28517715040588f, 30f, false, null, null);
                dron.IrAlPunto(1.978115994177971f, 41.28800969553971f, 30f, false, null, null);
            }
            if (Verti_destino == 2)
            {
                dron.IrAlPunto(1.974450069544436f, 41.28223112901948f, 40f, false, null, null);
                dron.IrAlPunto(1.976003201515988f, 41.27946024691047f, 40f, false, null, null);
                dron.IrAlPunto(1.970035535884251f, 41.27201586056459f, 40f, false, null, null);
            }
            if (Verti_destino == 3)
            {
                dron.IrAlPunto(1.97442042224494f, 41.28227584647858f, 50f, false, null, null);
                dron.IrAlPunto(1.977886141612211f, 41.28517715040588f, 50f, false, null, null);
                dron.IrAlPunto(1.978115994177971f, 41.28800969553971f, 50f, false, null, null);
            }
            if (Verti_destino == 4)
            {
                dron.IrAlPunto(1.974469123663518f, 41.28223368362573f, 60f, false, null, null);
                dron.IrAlPunto(1.983050799542174f, 41.28372696376741f, 60f, false, null, null);
                dron.IrAlPunto(1.987667618535942f, 41.28300131082602f, 60f, false, null, null);
            }
            if (Verti_destino == 5)
            {
                dron.IrAlPunto(1.974441276440291f, 41.28222923347127f, 70f, false, null, null);
                dron.IrAlPunto(1.978629738607129f, .241794357437671f, 70f, false, null, null);
                dron.IrAlPunto(1.979245491619148f, 41.27742091318283f, 70f, false, null, null);
                dron.IrAlPunto(1.982542135377885f, 41.27468360752572f, 70f, false, null, null);

            }


        }

        private void Volver_Base(Dron dron)
        {
            int Vertiport_orgien = dron.GetVertiport();
            if (Vertiport_orgien == 1)
            {
                dron.IrAlPunto(1.965876289288155f, 41.28003834530395f, 35f, false, null, null);
                dron.IrAlPunto(1.978115994177971f, 41.28800969553971f, 35f, false, null, null);
                dron.IrAlPunto(1.977886141612211f, 41.28517715040588f, 35f, false, null, null);
                dron.IrAlPunto(1.97442042224494f, 41.28227584647858f, 35f, false, null, null);
            }
            if (Vertiport_orgien == 2)
            {
                dron.IrAlPunto(1.969997990106584f, 41.27184861877969f, 45f, false, null, null);
                dron.IrAlPunto(1.970035535884251f, 41.27201586056459f, 45f, false, null, null);
                dron.IrAlPunto(1.976003201515988f, 41.27946024691047f, 45f, false, null, null);
                dron.IrAlPunto(1.974450069544436f, 41.28223112901948f, 45f, false, null, null);

            }
            if (Vertiport_orgien == 3)
            {
                dron.IrAlPunto(1.9780956067817f, 41.28791137200264f, 55f, false, null, null);
                dron.IrAlPunto(1.978115994177971f, 41.28800969553971f, 55f, false, null, null);
                dron.IrAlPunto(1.977886141612211f, 41.28517715040588f, 55f, false, null, null);
                dron.IrAlPunto(1.97442042224494f, 41.28227584647858f, 55f, false, null, null);


            }
            if (Vertiport_orgien == 4)
            {
                dron.IrAlPunto(1.9875981218196f, 41.28291843857831f, 65f, false, null, null);
                dron.IrAlPunto(1.974469123663518f, 41.28223368362573f, 65f, false, null, null);
                dron.IrAlPunto(1.983050799542174f, 41.28372696376741f, 65f, false, null, null);
                dron.IrAlPunto(1.987667618535942f, 41.28300131082602f, 65f, false, null, null);
            }
            if (Vertiport_orgien == 5)
            {
                dron.IrAlPunto(1.982551304247888f, 41.27463819858801f, 75f, false, null, null);
                dron.IrAlPunto(1.982542135377885f, 41.27468360752572f, 75f, false, null, null);
                dron.IrAlPunto(1.979245491619148f, 41.27742091318283f, 75f, false, null, null);
                dron.IrAlPunto(1.978629738607129f, .241794357437671f, 75f, false, null, null);
                dron.IrAlPunto(1.974441276440291f, 41.28222923347127f, 75f, false, null, null);
            }
        }


        private void _IrAlPunto(float lat, float lon, float alt, Action<object> f = null, object param = null)
        {
            // Paramos el bucle de navegación (por si estuviera en marcha)
            this.navegando = false;
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
                timeout: -1
            );

            if (f != null)
                f(param);
        }
        public void IrAlPunto(float lat, float lon, float alt, Boolean bloquear = true, Action<object> f = null, object param = null)
        {
            if (bloquear)
            {
                this._IrAlPunto(lat, lon, alt);
            }
            else
            {
                Thread t = new Thread(() => _IrAlPunto(lat, lon, alt, f, param));
                t.Start();
            }
        }

    }
}