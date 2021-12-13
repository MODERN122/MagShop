using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Entities
{
    public abstract class BaseDateTimeEntity:IAggregateRoot
    {
        public BaseDateTimeEntity(string userId="") { ChangedByUserId = userId; }
        public DateTime PublicationDateTime { get; set; } = DateTime.UtcNow;
        public DateTime ChangedDateTime { get; set; }
        public string ChangedByUserId { get; set; }
    }
}
