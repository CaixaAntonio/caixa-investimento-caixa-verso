using System.Text.RegularExpressions;

namespace Painel.Investimento.Domain.Valueobjects
{
    public class Cpf
    {
        public string Numero { get; private set; }

        public Cpf(string numero)
        {
            Numero = numero;
        }

        public static Cpf Criar(string numero)
        {
            if (string.IsNullOrWhiteSpace(numero))
                throw new ArgumentException("CPF não pode ser vazio.");

            // Remove caracteres não numéricos
            var apenasNumeros = Regex.Replace(numero, "[^0-9]", "");

            if (apenasNumeros.Length != 11)
                throw new ArgumentException("CPF deve conter 11 dígitos.");

            if (!ValidarCpf(apenasNumeros))
                throw new ArgumentException("CPF inválido.");

            return new Cpf(apenasNumeros);
        }

        private static bool ValidarCpf(string cpf)
        {
            // Validação oficial do CPF
            if (cpf.All(c => c == cpf[0])) return false; // Todos dígitos iguais

            int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf.Substring(0, 9);
            var resto = 0;
            resto = resto < 2 ? 0 : 11 - resto;
            string digito = resto.ToString();

            tempCpf += digito;
            var soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;
            digito += resto.ToString();

            return cpf.EndsWith(digito);
        }

        public override string ToString() => Convert.ToUInt64(Numero).ToString(@"000\.000\.000\-00");
    }
}
