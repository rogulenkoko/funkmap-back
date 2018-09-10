using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Auth.Domain.Models;
using Funkmap.Common.Models;
using Funkmap.Cqrs;

namespace Funkmap.Auth.Domain.Abstract
{
    public interface IAuthRepository
    {
        Task<User> GetAsync(string login);
        Task<List<string>> GetBookedEmailsAsync();

       
        Task<User> LoginAsync(string login, string hashedPassword);


        Task<byte[]> GetAvatarAsync(string login);
        Task<string> SaveAvatarAsync(string login, byte[] image);


        Task<BaseResponse> TryCreateAsync(User user, string hashedPassword);
        Task<BaseResponse> TryCreateSocialAsync(User user);
        Task UpdateLastVisitDateAsync(string login, DateTime date);
        Task UpdateLocaleAsync(string login, string locale);
        Task UpdatePasswordAsync(string login, string hashedPassword);

        
    }
}
