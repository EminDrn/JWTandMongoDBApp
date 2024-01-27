using JWTApp.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTApp.Data.Repositories
{
    public class GenericRepository<Tentity>:IGenericRepository<Tentity> where Tentity : class
    {

    }
}
