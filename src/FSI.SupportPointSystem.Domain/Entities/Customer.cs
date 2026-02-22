using System;
using FSI.SupportPoint.Domain.Exceptions;
using FSI.SupportPoint.Domain.Entities;
using FSI.SupportPoint.Domain.ValueObjects;
namespace FSI.SupportPoint.Domain.Entities
{
    public class Customer
    {
        public Guid Id { get; private set; }
        public string CompanyName { get; private set; }
        public string Cnpj { get; private set; }
        public Coordinates LocationTarget { get; private set; }
        public Customer(string companyName, string cnpj, Coordinates locationTarget)
        {
            if (string.IsNullOrWhiteSpace(cnpj))
                throw new DomainException("CNPJ é obrigatório.");

            Id = Guid.NewGuid();
            CompanyName = companyName;
            Cnpj = cnpj;
            LocationTarget = locationTarget;
        }
        public void UpdateLocation(Coordinates newLocation)
        {
            LocationTarget = newLocation;
        }
    }
}