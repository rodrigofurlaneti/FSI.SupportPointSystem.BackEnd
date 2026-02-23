using FSI.SupportPointSystem.Domain.Exceptions;

namespace FSI.SupportPointSystem.Domain.ValueObjects
{
    public record Cnpj
    {
        public string Value { get; }

        public Cnpj(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length != 14)
                throw new DomainException("O CNPJ deve conter exatamente 14 dígitos.");

            Value = value;
        }

        public override string ToString() => Value;
    }
}