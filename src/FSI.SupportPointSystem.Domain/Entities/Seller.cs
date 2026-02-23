namespace FSI.SupportPointSystem.Domain.Entities
{
    public class Seller
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public Cpf Cpf { get; private set; }
        public Password Password { get; private set; }
        public string Email { get; private set; }

        // Construtor de Negócio
        public Seller(string name, Cpf cpf, string email, Password password)
        {
            Id = Guid.NewGuid();
            Name = name;
            Cpf = cpf;
            Email = email;
            Password = password;
        }
    }
}

