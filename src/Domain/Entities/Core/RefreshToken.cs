﻿using System;

namespace EventSourcingExample.Domain.Entities.Core
{
    public class RefreshToken
    {
        public long Id { get; set; }
        public Guid UserPublicId { get; set; }
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}