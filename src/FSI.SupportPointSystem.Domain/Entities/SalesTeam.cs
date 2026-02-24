using System;
using System.Collections.Generic;

namespace FSI.SupportPointSystem.Domain.Entities
{
    public class SalesTeam
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;
        public string? Description { get; private set; }
        public bool Active { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public ICollection<Seller> Sellers { get; private set; } = new List<Seller>();
        protected SalesTeam() { }
        public SalesTeam(string name, string description) 
        {
            Id = Guid.NewGuid();
            SetName(name);
            SetDescription(description);
            Active = true;
            CreatedAt = DateTime.Now; 
            Sellers = new List<Seller>();
        }
        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("O nome do time é obrigatório.");
            Name = name;
        }
        public void SetDescription(string description)
        {
            Description = description;
        }
        public void Update(string name, string description, bool active)
        {
            SetName(name);
            SetDescription(description);
            Active = active;
        }
        public void Deactivate() => Active = false;
        public void Activate() => Active = true;
    }
}