using System.ComponentModel.DataAnnotations;

namespace FSI.SupportPointSystem.Application.Dtos.Login.Request
{
    public record LoginRequest(
        [Required(ErrorMessage = "O CPF é obrigatório")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "O CPF deve ter 11 dígitos")]
        string Cpf,

        [Required(ErrorMessage = "A senha é obrigatória")]
        string Password
    );
}