using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Todo.Domain
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class TodoRecord
    {
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; } = null!;

        public DateTime CreatedAtUtc { get; set; }

        public TodoRecord()
        {
            Id = Guid.NewGuid();
        }

        private string GetDebuggerDisplay()
        {
            return $"{Id}-{Title}";
        }
    }
}
