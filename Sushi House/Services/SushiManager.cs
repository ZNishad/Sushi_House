using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sushi_House.Models;

namespace Sushi_House.Services
{
    public class SushiManager:ISushiService
    {
        private readonly SushiContext _su;
        public SushiManager(SushiContext su)
        {
            _su = su;
        }

        public List<Sushi> GetSushi()
        {
            return _su.Sushis.Include(s => s.SushiSets).ToList();
        }

        public List<Set> GetSet()
        {
            return _su.Sets.Include(s => s.SushiSets).ToList();
        }

        public void PostSushi(Sushi s, IFormFile photo, IWebHostEnvironment env)
        {
            if (photo == null || photo.Length == 0)
            {
                throw new ArgumentException("Failed to upload photo.");
            }
            if (photo.ContentType != "image/png" && photo.ContentType != "image/jpeg")
            {
                throw new ArgumentException("Unsupported file type. Please upload a file with .png or .jpeg extension.");
            }
                var filename = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
                var filePath = Path.Combine(env.WebRootPath, "img", filename);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    photo.CopyTo(stream);
                }
                s.SushiPicName = filename;
                _su.Sushis.Add(s);
                _su.SaveChanges();
        }

        public void PostSet(SushiSet ss, Set set, IFormFile ph, IWebHostEnvironment env)
        {
            if (ph == null || ph.Length == 0)
            {
                throw new ArgumentException("Failed to upload photo.");
            }
            if (ph.ContentType != "image/png" && ph.ContentType != "image/jpeg")
            {
                throw new ArgumentException("Unsupported file type. Please upload a file with .png or .jpeg extension.");
            }
                var filename = Guid.NewGuid().ToString() + Path.GetExtension(ph.FileName);
                var filepath = Path.Combine(env.WebRootPath, "img", filename);
                using (var strem = new FileStream(filepath, FileMode.Create))
                {
                    ph.CopyTo(strem);
                }
                set.SetPicName = filename;
                _su.Sets.Add(set);
                _su.SushiSets.Add(ss);
                _su.SaveChanges();
        }

        public void DeleteSushi(int id)
        {
            var sushi = _su.Sushis.FirstOrDefault(x => x.SushiId == id);
            if (sushi != null)
            {
                _su.Sushis.Remove(sushi);
            }
            _su.SaveChanges();
        }

        public void DeleteSet(int id)
        {
            var set = _su.Sets.FirstOrDefault(x => x.SetId == id);
            if (set != null)
            {
                _su.Sets.Remove(set);
            }
            _su.SaveChanges();
        }
    }
}
