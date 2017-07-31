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

        public async Task<ICollection<MessageEntity>> GetDilaogMessages(DialogMessagesParameter messagesParameter)
        {
            //диалог с самим собой
            if (messagesParameter.Members.Length == 2 && messagesParameter.Members[0] == messagesParameter.Members[1])
            {
                return await _collection.Find(x => x.Receiver == messagesParameter.Members[0] && x.Sender == messagesParameter.Members[0]).ToListAsync();
            }

            var result = await _collection.Find(x => messagesParameter.Members.Contains(x.Receiver)
                                                           && messagesParameter.Members.Contains(x.Sender)
                                                           && !(x.Sender == messagesParameter.Members[0] && x.Receiver == messagesParameter.Members[0]))
                .Skip(messagesParameter.Skip)
                .Limit(messagesParameter.Take)
                .ToListAsync();

            return result;
        }


    }
}
