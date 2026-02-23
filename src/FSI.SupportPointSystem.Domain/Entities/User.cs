using System;
using FSI.SupportPointSystem.Domain.Exceptions;

namespace FSI.SupportPointSystem.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Cpf { get; private set; }
        public string PasswordHash { get; private set; }
        public string Role { get; private set; }
        public string Name { get; private set; }
        public Seller? Seller { get; private set; }
        private User() { }
        public User(string cpf, string passwordHash, string role)
        {
            if (string.IsNullOrWhiteSpace(cpf) || cpf.Length != 11)
                throw new DomainException("CPF deve conter 11 dígitos.");

            Id = Guid.NewGuid();
            Cpf = cpf;
            PasswordHash = passwordHash;
            Role = role;
            Name = string.Empty; // Inicia vazio
        }
        public User(Guid id, string cpf, string passwordHash, string role, string name, Seller? seller = null)
        {
            Id = id;
            Cpf = cpf;
            PasswordHash = passwordHash;
            Role = role;
            Name = name;
            Seller = seller;
        }
    }
}