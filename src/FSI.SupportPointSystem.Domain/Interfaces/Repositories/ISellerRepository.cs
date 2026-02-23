namespace FSI.SupportPointSystem.Domain.Interfaces.Repository
{
    public interface ISellerRepository
    {
        Task CreateAsync(string cpf, string passwordHash, string name, string phone);
        Task<Seller?> GetByIdAsync(Guid id);
    }
}