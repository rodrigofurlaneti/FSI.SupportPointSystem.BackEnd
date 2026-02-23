namespace FSI.SupportPoint.Application.Dtos.Seller.Request
{
    public record UpdateSellerRequest(
            string Name,
            string Email,
            string Phone
        );
}
