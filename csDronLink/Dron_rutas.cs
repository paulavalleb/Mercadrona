using GMap.NET;
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
        public void crearMision()
        {
            var mision = new List<(float lat, float lon)>();
            int Verti_destino = GetVertiport();

            if (Verti_destino == 1)
            {
                mision.Add((41.28227584647858f, 1.97442042224494f));
                mision.Add((41.28517715040588f, 1.977886141612211f));
                mision.Add((41.28800969553971f, 1.978115994177971f));
            }
            else if (Verti_destino == 2)
            {
                mision.Add((41.28223112901948f, 1.974450069544436f));
                mision.Add((41.27946024691047f, 1.976003201515988f));
                mision.Add((41.27201586056459f, 1.970035535884251f));
            }
            else if (Verti_destino == 3)
            {
                mision.Add((41.28227584647858f, 1.97442042224494f));
                mision.Add((41.28517715040588f, 1.977886141612211f));
                mision.Add((41.28800969553971f, 1.978115994177971f));
            }
            else if (Verti_destino == 4)
            {
                mision.Add((41.28223368362573f, 1.974469123663518f));
                mision.Add((41.28372696376741f, 1.983050799542174f));
                mision.Add((41.28300131082602f, 1.987667618535942f));
            }
            else if (Verti_destino == 5)
            {
                mision.Add((41.28222923347127f, 1.974441276440291f));
                mision.Add((41.241794357437671f, 1.978629738607129f));
                mision.Add((41.27742091318283f, 1.979245491619148f));
                mision.Add((41.27468360752572f, 1.982542135377885f));
            }

            this.mision = mision;
        }
        
    
        public void volverMision()
        {
            var mision = new List<(float lat, float lon)>();
            int Verti_destino = GetVertiport();
            if (Verti_destino == 1)
            {
                mision.Add((41.28227584647858f, 1.97442042224494f));
                mision.Add((41.28517715040588f, 1.977886141612211f));
                mision.Add((41.28800969553971f, 1.978115994177971f));
            }
            if (Verti_destino == 2)
            {
                mision.Add((41.28223112901948f, 1.974450069544436f));
                mision.Add((41.27946024691047f, 1.976003201515988f));
                mision.Add((41.27201586056459f, 1.970035535884251f));
            }
            if (Verti_destino == 3)
            {
                mision.Add((41.28227584647858f, 1.97442042224494f));
                mision.Add((41.28517715040588f, 1.977886141612211f));
                mision.Add((41.28800969553971f, 1.978115994177971f));
            }
            if (Verti_destino == 4)
            {
                mision.Add((41.28223368362573f, 1.974469123663518f));
                mision.Add((41.28372696376741f, 1.983050799542174f));
                mision.Add((41.28300131082602f, 1.987667618535942f));
            }
            if (Verti_destino == 5)
            {
                mision.Add((41.28222923347127f, 1.974441276440291f));
                mision.Add((41.241794357437671f, 1.978629738607129f));
                mision.Add((41.27742091318283f, 1.979245491619148f));
                mision.Add((41.27468360752572f, 1.982542135377885f));

            }
            this.mision = mision;
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