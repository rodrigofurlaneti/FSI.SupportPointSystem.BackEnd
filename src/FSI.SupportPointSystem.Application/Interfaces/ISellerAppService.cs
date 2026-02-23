using FSI.SupportPointSystem.Application.Dtos;

namespace FSI.SupportPointSystem.Application.Interfaces
{
    public interface ISellerAppService
    {
        Task RegisterSellerAsync(CreateSellerRequest request);
    }
}

