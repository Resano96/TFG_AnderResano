using MiPlugin.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPlugin.Core.Models
{
    public class Muro : Medir
    {
        private string _id;
        private double _largo;
        private double _alto;
        
        

        public Muro(string id, double largo, double alto)
        {
            _id = id;
            _largo = largo;
            _alto = alto;
        }

        public double Area()
        {
            return _largo * _alto;
        }


        public double Perimetro()
        {
            return _largo;
        }

        public double Volumen()
        {
            throw new NotImplementedException();
        }
        public string Mostrar()
        {
            double area =Area();
            double perimetro = Perimetro();
            return $"El id es :{_id}, \tel perimetro es {Perimetro()} \ty el area es {Area()}";
        }
    }
}
