using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordDB.Models
{
    public class ArtistRecordReview
    {
        public int ArtistId { get; set; }
        public int RecordId { get; set; }
        public string ArtistName { get; set; }
        public string RecordName { get; set; }
        public string Recorded { get; set; }
    }
}
