using System;

namespace Todo.Domain
{
    public abstract class EntityBase
    {
        public Guid Id { get; private set; }

        public DateTime CreatedAtUtc { get; private set; }

        protected EntityBase(Guid id)
        {
            Id = id;
            CreatedAtUtc = DateTime.UtcNow;
        }

        protected EntityBase()
        {
        }
    }
}
