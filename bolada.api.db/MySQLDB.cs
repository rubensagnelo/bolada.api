using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace proxy.database
{
	public struct struturaExecSQL
	{
		public Int64 linhasafetadas;
		public bool erro;
		public string mensagem;
		public string mensagemDetalhada;
		public System.Data.Common.DbDataReader Reader;


		public void verificarErro()
		{
			if (erro)
				throw new Exception(this.mensagem);
		}
	}

	public class MySQLDB : dbDriver
	{

		private static MySql.Data.MySqlClient.MySqlConnection db;
		private static MySql.Data.MySqlClient.MySqlDataReader reader = null;
		private static MySql.Data.MySqlClient.MySqlConnection mconn = null;

		public static string connStr {
			get {
				string cnnstr = util.configTools.getConfig("mysqldb");
				return cnnstr;
			}
		}

		public static List<MySqlParameter> prm = null;

		public static void AddParametro(string nomeparametro, object valor)
		{
			if (prm == null)
				prm = new List<MySqlParameter>();
			MySqlParameter pr = new MySqlParameter(nomeparametro, MySqlDbType.String);
			pr.Value = valor;
			prm.Add(pr);
		}



		private static MySqlConnection dbMySQL()
		{
			MySql.Data.MySqlClient.MySqlConnection result = null;
			try
			{
				if (mconn == null || mconn.State != ConnectionState.Open)
					mconn = new MySqlConnection(connStr);

				if (mconn.State == ConnectionState.Closed)
				{
					mconn.Open();
				}
				result = mconn;
			}
			catch (MySqlException ex)
			{
				throw ex;
			}
			return result;
		}

		public static void Close()
		{
			forceCloserReader();
			forceCloserDataBase();
		}

		private static void forceCloserReader()
		{
			try
			{
				reader.Close();
			}
			catch { };
		}

		private static void forceCloserDataBase()
		{
			try
			{
				db.Close();
			}
			catch { };
		}

		public static struturaExecSQL execSQL(string sql)
		{
			return execSQL(sql, null);
		}

		public static struturaExecSQL execSQL(string sql, List<MySqlParameter> parameters)
		{
			//Int64 result = -1;
			struturaExecSQL result = new struturaExecSQL();
			int linafe = 0;

			db = null;
			try
			{
				db = dbMySQL();
				MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, db);

				try { cmd.Parameters.Clear(); } catch { }

				forceCloserReader();

				if (parameters != null)
				{
					foreach (var item in parameters)
					{
						MySqlParameter par = new MySqlParameter(item.ParameterName, item.MySqlDbType);
						par.Value = item.Value;
						cmd.Parameters.Add(par);
					}
				}


				linafe = cmd.ExecuteNonQuery();

				result.linhasafetadas = linafe; //"0;OK;" + linafe.ToString();
				result.erro = false;
				result.mensagem = string.Empty;
			}
			catch (Exception ex)
			{
				result.linhasafetadas = 0; //"0;OK;" + linafe.ToString();
				result.erro = true;
				result.mensagem = ex.Message;
				result.mensagemDetalhada = string.Empty;
				//"1;ERRO:" + ex.Message + ";0";

				Console.WriteLine(ex.Message);
				if (ex.InnerException != null && ex.InnerException.Message != null)
				{
					result.mensagemDetalhada = ex.InnerException.Message;
					Console.WriteLine(ex.InnerException.Message);
				}
			}
			finally
			{
				if (parameters != null)
					parameters.Clear();
				try
				{
					if (db != null && db.State == System.Data.ConnectionState.Open)
						Close();
				}
				catch (Exception) { }

			}
			return result;

		}

		public static struturaExecSQL execReader(string sql)
		{
			return execReader(sql, null);
		}

		public static struturaExecSQL execReader(string sql, List<MySqlParameter> parameters)
		{
			Close();
			db = dbMySQL();
			struturaExecSQL result = new struturaExecSQL();
			reader = null;
			try
			{
				MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, db);
				try { cmd.Parameters.Clear(); } catch { }

				if (parameters != null)
				{
					foreach (var item in parameters)
					{
						MySqlParameter par = new MySqlParameter(item.ParameterName, item.MySqlDbType);
						par.Value = item.Value;
						cmd.Parameters.Add(par);
					}
				}
				reader = cmd.ExecuteReader();
			}
			catch (Exception ex)
			{
				result.erro = true;
				result.mensagem = ex.Message;
				result.mensagemDetalhada = string.Empty;
				result.Reader = null;

				Console.WriteLine(ex.Message);
				if (ex.InnerException != null && ex.InnerException.Message != null)
				{
					result.mensagemDetalhada = ex.InnerException.Message;
					Console.WriteLine(ex.InnerException.Message);
				}
			}
			finally
			{
				if (parameters != null)
					parameters.Clear();
			}
			result.Reader = reader;
			return result;
		}

		public static String max(string tabela, string coluna, string filtro = "")
		{

			MySQLDB.validainjecaoSQL(tabela);
			MySQLDB.validainjecaoSQL(coluna);

			String result = string.Empty;
			try
			{
				string strSql = new StringBuilder("SELECT MAX(").Append(coluna).Append(") FROM ").Append(tabela).ToString();

				if (!String.IsNullOrWhiteSpace(filtro))
					strSql = new StringBuilder(strSql).Append(" WHERE ").Append(filtro).ToString();

				//Parametros
				//List<MySqlParameter> prm = new List<MySqlParameter>();
				//prm.Add(new MySqlParameter("@IDGROWLER", IdGrowler));


				struturaExecSQL resultSQL = MySQLDB.execReader(strSql);

				if (!resultSQL.erro)
				{
					System.Data.Common.DbDataReader rs = resultSQL.Reader;
					while (rs.Read())
					{
						result = rs[0].ToString().ToString();
						break;
					}
				}

			}
			catch (Exception ex)
			{
				result = "ERRO";
				throw ex;
			}
			finally
			{
				MySQLDB.Close();
			}
			return result;
		}

		public static String consultavalor(string tabela, string coluna, string filtro = "")
		{

			MySQLDB.validainjecaoSQL(tabela);
			MySQLDB.validainjecaoSQL(coluna);

			String result = string.Empty;
			try
			{
				string strSql = new StringBuilder("SELECT ").Append(coluna).Append(" FROM ").Append(tabela).ToString();

				if (!String.IsNullOrWhiteSpace(filtro))
					strSql = new StringBuilder(strSql).Append(" WHERE ").Append(filtro).ToString();

				strSql = new StringBuilder(strSql).Append(" LIMIT 1 ").ToString();

				//Parametros
				//List<MySqlParameter> prm = new List<MySqlParameter>();
				//prm.Add(new MySqlParameter("@IDGROWLER", IdGrowler));


				struturaExecSQL resultSQL = MySQLDB.execReader(strSql);

				if (!resultSQL.erro)
				{
					System.Data.Common.DbDataReader rs = resultSQL.Reader;
					while (rs.Read())
					{
						result = rs[0].ToString().ToString();
						break;
					}
				}

			}
			catch (Exception ex)
			{
				result = "ERRO";
				throw ex;
			}
			finally
			{
				MySQLDB.Close();
			}
			return result;
		}


	}
}
