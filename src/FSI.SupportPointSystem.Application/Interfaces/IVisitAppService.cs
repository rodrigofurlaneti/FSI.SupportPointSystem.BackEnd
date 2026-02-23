using FSI.SupportPointSystem.Application;
using FSI.SupportPointSystem.Application.Dtos;
namespace FSI.SupportPointSystem.Application.Interfaces
{
    public interface IVisitAppService
    {
        Task<VisitResponse> RegisterCheckinAsync(CheckinRequest request);
        Task<VisitResponse> RegisterCheckoutAsync(CheckoutRequest request);
    }
}