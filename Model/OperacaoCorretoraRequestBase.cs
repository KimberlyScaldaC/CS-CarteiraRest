namespace CS_CarteiraRest.Model
{
    public class OperacaoCorretoraRequestBase
    {
        public int CarteiraId { get; set; }       // ID da carteira que vai comprar ou vender
        public string Moeda { get; set; } = "";   // Nome da moeda (ex: BTC, ETH)
        public decimal Quantidade { get; set; }   // Quanto deseja comprar ou vender
        public string TipoOperacao { get; set; } = ""; // "compra" ou "venda"
    }
}