using ApiProduct.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiProduct.Services
{
    //Informes - Lista Valores de Serviço Bancário
    public class ServicoBancarioService
    {
        public static async Task<string> RetornaServicosBancariosGet(string url, HttpClient client)
        {
            HttpResponseMessage response = await client.GetAsync(url);
            string result = string.Empty;
            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadAsStringAsync();
            return result;
        }
    }
}
