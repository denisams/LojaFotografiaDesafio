using Catalogo.Core.Entities;
using Catalogo.Core.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Driver;
using MongoDB.Driver.Linq;


namespace Catalogo.Infrastructure.Repositories
{
    public class AcessorioRepository : CachedRepository<Acessorio>, IAcessorioRepository
    {
        private readonly IMongoCollection<Acessorio> _acessorios;

        public AcessorioRepository(IMongoDatabase database, IDistributedCache cache) : base(cache)
        {
            _acessorios = database.GetCollection<Acessorio>("Acessorios");
        }

        public async Task<IEnumerable<Acessorio>> GetAllAsync()
        {
            return await GetOrAddAsync("acessorios_all", async () =>
            {
                return await _acessorios.Find(acessorio => true).ToListAsync();
            });
        }

        public async Task<Acessorio> GetByIdAsync(int id)
        {
            return await GetOrAddAsync($"acessorio_{id}", async () =>
            {
                return await _acessorios.Find(acessorio => acessorio.Id == id).FirstOrDefaultAsync();
            });
        }

        public async Task AddAsync(Acessorio acessorio)
        {
            // Gerar um novo ID único
            acessorio.Id = await GetNextIdAsync();
            await _acessorios.InsertOneAsync(acessorio);
            await RemoveAsync("acessorios_all");
        }

        public async Task UpdateAsync(Acessorio acessorio)
        {
            await _acessorios.ReplaceOneAsync(a => a.Id == acessorio.Id, acessorio);
            await RemoveAsync($"acessorio_{acessorio.Id}");
            await RemoveAsync("acessorios_all");
        }

        public async Task DeleteAsync(int id)
        {
            await _acessorios.DeleteOneAsync(a => a.Id == id);
            await RemoveAsync($"acessorio_{id}");
            await RemoveAsync("acessorios_all");
        }

        public async Task<int> GetNextIdAsync()
        {
            // Obter o maior Id atual
            var maxId = await _acessorios.AsQueryable().OrderByDescending(c => c.Id).FirstOrDefaultAsync();
            return (maxId?.Id ?? 0) + 1;
        }
    }
}
