using FSI.SupportPointSystem.Application.Dtos.Checkin.Request;
using FSI.SupportPointSystem.Application.Dtos.Checkout.Request;
using FSI.SupportPointSystem.Application.Dtos.Visit.Response;
using FSI.SupportPointSystem.Application;
namespace FSI.SupportPointSystem.Application.Interfaces
{
    public interface IVisitAppService
    {
        Task<VisitResponse> RegisterCheckinAsync(CheckinRequest request);
        Task<VisitResponse> RegisterCheckoutAsync(CheckoutRequest request);
    }
}