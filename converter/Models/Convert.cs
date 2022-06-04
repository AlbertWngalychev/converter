using System;
using System.Collections.Generic;

namespace converter.Models
{
    public partial class Convert
    {
        public Convert()
        {
            Statuses = new HashSet<Status>();
        }

        public long Id { get; set; }
        public string FileName { get; set; } = null!;
        public string ContentType { get; set; } = null!;

        public virtual ICollection<Status> Statuses { get; set; }
    }
}
