using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serial_Numbers_Distributor.Models
{
    /// <summary>
    /// Файл, из которого считывются серийные номера
    /// </summary>
    internal class SerialsFile
    {
        public SerialsFile(string path, string name)
        {
            Path = path;
            Name = name;
            Content = new List<string>();
        }

        public string Path { get; set; }

        public string Name { get; set; }

        public List<string> Content { get; set; }
    }
}
