using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sushi_House.DTOModels;
using Sushi_House.Mapping;
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

        public async Task<List<SushiDTO>> GetSushi()
        {
            var sushis = await _su.Sushis.Include(s => s.SushiSets).ToListAsync();
            return sushis.Select(SushiMapper.toDTO).ToList();
        }

        public async Task<List<SetDTO>> GetSet()
        {
            var sets = await _su.Sets.Include(s => s.SushiSets).ToListAsync();
            return sets.Select(SetMapper.toDTO).ToList();
        }

        public async Task<List<STypeDTO>> GetSType()
        {
            var stype = await _su.Stypes.ToListAsync();
            return stype.Select(STypeMapper.toDTO).ToList();
        }

        public async Task PostSushi(SushiDTO sushiDto, IFormFile photo, IWebHostEnvironment env)
        {
            if (photo == null || photo.Length == 0)
                throw new ArgumentException("Failed to upload photo.");

            if (photo.ContentType != "image/png" && photo.ContentType != "image/jpeg")
                throw new ArgumentException("Unsupported file type. Please upload a file with .png or .jpeg extension.");

            var filename = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
            var filePath = Path.Combine(env.WebRootPath, "img", filename);
            using (var transaction = await _su.Database.BeginTransactionAsync())
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await photo.CopyToAsync(stream);
                }

                var sushi = SushiMapper.toEntity(sushiDto);
                sushi.SushiPicName = filename;

                await _su.Sushis.AddAsync(sushi);
                await _su.SaveChangesAsync();

                await transaction.CommitAsync();
            }
        }

        public async Task PostSet(SetDTO setDto, List<SushiDTO> sushiDtos, IFormFile ph, IWebHostEnvironment env)
        {
            if (ph == null || ph.Length == 0)
                throw new ArgumentException("Failed to upload photo.");

            if (ph.ContentType != "image/png" && ph.ContentType != "image/jpeg")
                throw new ArgumentException("Unsupported file type. Please upload a file with .png or .jpeg extension.");

            var filename = Guid.NewGuid().ToString() + Path.GetExtension(ph.FileName);
            var filepath = Path.Combine(env.WebRootPath, "img", filename);
            using (var transaction = await _su.Database.BeginTransactionAsync())
            {
                using (var stream = new FileStream(filepath, FileMode.Create))
                {
                    await ph.CopyToAsync(stream);
                }
                var set = SetMapper.toEntity(setDto);
                set.SetPicName = filename;

                var addSet = (await _su.Sets.AddAsync(set)).Entity;
                await _su.SaveChangesAsync();

                foreach (var sushiDto in sushiDtos)
                {
                    var sushiSet = new SushiSet
                    {
                        SushiSetSetId = addSet.SetId,
                        SushiSetSushiId = sushiDto.SushiId
                    };
                    await _su.SushiSets.AddAsync(sushiSet);
                }
                await _su.SaveChangesAsync();

                await transaction.CommitAsync();
            }
        }

        public async Task DeleteSushi(int id)
        {
            var sushi = await _su.Sushis.FirstOrDefaultAsync(x => x.SushiId == id);
            if (sushi == null)
                throw new ArgumentException("Sushi not found");

            _su.Sushis.Remove(sushi);
            await _su.SaveChangesAsync();
        }

        public async Task DeleteSet(int id)
        {
            var set = await _su.Sets.FirstOrDefaultAsync(x => x.SetId == id);
            if (set == null) 
                throw new ArgumentException("Set not found");

            _su.Sets.Remove(set);
            await _su.SaveChangesAsync();
        }

        public async Task PutSushi(int id, SushiDTO sushiDto, IFormFile photo, IWebHostEnvironment env)
        {
            var OldSushi = await _su.Sushis.FirstOrDefaultAsync(x => x.SushiId == id);
            if (OldSushi == null)
                throw new ArgumentException("User not found");

            if (photo == null || photo.Length == 0)
                throw new ArgumentException("Failed to upload photo.");

            if (photo.ContentType != "image/png" && photo.ContentType != "image/jpeg")
                throw new ArgumentException("Unsupported file type. Please upload a file with .png or .jpeg extension.");

            string oldExtension = Path.GetExtension(OldSushi.SushiPicName).ToLower();
            string newExtension = Path.GetExtension(photo.FileName).ToLower();
            if (newExtension != oldExtension)
                throw new ArgumentException($"Uploaded file must have the same extension as the previous file ({oldExtension}).");

            var filePath = Path.Combine(env.WebRootPath, "img", OldSushi.SushiPicName);
            using (var transaction = await _su.Database.BeginTransactionAsync())
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await photo.CopyToAsync(stream);
                }

                OldSushi.SushiName = sushiDto.SushiName;
                OldSushi.SushiTypeId = sushiDto.SushiTypeId;
                OldSushi.SushiPrice = sushiDto.SushiPrice;
                OldSushi.SushiInqr = sushiDto.SushiInqr;
                await _su.SaveChangesAsync();

                await transaction.CommitAsync();
            }
        }

        public async Task PutSet(int id, SetDTO setDto, List<SushiDTO> sushiDtos, IFormFile ph, IWebHostEnvironment env)
        {
            var oldSet = await _su.Sets.FirstOrDefaultAsync(x => x.SetId == id);
            if (oldSet == null)
                throw new ArgumentException("Set not found");

            var oldSushiSet = _su.SushiSets.Where(x => x.SushiSetSetId == id).ToList();

            if (ph == null || ph.Length == 0)
                throw new ArgumentException("Failed to upload photo.");

            if (ph.ContentType != "image/png" && ph.ContentType != "image/jpeg")
                throw new ArgumentException("Unsupported file type. Please upload a file with .png or .jpeg extension.");

            string oldExtension = Path.GetExtension(oldSet.SetPicName).ToLower();
            string newExtension = Path.GetExtension(ph.FileName).ToLower();
            if (newExtension != oldExtension)
                throw new ArgumentException($"Uploaded file must have the same extension as the previous file ({oldExtension}).");

            var filePath = Path.Combine(env.WebRootPath, "img", oldSet.SetPicName);
            using (var transaction = await _su.Database.BeginTransactionAsync())
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ph.CopyToAsync(stream);
                }

                oldSet.SetName = setDto.SetName;

                if (oldSushiSet.Count == sushiDtos.Count)
                {
                    for (int i = 0; i < oldSushiSet.Count && i < sushiDtos.Count; i++)
                    {
                        oldSushiSet[i].SushiSetSushiId = sushiDtos[i].SushiId;
                    }
                }
                else
                {
                    _su.SushiSets.RemoveRange(oldSushiSet);

                    foreach (var sushiDto in sushiDtos)
                    {
                        var newSushiSet = new SushiSet
                        {
                            SushiSetSetId = id,
                            SushiSetSushiId = sushiDto.SushiId
                        };
                        _su.SushiSets.Add(newSushiSet);
                    }
                }

                await _su.SaveChangesAsync();

                await transaction.CommitAsync();
            }
        }
    }
}
