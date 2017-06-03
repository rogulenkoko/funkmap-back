using System.Data.Entity;
using Funkmap.Auth.Data.Entities;

namespace Funkmap.Auth.Data.Abstract
{
    public interface IAuthContext
    {
        DbSet<UserEntity> Users { get; set; }
    }
}
