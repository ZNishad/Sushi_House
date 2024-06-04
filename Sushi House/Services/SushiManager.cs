using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sushi_House.Models;

namespace Sushi_House.Services
{
    public class SushiManager : ISushiService
    {
        private readonly SushiContext _su;
        public SushiManager(SushiContext su)
        {
            _su = su;
        }

        public async Task<List<Sushi>> GetSushi()
        {
            return await _su.Sushis.Include(s => s.SushiSets).ToListAsync();
        }

        public async Task<List<Set>> GetSet()
        {
            return await _su.Sets.Include(s => s.SushiSets).ToListAsync ();
        }

        public async Task<List<Stype>> GetSType()
        {
            return await _su.Stypes.ToListAsync();
        }

        public async Task PostSushi(Sushi s, IFormFile photo, IWebHostEnvironment env)
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
            using(var transaction = await _su.Database.BeginTransactionAsync())
            {
                try
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await photo.CopyToAsync(stream);
                    }
                    s.SushiPicName = filename;

                    await _su.Sushis.AddAsync(s);
                    await _su.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception($"An error occurred while saving data: {ex.Message}", ex);
                }
            }
        }

        public async Task PostSet(Set set, Sushi su, IFormFile ph, IWebHostEnvironment env)
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
            using (var transaction = await _su.Database.BeginTransactionAsync())
            {
                try
                {
                    using (var stream = new FileStream(filepath, FileMode.Create))
                    {
                        await ph.CopyToAsync(stream);
                    }
                    set.SetPicName = filename;

                    var added =  _su.Sets.AddAsync(set).Result.Entity;
                    await  _su.SaveChangesAsync();

                    foreach (var item in set.SushiSets)
                    {
                        var rel = new SushiSet { SushiSetSetId = added.SetId, SushiSetSushiId = su.SushiId };
                        await _su.SushiSets.AddAsync(rel);
                    }
                    await  _su.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception($"An error occurred while saving data: {ex.Message}", ex);
                }
            }
        }

        public async Task DeleteSushi(int id)
        {
            var sushi = await _su.Sushis.FirstOrDefaultAsync(x => x.SushiId == id);
            if (sushi != null)
            {
                _su.Sushis.Remove(sushi);
                await _su.SaveChangesAsync();
            }
        }

        public async Task DeleteSet(int id)
        {
            var set = await _su.Sets.FirstOrDefaultAsync(x => x.SetId == id);
            if (set != null)
            {
                _su.Sets.Remove(set);
                await _su.SaveChangesAsync();
            }
        }

        public async Task PutSushi(int id, Sushi s, IFormFile photo, IWebHostEnvironment env)
        {
            var OldSushi = await _su.Sushis.FirstOrDefaultAsync(x => x.SushiId == id);
            if (OldSushi == null)
            {
                throw new ArgumentException("User not found");
            }

            if (photo == null || photo.Length == 0)
            {
                throw new ArgumentException("Failed to upload photo.");
            }
            if (photo.ContentType != "image/png" && photo.ContentType != "image/jpeg")
            {
                throw new ArgumentException("Unsupported file type. Please upload a file with .png or .jpeg extension.");
            }

            string oldExtension = Path.GetExtension(OldSushi.SushiPicName).ToLower();
            string newExtension = Path.GetExtension(photo.FileName).ToLower();
            if (newExtension != oldExtension)
            {
                throw new ArgumentException($"Uploaded file must have the same extension as the previous file ({oldExtension}).");
            }

            var filePath = Path.Combine(env.WebRootPath, "img", OldSushi.SushiPicName);
            using (var transaction = await _su.Database.BeginTransactionAsync())
            {
                try
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await photo.CopyToAsync(stream);
                    }

                    OldSushi.SushiName = s.SushiName;
                    OldSushi.SushiTypeId = s.SushiTypeId;
                    OldSushi.SushiPrice = s.SushiPrice;
                    OldSushi.SushiInqr = s.SushiInqr;
                    await _su.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception($"An error occurred while saving data: {ex.Message}", ex);
                }
            }
        }

        public async Task PutSet(int id, Sushi su, Set set, IFormFile ph, IWebHostEnvironment env)
        {
            var OldSet = await _su.Sets.FirstOrDefaultAsync(x => x.SetId == id);
            if (OldSet == null)
            {
                throw new ArgumentException("Set not found");
            }

            var OldSushiSet = _su.SushiSets.Where(x => x.SushiSetSetId == id).ToList();

            if (ph == null || ph.Length == 0)
            {
                throw new ArgumentException("Failed to upload photo.");
            }
            if (ph.ContentType != "image/png" && ph.ContentType != "image/jpeg")
            {
                throw new ArgumentException("Unsupported file type. Please upload a file with .png or .jpeg extension.");
            }

            string oldExtension = Path.GetExtension(OldSet.SetPicName).ToLower();
            string newExtension = Path.GetExtension(ph.FileName).ToLower();
            if (newExtension != oldExtension)
            {
                throw new ArgumentException($"Uploaded file must have the same extension as the previous file ({oldExtension}).");
            }

            var filePath = Path.Combine(env.WebRootPath, "img", OldSet.SetPicName);
            using (var transaction = await _su.Database.BeginTransactionAsync())
            {
                try
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ph.CopyToAsync(stream);
                    }

                    OldSet.SetName = set.SetName;
                    foreach(var item in OldSushiSet)
                    {
                        item.SushiSetSushiId = su.SushiId;
                    }
                    await _su.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception($"An error occurred while saving data: {ex.Message}", ex);
                }
            }
        }
    }
}
