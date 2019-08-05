using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace estrutura.usuario
{
    public class EstruturaUsuarioLoginEntrada : EstruturaEntradaBase
    {
        public string usuario { get; set; }
        public string senha { get; set; }
        public string token { get; set; }
        public string aparelho { get; set; }
    }
}
