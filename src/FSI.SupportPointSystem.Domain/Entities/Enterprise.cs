using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FSI.SupportPointSystem.Domain.Entities
{
    public class Enterprise
    {
        [JsonPropertyName("cnpj")]
        public string Cnpj { get; set; }

        [JsonPropertyName("razao_social")]
        public string RazaoSocial { get; set; }

        [JsonPropertyName("nome_fantasia")]
        public string NomeFantasia { get; set; }

        [JsonPropertyName("situacao_cadastral")]
        public string SituacaoCadastral { get; set; }

        [JsonPropertyName("data_situacao_cadastral")]
        public string DataSituacaoCadastral { get; set; }

        [JsonPropertyName("matriz_filial")]
        public string MatrizFilial { get; set; }

        [JsonPropertyName("data_inicio_atividade")]
        public string DataInicioAtividade { get; set; }

        [JsonPropertyName("cnae_principal")]
        public string CnaePrincipal { get; set; }

        [JsonPropertyName("cnaes_secundarios")]
        public List<string> CnaesSecundarios { get; set; }

        [JsonPropertyName("natureza_juridica")]
        public string NaturezaJuridica { get; set; }

        [JsonPropertyName("logradouro")]
        public string Logradouro { get; set; }

        [JsonPropertyName("numero")]
        public string Numero { get; set; }

        [JsonPropertyName("complemento")]
        public string Complemento { get; set; }

        [JsonPropertyName("bairro")]
        public string Bairro { get; set; }

        [JsonPropertyName("cep")]
        public string Cep { get; set; }

        [JsonPropertyName("uf")]
        public string Uf { get; set; }

        [JsonPropertyName("municipio")]
        public string Municipio { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("telefones")]
        public List<Telephone> Telefones { get; set; }

        [JsonPropertyName("capital_social")]
        public string CapitalSocial { get; set; }

        [JsonPropertyName("porte_empresa")]
        public string PorteEmpresa { get; set; }

        [JsonPropertyName("opcao_simples")]
        public string OpcaoSimples { get; set; }

        [JsonPropertyName("data_opcao_simples")]
        public string DataOpcaoSimples { get; set; }

        [JsonPropertyName("opcao_mei")]
        public string OpcaoMei { get; set; }

        [JsonPropertyName("data_opcao_mei")]
        public string DataOpcaoMei { get; set; }

        [JsonPropertyName("QSA")]
        public List<Partner> Qsa { get; set; }
    }
}
