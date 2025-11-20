using System.Text.RegularExpressions;

namespace Painel.Investimento.Domain.Valueobjects
{
    public class Celular
    {
        public string Numero { get; private set; }

        public Celular(string numero)
        {
            Numero = numero;
        }

        public static Celular Criar(string numero)
        {
            if (string.IsNullOrWhiteSpace(numero))
                throw new ArgumentException("Número de celular não pode ser vazio.");

            // Remove caracteres não numéricos
            var apenasNumeros = Regex.Replace(numero, "[^0-9]", "");

            // Validação básica: DDD + número (Brasil: 11 dígitos)
            if (apenasNumeros.Length != 11)
                throw new ArgumentException("Número de celular deve conter 11 dígitos (incluindo DDD).");

            if (!ValidarCelular(apenasNumeros))
                throw new ArgumentException("Número de celular inválido.");

            return new Celular(apenasNumeros);
        }

        private static bool ValidarCelular(string numero)
        {
            // Regra simples: começa com DDD válido e dígito 9 para celular
            var regex = new Regex(@"^[1-9]{2}9[0-9]{8}$");
            return regex.IsMatch(numero);
        }

        public override string ToString()
        {
            // Formato: (XX) 9XXXX-XXXX
            return $"({Numero.Substring(0, 2)}) {Numero.Substring(2, 1)}{Numero.Substring(3, 4)}-{Numero.Substring(7, 4)}";
        }
    }
}
