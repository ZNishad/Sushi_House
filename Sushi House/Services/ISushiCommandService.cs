﻿using Sushi_House.Models;

namespace Sushi_House.Services
{
    public interface ISushiCommandService
    {
        void PostSushi(Sushi s, IFormFile photo, IWebHostEnvironment env);
        void PostSet(SushiSet ss, Set set, IFormFile ph, IWebHostEnvironment env);
        void DeleteSushi(int id);
        void DeleteSet(int id);
    }
}