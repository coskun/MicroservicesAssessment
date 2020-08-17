using RabbitMQ.Client;

namespace RabbitMqConnectionFactory
{
    public interface IRabbitMqConnectionFactory
    {
        IModel GetModel();
    }
}