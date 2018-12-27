using System;
using Funkmap.Auth.Abstract;

namespace Funkmap.Auth.Services
{
    /// <inheritdoc cref="IConfirmationCodeGenerator"/>
    public class ConfirmationCodeGenerator : IConfirmationCodeGenerator
    {
        /// <inheritdoc cref="IConfirmationCodeGenerator.Generate"/>
        public string Generate()
        {
            return new Random().Next(100000, 999999).ToString();
        }
    }
}
