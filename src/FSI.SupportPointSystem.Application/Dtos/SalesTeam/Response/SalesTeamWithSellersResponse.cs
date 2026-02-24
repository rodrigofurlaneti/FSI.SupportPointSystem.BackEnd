using FSI.SupportPointSystem.Application.Dtos.Seller.Response;
using FSI.SupportPointSystem.Application.Dtos.SalesTeam.Response;
using System.Collections.Generic;

namespace FSI.SupportPointSystem.Application.Dtos.SalesTeam.Response
{
    public class SalesTeamWithSellersResponse : SalesTeamResponse
    {
        public IEnumerable<SellerResponse> Sellers { get; set; } = new List<SellerResponse>();
    }
}