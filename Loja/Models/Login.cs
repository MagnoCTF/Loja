using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loja.Models
{
    public class Login
    {
        public int Id { get; set; }
        public int IdFuncionario { get; set; }
        public string Usuario { get; set; }
        public string Senha{ get; set; }
        
    }
}
