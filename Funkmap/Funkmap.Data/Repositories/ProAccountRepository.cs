using System;
using System.Threading.Tasks;
using Funkmap.Common.Data.Mongo;
using Funkmap.Data.Entities.Entities;
using Funkmap.Data.Mappers;
using Funkmap.Domain.Abstract.Repositories;
using Funkmap.Domain.Models;
using MongoDB.Driver;

namespace Funkmap.Data.Repositories
{
    public class ProAccountRepository : RepositoryBase<ProAccountEntity>, IProAccountRepository
    {
        public ProAccountRepository(IMongoCollection<ProAccountEntity> collection) : base(collection)
        {
        }

        public async Task CreateAsync(ProAccount proAccount)
        {
            var entity = proAccount.ToEntity();
            await _collection.InsertOneAsync(entity);
        }

        public async Task<ProAccount> GetAsync(string userLogin)
        {
            var now = DateTime.UtcNow;
            var filter = Builders<ProAccountEntity>.Filter.Eq(x => x.UserLogin, userLogin) &
                         Builders<ProAccountEntity>.Filter.Lte(x => x.ExpireAtUtc, now);

            var proAccount = await _collection.Find(filter).SingleOrDefaultAsync();
            return proAccount.ToModel();
        }
    }
}
