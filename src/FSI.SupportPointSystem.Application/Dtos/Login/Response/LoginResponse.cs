namespace FSI.SupportPoint.Application.Dtos.Login.Response
{
    public record LoginResponse(
        string Token,
        string UserName,
        string Role,
        Guid UserId,
        Guid? SellerId 
    );
}