using System.Text.RegularExpressions;

namespace Painel.Investimento.Domain.Valueobjects
{
    public class Email
    {
        private static readonly Regex EmailRegex = new Regex(
            pattern: @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            options: RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public Email(string value)
        {
            if (!EmailRegex.IsMatch(input: value))
            {
                throw new ArgumentException(message: "E-mail inválido.");
            }

            Value = value;
        }

        public string Value { get; }
    }
}
