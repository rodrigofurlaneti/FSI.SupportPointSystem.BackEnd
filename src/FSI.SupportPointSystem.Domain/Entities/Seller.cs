using System;
using FSI.SupportPointSystem.Domain.ValueObjects;

namespace FSI.SupportPointSystem.Domain.Entities
{
    public class Seller
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public Cpf Cpf { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public Seller(Guid id, string name, Cpf cpf, string email, string phone)
        {
            Id = id;
            Name = name;
            Cpf = cpf;
            Email = email;
            Phone = phone;
        }
        public void UpdateDetails(string name, string email, string phone)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("O nome não pode ser vazio.");
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("O e-mail não pode ser vazio.");
            Name = name;
            Email = email;
            Phone = phone;
        }
    }
}