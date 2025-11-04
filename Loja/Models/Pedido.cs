using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loja.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public int IdCliente { get; set;}
        public DateTime DataPedido { get; set;}
        public string Status { get; set;}
        public decimal Total { get; set;}
    }
}
