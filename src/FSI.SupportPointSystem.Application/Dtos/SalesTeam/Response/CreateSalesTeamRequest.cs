using System.ComponentModel.DataAnnotations;

namespace FSI.SupportPointSystem.Application.Dtos.SalesTeam.Response
{
    public class CreateSalesTeamRequest
    {
        [Required(ErrorMessage = "O nome do time é obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres")]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }
    }
}
