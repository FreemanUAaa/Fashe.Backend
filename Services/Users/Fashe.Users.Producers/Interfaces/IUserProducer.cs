using System;
using System.Threading.Tasks;

namespace Fashe.Users.Producers.Interfaces
{
    public interface IUserProducer
    {
        Task UserCreated(Guid userId);

        Task UserDeleted(Guid userId);
    }
}
