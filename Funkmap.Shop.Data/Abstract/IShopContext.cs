using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Shop.Data.Entities;

namespace Funkmap.Shop.Data.Abstract
{
    interface IShopContext
    {
        DbSet<ShopEntity> Shops { get; set; }
    }
}
