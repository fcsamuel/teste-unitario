using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Domain
{
    public class Produto
    {
        public int CdProduto { get; set; }
        public string DsProduto { get; set; }
        public int? CdMarca { get; set; }
        public string DsObs { get; set; }
        public decimal? NrValor { get; set; }
        public Marca CdMarcaNavigation { get; set; }
        public string DsCategoriaProduto { get; set; }
        public string UnidMedida { get; set; }

        public Produto() { }

        public Produto(int cdProduto, string dsProduto, int? cdMarca, string dsObs, decimal? nrValor)
        {
            CdProduto = cdProduto;
            DsProduto = dsProduto;
            CdMarca = cdMarca;
            DsObs = dsObs;
            NrValor = nrValor;
        }

        public static bool IsDsProdutoValida(string descricao)
        {
            if (descricao != null && descricao.Trim().Length > 10 && descricao.Trim().Length < 150)
            {
                return true;
            }
            return false;
        }

        public void setCategoriaProduto(decimal valor)
        {
            if (valor > 0)
            {
                if (valor <= 1000)
                {
                    DsCategoriaProduto = "PC"; //Preço comum
                }
                else if (valor <= 10000)
                {
                    DsCategoriaProduto = "AV"; //Alto valor
                }
                else if (valor > 10000)
                {
                    DsCategoriaProduto = "PL"; //Produto de luxo
                }
            }
            else
            {
                DsCategoriaProduto = "SC"; //Sem Categoria
            }


        }

        public bool isValorProdutoValido()
        {
            if (NrValor >= 0 && NrValor < 99999999.00M)
            {
                return true;
            }
            return false;
        }

    }
}
