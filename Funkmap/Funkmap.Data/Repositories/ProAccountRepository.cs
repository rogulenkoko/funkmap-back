using System;
using System.Threading.Tasks;
using Funkmap.Data.Entities.Entities;
using Funkmap.Data.Mappers;
using Funkmap.Domain.Abstract.Repositories;
using Funkmap.Domain.Models;
using MongoDB.Driver;

namespace Funkmap.Data.Repositories
{
    public class ProAccountRepository : IProAccountRepository
    {
        private readonly IMongoCollection<ProAccountEntity> _collection;
        
        public ProAccountRepository(IMongoCollection<ProAccountEntity> collection)
        {
            _collection = collection;
        }

        public async Task CreateAsync(ProAccount proAccount)
        {
            var filter = Builders<ProAccountEntity>.Filter.Eq(x => x.UserLogin, proAccount.UserLogin);
            var entity = proAccount.ToEntity();
            await _collection.ReplaceOneAsync(filter, entity);
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
