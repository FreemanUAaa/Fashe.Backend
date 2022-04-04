namespace Fashe.Users.Producers.Contracts
{
    public static class RabbitMQContracts
    {
        public static string UserCreated => "queue:user-created";

        public static string UserDeleted => "queue:user-deleted";
    }
}
