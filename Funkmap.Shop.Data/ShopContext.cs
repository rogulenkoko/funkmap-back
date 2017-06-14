using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Shop.Data.Abstract;
using Funkmap.Shop.Data.Configurations;
using Funkmap.Shop.Data.Entities;

namespace Funkmap.Shop.Data
{
    public class ShopContext :DbContext, IShopContext
    {
        public ShopContext() : base("FunkmapConnection") { }

        public ShopContext(string nameOrConnectionString) : base(nameOrConnectionString) { }
        public DbSet<ShopEntity> Shops { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBiBuilder)
        {
            modelBiBuilder.Configurations.Add(new ShopConfiguration());
            base.OnModelCreating(modelBiBuilder);
        }

    }
}
