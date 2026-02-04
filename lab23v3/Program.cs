using System;

namespace Lab23
{
    // Платіжні шлюзи
    public class CreditCardGateway
    {
        public void PayWithCard(decimal amount)
        {
            Console.WriteLine($"Оплачено {amount} грн карткою.");
        }
    }

    public class PayPalService
    {
        public void PayWithPayPal(decimal amount)
        {
            Console.WriteLine($"Оплачено {amount} грн через PayPal.");
        }
    }

    public class CryptoExchange
    {
        public void PayWithCrypto(decimal amount)
        {
            Console.WriteLine($"Оплачено {amount} грн криптовалютою.");
        }
    }

    // Клас PaymentHandler порушує ISP та DIP
    public class PaymentHandler
    {
        private CreditCardGateway cardGateway;
        private PayPalService paypalService;
        private CryptoExchange cryptoExchange;

        public PaymentHandler()
        {
            // Жорстке створення залежностей → порушення DIP
            cardGateway = new CreditCardGateway();
            paypalService = new PayPalService();
            cryptoExchange = new CryptoExchange();
        }

        // Клієнт змушений залежати від всіх методів → порушення ISP
        public void PayByCard(decimal amount) => cardGateway.PayWithCard(amount);
        public void PayByPayPal(decimal amount) => paypalService.PayWithPayPal(amount);
        public void PayByCrypto(decimal amount) => cryptoExchange.PayWithCrypto(amount);
    }

    class Program
    {
        static void Main(string[] args)
        {
            var handler = new PaymentHandler();

            handler.PayByCard(500);     // Клієнт хоче платити тільки карткою
            handler.PayByPayPal(300);   // Але клас тягне всі методи
        }
    }
}