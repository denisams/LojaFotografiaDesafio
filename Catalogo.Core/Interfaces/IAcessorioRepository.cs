using Catalogo.Core.Entities;

namespace Catalogo.Core.Interfaces
{
    public interface IAcessorioRepository
    {
        Task<IEnumerable<Acessorio>> GetAllAsync();
        Task<Acessorio> GetByIdAsync(int id);
        Task AddAsync(Acessorio acessorio);
        Task UpdateAsync(Acessorio acessorio);
        Task DeleteAsync(int id);
        Task<int> GetNextIdAsync();
    }
}
