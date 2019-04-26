using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KewinWarzechaZad6.Models
{
    public class AddAdvertisment
    {
        public int Id { get; set; }
        public string Title{ get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public DateTime DateOfAddition{ get; set; }
        public decimal Price{ get; set; }
        public int Phone { get; set; }
        public string Email { get; set; }
    }
}