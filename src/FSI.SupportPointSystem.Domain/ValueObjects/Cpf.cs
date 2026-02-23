using FSI.SupportPointSystem.Domain.Exceptions;
namespace FSI.SupportPointSystem.Domain.ValueObjects
{
    public record Cpf
    {
        public string Value { get; }

        public Cpf(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length != 11)
                throw new DomainException("CPF inválido. Deve conter exatamente 11 dígitos numéricos.");
            Value = value;
        }
        public override string ToString() => Value;
    }
}