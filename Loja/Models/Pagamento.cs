using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loja.Models
{
    public class Pagamento
    {
        public int Id { get; set; }
        public int IdPedido { get; set; }
        public string FormaPagamento { get; set; }
        public DateTime DataPagamento { get; set; }
        public decimal Valor { get; set; }
    }
}
