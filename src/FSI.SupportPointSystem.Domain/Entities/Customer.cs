using System;
using FSI.SupportPointSystem.Domain.Exceptions;
using FSI.SupportPointSystem.Domain.ValueObjects;
namespace FSI.SupportPointSystem.Domain.Entities
{
    public class Customer
    {
        public Guid Id { get; private set; }
        public string CompanyName { get; private set; }
        public Cnpj Cnpj { get; private set; } 
        public Coordinates LocationTarget { get; private set; }
        public Customer(Guid id, string companyName, Cnpj cnpj, Coordinates locationTarget)
        {
            if (id == Guid.Empty) throw new DomainException("ID inválido.");
            if (string.IsNullOrWhiteSpace(companyName)) throw new DomainException("Nome inválido.");
            Id = id;
            CompanyName = companyName;
            Cnpj = cnpj ?? throw new DomainException("CNPJ obrigatório.");
            LocationTarget = locationTarget ?? throw new DomainException("Localização obrigatória.");
        }
        public void UpdateLocation(Coordinates newLocation)
        {
            LocationTarget = newLocation ?? throw new DomainException("A nova localização não pode ser nula.");
        }
        public void UpdateCompanyName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new DomainException("O novo nome da empresa não pode ser vazio.");
            CompanyName = newName;
        }
    }
}