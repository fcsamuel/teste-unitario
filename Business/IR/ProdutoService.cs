using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Business.IR
{
    public class ProdutoService
    {
        public static async Task<string> RetornaProdutosGet(string url, HttpClient client)
        {
            HttpResponseMessage response = await client.GetAsync(url);
            string result = string.Empty;
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
            }
            return result;
        }
    }
}
