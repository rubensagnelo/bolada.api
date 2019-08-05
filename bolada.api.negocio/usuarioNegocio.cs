using estrutura;
using estrutura.usuario;
//using MySql.Data.MySqlClient;
using proxy.database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public class usuarioNegocio
    {

        private static bool loginEhValido(string email, string senha)
        {
            string strSql = "SELECT 1 FROM USUARIO WHERE EMAILUSUARIO=@EMAIL AND SENHAUSUARIO=@SENHA ";
			MySQLDB.AddParametro("@EMAIL", email);
            MySQLDB.AddParametro("@SENHA", senha);

            struturaExecSQL resultSQL = MySQLDB.execReader(strSql, MySQLDB.prm);

            object objresult = null;
            if (!resultSQL.erro)
            {
                System.Data.Common.DbDataReader rs = resultSQL.Reader;
                while (rs.Read())
                {
                    objresult = rs[0];
                }
            }

            try
            {
                return !(objresult == null || objresult == DBNull.Value || objresult == String.Empty);
            }
            catch { return false; }
        }

        public static EstruturaUsuarioLoginRetorno login(EstruturaUsuarioLoginEntrada ent)
        {
            EstruturaUsuarioLoginRetorno result = new EstruturaUsuarioLoginRetorno();
            
            
            try
            {

				if (!loginEhValido(ent.usuario, ent.senha))
					throw new Exception(mensagem.mensagem.consultar(1));
				else
					result.msg = "Acesso autorizado !!!";

                //string strSql = @"
                //                    SELECT 
                //                        usuario.idusuario,
                //                        usuario.email,
                //                        usuario.nome 'nomeusuario',
	               //                     dominio.iddominio, 
	               //                     dominio.nome, 
                //                        dominio.descricao 
                //                    FROM 
	               //                     usuario_dominio, 
                //                        usuario, 
                //                        dominio 
                //                     where 
                //                     usuario_dominio.idusuario = usuario.idusuario and 
                //                     usuario_dominio.iddominio = dominio.iddominio and 
                //                     USUARIO.EMAIL=@EMAIL;";


                //MySQLDB.AddParametro("@EMAIL", ent.usuario);

                //struturaExecSQL resultSQL = MySQLDB.execReader(strSql, MySQLDB.prm);

                //List<Int32> objresult = new List<Int32>();
                //if (!resultSQL.erro)
                //{
                    
                    //System.Data.Common.DbDataReader rs = resultSQL.Reader;
                    //if (rs.HasRows)
                    //    result.dominios = new List<estrutura.ivy.dominio.dominio>();

                    //while (rs.Read())
                    //{
                    //    if (result.usuario == null)
                    //    {
                    //        result.usuario = new usuario()
                    //        {
                    //            email = rs["email"].ToString(),
                    //            idusuario = rs["idusuario"].ToString(),
                    //            nome = rs["nomeusuario"].ToString(),
                    //            senha = ""
                    //        };
                    //    }

                    //    result.dominios.Add(new estrutura.ivy.dominio.dominio()
                    //    {
                    //        id = rs["iddominio"].ToString(),
                    //        nome = rs["nome"].ToString(),
                    //        descricao = rs["descricao"].ToString()
                    //    });
                        
                    //}
                //}



            }
            catch (Exception ex)
            {
                result.IdcErr = 1;
                result.CodErr = 1;
                result.msg = mensagem.mensagem.consultar(result.CodErr);
                result.ExceptionMsg = ex.Message + " | " + ex.InnerException.Message;
            }

            return result;

        }

        private static string proximoid()
        {
            return System.Convert.ToString(System.Convert.ToInt64(MySQLDB.max("USUARIO", "idusuario"))+1);
        }

        private static bool ehValidasenhadigitada(string senha, string confirmacao)
        {
            return senha == confirmacao;
        }

        private static bool ehEmailjacadastrado(string email)
        {
            string value = MySQLDB.consultavalor("USUARIO", "1", new StringBuilder("email='").Append(email).Append("'").ToString());
            return value == "1";
        }

        public static EstruturaUsuarioIncluirRetorno incluir(EstruturaUsuarioIncluirEntrada ent)
        {
            EstruturaUsuarioIncluirRetorno result = new EstruturaUsuarioIncluirRetorno();

            try
            {
                if (!ehValidasenhadigitada(ent.senha, ent.senhaConfirmacao))
                {
                    result.CodErr = 4;
                    throw new Exception(mensagem.mensagem.consultar(result.CodErr));//A senha difere da confirmação.
                }

                if (ehEmailjacadastrado(ent.email))
                {
                    result.CodErr = 5;
                    throw new Exception(mensagem.mensagem.consultar(result.CodErr));//Esse e-mail ja esta em uso por um usuário.
                }

                ent.idusuario = proximoid();



                string strSql = @" INSERT INTO USUARIO
                                    (IDUSUARIO,
                                    EMAIL,
                                    SENHA,
                                    NOME) 
                                    VALUES 
                                    (@IDUSUARIO,
                                    @EMAIL,
                                    @SENHA,
                                    @NOME)";


                MySQLDB.AddParametro("@IDUSUARIO", ent.idusuario);
                MySQLDB.AddParametro("@EMAIL", ent.email);
                MySQLDB.AddParametro("@SENHA", ent.senha);
                MySQLDB.AddParametro("@NOME", ent.nome);

                struturaExecSQL resultsql = MySQLDB.execSQL(strSql, MySQLDB.prm);
                if (resultsql.erro)
                    throw new Exception(new StringBuilder(resultsql.mensagem).Append(" detalhe:").Append(resultsql.mensagemDetalhada).ToString());

                result.msg = result.msg = mensagem.mensagem.consultar(3);

            }
            catch (Exception ex)
            {
                result.IdcErr = 1;
                if (result.CodErr==0)
                    result.CodErr = 2;
                result.msg = mensagem.mensagem.consultar(result.CodErr);
                result.ExceptionMsg = ex.Message;
            }

            return result;


        }




    }
}
