using System;

namespace FSI.SupportPointSystem.Application.Dtos.Seller.Response
{
    public record SellerResponse(
        Guid Id,
        string Name,
        string Cpf,
        string Email,
        string Phone
    );
}