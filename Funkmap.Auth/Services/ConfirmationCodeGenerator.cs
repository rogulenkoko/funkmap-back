using System;
using Funkmap.Module.Auth.Abstract;

namespace Funkmap.Module.Auth.Services
{
    public class ConfirmationCodeGenerator : IConfirmationCodeGenerator
    {
        public string Generate()
        {
            return new Random().Next(100000, 999999).ToString();
        }
    }
}
