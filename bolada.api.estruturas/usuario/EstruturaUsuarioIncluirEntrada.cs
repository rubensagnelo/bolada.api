using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace estrutura.usuario
{
    public class EstruturaUsuarioIncluirEntrada : EstruturaEntradaBase
    {
        public String idusuario { get; set; }
        public String email { get; set; }
        public String senha { get; set; }
        public String senhaConfirmacao { get; set; }
        public String nome { get; set; }
    }
}
