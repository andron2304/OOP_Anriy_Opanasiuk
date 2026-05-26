using System;

namespace IndependentWork16
{
    // ===== "ПОГАНИЙ" КЛАС (ПОРУШЕННЯ SRP) =====
    class BadNotificationSender
    {
        public void Send(string message, string channel)
        {
            Console.WriteLine("Формування повідомлення");
            string formattedMessage = "[Повідомлення]: " + message;

            Console.WriteLine("Вибір каналу");
            if (channel == "email")
            {
                Console.WriteLine("Відправка електронного листа");
                Console.WriteLine(formattedMessage);
            }
            else if (channel == "sms")
            {
                Console.WriteLine("Відправка SMS-повідомлення");
                Console.WriteLine(formattedMessage);
            }

            Console.WriteLine("Логування відправки");
        }
    }

    // ===== ІНТЕРФЕЙСИ (SRP + DIP) =====
    interface IMessageFormatter
    {
        string Format(string message);
    }

    interface IChannelSelector
    {
        string SelectChannel();
    }

    interface IEmailSender
    {
        void Send(string message);
    }

    interface ISmsSender
    {
        void Send(string message);
    }

    // ===== РЕАЛІЗАЦІЇ (ЗАГЛУШКИ) =====
    class SimpleMessageFormatter : IMessageFormatter
    {
        public string Format(string message)
        {
            return "[Повідомлення]: " + message;
        }
    }

    class SimpleChannelSelector : IChannelSelector
    {
        public string SelectChannel()
        {
            return "email";
        }
    }

    class EmailSender : IEmailSender
    {
        public void Send(string message)
        {
            Console.WriteLine("Електронний лист відправлено:");
            Console.WriteLine(message);
        }
    }

    class SmsSender : ISmsSender
    {
        public void Send(string message)
        {
            Console.WriteLine("SMS-повідомлення відправлено:");
            Console.WriteLine(message);
        }
    }

    // ===== СЕРВІС З ЄДИНОЮ ВІДПОВІДАЛЬНІСТЮ =====
    class NotificationService
    {
        private readonly IMessageFormatter _formatter;
        private readonly IChannelSelector _channelSelector;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;

        public NotificationService(
            IMessageFormatter formatter,
            IChannelSelector channelSelector,
            IEmailSender emailSender,
            ISmsSender smsSender)
        {
            _formatter = formatter;
            _channelSelector = channelSelector;
            _emailSender = emailSender;
            _smsSender = smsSender;
        }

        public void Send(string message)
        {
            string formattedMessage = _formatter.Format(message);
            string channel = _channelSelector.SelectChannel();

            if (channel == "email")
                _emailSender.Send(formattedMessage);
            else
                _smsSender.Send(formattedMessage);

            Console.WriteLine("Повідомлення залоговано");
        }
    }

    // ===== MAIN =====
    class Program
    {
        static void Main()
        {
            IMessageFormatter formatter = new SimpleMessageFormatter();
            IChannelSelector channelSelector = new SimpleChannelSelector();
            IEmailSender emailSender = new EmailSender();
            ISmsSender smsSender = new SmsSender();

            NotificationService service = new NotificationService(
                formatter,
                channelSelector,
                emailSender,
                smsSender);

            service.Send("Ваше замовлення успішно оброблено");
        }
    }
}
