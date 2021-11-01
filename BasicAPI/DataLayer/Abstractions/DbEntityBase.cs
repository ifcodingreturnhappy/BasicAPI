using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Abstractions
{
    public abstract class DbEntityBase
    {
        [BsonId]
        public int Id { get; set; }
    }
}
