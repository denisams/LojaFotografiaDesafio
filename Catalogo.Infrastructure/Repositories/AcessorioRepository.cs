using Catalogo.Core.Entities;
using Catalogo.Core.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Driver;
using MongoDB.Driver.Linq;


namespace Catalogo.Infrastructure.Repositories
{
    public class AcessorioRepository : CachedRepository<Accessory>, IAccessoryRepository
    {
        private readonly IMongoCollection<Accessory> _accessories;

        public AcessorioRepository(IMongoDatabase database, IDistributedCache cache) : base(cache)
        {
            _accessories = database.GetCollection<Accessory>("Acessorios");
        }

        public async Task<IEnumerable<Accessory>> GetAllAsync()
        {
            return await GetOrAddAsync("acessorios_all", async () =>
            {
                return await _accessories.Find(acessorio => true).ToListAsync();
            });
        }

        public async Task<Accessory> GetByIdAsync(int id)
        {
            return await GetOrAddAsync($"acessorio_{id}", async () =>
            {
                return await _accessories.Find(acessorio => acessorio.Id == id).FirstOrDefaultAsync();
            });
        }

        public async Task AddAsync(Accessory acessorio)
        {
            // Gerar um novo ID único
            acessorio.Id = await GetNextIdAsync();
            await _accessories.InsertOneAsync(acessorio);
            await RemoveAsync("acessorios_all");
        }

        public async Task UpdateAsync(Accessory acessorio)
        {
            await _accessories.ReplaceOneAsync(a => a.Id == acessorio.Id, acessorio);
            await RemoveAsync($"acessorio_{acessorio.Id}");
            await RemoveAsync("acessorios_all");
        }

        public async Task DeleteAsync(int id)
        {
            await _accessories.DeleteOneAsync(a => a.Id == id);
            await RemoveAsync($"acessorio_{id}");
            await RemoveAsync("acessorios_all");
        }

        public async Task<int> GetNextIdAsync()
        {
            // Obter o maior Id atual
            var maxId = await _accessories.AsQueryable().OrderByDescending(c => c.Id).FirstOrDefaultAsync();
            return (maxId?.Id ?? 0) + 1;
        }
    }
}
