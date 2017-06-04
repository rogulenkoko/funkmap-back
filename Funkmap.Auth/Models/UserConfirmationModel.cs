using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Auth.Data.Entities;

namespace Funkmap.Module.Auth.Models
{
    public class UserConfirmationModel
    {
        public UserEntity User { get; set; }
        public string Code { get; set; }
    }
}
