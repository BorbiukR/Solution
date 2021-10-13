using System.Threading.Tasks;

namespace Solution.BLL.Interfaces
{
    public interface ICrud<TModel> where TModel : class
    {
        Task<bool> DeleteByIdAsync(int modelId);
    }
}