using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesoDatos.Entidades
{
    public class FormatoEntrada
    {
        public int folio { get; set; }
        public int reciboDonativo { get; set; }
        public DateTime fechaEntrada { get; set; }
        public int id_aliado { get; set; }
        public string Programa { get; set; }
        public decimal tarimas { get; set; }
        public decimal mermas { get; set; }
        
    }

}
