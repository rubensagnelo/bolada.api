using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using proxy.database;
using MySql.Data.MySqlClient;



namespace mensagem
{
    public class mensagem
    {
        public static string consultar(long idmensagem)
        {
            string result = string.Empty;

            try
            {
                string strSql = "SELECT DSCMENSAGEN FROM MENSAGEM WHERE IDMENSAGEM=@IDMENSAGEM ";
                MySQLDB.AddParametro("@IDMENSAGEM", idmensagem);

                struturaExecSQL resultSQL = MySQLDB.execReader(strSql, MySQLDB.prm);

                object objresult = null;
                if (!resultSQL.erro)
                {
                    System.Data.Common.DbDataReader rs = resultSQL.Reader;
                    while (rs.Read())
                    {
                        objresult = rs["DSCMENSAGEN"];
                    }
                }

                if (objresult == null || objresult == DBNull.Value)
                    throw new Exception(new StringBuilder("Mensagem ").Append(idmensagem.ToString()).Append(" não encontrada.").ToString());

                result = objresult.ToString();

            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }
    }
}
