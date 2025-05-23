﻿using GMap.NET;
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
        public void Ir_Vertiport_desde_Origen()
        {
            int Verti_destino = GetVertiport();
            if (Verti_destino == 1)
            {
                IrAlPunto(1.97442042224494f, 41.28227584647858f, 30f, false, null, null);
                IrAlPunto(1.977886141612211f, 41.28517715040588f, 30f, false, null, null);
                IrAlPunto(1.978115994177971f, 41.28800969553971f, 30f, false, null, null);
            }
            if (Verti_destino == 2)
            {
                IrAlPunto(1.974450069544436f, 41.28223112901948f, 40f, false, null, null);
                IrAlPunto(1.976003201515988f, 41.27946024691047f, 40f, false, null, null);
                IrAlPunto(1.970035535884251f, 41.27201586056459f, 40f, false, null, null);
            }
            if (Verti_destino == 3)
            {
                IrAlPunto(1.97442042224494f, 41.28227584647858f, 50f, false, null, null);
                IrAlPunto(1.977886141612211f, 41.28517715040588f, 50f, false, null, null);
                IrAlPunto(1.978115994177971f, 41.28800969553971f, 50f, false, null, null);
            }
            if (Verti_destino == 4)
            {
                IrAlPunto(1.974469123663518f, 41.28223368362573f, 60f, false, null, null);
                IrAlPunto(1.983050799542174f, 41.28372696376741f, 60f, false, null, null);
                IrAlPunto(1.987667618535942f, 41.28300131082602f, 60f, false, null, null);
            }
            if (Verti_destino == 5)
            {
                IrAlPunto(1.974441276440291f, 41.28222923347127f, 70f, false, null, null);
                IrAlPunto(1.978629738607129f, .241794357437671f, 70f, false, null, null);
                IrAlPunto(1.979245491619148f, 41.27742091318283f, 70f, false, null, null);
                IrAlPunto(1.982542135377885f, 41.27468360752572f, 70f, false, null, null);

            }


        }

        public void Volver_Base(Dron dron)
        {
            int Vertiport_orgien = GetVertiport();
            if (Vertiport_orgien == 1)
            {
                IrAlPunto(1.965876289288155f, 41.28003834530395f, 35f, false, null, null);
                IrAlPunto(1.978115994177971f, 41.28800969553971f, 35f, false, null, null);
                IrAlPunto(1.977886141612211f, 41.28517715040588f, 35f, false, null, null);
                IrAlPunto(1.97442042224494f, 41.28227584647858f, 35f, false, null, null);
            }
            if (Vertiport_orgien == 2)
            {
                IrAlPunto(1.969997990106584f, 41.27184861877969f, 45f, false, null, null);
                IrAlPunto(1.970035535884251f, 41.27201586056459f, 45f, false, null, null);
                IrAlPunto(1.976003201515988f, 41.27946024691047f, 45f, false, null, null);
                IrAlPunto(1.974450069544436f, 41.28223112901948f, 45f, false, null, null);

            }
            if (Vertiport_orgien == 3)
            {
                IrAlPunto(1.9780956067817f, 41.28791137200264f, 55f, false, null, null);
                IrAlPunto(1.978115994177971f, 41.28800969553971f, 55f, false, null, null);
                IrAlPunto(1.977886141612211f, 41.28517715040588f, 55f, false, null, null);
                IrAlPunto(1.97442042224494f, 41.28227584647858f, 55f, false, null, null);


            }
            if (Vertiport_orgien == 4)
            {
                IrAlPunto(1.9875981218196f, 41.28291843857831f, 65f, false, null, null);
                IrAlPunto(1.974469123663518f, 41.28223368362573f, 65f, false, null, null);
                IrAlPunto(1.983050799542174f, 41.28372696376741f, 65f, false, null, null);
                IrAlPunto(1.987667618535942f, 41.28300131082602f, 65f, false, null, null);
            }
            if (Vertiport_orgien == 5)
            {
                IrAlPunto(1.982551304247888f, 41.27463819858801f, 75f, false, null, null);
                IrAlPunto(1.982542135377885f, 41.27468360752572f, 75f, false, null, null);
                IrAlPunto(1.979245491619148f, 41.27742091318283f, 75f, false, null, null);
                IrAlPunto(1.978629738607129f, .241794357437671f, 75f, false, null, null);
                IrAlPunto(1.974441276440291f, 41.28222923347127f, 75f, false, null, null);
            }
        }

        public void asignarVertiport(PointLatLng direccion, List<PointLatLng> lista_vertiports)
        {
            float distMinima = 10000;
            int vertiport_count = 0;
            int vertiport = 0;
            foreach (var point in lista_vertiports)
            {
                vertiport_count += 1;
                float distancia = calcula_dist(direccion, point);
                if (distancia < distMinima)
                {
                    distMinima = distancia;
                    vertiport = vertiport_count;
                }
            }
            SetVertiport(vertiport);
        }
        public float calcula_dist(PointLatLng coordenadas, PointLatLng point)
        {
            double dist_x = Math.Abs(coordenadas.Lat - point.Lat);
            double dist_y = Math.Abs(coordenadas.Lng - point.Lng);
            float distancia = Convert.ToSingle(Math.Sqrt(dist_x * dist_x + dist_y * dist_y));
            return distancia;
        }
    }
}