using Catalogo.Core.Entities;
using Catalogo.Core.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Catalogo.Infrastructure.Repositories
{
    public class CameraRepository : CachedRepository<Camera>, ICameraRepository
    {
        private readonly IMongoCollection<Camera> _cameras;

        public CameraRepository(IMongoDatabase database, IDistributedCache cache) : base(cache)
        {
            _cameras = database.GetCollection<Camera>("Cameras");
        }

        public async Task<IEnumerable<Camera>> GetAllAsync()
        {
            return await GetOrAddAsync("cameras_all", async () =>
            {
                return await _cameras.Find(camera => true).ToListAsync();
            });
        }

        public async Task<Camera> GetByIdAsync(int id)
        {
            return await GetOrAddAsync($"camera_{id}", async () =>
            {
                return await _cameras.Find(camera => camera.Id == id).FirstOrDefaultAsync();
            });
        }

        public async Task AddAsync(Camera camera)
        {
            // Gerar um novo ID único
            camera.Id = await GetNextIdAsync();
            await _cameras.InsertOneAsync(camera);
            await RemoveAsync("cameras_all");
        }

        public async Task UpdateAsync(Camera camera)
        {
            await _cameras.ReplaceOneAsync(c => c.Id == camera.Id, camera);
            await RemoveAsync($"camera_{camera.Id}");
            await RemoveAsync("cameras_all");
        }

        public async Task DeleteAsync(int id)
        {
            await _cameras.DeleteOneAsync(camera => camera.Id == id);
            await RemoveAsync($"camera_{id}");
            await RemoveAsync("cameras_all");
        }

        public async Task<int> GetNextIdAsync()
        {
            // Obter o maior Id atual
            var maxId = await _cameras.AsQueryable().OrderByDescending(c => c.Id).FirstOrDefaultAsync();
            return (maxId?.Id ?? 0) + 1;
        }
    }
}
