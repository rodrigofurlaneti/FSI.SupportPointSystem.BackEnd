using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FSI.SupportPointSystem.Domain.Entities
{
    public class Partner
    {
        [JsonPropertyName("nome_socio")]
        public string NomeSocio { get; set; }

        [JsonPropertyName("cnpj_cpf_socio")]
        public string CnpjCpfSocio { get; set; }

        [JsonPropertyName("qualificacao_socio")]
        public string QualificacaoSocio { get; set; }

        [JsonPropertyName("data_entrada_sociedade")]
        public string DataEntradaSociedade { get; set; }

        [JsonPropertyName("identificador_socio")]
        public string IdentificadorSocio { get; set; }

        [JsonPropertyName("faixa_etaria")]
        public string FaixaEtaria { get; set; }
    }
}
