using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serial_Numbers_Distributor.Models
{
    internal class Distributor
    {
        public Distributor(SerialsFile currentFile)
        {
            CurrentFile = currentFile;
        }

        public Distributor() { }

        public SerialsFile CurrentFile { get; set; }

        public int N { get; set; } = 0;
    }
}
