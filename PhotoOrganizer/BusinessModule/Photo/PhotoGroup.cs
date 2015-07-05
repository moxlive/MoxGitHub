using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoOrganizer.BusinessModule.Photos
{
    public sealed class PhotoGroup
    {
        public DateTime Date { get; set; }

        public string CustomSeqNum { get; set; }

        public string ScanSeqNum { get; set; }

        public Photo Front { get; set; }

        public Photo Back { get; set; }

    }
}
