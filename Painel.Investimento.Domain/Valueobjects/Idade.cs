namespace Painel.Investimento.Domain.Valueobjects
{
    public sealed class Idade : IEquatable<Idade>
    {
        public int Valor { get; }

        public Idade(int valor)
        {
            if (valor < 0 || valor > 150)
                throw new ArgumentOutOfRangeException(nameof(valor), "Idade deve estar entre 0 e 150 anos.");

            Valor = valor;
        }

        public bool IsMaiorDeIdade() => Valor >= 18;

        public bool IsIdoso() => Valor >= 60;

        public override string ToString() => $"{Valor} anos";

        public override bool Equals(object obj) => Equals(obj as Idade);

        public bool Equals(Idade other) => other != null && Valor == other.Valor;

        public override int GetHashCode() => Valor.GetHashCode();

        public static bool operator ==(Idade left, Idade right) => Equals(left, right);
        public static bool operator !=(Idade left, Idade right) => !Equals(left, right);
    }
}