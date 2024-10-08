﻿using System.Data;
using ControlAcceso.Data.Roles;
using Microsoft.AspNetCore.Mvc;

namespace ControlAcceso.Endpoints.Roles
{
    [ApiController]
    [Route("roles")]
    public class Endpoint : ControllerBase
    {
        private IRolesDbContext? _roles { get; }
        
        public Endpoint(IRolesDbContext? roles)
        {
            _roles = roles;
        }

        [HttpPost("create")]
        public IActionResult CreateRole([FromBody] Request request)
        {
            try
            {
                _roles?.InsertRole(new()
                {
                    Name = request.Name,
                });
                return Ok(new Response { Message = "OK" });
            }
            catch (DataException e)
            {
                return BadRequest(new Response { Message = e.Message });
            }
        }
    }
}