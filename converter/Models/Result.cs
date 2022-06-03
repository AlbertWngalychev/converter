using System;
using System.Collections.Generic;

namespace converter.Models
{
    public partial class Result
    {
        public Result()
        {
            Statuses = new HashSet<Status>();
        }

        public long Id { get; set; }
        public string Title { get; set; } = null!;

        public virtual ICollection<Status> Statuses { get; set; }
    }
}
