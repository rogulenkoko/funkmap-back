using System;

namespace Funkmap.Notifications.Contracts
{
    public class FunkmapNotificationAttribute : Attribute
    {
        public FunkmapNotificationAttribute(string name, bool needAnswer)
        {
            Name = name;
            NeedAnswer = needAnswer;
        }

        public string Name { get; }
        public bool NeedAnswer { get; }
    }
}
