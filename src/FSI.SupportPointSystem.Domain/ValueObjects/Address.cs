using FSI.SupportPointSystem.Domain.Exceptions;

namespace FSI.SupportPointSystem.Domain.ValueObjects
{
    public class Address
    {
        public string ZipCode { get; private set; }
        public string Street { get; private set; }
        public string Number { get; private set; }
        public string Complement { get; private set; }
        public string Neighborhood { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }

        public Address(string zipCode, string street, string number, string neighborhood, string city, string state, string complement = "")
        {
            if (string.IsNullOrWhiteSpace(zipCode)) throw new DomainException("CEP é obrigatório.");
            if (string.IsNullOrWhiteSpace(street)) throw new DomainException("Logradouro é obrigatório.");
            if (string.IsNullOrWhiteSpace(city)) throw new DomainException("Cidade é obrigatória.");
            if (state?.Length != 2) throw new DomainException("Estado deve ter 2 caracteres.");

            ZipCode = zipCode;
            Street = street;
            Number = number;
            Complement = complement;
            Neighborhood = neighborhood;
            City = city;
            State = state.ToUpper();
        }
        public override string ToString() => $"{Street}, {Number} - {Neighborhood}, {City}/{State}";
    }
}