using System;

namespace Messages.Users.Interfaces
{
    public interface IUserCreated
    {
        public Guid UserId { get; set; }
    }
}
