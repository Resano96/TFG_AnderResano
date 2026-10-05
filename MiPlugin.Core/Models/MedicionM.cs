using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPlugin.Core.Models
{
    internal class MedicionM
    {
        private string _id;
        private double _largo;

        public MedicionM(string id, double largo)
        {
            _id = id;
            _largo = largo;
        }
    }
    
}
