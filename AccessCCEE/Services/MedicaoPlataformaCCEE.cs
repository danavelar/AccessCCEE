using System;
using System.ServiceModel;
using AccessCCEE.wsListarMedida;

namespace AccessCCEE.Services
{
    public class MedicaoPlataformaCCEE
    {
        // tipoMedicao: C=Completa, F=Faltante
        public void ListarMedicaoCCEEAsync()
        {
            ParametrosMedicaoCCEE param = MontarParametrosMedicao(1, "1", DateTime.Now, DateTime.Now);
            MessageHeaderType hd = new MessageHeaderType();
            try
            {
                BasicHttpBinding binding = ConectarCCEE.GetBindingCCEE(param);
                binding.MaxReceivedMessageSize = 327680;
                // criar proxy para listar medições (proxy);
                ListarMedidaBSv1PortTypeClient client = new ListarMedidaBSv1PortTypeClient(binding, ConectarCCEE.GetEndPointAdress(param.URLService));
                
                // setar os certificados no proxy
                var certificates = ConectarCCEE.GetCertificadosCCEE(param);
                client.ClientCredentials.ClientCertificate.Certificate = certificates[0];
                client.ClientCredentials.ServiceCertificate.DefaultCertificate = certificates[1];

                hd.codigoPerfilAgente = param.CodigoPerfilAgente;

                SecurityHeaderType sh = new SecurityHeaderType();
                UsernameTokenType tp = new UsernameTokenType();
                tp.Username = param.UsuarioAcesso;
                tp.Password = param.SenhaAcesso;
                sh.UsernameToken = tp;

                // objeto a ser enviado;
                ListarMedidaRequest req = new ListarMedidaRequest();
                // para solicitar medição faltante;
                if (param.TipoMedicao == "F")
                {
                    Medidor med = new Medidor();
                    med.codigo = param.CodigoMedidorCCEE;
                    req.medidor = med;
                    req.tipoMedida = TipoMedida.FALTANTES;
                    req.tipoMedidaSpecified = true;
                }

                // para solicitar medição consolidada;
                if (param.TipoMedicao == "C")
                {
                    req.tipoMedida = TipoMedida.FINAL;
                    req.tipoMedidaSpecified = true;

                    //                    req.tipoMedicao = wsccee.TipoMedicao.COLETA;
                    //                    req.tipoMedicaoSpecified = true;

                    PontoMedicao pmed = new PontoMedicao();
                    pmed.codigo = param.CodigoMedidorCCEE;
                    req.pontoMedicao = pmed;
                }

                // DEFINIÇÃO DO PERIODO PARA PESQUISAR OS DADOS;
                Periodo per = new Periodo();
                per.inicio = param.PeriodoDataInicial;
                per.inicioSpecified = true;
                per.fim = param.PeriodoDataFinal;
                per.fimSpecified = true;
                req.periodo = per;


                var res = client.listarMedida(sh, ref hd, req);
                if (res != null)
                {

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Montar os parâmetros necessários para acessar a palataforma da CCEE;
        private static ParametrosMedicaoCCEE MontarParametrosMedicao(int CodigoEmpresa, string perfilAgente, DateTime dataInicial, DateTime dataFinal)
        {
            ParametrosMedicaoCCEE parm = new ParametrosMedicaoCCEE();

            if (CodigoEmpresa == 1)
            {
                // Dados para Delta
                parm.CertCliente = @"chaveEnviadaCCEE.p12";
                parm.CertClientePwd = "1234";
                parm.UsuarioAcesso = "DELTA_ENERGIA";
                parm.SenhaAcesso = "49817018";
            }

            if (CodigoEmpresa == 2)
            {
                // Dados para Thymos
                //TERMINAR
                //parm.CertCliente = @"chaveEnviadaCCEE.p12";
                //parm.CertClientePwd = ;
                //parm.UsuarioAcesso = ;
                //parm.SenhaAcesso = ;
            }

            parm.CertServer = @"servicoscceeorgbr.crt";
            parm.CodigoPerfilAgente = perfilAgente;
            parm.BindingName = "ListarMedidaBSv1SOAPBinding";
            parm.URLService = @"https://servicos.ccee.org.br:443/ws/medc/ListarMedidaBSv1";
            parm.PeriodoDataInicial = dataInicial;

            // sempre definir como data final o horario de 23:59:59
            parm.PeriodoDataFinal = dataFinal;
            parm.PeriodoDataFinal = parm.PeriodoDataFinal.AddHours(23);
            parm.PeriodoDataFinal = parm.PeriodoDataFinal.AddMinutes(59);
            parm.PeriodoDataFinal = parm.PeriodoDataFinal.AddSeconds(59);

            return parm;
        }
    }
}