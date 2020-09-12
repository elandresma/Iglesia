using Iglesia.Common.Entities;
using System;
using System.Collections.Generic;

namespace Iglesia.Web.Data.Entities
{
    public class Meeting
    {
        public int Id { get; set; }

        public Church Church { get; set; }

        public DateTime Date { get; set; }

        public ICollection<Assistance> Assistances { get; set; }

    }
}
