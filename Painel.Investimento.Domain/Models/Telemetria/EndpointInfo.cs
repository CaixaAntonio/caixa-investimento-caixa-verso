namespace Painel.Investimento.Domain.Models.Telemetria
{
    public class EndpointInfo
    {
        public string NomeDaApi { get; set; }
        public int QuantidadeRequisicoes { get; set; }
        public double TempoMedio { get; set; }
        public long TempoMinimo { get; set; }
        public long TempoMaximo { get; set; }
    }
}
