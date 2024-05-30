using Sushi_House.Services;

namespace Sushi_House.Models
{
    public interface ISushiService
    {
        List<Sushi> GetSushi();
        List<Set> GetSet();
        List<Stype> GetType();
        Task PostSushi(Sushi s, IFormFile photo, IWebHostEnvironment env);
        Task PostSet(Set set, Sushi su, IFormFile ph, IWebHostEnvironment env);
        void DeleteSushi(int id);
        void DeleteSet(int id);
        Task PutSushi(int id, Sushi s, IFormFile photo, IWebHostEnvironment env);
        void PutSet(int id, SushiSet ss, Set set, IFormFile ph, IWebHostEnvironment env);

    }
}


