using System.Collections.Generic;
using ControlAcceso.Data.Model;

namespace ControlAcceso.Endpoints.Packages
{
    public class Response
    {
        public string Message { get; set; }
        public List<PackageModel> Packages { get; set; }
    }
}
