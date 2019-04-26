using KewinWarzechaZad6.Models;
using KewinWarzechaZad6.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace KewinWarzechaZad6.Controllers
{
    public class HomeController : Controller
    {
        //zmienne przechowujące status logowania i id użytkownika zalogowanego
        public static bool isLoggedIn = false;
        public static int loggedInUserId = 0;
        /// <summary>
        /// Połączenie z Bazą danych
        /// </summary>
        AdvertisingPortalEntities database = new AdvertisingPortalEntities();
        /// <summary>
        /// Strona główna - wyszukiwarka ogłoszeń po tytule
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Przekierowanie po wyszukaniu ogłoszeń
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(string match)
        {
            var allAdvertisments = (from a in database.Advertisements
                                    join c in database.Categories
                                    on a.CategoryId equals c.Id
                                    select new
                                    {
                                        Id = a.Id,
                                        Title = a.Title,
                                        Category = c.Name,
                                        Price = a.Price,
                                        DateOfAddition = a.DateOfAddition
                                    }).ToList();
            allAdvertisments = allAdvertisments.Where(x => x.Title.ToLower().Contains(match.ToLower())).ToList();
            List<AllAdvertismentsVM> viewAllAdvertisments = new List<AllAdvertismentsVM>();
            foreach (var adv in allAdvertisments)
            {
                AllAdvertismentsVM advertisment = new AllAdvertismentsVM(adv.Title, adv.Category, Convert.ToSingle(adv.Price), Convert.ToDateTime(adv.DateOfAddition),adv.Id);
                viewAllAdvertisments.Add(advertisment);
            }
            return View("AllAdvertisments",viewAllAdvertisments);
        }
        /// <summary>
        /// Wyśiwetlanie wszystkich kategorii
        /// </summary>
        /// <returns></returns>
        public ActionResult Categories()
        {
            List<Categories> allCategories = database.Categories.ToList();

            List<CategoryVM> viewCategories = new List<CategoryVM>();
            foreach (var category in allCategories)
            {
                CategoryVM viewCategory = new CategoryVM(category.Name, category.Photo);
                viewCategories.Add(viewCategory);
            }
            return View(viewCategories);
        }
        /// <summary>
        /// Wyśiwetlanie ogłoszeń
        /// </summary>
        /// <returns></returns>
        public ActionResult AllAdvertisments()
        {
            var allAdvertisments = (from a in database.Advertisements
                                join c in database.Categories
                                on a.CategoryId equals c.Id
                                select new
                                {
                                    Id = a.Id,
                                    Title = a.Title,
                                    Category = c.Name,
                                    Price = a.Price,
                                    DateOfAddition = a.DateOfAddition
                                }).ToList();
                //allAdvertisments.Where(x => x.Title.Contains(match)).ToList();
            List<AllAdvertismentsVM> viewAllAdvertisments = new List<AllAdvertismentsVM>();
            foreach (var adv in allAdvertisments)
            {
                AllAdvertismentsVM advertisment = new AllAdvertismentsVM(adv.Title, adv.Category, Convert.ToSingle(adv.Price), Convert.ToDateTime(adv.DateOfAddition), adv.Id);
                viewAllAdvertisments.Add(advertisment);
            }
            return View(viewAllAdvertisments);
        }
        /// <summary>
        /// Dodawanie ogłoszenia
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddAdvertisment()
        {
            if (!isLoggedIn)
            {
                return View("LogIn");
            }
            return View();
        }
        /// <summary>
        /// Przechwycenie danych z formularza dodawania
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddAdvertisment(AddAdvertisment model)
        {
            Advertisements newadv = new Advertisements();
            newadv.Title = model.Title;
            newadv.CategoryId = model.CategoryId;
            newadv.Description = model.Description;
            newadv.Email = model.Email;
            newadv.Phone = model.Phone;
            newadv.Price = model.Price;
            newadv.DateOfAddition = model.DateOfAddition;
            newadv.UId = loggedInUserId;
            database.Advertisements.Add(newadv);
            database.SaveChanges();
            return View("Success");
        }
        /// <summary>
        /// Wyświetlenie 1 kategorii ogłoszeń
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public ActionResult OneCategoryAdvertisments(string category)
        {
            var allAdvertisments = (from a in database.Advertisements
                                    join c in database.Categories
                                    on a.CategoryId equals c.Id
                                    select new
                                    {
                                        Id = a.Id,
                                        Title = a.Title,
                                        Category = c.Name,
                                        CategoryId = a.CategoryId,
                                        Price = a.Price,
                                        DateOfAddition = a.DateOfAddition
                                    }).ToList();
            allAdvertisments = allAdvertisments.Where(x => x.Category == category).ToList();
            List<AllAdvertismentsVM> viewAllAdvertisments = new List<AllAdvertismentsVM>();
            foreach (var adv in allAdvertisments)
            {
                AllAdvertismentsVM advertisment = new AllAdvertismentsVM(adv.Title, adv.Category, Convert.ToSingle(adv.Price), Convert.ToDateTime(adv.DateOfAddition), adv.Id);
                viewAllAdvertisments.Add(advertisment);
            }
            return View("AllAdvertisments", viewAllAdvertisments);
        }
        /// <summary>
        /// Szczegóły ogłoszenia
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AdvertismentDetails(int id)
        {
            var adv = (from a in database.Advertisements
                       join c in database.Categories
                       on a.CategoryId equals c.Id
                       where a.Id == id
                       select new
                       {
                           Id = a.Id,
                           Title = a.Title,
                           Category = c.Name,
                           Price = a.Price,
                           DateOfAddition = a.DateOfAddition,
                           Description = a.Description,
                           Phone = a.Phone,
                           Email = a.Email
                       }).FirstOrDefault();
            AdvertismentDetailsVM advertisment = new AdvertismentDetailsVM(adv.Title, adv.Category, Convert.ToSingle(adv.Price), Convert.ToDateTime(adv.DateOfAddition), adv.Email, adv.Description, adv.Phone.ToString(), adv.Id); 

            return View(advertisment);
        }
        /// <summary>
        /// Logowanie - widok
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LogIn()
        {
            return View();
        }
        /// <summary>
        /// Wylogowywanie
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOut()
        {
            isLoggedIn = false;
            loggedInUserId = 0;
            return View("Index");
        }
        /// <summary>
        /// Logowanie - 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LogIn(LogIn model)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(model.Password);
            SHA512 shaM = new SHA512Managed();
            byte[] pass = shaM.ComputeHash(bytes);
            System.Text.Encoding enc = System.Text.Encoding.ASCII;

            string login = model.Login;
            string password = model.Password;
            /*string password = enc.GetString(pass);
            string pass2 = database.Users.Select(x => x.password).FirstOrDefault();
            if(password == pass2)
            {
                return View("Index");
            }*/
            if (database.Users.Where(x => x.password == password && x.username == login).Count() == 1)
            {
                loggedInUserId = database.Users.Where(x => x.password == password && x.username == login).FirstOrDefault().id;
                isLoggedIn = true;
            }
            else
            {
                return View("LogIn");
            }
            
            return View("Index");
        }
        /// <summary>
        /// Rejestracja - widok formularza
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        /// <summary>
        /// Rejestracja - po wypełnieniu formularza dodawanie użytkownika
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Register(Register model)
        {
            Users user = new Users();
            user.username = model.Login;
            user.password = model.Password;
            user.email = model.Email;
            database.Users.Add(user);
            database.SaveChanges();
            return View("RegistrationSuccess");
        }
        /// <summary>
        /// Zwraca widok z ogłoszeniami danego użytkownika
        /// </summary>
        /// <returns></returns>
        public ActionResult MyAdvertisments()
        {
            var allAdvertisments = (from a in database.Advertisements
                                    join c in database.Categories
                                    on a.CategoryId equals c.Id
                                    select new
                                    {
                                        Id = a.Id,
                                        Title = a.Title,
                                        Category = c.Name,
                                        CategoryId = a.CategoryId,
                                        Price = a.Price,
                                        DateOfAddition = a.DateOfAddition,
                                        UId = a.UId
                                    }).ToList();
            allAdvertisments = allAdvertisments.Where(x => x.UId == loggedInUserId).ToList();
            List<MyAdvertismentVM> viewMyAdvertisments = new List<MyAdvertismentVM>();
            foreach (var adv in allAdvertisments)
            {
                MyAdvertismentVM advertisment = new MyAdvertismentVM(adv.Title, adv.Category, Convert.ToSingle(adv.Price), Convert.ToDateTime(adv.DateOfAddition), adv.Id, Convert.ToInt32(adv.UId));
                viewMyAdvertisments.Add(advertisment);
            }
            return View("MyAdvertisments", viewMyAdvertisments);
        }
        /// <summary>
        /// Zwraca widok modyfikacji danego ogłoszenia
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>=
        public ActionResult ModifyAdvertisment(int id)
        {
            var adv = (from a in database.Advertisements
                       join c in database.Categories
                       on a.CategoryId equals c.Id
                       where a.Id == id
                       select new
                       {
                           Id = a.Id,
                           Title = a.Title,
                           Category = a.CategoryId,
                           Price = a.Price,
                           DateOfAddition = a.DateOfAddition,
                           Description = a.Description,
                           Phone = a.Phone,
                           Email = a.Email
                       }).FirstOrDefault();
            AdvertismentDetailsVM advertisment = new AdvertismentDetailsVM(adv.Title, adv.Category.ToString(), Convert.ToSingle(adv.Price), Convert.ToDateTime(adv.DateOfAddition), adv.Email, adv.Description, adv.Phone.ToString(), adv.Id);

            return View(advertisment);
        }
        /// <summary>
        /// Modyfikacja ogłoszenia
        /// </summary>
        /// <param name="details"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ModifyAdvertisment(AddAdvertisment model)
        {
            
            Advertisements newadv = database.Advertisements.Where(x=>x.Id == model.Id).FirstOrDefault();
            newadv.Title = model.Title;
            newadv.Description = model.Description;
            newadv.Email = model.Email;
            newadv.Phone = Convert.ToDecimal(model.Phone);
            newadv.Price = Convert.ToDecimal(model.Price);
            database.Entry(newadv).State = System.Data.Entity.EntityState.Modified;
            database.SaveChanges();
            return MyAdvertisments();
        }
        /// <summary>
        /// Usuwa dane ogłoszenie
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteAdvertisment(int id)
        {
            Advertisements adv = new Advertisements() { Id = id };
            database.Advertisements.Attach(adv);
            database.Advertisements.Remove(adv);
            database.SaveChanges();
            return MyAdvertisments();
        }

    }
}