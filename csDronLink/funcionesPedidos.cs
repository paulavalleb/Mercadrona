using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csDronLink
{
    public partial class funcionesPedidos
    {
        private List<Pedido> pedidos = new List<Pedido>();
        private DataTable table;

        public void setPedidos(List<Pedido>pedidos)
        {
            this.pedidos = pedidos;
        }
    
        public List<Pedido> GetPedidos()
        {
            return pedidos;
        }
        public void setTable(DataTable table)
        {
            this.table = table;
        }

        public DataTable GetTable()
        {
            return table;
        }
    }
}
