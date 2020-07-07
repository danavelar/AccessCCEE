using System;

namespace AccessCCEE.Services
{
    public class ParametrosMedicaoCCEE : ParametrosConexaoCCEE
    {
        // TipoMedicao: F = Faltantes; C=Consolidada;
        public string TipoMedicao { get; set; }
        // Medidor;
        public string CodigoMedidorCCEE { get; set; }
        // Código do perfil do agente que vou buscar os dados de medição;
        public string CodigoPerfilAgente { get; set; }
        // data inicial;
        public DateTime PeriodoDataInicial { get; set; }
        // data final;
        public DateTime PeriodoDataFinal { get; set; }
    }
}