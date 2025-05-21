using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csDronLink
{
    public partial class funcionesPedidos
    {
        private List<Pedido> pedidos = new List<Pedido>();

        public void setPedidos(List<Pedido>pedidos)
        {
            this.pedidos = pedidos;
        }
    
        public List<Pedido> GetPedidos()
        {
            return pedidos;
        }
    }
}
