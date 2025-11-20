namespace Painel.Investimento.Domain.Valueobjects
{
    public sealed class DataDeNascimento : IEquatable<DataDeNascimento>
    {
        public DateTime Valor { get; }

        public DataDeNascimento(DateTime valor)
        {
            if (valor > DateTime.Today)
                throw new ArgumentOutOfRangeException(nameof(valor), "Data de nascimento não pode ser no futuro.");

            Valor = valor.Date; // garante que não tenha hora
        }

        public int CalcularIdade()
        {
            var hoje = DateTime.Today;
            int idade = hoje.Year - Valor.Year;

            if (hoje < Valor.AddYears(idade))
                idade--;

            return idade;
        }

        public bool IsMaiorDeIdade() => CalcularIdade() >= 18;

        public override string ToString() => Valor.ToString("dd/MM/yyyy");

        // public override bool Equals(object obj) => Equals(obj as DataDeNascimento);

        public bool Equals(DataDeNascimento other) => other != null && Valor.Equals(other.Valor);

        public override int GetHashCode() => Valor.GetHashCode();

        public static bool operator ==(DataDeNascimento left, DataDeNascimento right) => Equals(left, right);
        public static bool operator !=(DataDeNascimento left, DataDeNascimento right) => !Equals(left, right);
    }
}