using Sushi_House.Services;

namespace Sushi_House.Models
{
    public interface ISushiService
    {
        Task<List<Sushi>> GetSushi();
        Task<List<Set>> GetSet();
        Task<List<Stype>> GetSType();
        Task PostSushi(Sushi s, IFormFile photo, IWebHostEnvironment env);
        Task PostSet(Set set, Sushi su, IFormFile ph, IWebHostEnvironment env);
        Task DeleteSushi(int id);
        Task DeleteSet(int id);
        Task PutSushi(int id, Sushi s, IFormFile photo, IWebHostEnvironment env);
        Task PutSet(int id, Sushi su, Set set, IFormFile ph, IWebHostEnvironment env);

    }
}


