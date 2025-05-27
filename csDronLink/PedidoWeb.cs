using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csDronLink
{
    public class PedidoWeb
    {
        public long Id { get; set; } // JSON: "id" -> OK por camelCase
        public List<ProductoWeb> Productos { get; set; } // JSON: "productos" -> OK por camelCase
        public double Latitud { get; set; }  // JSON: "latitud" -> OK por camelCase
        public double Longitud { get; set; } // JSON: "longitud" -> OK por camelCase

        public string Destinatario { get; set; } // JSON: "destinatario" -> OK por camelCase

        public double PrecioTotal { get; set; }  // JSON: "precioTotal" -> OK por camelCase
        public double PesoTotal { get; set; }    // JSON: "pesoTotal" -> OK por camelCase
    }

    public class ProductoWeb
    {
        // También revisa estas, pero lo normal es que ya estén bien por camelCase
        public string Nombre { get; set; }   // JSON: "nombre" -> OK por camelCase
        public int Cantidad { get; set; }    // JSON: "cantidad" -> OK por camelCase
        public double Peso { get; set; }     // JSON: "peso" -> OK por camelCase
        public double Precio { get; set; }   // JSON: "precio" -> OK por camelCase
    }
}