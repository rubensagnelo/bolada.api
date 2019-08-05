using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace util
{
	public class configTools
	{

		public static config fcfg;
		public static config cfg
		{
			get
			{

#if !DEBUG
							string sufix = ".PRD";

#else
				string sufix = ".DEV";
#endif



				if (fcfg == null)
					fcfg = new config("configuracao" + sufix);
				return fcfg;
			}
		}


		public static string getConfig(string key)
		{
			//string text = System.IO.File.ReadAllText(@"

			//return System.Configuration.ConfigurationManager.AppSettings[new StringBuilder(key).Append(sufix).ToString()].ToString();
			return cfg.get(key);

		}

	}

	public class itemconfig
	{
		public string chave { get; set; }
		public string valor { get; set; }
	}

	public class config {

		public config(string arquivo="")
		{
			if (!string.IsNullOrEmpty(arquivo))
				carregar(arquivo);
		}


		public List<itemconfig> itens {get;set;}

		public void carregar(string nomarqcfg) {

			string patthfile = @System.IO.Directory.GetCurrentDirectory() + "/" + nomarqcfg;
			string text = "";
			if (System.IO.File.Exists(patthfile))
				text = System.IO.File.ReadAllText(patthfile);
			if (string.IsNullOrWhiteSpace(text))
			{
				var cfg = new List<itemconfig>();
				cfg.Add(new itemconfig { chave = "chaveTeste1", valor = "valorTeste1" });
				cfg.Add(new itemconfig { chave = "chaveTeste2", valor = "valorTeste2" });
				cfg.Add(new itemconfig { chave = "chaveTest3", valor = "valorTeste3" });
				text = Newtonsoft.Json.JsonConvert.SerializeObject(cfg);
				System.IO.File.WriteAllText(patthfile,text);
			}

			itens = Newtonsoft.Json.JsonConvert.DeserializeObject< List<itemconfig>>(text);
		}

		public string get(string key) {
			itemconfig itm = itens.Find(x => x.chave.Contains(key));
			return itm.valor;
		}
	}

}
