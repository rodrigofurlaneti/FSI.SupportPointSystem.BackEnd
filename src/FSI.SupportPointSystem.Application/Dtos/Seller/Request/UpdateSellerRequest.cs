namespace FSI.SupportPointSystem.Application.Dtos.Seller.Request
{
    public record UpdateSellerRequest(
        string Name,
        string Email,
        string Phone,
        bool IsActive
    );
}
