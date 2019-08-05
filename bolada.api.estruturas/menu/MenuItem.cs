using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace estrutura.menu
{
	public class MenuItem
	{
		public string id { get; set; }
		public string idpai { get; set; }
		public string ordem { get; set; }
		public string titulo { get; set; }
		public string descricao { get; set; }
		public string imagem { get; set; }
		public string status { get; set; }
		public List<MenuItem> filhos { get; set; }

	}
}
