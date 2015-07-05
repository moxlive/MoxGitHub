using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoOrganizer.BusinessModule.Photos
{
    public sealed class PickedLocalPhoto : Photo
    {
        public string Uri { get; set; }
    }
}
