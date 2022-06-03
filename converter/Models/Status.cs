using System;
using System.Collections.Generic;

namespace converter.Models
{
    public partial class Status
    {
        public long Id { get; set; }
        public long DateTime { get; set; }
        public long? ResultId { get; set; }
        public long ConvertId { get; set; }

        public virtual Convert Convert { get; set; } = null!;
        public virtual Result? Result { get; set; }
    }
}
