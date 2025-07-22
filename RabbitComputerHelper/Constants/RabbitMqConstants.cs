namespace RabbitComputerHelper.Constants
{
    internal static class RabbitMqConstants
    {
        public const string EventLogQueueName = "eventLog_queue";
        public const string GarageSensorQueueName = "garage_sensor_log";
        public const string TemperatureQueueName = "esp8266_amqp";

        public const string HostName = "192.168.50.2";
        public const string UserName = "theAdmin";
        public const string Password = "secret";
    }
}
