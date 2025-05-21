using GMap.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csDronLink
{
    public partial class Pedido // partial?
    {
        // Atributos
        int id;
        List<(string nombre, int cantidad, double peso)> productos = new List<(string nombre, int cantidad, double peso)>();
        PointLatLng direccion_coord = new PointLatLng();
        string destinatario;
        float lat_base = 0;
        float lon_base = 0;
        PointLatLng base_coords;



        public Pedido()
        {
            base_coords = new PointLatLng(lat_base, lon_base);
        }
        public void crear_pedido(int id, List<(string nombre, int cantidad, double peso)> productos, PointLatLng direccion_coord, string destinatario)
        {
            this.id = id;
            this.productos = productos;
            this.direccion_coord = direccion_coord;
            this.destinatario = destinatario;
        }


        public void asignar_pedido(List<Dron> drones)
        {
            List<Dron> drones_disponibles = new List<Dron>();
            for (int i = 0; i < drones.Count; i++)
            {
                Dron dron = drones[i];
                if (dron.GetEstado() == "disponible")
                {
                    drones_disponibles.Add(dron);
                }
            }
            if (drones_disponibles.Count != 0)
            {
                for (int j = 0; j < drones_disponibles.Count; j++)
                {
                    Dron dron = drones_disponibles[j];
                    float dist_base = calcula_dist(dron, base_coords);
                    dron.SetDist_base(dist_base);
                }
                drones_disponibles.OrderBy(dron => dron.GetDist_base());
                drones_disponibles[0].SetEstado("ocupado");
                drones_disponibles[0].SetPedido_id(id);
            }
            else
            {
                drones.OrderBy(dron => dron.GetPedidos_en_cola());
                drones[0].SetEstado("ocupado");
                drones[0].SetPedido_id(id);
            }
        }

        public float calcula_dist(Dron dron, PointLatLng coordenadas)
        {
            double dist_x = Math.Abs(coordenadas.Lat - dron.GetLat());
            double dist_y = Math.Abs(coordenadas.Lng - dron.GetLon());
            float distancia = Convert.ToSingle(Math.Sqrt(dist_x * dist_x + dist_y * dist_y));
            return distancia;
        }


    }
}