using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPlugin.Core.Models
{
    public class MedicionM2
    {
        private string _id;
        private double _largo;
        private double _ancho;


        public MedicionM2(string id, double largo, double ancho)
        {
            _id = id;
            _largo = largo;
            _ancho = ancho;
        }

    }
}
