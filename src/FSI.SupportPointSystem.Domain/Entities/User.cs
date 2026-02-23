using System;
using FSI.SupportPointSystem.Domain.Exceptions;
using FSI.SupportPointSystem.Domain.Entities;
using FSI.SupportPointSystem.Domain.ValueObjects;
namespace FSI.SupportPointSystem.Domain.Entities

{
    public class User
    {
        public Guid Id { get; private set; }
        public string Cpf { get; private set; }
        public string PasswordHash { get; private set; }
        public string Role { get; private set; } // "ADMIN" ou "SELLER"

        public User(string cpf, string passwordHash, string role)
        {
            if (string.IsNullOrWhiteSpace(cpf) || cpf.Length != 11)
                throw new DomainException("CPF deve conter 11 dígitos.");

            Id = Guid.NewGuid();
            Cpf = cpf;
            PasswordHash = passwordHash;
            Role = role;
        }
    }
}