using StoreService_AT.MessageBuss;

namespace StoreService_AT.RabbitMQ.Sender
{
    public interface IRabbitMQMessageSender
    {
        void SendMessage(BaseMessage message, string queueName);
    }
}
