using MyFinances.Models.Domains;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFinances.Models
{
    public interface IOperationRepository
    {
        Task<int> AddAsync(Operation operation);
        Task DeleteAsync(Operation operation);
        Task<IEnumerable<Operation>> GetAsync();
        Task<Operation> GetAsync(int id);
        Task UpdateAsync(Operation operation);
    }
}