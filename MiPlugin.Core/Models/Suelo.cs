using MiPlugin.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPlugin.Core.Models
{
    internal class Suelo : Medir
    {
        private string _id;
        private double _largo;
        private double _ancho;



        public Suelo(string id, double largo, double ancho)
        {
            _id = id;
            _largo = largo;
            _ancho = ancho;
        }


        public double Perimetro()
        {
            return _largo * 2 + 2 * _ancho;
        }

        public double Area()
        {
            return _largo * _ancho;
        }

        public double Volumen()
        {
            throw new NotImplementedException();
        }
    }
}
