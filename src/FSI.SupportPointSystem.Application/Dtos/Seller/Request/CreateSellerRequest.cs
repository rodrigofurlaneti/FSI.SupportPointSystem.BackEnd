namespace FSI.SupportPointSystem.Application.Dtos.Seller.Request
{
    public record CreateSellerRequest(
        string Name,
        string Cpf,
        string Email,
        string Password,
        string Phone
    );
}

