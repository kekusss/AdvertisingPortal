using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KewinWarzechaZad6.ViewModels
{
    public class AdvertismentDetailsVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string CategoryName { get; set; }
        public float Price { get; set; }
        public DateTime DateOfAddition { get; set; }
        public string Email{ get; set; }
        public string Description { get; set; }
        public string Phone { get; set; }

        public AdvertismentDetailsVM(string title, string categoryName, float price, DateTime dateOfAddition, string email, string description, string phone, int id)
        {
            Title = title;
            CategoryName = categoryName;
            Price = price;
            DateOfAddition = dateOfAddition;
            Email = email;
            Description = description;
            Phone = phone;
            Id = id;
        }
    }
}