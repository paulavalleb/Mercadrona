using GMap.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace csDronLink
{
    public partial class Pedido // partial?
    {
        // Atributos
        int id;
        List<(string nombre, int cantidad, double peso, double precio)> productos = new List<(string nombre, int cantidad, double peso, double precio)>();
        PointLatLng direccion_coord = new PointLatLng();
        string destinatario;
        double precio_total;
        double peso_total;
        float lat_base = 0;
        float lon_base = 0;
        PointLatLng base_coords;



        public Pedido()
        {
            base_coords = new PointLatLng(lat_base, lon_base);
        }
        public void crear_pedido(int id, List<(string nombre, int cantidad, double peso, double precio)> productos, PointLatLng direccion_coord, string destinatario, double precio_total, double peso_total)
        {
            this.id = id;
            this.productos = productos;
            this.direccion_coord = direccion_coord;
            this.destinatario = destinatario;
            this.precio_total = precio_total;
            this.peso_total = peso_total;
        }

        public Dron asignar_pedido(List<Dron> drones)
        {
            List<Dron> drones_disponibles = new List<Dron>();
            foreach (Dron dron in drones)
            {
                if (dron.GetEstado() == "disponible")
                {
                    drones_disponibles.Add(dron);
                }
            }
            if (drones_disponibles.Count != 0)
            {
                foreach (Dron d in drones_disponibles)
                {
                    if (d.GetCargaMax() > this.peso_total)
                    {
                        d.SetEstado("ocupado");
                        d.SetPedido_id(this.id);
                        return d;
                    }
                }
                MessageBox.Show("No hay drones disponibles con suficiente carga");
               return null;
                
            }
            else
            {
                MessageBox.Show("No hay drones disponibles");
                return null;
            }            
        }

        
        public void asignar_pedido_encola(List<Dron> drones)
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

        public void setDireccion(PointLatLng direccion)
        {
            this.direccion_coord = direccion;
      
        }
        public PointLatLng getDireccion()
        {
            return this.direccion_coord;
        }


    }
}