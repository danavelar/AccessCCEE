using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ApiServiceCCEE.Services;

namespace ApiServiceCCEE.Controllers
{
    public class ListMeasurementController : ApiController
    {
        private MedicaoPlataformaCCEE medicaoPlataformaCCEE;

        public ListMeasurementController()
        {
            this.medicaoPlataformaCCEE = new MedicaoPlataformaCCEE();
        }

        // GET api/listarMedicao
        public IEnumerable<object> Get()
        {
            var listaMedidas = medicaoPlataformaCCEE.ListarMedicaoCCEE(HttpContext.Current.Server.MapPath("/Services"));

            return listaMedidas.medidas;
        }
    }
}
