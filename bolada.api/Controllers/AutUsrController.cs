using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using estrutura.usuario;

namespace bolada.api.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class AutUsrController : ControllerBase
    {

		[Route("api/[controller]/login")]
		[HttpPost]
		public EstruturaUsuarioLoginRetorno login([FromBody] EstruturaUsuarioLoginEntrada ent)
		{
			return negocio.usuarioNegocio.login(ent);
		}



		//// GET: api/AutUsr
		//[HttpGet]
  //      public IEnumerable<string> Get()
  //      {
  //          return new string[] { "value1", "value2" };
  //      }

  //      // GET: api/AutUsr/5
  //      [HttpGet("{id}", Name = "Get")]
  //      public string Get(int id)
  //      {
  //          return "value";
  //      }

  //      // POST: api/AutUsr
  //      [HttpPost]
  //      public void Post([FromBody] string value)
  //      {
  //      }

  //      // PUT: api/AutUsr/5
  //      [HttpPut("{id}")]
  //      public void Put(int id, [FromBody] string value)
  //      {
  //      }

  //      // DELETE: api/ApiWithActions/5
  //      [HttpDelete("{id}")]
  //      public void Delete(int id)
  //      {
  //      }
    }
}
