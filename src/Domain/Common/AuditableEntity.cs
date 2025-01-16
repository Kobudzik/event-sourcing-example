using EventSourcingExample.Domain.Entities.Core;
using System;

namespace EventSourcingExample.Domain.Common
{
    public abstract class AuditableEntity
    {
        public DateTime CreatedDateUtc { get; set; }

        /// <summary>
        /// User's Id
        /// </summary>
        public string CreatedById { get; set; }
        public UserAccount CreatedBy { get; set; }

        public DateTime? LastModifiedDateUtc { get; set; }

        /// <summary>
        /// User's Id
        /// </summary>
        public string LastModifiedById { get; set; }
        public UserAccount LastModifiedBy { get; set; }
    }
}
