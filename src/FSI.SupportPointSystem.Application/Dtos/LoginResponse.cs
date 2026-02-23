namespace FSI.SupportPointSystem.Application.Dtos
{
    public record LoginResponse(
        string Token,
        string UserName,
        string Role,
        Guid UserId,
        Guid? SellerId // Retornamos o SellerId para o App usar no Check-in
    );
}