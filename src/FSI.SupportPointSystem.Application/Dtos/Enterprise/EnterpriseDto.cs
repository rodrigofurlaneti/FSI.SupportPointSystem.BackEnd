namespace FSI.SupportPoint.Application.Dtos.Enterprise
{
    public class EnterpriseDto
    {
        public string Cnpj { get; set; }
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public string SituacaoCadastral { get; set; }
        public string DataInicioAtividade { get; set; }
        public string NaturezaJuridica { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public string Municipio { get; set; }
        public string Uf { get; set; }
        public string Email { get; set; }
        public string CapitalSocial { get; set; }
        public string PorteEmpresa { get; set; }

        // Listas simplificadas
        public List<string> CnaesSecundarios { get; set; }
        public List<TelephoneDto> Telefones { get; set; }
        public List<PartnerDto> Qsa { get; set; }
    }
}
