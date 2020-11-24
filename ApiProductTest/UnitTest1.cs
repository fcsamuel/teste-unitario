using ApiProduct.Models;
using ApiProduct.Services;
using Business.Domain;
using Business.IR;
using Newtonsoft.Json;
using NUnit.Framework;
using RichardSzalay.MockHttp;
using System.Net.Http;
using Marca = Business.Domain.Marca;
using Produto = Business.Domain.Produto;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async System.Threading.Tasks.Task Teste_MockHttp_Async()
        {
            var url = "https://olinda.bcb.gov.br/olinda/servico/Informes_ListaValoresDeServicoBancario/versao/v1/odata/GruposConsolidados?%24format=json&%24top=100";
            var client = GetHttpClient();

            var dadosRetorno = await ServicoBancarioService.RetornaServicosBancariosGet(url, client);


            var servicosBancarios = JsonConvert.DeserializeObject<Root>(dadosRetorno);

            Assert.AreEqual(servicosBancarios.value.Count, 2);
        }

        [Test] 
        public void Teste_mockHttp_url_invalida()
        {
            var url = "https://olinda.bcb.gov.br/olinda/servico/Informess_ListaValoresDeServicoBancario/versao/v1/odata/GruposConsolidados?%24format=json&%24top=100";
            var client = GetHttpClient();

            var dadosRetorno = ServicoBancarioService.RetornaServicosBancariosGet(url, client).Result;

            Assert.IsEmpty(dadosRetorno);
        }

        private HttpClient GetHttpClient()
        {
            var mockHttp = new MockHttpMessageHandler();
            var jsonMock = ApiProductTest.Properties.Resources.JsonMockServicoBancario;

            mockHttp.When("https://olinda.bcb.gov.br/olinda/servico/Informes_ListaValoresDeServicoBancario/versao/v1/odata/*")
                    .Respond("application/json", jsonMock); // Respond with JSON

            var client = mockHttp.ToHttpClient();
            return client;
        }

        [TestCase("", false)]
        [TestCase(null, false)]
        [TestCase("teste valido teste valido teste valido", true)]
        [TestCase("teste                                                                              ", false)]
        public void Verifica_ds_produto(string dsProduto, bool isValid)
        {
            Assert.AreEqual(Produto.IsDsProdutoValida(dsProduto), isValid);
        }

        [Test]
        public void Verifica_categoria_produto()
        {
            Produto produto = new Business.Domain.Produto();
            produto.NrValor = 500;
            produto.setCategoriaProduto(produto.NrValor ?? 0);
            Assert.AreEqual(produto.DsCategoriaProduto, "PC");
            produto.NrValor = 5000;
            produto.setCategoriaProduto(produto.NrValor ?? 0);
            Assert.AreEqual(produto.DsCategoriaProduto, "AV");
            produto.NrValor = 100000;
            produto.setCategoriaProduto(produto.NrValor ?? 0);
            Assert.AreEqual(produto.DsCategoriaProduto, "PL");
            produto.NrValor = -9999;
            produto.setCategoriaProduto(produto.NrValor ?? 0);
            Assert.AreEqual(produto.DsCategoriaProduto, "SC");
            produto.NrValor = 0;
            produto.setCategoriaProduto(produto.NrValor ?? 0);
            Assert.AreEqual(produto.DsCategoriaProduto, "SC");
        }

        [Test]
        public void Verifica_valor_produto()
        {
            Produto produto = new Produto();
            produto.NrValor = 0;
            Assert.AreEqual(produto.isValorProdutoValido(), true);
            produto.NrValor = 500;
            Assert.AreEqual(produto.isValorProdutoValido(), true);
            produto.NrValor = 999999999.01M;
            Assert.AreEqual(produto.isValorProdutoValido(), false);
            produto.NrValor = -1;
            Assert.AreEqual(produto.isValorProdutoValido(), false);
        }

        [TestCase("", false)]
        [TestCase(null, false)]
        [TestCase("abc", false)]
        [TestCase("Masca teste", true)]
        [TestCase("Masca teste                                                             ", true)]
        [TestCase("TESTE GIGANTE TESTE GIGANTE TESTE GIGANTE TESTE GIGANTE TESTE GIGANTE TESTE GIGANTE TESTE GIGANTE TESTE GIGANTE TESTE GIGANTE TESTE GIGANTE", false)]
        public void Valida_ds_marca(string dsMarca, bool result)
        {
            Assert.AreEqual(Marca.IsDsMarcaValida(dsMarca), result);

        }

        [TestCase("")]
        [TestCase(null)]
        public void Valida_marca(string dsMarca)
        {
            Assert.IsFalse(Marca.IsDsMarcaValida(dsMarca));

        }

        [Test]
        public void Calcula_preco_venda_produto()
        {
            Assert.AreEqual(Helper.CalculaPrecoVenda(25, 10), 27.5M);
            Assert.AreNotEqual(Helper.CalculaPrecoVenda(10, 10), 5M);
        }

        [Test]
        public void Verifica_cadastro_completo()
        {
            Produto produto = new Produto();
            produto.CdProduto = 1;
            produto.DsProduto = "Produto teste";
            produto.CdMarca = 1;
            produto.DsCategoriaProduto = "PC";
            produto.NrValor = 25.00M;
            Assert.IsTrue(Helper.IsCadastroCompleto(produto));

            produto = new Produto();
            produto.CdProduto = 1;
            produto.DsCategoriaProduto = "PC";
            produto.NrValor = 25.00M;
            Assert.IsFalse(Helper.IsCadastroCompleto(produto));
        }

        [Test]
        public void aplica_desconto_valor()
        {
            Produto produto = new Produto();
            produto.NrValor = 255.50M;
            Assert.AreEqual(Helper.AplicaValorDesconto(produto.NrValor ?? 0, 270), 0);
            produto = new Produto();
            produto.NrValor = 180.20M;
            Assert.AreEqual(Helper.AplicaValorDesconto(produto.NrValor ?? 0, 50), 130.20);
            produto.NrValor = 0;
            Assert.AreEqual(Helper.AplicaValorDesconto(produto.NrValor ?? 0, 50), 0);
        }

        [Test]
        public void aplica_desconto_porcent()
        {
            Produto produto = new Produto();
            produto.NrValor = 260.50M;
            Assert.AreEqual(Helper.AplicaPorcentDesconto(produto.NrValor ?? 0, 50), 130.25);
            produto.NrValor = 260.50M;
            Assert.AreEqual(Helper.AplicaPorcentDesconto(produto.NrValor ?? 0, 120), 260.50M);
        }

        [Test]
        public void valida_preco_custo()
        {
            Assert.IsFalse(Helper.validaPrecoCusto(100.50M, 105.00M));
            Assert.IsTrue(Helper.validaPrecoCusto(100.50M, 40.25M));
        }

        [TestCase("KG")]
        [TestCase("UN")]
        [TestCase("PC")]
        [TestCase("MG       ")]
        public void Sg_unidmedida_valido(string sigla)
        {
            Assert.IsTrue(Helper.ValidaSgUnidMedida(sigla));
        }

        [TestCase("un")]
        [TestCase("KILO")]
        [TestCase("")]
        [TestCase("B")]
        [TestCase(null)]
        public void Sg_unidmedida_invalido(string sigla)
        {
            Assert.IsFalse(Helper.ValidaSgUnidMedida(sigla));
        }

        [TestCase("KG", "UN", 5, 1, 50, 10)]
        [TestCase("ML", "LT", 1000, 1, 5000, 5)]
        [TestCase("KG", "MG", 1, 1000, 10, 10000)]
        public void Conversao_unidade_medida(string unidMedidaIn,
                                             string unidMedidaOut,
                                             double qtUnidMedidaIn,
                                             double qtUnidMedidaOut,
                                             double qtConversao,
                                             double result)
        {
            Assert.AreEqual(Helper.converteUnidMedida(unidMedidaIn, unidMedidaOut, qtUnidMedidaIn, qtUnidMedidaOut, qtConversao), result);
        }

        [TestCase("K", "ML", 5, 10, 25, 50)]
        [TestCase("SC", "UN", 2, 50, 4, 150)]
        [TestCase("SC", "un", 2, 50, 4, 150)]
        [TestCase("", "UN", 2, 50, 4, 10)]
        public void Conversao_unid_medida_invalida(string unidMedidaIn,
                                                   string unidMedidaOut,
                                                   double qtUnidMedidaIn,
                                                   double qtUnidMedidaOut,
                                                   double qtConversao,
                                                   double result)
        {
            Assert.AreNotEqual(Helper.converteUnidMedida(unidMedidaIn, unidMedidaOut, qtUnidMedidaIn, qtUnidMedidaOut, qtConversao), result);
        }

        [Test]
        public void Teste_mockHttp_get_invalido()
        {
            var url = "http://localhost:5000/api/produto";
            var client = GetHttpClient();
            var dadosRetorno = ProdutoService.RetornaProdutosGet(url, client).Result;
            Assert.IsEmpty(dadosRetorno);
        }


    }
}