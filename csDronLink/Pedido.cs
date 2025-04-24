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
        List<(string nombre, float cantidad, float peso)> productos = new List<(string nombre, float cantidad, float peso)>();
        List<(float x, float y)> direccion_coord = new List<(float x, float y)>();
        string destinatario;
        float lat_base = 0;
        float lon_base = 0;
        PointLatLng base_coords;



        public Pedido()
        {
            base_coords = new PointLatLng(lat_base, lon_base);
        }
        public void crear_pedido(List<(string nombre, float cantidad, float peso)> productos, List<(float x, float y)> direccion_coord, string destinatario)
        {
            this.productos = productos;
            this.direccion_coord = direccion_coord;
            this.destinatario = destinatario;
        }

        public void eliminar_pedido()
        {

        }

        public void asignar_pedido(List<Dron> drones, int pedido_id)
        {
            List<Dron> drones_disponibles = new List<Dron>();
            for (int i = 0; i < drones.Count; i++)
            {
                Dron dron = drones[i];
                if (dron.estado == "disponible")
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
                    dron.dist_base = dist_base;
                }
                drones_disponibles.OrderBy(dron => dron.dist_base);
                drones_disponibles[0].estado = "ocupado";
                drones_disponibles[0].pedido_id = pedido_id;
            }
            else
            {
                drones.OrderBy(dron => dron.pedidos_en_cola);
                drones[0].estado = "ocupado";
                drones[0].pedido_id = pedido_id;
            }
        }

        public float calcula_dist(Dron dron, PointLatLng coordenadas)
        {
            double dist_x = Math.Abs(coordenadas.Lat - dron.lat);
            double dist_y = Math.Abs(coordenadas.Lng - dron.lon);
            float distancia = Convert.ToSingle(Math.Sqrt(dist_x * dist_x + dist_y * dist_y));
            return distancia;
        }


    }
}