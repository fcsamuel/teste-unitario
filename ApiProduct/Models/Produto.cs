using System;
using System.Collections.Generic;

namespace ApiProduct.Models
{
    public partial class Produto
    {
        public int CdProduto { get; set; }
        public string DsProduto { get; set; }
        public int? CdMarca { get; set; }
        public string DsObs { get; set; }
        public decimal? NrValor { get; set; }
        public Marca CdMarcaNavigation { get; set; }
    }
}
