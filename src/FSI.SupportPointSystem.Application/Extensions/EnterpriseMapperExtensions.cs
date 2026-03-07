using FSI.SupportPoint.Application.Dtos.Enterprise;
using FSI.SupportPointSystem.Application.Dtos;
using FSI.SupportPointSystem.Domain.Entities;
using System.Linq;

namespace FSI.SupportPointSystem.Application.Extensions
{
    public static class EnterpriseMapperExtensions
    {
        public static EnterpriseDto ToDto(this Enterprise entity)
        {
            if (entity == null) return null;

            return new EnterpriseDto
            {
                Cnpj = entity.Cnpj,
                RazaoSocial = entity.RazaoSocial,
                NomeFantasia = entity.NomeFantasia,
                SituacaoCadastral = entity.SituacaoCadastral,
                DataInicioAtividade = entity.DataInicioAtividade,
                NaturezaJuridica = entity.NaturezaJuridica,
                Logradouro = entity.Logradouro,
                Numero = entity.Numero,
                Bairro = entity.Bairro,
                Cep = entity.Cep,
                Municipio = entity.Municipio,
                Uf = entity.Uf,
                Email = entity.Email,
                CapitalSocial = entity.CapitalSocial,
                PorteEmpresa = entity.PorteEmpresa,
                CnaesSecundarios = entity.CnaesSecundarios,

                // Mapeando a lista de telefones
                Telefones = entity.Telefones?.Select(t => new TelephoneDto
                {
                    Ddd = t.Ddd,
                    Numero = t.Numero,
                    IsFax = t.IsFax
                }).ToList(),

                // Mapeando a lista de sócios (QSA)
                Qsa = entity.Qsa?.Select(s => new PartnerDto
                {
                    NomeSocio = s.NomeSocio,
                    QualificacaoSocio = s.QualificacaoSocio,
                    FaixaEtaria = s.FaixaEtaria
                }).ToList()
            };
        }
    }
}