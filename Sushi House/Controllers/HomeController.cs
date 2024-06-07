using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sushi_House.DTOModels;
using Sushi_House.Models;
using Sushi_House.Services;

namespace Sushi_House.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ISushiService _sushiService;
        public HomeController(ISushiService sushiService)
        {
            _sushiService = sushiService;
        }

        [HttpGet("sushi")]
        public IActionResult GetSushi()
        {
            var sushis = _sushiService.GetSushi();
            return Ok(sushis);
        }

        [HttpGet("Set")]
        public IActionResult GetSet()
        {
            var sets = _sushiService.GetSet();
            return Ok(sets);
        }

        [HttpGet("Type")]
        public IActionResult GetSType()
        {
            var types = _sushiService.GetSType();
            return Ok(types);
        }

        [HttpPost("sushi")]
        [Authorize(Policy = "UserStatusLimit")]
        public IActionResult PostSushi([FromBody] SushiDTO sushiDto, [FromForm] IFormFile photo, [FromServices] IWebHostEnvironment env)
        {
            _sushiService.PostSushi(sushiDto, photo, env);
            return Ok(sushiDto);
        }

        [HttpPost("Set")]
        [Authorize(Policy = "UserStatusLimit")]
        public IActionResult PostSet([FromBody] SetDTO setDto, [FromForm] List<SushiDTO> sushiDtos, [FromForm] IFormFile photo, [FromServices] IWebHostEnvironment env)
        {
            _sushiService.PostSet(setDto, sushiDtos, photo, env);
            return Ok(setDto);
        }

        [HttpDelete("Sushi")]
        [Authorize(Policy = "UserStatusLimit")]
        public IActionResult DeleteSushi(int id)
        {
            _sushiService.DeleteSushi(id);
            return Ok("Sushi deleted successfully");
        }

        [HttpDelete("Set")]
        [Authorize(Policy = "UserStatusLimit")]
        public IActionResult DeleteSet(int id)
        {
            _sushiService.DeleteSet(id);
            return Ok("Set deleted successfully");
        }

        [HttpPut("Sushi")]
        [Authorize(Policy = "UserStatusLimit")]
        public IActionResult PutSushi(int id, [FromBody] SushiDTO sushiDto, [FromForm] IFormFile photo, [FromServices] IWebHostEnvironment env)
        {
            _sushiService.PutSushi(id, sushiDto, photo, env);
            return Ok("User updated successfully");
        }

        [HttpPut("Set")]
        [Authorize(Policy = "UserStatusLimit")]
        public IActionResult PutSet(int id, [FromBody] SetDTO setDto, [FromForm] List<SushiDTO> sushiDtos, [FromForm] IFormFile ph, [FromServices] IWebHostEnvironment env)
        {
            _sushiService.PutSet(id, setDto, sushiDtos, ph, env);
            return Ok("User updated successfully");
        }
    }
}
