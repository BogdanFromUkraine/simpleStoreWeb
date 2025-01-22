using ProductService.Models;

namespace CartService.Kafka.Consumer
{
    public class MessageStorageService : IMessageStorageService
    {
        private readonly List<IEnumerable<Products>> _messages;

        public MessageStorageService()
        {
            _messages = new List<IEnumerable<Products>>();
        }

        // Додати повідомлення до списку
        public void AddMessage(IEnumerable<Products> message)
        {
            _messages.Add(message);
        }

        // Отримати всі повідомлення
        public IEnumerable<IEnumerable<Products>> GetAllMessages()
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
