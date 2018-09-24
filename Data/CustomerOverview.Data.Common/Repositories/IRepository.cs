using System.Linq;
using System.Threading.Tasks;

namespace CustomerOverview.Data.Common.Repositories
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        IQueryable<TEntity> All();

        Task<TEntity> GetByIdAsync(params object[] id);

        IQueryable<TEntity> GetByIdQueryable(params object[] id);

        void Add(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        void Delete(params object[] id);

        /// <remarks>
        /// Doesn't guarantee transactional behavior.
        /// </remarks>
        Task SaveChangesAsync();
    }
}
