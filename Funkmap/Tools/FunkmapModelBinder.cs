using System;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using Funkmap.Data.Entities;
using Funkmap.Domain;
using Funkmap.Domain.Models;
using Funkmap.Models;

namespace Funkmap.Tools
{
    public class FunkmapModelBinderProvider : ModelBinderProvider
    {
        public override IModelBinder GetBinder(HttpConfiguration configuration, Type modelType)
        {
            return new FunkmapModelBinder();
        }
    }

    public class FunkmapModelBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            var body = actionContext.Request.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var entityTypeModel = Newtonsoft.Json.JsonConvert.DeserializeObject<EntityTypeModel>(body);

            if(entityTypeModel.EntityType == 0) throw new ArgumentNullException(nameof(entityTypeModel.EntityType), "EntityType can not be empty");

            switch (entityTypeModel.EntityType)
            {
                case EntityType.Musician:
                    bindingContext.Model = Newtonsoft.Json.JsonConvert.DeserializeObject<Musician>(body);
                    break;

                case EntityType.Band:
                    bindingContext.Model = Newtonsoft.Json.JsonConvert.DeserializeObject<Band>(body);
                    break;

                case EntityType.Shop:
                    bindingContext.Model = Newtonsoft.Json.JsonConvert.DeserializeObject<Shop>(body);
                    break;

                case EntityType.RehearsalPoint:
                    bindingContext.Model = Newtonsoft.Json.JsonConvert.DeserializeObject<RehearsalPoint>(body);
                    break;

                case EntityType.Studio:
                    bindingContext.Model = Newtonsoft.Json.JsonConvert.DeserializeObject<Studio>(body);
                    break;
            }
            return true;
        }
    }

    public class EntityTypeModel
    {
        public EntityType EntityType { get; set; }
    }
}
