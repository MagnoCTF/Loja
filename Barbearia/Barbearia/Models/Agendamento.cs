using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barbearia.Models
{
    public class Agendamento
    {
        public int ID { get; set; }
        public string NomeCliente { get; set; }
        public string Servico {  get; set; }
        public DateTime DataHora { get; set; }

    }
}
