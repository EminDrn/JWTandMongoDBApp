using JWTApp.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JWTApp.Data.Repositories
{
    public class GenericRepository<Tentity> : IGenericRepository<Tentity> where Tentity : class
    {
        public Task AddAsync(Tentity entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Tentity>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Tentity> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Remove(Tentity entity)
        {
            throw new NotImplementedException();
        }

        public Tentity Update(Tentity entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Tentity> Where(Expression<Func<Tentity, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
