using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AccessCCEE.Services;

namespace AccessCCEE.Controllers
{
    public class ValuesController : ApiController
    {
        private MedicaoPlataformaCCEE medicaoPlataformaCCEE;

        public ValuesController()
        {
            this.medicaoPlataformaCCEE = new MedicaoPlataformaCCEE();
        }

        // GET api/values
        public IEnumerable<string> Get()
        {
            medicaoPlataformaCCEE.ListarMedicaoCCEEAsync();
            return new string[] { "value1", "value2" };
        }
    }
}
