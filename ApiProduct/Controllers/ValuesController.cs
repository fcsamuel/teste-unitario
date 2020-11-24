using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ApiProduct.Models;
using ApiProduct.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ApiProduct.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("RetornaDados")]
        public List<Value> Get(int id)
        {
            HttpClient client = new HttpClient();
            var url = "https://olinda.bcb.gov.br/olinda/servico/Informes_ListaValoresDeServicoBancario/versao/v1/odata/GruposConsolidados?%24format=json&%24top=100";
            var dados = ServicoBancarioService.RetornaServicosBancariosGet(url, client).Result;
            var servicosBancarios = JsonConvert.DeserializeObject<Root>(dados);
            return servicosBancarios.value;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value) 
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
