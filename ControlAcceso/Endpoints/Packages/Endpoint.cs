using ControlAcceso.Data.Packages;
using Microsoft.AspNetCore.Mvc;

namespace ControlAcceso.Endpoints.Packages
{
    [ApiController]
    [Route("packages")]
    public class Endpoint : ControllerBase
    {
        private IPackagesDbContext? _packages { get; }

        public Endpoint(IPackagesDbContext? packages)
        {
            _packages = packages;
        }

        [HttpGet]
        public IActionResult GetPackageList()
        {
            var packages = _packages?.SelectPackages();
            return Ok(new Response { Message = "OK", Packages = packages });
        }
    }
}
