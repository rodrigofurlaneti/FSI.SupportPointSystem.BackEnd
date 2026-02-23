using FSI.SupportPoint.Application.Dtos.Checkin.Request;
using FSI.SupportPoint.Application.Dtos.Checkout.Request;
using FSI.SupportPoint.Application.Dtos.Visit.Response;
using FSI.SupportPointSystem.Application;
namespace FSI.SupportPointSystem.Application.Interfaces
{
    public interface IVisitAppService
    {
        Task<VisitResponse> RegisterCheckinAsync(CheckinRequest request);
        Task<VisitResponse> RegisterCheckoutAsync(CheckoutRequest request);
    }
}