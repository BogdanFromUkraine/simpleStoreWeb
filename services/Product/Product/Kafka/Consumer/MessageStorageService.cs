using ProductService.Models;

namespace Product.Kafka.Consumer
{
    public class MessageStorageService : IMessageStorageService
    {
        private readonly List<string> _messages;

        public MessageStorageService()
        {
            _messages = new List<string>();
        }

        // Додати повідомлення до списку
        public void AddMessage(string message)
        {
            _messages.Add(message);
        }

        // Отримати всі повідомлення
        public IEnumerable<string> GetAllMessages()
        {
            return _messages;
        }

        // Очистити збережені повідомлення
        public void ClearMessages()
        {
            _messages.Clear();
        }
    }

}
