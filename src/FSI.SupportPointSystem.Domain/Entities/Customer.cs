using System;
using FSI.SupportPointSystem.Domain.Exceptions;
using FSI.SupportPointSystem.Domain.ValueObjects;

namespace FSI.SupportPointSystem.Domain.Entities
{
    public class Customer
    {
        public Guid Id { get; private set; }
        public string CompanyName { get; private set; }
        public string Cnpj { get; private set; }
        public Coordinates LocationTarget { get; private set; }
        public Customer(string companyName, string cnpj, Coordinates locationTarget)
        {
            if (string.IsNullOrWhiteSpace(companyName))
                throw new DomainException("O nome da empresa (CompanyName) não pode ser vazio.");
            if (string.IsNullOrWhiteSpace(cnpj))
                throw new DomainException("O CNPJ é obrigatório.");
            if (cnpj.Length != 14)
                throw new DomainException("O CNPJ deve conter exatamente 14 dígitos.");
            LocationTarget = locationTarget ?? throw new DomainException("A localização alvo é obrigatória.");
            Id = Guid.NewGuid();
            CompanyName = companyName;
            Cnpj = cnpj;
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