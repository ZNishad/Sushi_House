using Microsoft.AspNetCore.Mvc;
using Sushi_House.Models;

namespace Sushi_House.Services
{
    public interface ISushiCommandService
    {
        Task PostSushi(Sushi s, IFormFile photo, IWebHostEnvironment env);
        Task PostSet(Set set, Sushi su, IFormFile ph, IWebHostEnvironment env);
        void DeleteSushi(int id);
        void DeleteSet(int id);
        Task PutSushi(int id, Sushi s, IFormFile photo, IWebHostEnvironment env);
        void PutSet(int id, SushiSet ss, Set set, IFormFile ph, IWebHostEnvironment env);

    }
}
