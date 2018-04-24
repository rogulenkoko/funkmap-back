using System;
using Funkmap.Auth.Abstract;

namespace Funkmap.Auth.Services
{
    public class ConfirmationCodeGenerator : IConfirmationCodeGenerator
    {
        public string Generate()
        {
            return new Random().Next(100000, 999999).ToString();
        }
    }
}
