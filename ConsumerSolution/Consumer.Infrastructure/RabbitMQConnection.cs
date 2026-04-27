using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consumer.Infrastructure
{
    public class RabbitMQConnection
    {
        private readonly IConnection _connection; // IConnection is an interface provided by RabbitMQ.Client library that represents a connection to RabbitMQ server. // _connection is used to create channels for communication with RabbitMQ server.

        public RabbitMQConnection()
        {
            var factory = new ConnectionFactory() // ConnectionFactory is a class provided by RabbitMQ.Client library to create connections to RabbitMQ server. // factory is used to configure the connection settings such as hostname, port, username, password, etc.
            { 
                HostName = "localhost" 
            };
            _connection = factory.CreateConnection(); // creates only once
        }
        public IConnection GetConnection() // GetConnection method is used to retrieve the established connection to RabbitMQ server. // It returns the IConnection instance that can be used by other classes (like RabbitMQConsumer) to create channels and communicate with RabbitMQ server.
        {
            return _connection;
        }
    }
}
