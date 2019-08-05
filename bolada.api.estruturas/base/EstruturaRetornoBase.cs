using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace estrutura
{
    public class EstruturaRetornoBase
    {

        public Int32 IdcErr { get; set; }
        public Int32 CodErr { get; set; }
        public String ExceptionMsg { get; set; }
        public String msg { get; set; }

        public EstruturaRetornoBase()
        {
            this.IdcErr = 0;
            this.CodErr = 0;
            this.ExceptionMsg = "";
            this.msg = "";

        }

        public EstruturaRetornoBase(int idcErr, int codErr, String exceptionMsg)
        {
            this.IdcErr = idcErr;
            this.CodErr = codErr;
            this.ExceptionMsg = exceptionMsg;
        }

        public EstruturaRetornoBase(Exception ex, int pCodErr, string pmsg)
        {
            this.IdcErr = 1;
            this.CodErr = pCodErr;
            this.ExceptionMsg = ex.Message;
            this.msg = pmsg;


        }

        public String toString()
        {
            return ("IdcErr: " + IdcErr.ToString() + ", " + "CodErr: " + CodErr.ToString() + ", ExceptionMsg: " + ExceptionMsg);
        }
    }


}
