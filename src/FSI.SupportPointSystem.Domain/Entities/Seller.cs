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
        public Guid? SalesTeamId { get; private set; }
        public virtual SalesTeam SalesTeam { get; private set; }
        public bool Active { get; private set; }
        protected Seller() { }
        public Seller(Guid id, string name, Cpf cpf, string email, string phone, Guid? salesTeamId = null)
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            Name = name;
            Cpf = cpf;
            Email = email;
            Phone = phone;
            SalesTeamId = salesTeamId;
            Active = true;
        }
        public void UpdateDetails(string name, string email, string phone, bool active)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("O nome não pode ser vazio.");
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("O e-mail não pode ser vazio.");
            Name = name;
            Email = email;
            Phone = phone;
            Active = active;
        }
        public void ChangeTeam(Guid newTeamId)
        {
            if (newTeamId == Guid.Empty)
                throw new ArgumentException("ID do time inválido.");

            SalesTeamId = newTeamId;
        }
        public void Deactivate() => Active = false;
        public void Activate() => Active = true;
    }
}