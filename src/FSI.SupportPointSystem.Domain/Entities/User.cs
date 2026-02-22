using System;
using FSI.SupportPoint.Domain.Exceptions;
using FSI.SupportPoint.Domain.Entities;
using FSI.SupportPoint.Domain.ValueObjects;
namespace FSI.SupportPoint.Domain.Entities

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