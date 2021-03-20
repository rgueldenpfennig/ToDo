using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Todo.Domain
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class TodoRecord : EntityBase
    {
        [Required]
        public string Title { get; private set; } = null!;

        public TodoRecord(string title) : base(Guid.NewGuid())
        {
            Title = title;
        }

        private TodoRecord() : base()
        {
        }

        private string GetDebuggerDisplay()
        {
            return $"{Id}-{Title}";
        }
    }
}
