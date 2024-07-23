using Catalogo.Core.Entities;

namespace Catalogo.Core.Interfaces
{
    public interface IAccessoryRepository
    {
        Task<IEnumerable<Accessory>> GetAllAsync();
        Task<Accessory> GetByIdAsync(int id);
        Task AddAsync(Accessory accessory);
        Task UpdateAsync(Accessory accessory);
        Task DeleteAsync(int id);
        Task<int> GetNextIdAsync();
    }
}
