namespace FSI.SupportPointSystem.Domain.ValueObjects
{
    public record Cpf
    {
        public string Value { get; }
        public Cpf(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length != 11)
                throw new DomainException("CPF inválido.");
            Value = value;
        }
    }
}

