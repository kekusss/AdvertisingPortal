using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KewinWarzechaZad6.ViewModels
{
    public class CategoryVM
    {
        //Pola VM 
        public string Name { get; set; }
        public string Photo { get; set; }
        //Konstruktor
        public CategoryVM(string name, string photo)
        {
            Name = name;
            Photo = photo;
        }
    }
}