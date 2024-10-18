using System.Diagnostics.CodeAnalysis;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace ControlAcceso.Tools.HttpContext
{
    [ExcludeFromCodeCoverage]
    public class HttpContext : ControllerBase, IHttpContext
    {
        public string GetIpAddress()
        {
            IPAddress? ipAddress = null;
            if (HttpContext?.Connection?.RemoteIpAddress != null)
                ipAddress = HttpContext.Connection.RemoteIpAddress;

            // Verificar si la IP es IPv4 o IPv6
            if (ipAddress != null && ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
            {
                // Si es IPv6, puedes convertirla a una cadena legible
                ipAddress = Dns.GetHostEntry(ipAddress).AddressList.First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
            }

            return ipAddress?.ToString() ?? "";
        }
    }
}