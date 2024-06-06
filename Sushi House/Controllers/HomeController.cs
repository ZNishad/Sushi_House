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

        // Remove try cath all over the application
        // 1. Create midleware for custom exception handling
        // 2. Remove all Protocoles instead off structor with versioning

        [HttpGet("sushi")]
        public IActionResult GetSushi()
        {
            try
            {
                var sushis = _sushiService.GetSushi();
                return Ok(sushis);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("Set")]
        public IActionResult GetSet()
        {
            try
            {
                var sets = _sushiService.GetSet();
                return Ok(sets);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("Type")]
        public IActionResult GetSType()
        {
            try
            {
                var types = _sushiService.GetSType();
                return Ok(types);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("sushi")]
        [Authorize(Policy = "UserStatusLimit")]
        public IActionResult PostSushi([FromBody] SushiDTO sushiDto,[FromForm] IFormFile photo, [FromServices] IWebHostEnvironment env)
        {
            try
            {
                _sushiService.PostSushi(sushiDto, photo, env);
                return Ok(sushiDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("Set")]
        [Authorize(Policy = "UserStatusLimit")]
        public IActionResult PostSet([FromBody] SetDTO setDto, [FromForm] List<SushiDTO> sushiDtos, [FromForm] IFormFile photo, [FromServices] IWebHostEnvironment env)
        {
            try
            {
                _sushiService.PostSet(setDto, sushiDtos, photo, env);
                return Ok(setDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("Sushi")]
        [Authorize(Policy = "UserStatusLimit")]
        public IActionResult DeleteSushi(int id)
        {
            try
            {
                _sushiService.DeleteSushi(id);
                return Ok("Sushi deleted successfully");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("Set")]
        [Authorize(Policy = "UserStatusLimit")]
        public IActionResult DeleteSet(int id)
        {
            try
            {
                _sushiService.DeleteSet(id);
                return Ok("Set deleted successfully");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("Sushi")]
        [Authorize(Policy = "UserStatusLimit")]
        public IActionResult PutSushi(int id, [FromBody] SushiDTO sushiDto, [FromForm] IFormFile photo, [FromServices] IWebHostEnvironment env)
        {
            try
            {
                _sushiService.PutSushi(id, sushiDto, photo, env);
                return Ok("User updated successfully");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("Set")]
        [Authorize(Policy = "UserStatusLimit")]
        public IActionResult PutSet(int id, [FromBody] SetDTO setDto, [FromForm] List<SushiDTO> sushiDtos, [FromForm] IFormFile ph, [FromServices] IWebHostEnvironment env)
        {
            try
            {
                _sushiService.PutSet(id, setDto, sushiDtos, ph, env);
                return Ok("User updated successfully");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
