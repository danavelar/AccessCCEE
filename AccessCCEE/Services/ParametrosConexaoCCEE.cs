namespace AccessCCEE.Services
{
    public class ParametrosConexaoCCEE
    {
        public ParametrosConexaoCCEE()
        {
            MessageHeaderNamespace = @"http://xmlns.energia.org.br/MH/v1";
        }
        // Url do serviço;
        public string URLService { get; set; }
        // nome do mettodo binding no web-service;
        // exemplo: ListarMedidaBSv1SOAPBinding
        public string BindingName { get; set; }
        // endereço do messageheader para acesso ao webservice;
        public string MessageHeaderNamespace { get; protected set; }
        // endereço e arquivo do certificado cliente;
        public string CertCliente { get; set; }
        // senha do certificado do cliente;
        public string CertClientePwd { get; set; }
        // endereço e arquivo do certificado do servidor;
        public string CertServer { get; set; }
        // senha do certificado do servidor;
        public string CertServerPwd { get; set; }
        // Usuário de acesso;
        public string UsuarioAcesso { get; set; }
        // Senha Acesso;
        public string SenhaAcesso { get; set; }
    }
}