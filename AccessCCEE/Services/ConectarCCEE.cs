using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Web.Http;

namespace AccessCCEE.Services
{
    public class ConectarCCEE
    {
        // bindingName = nome do binding definido no arquivo de configuração (EX: PingServiceBinding);
        // messageHeaderNamespace = dado do namespace remoto a ser definido no header da mensagem;
        public static BasicHttpBinding GetBindingCCEE(ParametrosConexaoCCEE param)
        {
            try
            {
                X509Certificate2Collection certificates = GetCertificadosCCEE(param);
                if (certificates == null)
                    throw new Exception("ERRO NA PREPARAÇÃO DOS CERTIFICADOS");

                // criar conexão ssl e meio de transporte (https)
                BasicHttpBinding binding = new BasicHttpBinding();
                binding.Name = param.BindingName;
                binding.Security.Mode = BasicHttpSecurityMode.Transport;
                binding.SendTimeout = TimeSpan.FromSeconds(120);
                binding.Namespace = param.MessageHeaderNamespace;
                binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;
                binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;

                return binding;
            }
            catch
            {
                return null;
            }
        }

        // prepara os certificados a serem utilizados na conexão com a CCEE:
        // define local, nome e senha onde ficam os certificados para serem enviados;
        public static X509Certificate2Collection GetCertificadosCCEE(ParametrosConexaoCCEE param)
        {
            try
            {
                var clientCertificate = param.CertCliente;
                var clientCertificatePassword = param.CertClientePwd;
                var serverCertificate = param.CertServer;
                var serverCertificatePassword = param.CertServerPwd;

                var filePath = @"C:\Users\davelar\source\repos\AccessCCEE\AccessCCEE\Services\ChaveEnviadaCCEE.p12";
                var serverCertificatePath = @"C:\Users\davelar\source\repos\AccessCCEE\AccessCCEE\Services\servicoscceeorgbr.crt";

                // montar os certificados;
                var certificates = new X509Certificate2Collection(new X509Certificate2(filePath, clientCertificatePassword, X509KeyStorageFlags.PersistKeySet));
                certificates.Add(new X509Certificate2(serverCertificatePath, serverCertificatePassword, X509KeyStorageFlags.PersistKeySet));
                return certificates;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // retorna o EndPointAdress definido pela CCEE;
        public static EndpointAddress GetEndPointAdress(string url)
        {
            EndpointAddress adress = new EndpointAddress(url);
            return adress;
        }
    }
}