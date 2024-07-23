using Catalogo.Core.Entities;

namespace Catalogo.Core.Interfaces
{
    public interface ICameraRepository
    {
        Task<IEnumerable<Camera>> GetAllAsync();
        Task<Camera> GetByIdAsync(int id);
        Task AddAsync(Camera camera);
        Task UpdateAsync(Camera camera);
        Task DeleteAsync(int id);
        Task<int> GetNextIdAsync();
    }
}
