using Microsoft.Extensions.Configuration;

using RabbitMQ.Client;

using System;

namespace RabbitMqConnectionFactory
{
    public class RabbitMqConnectionFactory : IRabbitMqConnectionFactory, IDisposable
    {
        private readonly ConnectionFactory _connFactory;
        private IConnection _connection;
        private IModel _model;

        public RabbitMqConnectionFactory(IConfiguration configuration)
        {
            _connFactory = new ConnectionFactory()
            {
                HostName = !string.IsNullOrEmpty(configuration["RABBITMQ-HOST"]) ? configuration["RABBITMQ-HOST"] : "docker.for.win.localhost",
                UserName = !string.IsNullOrEmpty(configuration["RABBITMQ-USER"]) ? configuration["RABBITMQ-USER"] : "guest",
                Password = !string.IsNullOrEmpty(configuration["RABBITMQ-PASS"]) ? configuration["RABBITMQ-PASS"] : "guest",
            };
        }

        public IModel GetModel()
        {
            _connection = _connFactory.CreateConnection();
            _model = _connection.CreateModel();

            return _model;
        }

        #region IDisposable Support

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _model?.Dispose();
                    _connection?.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}