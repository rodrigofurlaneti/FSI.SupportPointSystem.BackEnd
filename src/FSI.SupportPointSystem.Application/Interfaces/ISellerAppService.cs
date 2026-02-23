using FSI.SupportPoint.Application.Dtos.Seller.Request;
using FSI.SupportPoint.Application.Dtos.Seller.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FSI.SupportPointSystem.Application.Interfaces
{
    public interface ISellerAppService
    {
        Task RegisterSellerAsync(CreateSellerRequest request);
        Task<IEnumerable<SellerResponse>> GetAllAsync();
        Task<SellerResponse?> GetByIdAsync(Guid id);
        Task UpdateSellerAsync(Guid id, UpdateSellerRequest request);
        Task DeleteSellerAsync(Guid id);
    }
}