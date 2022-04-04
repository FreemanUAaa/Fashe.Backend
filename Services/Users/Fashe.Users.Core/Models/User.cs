﻿using System;

namespace Fashe.Users.Core.Models
{
    public class User
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }
    }
}
