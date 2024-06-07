using Sushi_House.DTOModels;
using Sushi_House.Services;

namespace Sushi_House.Models
{
    public interface ISushiService
    {
        Task<List<SushiDTO>> GetSushi();
        Task<List<SetDTO>> GetSet();
        Task<List<STypeDTO>> GetSType();
        Task PostSushi(SushiDTO sushiDto, IFormFile photo, IWebHostEnvironment env);
        Task PostSet(SetDTO setDto, List<SushiDTO> sushiDtos, IFormFile ph, IWebHostEnvironment env);
        Task DeleteSushi(int id);
        Task DeleteSet(int id);
        Task PutSushi(int id, SushiDTO sushiDto, IFormFile photo, IWebHostEnvironment env);
        Task PutSet(int id, SetDTO setDto, List<SushiDTO> sushiDtos, IFormFile ph, IWebHostEnvironment env);
    }
}


