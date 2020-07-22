using System;
using System.ServiceModel;
using wsccee = AccessCCEE.wsListarMed;

namespace ApiServiceCCEE.Services
{
    public class MedicaoPlataformaCCEE
    {
        // tipoMedicao: C=Completa, F=Faltante
        public wsccee.ListarMedidaResponse ListarMedicaoCCEE(string path)
        {
            ParametrosMedicaoCCEE param = MontarParametrosMedicao(1, "18994", DateTime.Parse("01/05/2020"), DateTime.Parse("03/05/2020"), path);
            param.TipoMedicao = "C";
            param.CodigoMedidorCCEE = "SPAVN-URPAR03";
            wsccee.ListarMedidaResponse resposta;

            wsccee.MessageHeaderType hd = new wsccee.MessageHeaderType();
            try
            {
                BasicHttpBinding binding = ConectarCCEE.GetBindingCCEE(param);
                binding.MaxReceivedMessageSize = 327680;
                // criar proxy para listar medições (proxy);
                wsccee.ListarMedidaBSv1PortTypeClient client = new wsccee.ListarMedidaBSv1PortTypeClient(binding, ConectarCCEE.GetEndPointAdress(param.URLService));
                
                // setar os certificados no proxy
                var certificates = ConectarCCEE.GetCertificadosCCEE(param);
                client.ClientCredentials.ClientCertificate.Certificate = certificates[0];
                client.ClientCredentials.ServiceCertificate.DefaultCertificate = certificates[1];

                hd.codigoPerfilAgente = param.CodigoPerfilAgente;

                wsccee.SecurityHeaderType sh = new wsccee.SecurityHeaderType();
                wsccee.UsernameTokenType tp = new wsccee.UsernameTokenType();
                tp.Username = param.UsuarioAcesso;
                tp.Password = param.SenhaAcesso;
                sh.UsernameToken = tp;

                // objeto a ser enviado;
                wsccee.ListarMedidaRequest req = new wsccee.ListarMedidaRequest();
                // para solicitar medição faltante;
                if (param.TipoMedicao == "F")
                {
                    wsccee.Medidor med = new wsccee.Medidor();
                    med.codigo = param.CodigoMedidorCCEE;
                    req.medidor = med;
                    req.tipoMedida = wsccee.TipoMedida.FALTANTES;
                    req.tipoMedidaSpecified = true;
                }

                // para solicitar medição consolidada;
                if (param.TipoMedicao == "C")
                {
                    req.tipoMedida = wsccee.TipoMedida.FINAL;
                    req.tipoMedidaSpecified = true;

                    //                    req.tipoMedicao = wsccee.TipoMedicao.COLETA;
                    //                    req.tipoMedicaoSpecified = true;

                    wsccee.PontoMedicao pmed = new wsccee.PontoMedicao();
                    pmed.codigo = param.CodigoMedidorCCEE;
                    req.pontoMedicao = pmed;
                }

                // DEFINIÇÃO DO PERIODO PARA PESQUISAR OS DADOS;
                wsccee.Periodo per = new wsccee.Periodo();
                per.inicio = param.PeriodoDataInicial;
                per.inicioSpecified = true;
                per.fim = param.PeriodoDataFinal;
                per.fimSpecified = true;
                req.periodo = per;


                resposta = client.listarMedida(sh, ref hd, req);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return resposta;
        }

        // Montar os parâmetros necessários para acessar a palataforma da CCEE;
        private static ParametrosMedicaoCCEE MontarParametrosMedicao(int CodigoEmpresa, string perfilAgente, DateTime dataInicial, DateTime dataFinal, string path)
        {
            ParametrosMedicaoCCEE parm = new ParametrosMedicaoCCEE();

            if (CodigoEmpresa == 1)
            {
                // Dados para Delta
                parm.CertCliente = path + "\\ChaveEnviadaParaCCEE.p12";
                parm.CertClientePwd = "Delta@123";
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

            parm.CertServer = path + "\\servicoscceeorgbr.crt";
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