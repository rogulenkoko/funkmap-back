using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Common.Data.Mongo;
using Funkmap.Messenger.Data.Entities;
using Funkmap.Messenger.Data.Parameters;
using Funkmap.Messenger.Data.Repositories.Abstract;
using MongoDB.Driver;

namespace Funkmap.Messenger.Data.Repositories
{
    public class MessageRepository : MongoRepository<MessageEntity>, IMessageRepository
    {
        public MessageRepository(IMongoCollection<MessageEntity> collection) : base(collection)
        {
        }

        public override Task<UpdateResult> UpdateAsync(MessageEntity entity)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<MessageEntity>> GetDilaogMessages(DialogParameter parameter)
        {
            var keys = Builders<MessageEntity>.IndexKeys.Descending(x => x.Date);
            _collection.Indexes.CreateOne(keys);

            //диалог с самим собой
            if (parameter.Members.Length == 2 && parameter.Members[0] == parameter.Members[1])
            {
                return await _collection.Find(x => x.Consumer == parameter.Members[0] && x.Sender == parameter.Members[0]).ToListAsync();
            }

            var result = await _collection.Find(x => parameter.Members.Contains(x.Consumer)
                                                           && parameter.Members.Contains(x.Sender)
                                                           && !(x.Sender == parameter.Members[0] && x.Consumer == parameter.Members[0]))
                .Skip(parameter.Skip)
                .Limit(parameter.Take)
                .ToListAsync();

            return result;
        }


    }
}
