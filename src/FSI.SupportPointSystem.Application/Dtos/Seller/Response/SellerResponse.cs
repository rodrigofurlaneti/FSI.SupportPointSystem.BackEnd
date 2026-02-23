using System;

namespace FSI.SupportPoint.Application.Dtos.Seller.Response
{
    public record SellerResponse(
        Guid Id,
        string Name,
        string Cpf,
        string Email,
        string Phone
    );
}