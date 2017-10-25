using Funkmap.Common.Notifications.Notification;

namespace Funkmap.Module.Auth.Notifications
{
    public class ConfirmationNotification : Notification
    {
        public ConfirmationNotification(string reciever, string name, string code) : base(reciever)
        {
            Subject = GetSubject();
            Title = GetConfirmationTitle(name);
            MainContent = GetConfirmationMainContent();
            Footer = GetConfirmationFooter(code);
        }


        private string GetSubject()
        {
            return "Подтверждение регистрации";
        }

        private string GetConfirmationTitle(string name)
        {
            return $"Приветствую, {name}";
        }

        private string GetConfirmationMainContent()
        {
            //todo локализация, вынести в ресурсы
            return "Спасибо, что зарегистрировались на  Band Map. <br>Каждый новый пользователь улучшает и развивает сервис,<br> делая его более обширным. Мы ценим Вас и Ваш выбор. <br><br> Добро пожаловать на самую крупную площадку <br> по поиску музыкантов и всего, что связывает их.";
        }

        private string GetConfirmationFooter(string code)
        {
            return "Код для подтверждения регистрации: " + $"<span style=\"font-family: 'Roboto', Arial, sans-serif; margin: 0 0 0; font-size: 26px; line-height: 29px; color:#ee9117;\"> {code} </span>";
        }
    }
}
