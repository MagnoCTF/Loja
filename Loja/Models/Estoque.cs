using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loja.Models
{
    public class Estoque
    {
        public int Id { get; set; }
        public int IdProduto { get; set; }
        public int Quantidade { get; set; }
        
    }
}
