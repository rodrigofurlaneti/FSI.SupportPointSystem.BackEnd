using System;
using FSI.SupportPoint.Domain.Exceptions;
using FSI.SupportPoint.Domain.Entities;
using FSI.SupportPoint.Domain.ValueObjects;
namespace FSI.SupportPoint.Domain.Entities

{
    public class Seller
	{
		public Guid Id { get; private set; }
		public Guid UserId { get; private set; }
		public string Name { get; private set; }
		public string Phone { get; private set; }
		public bool Active { get; private set; }

		public Seller(Guid userId, string name, string phone)
		{
			Id = Guid.NewGuid();
			UserId = userId;
			Name = name;
			Phone = phone;
			Active = true;
		}

		public void Deactivate() => Active = false;
	}
}

