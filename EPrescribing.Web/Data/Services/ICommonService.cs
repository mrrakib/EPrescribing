using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPrescribing.Web.Data.Services
{
    public interface ICommonService<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<int> GetCountAsync();
        Task<T> FindAsync(int? Id);

        Task<bool> AddAsync(T model);
        Task<bool> UpdateAsync(T model);
        Task<bool> DeleteAsync(int Id);
        Task<bool> IsExistItemAsync(string name = "");
        Task<bool> IsExistItemForUpdateAsync(int id, string name = "");
        Task<PagedList.IPagedList<T>> GetAllPageListAsync(int pageNo, int rowNo, string searchString);
    }
}
