using _NorthwindWebService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

namespace _NorthwindWebService
{
    /// <summary>
    /// Summary description for NorthwindDataService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class NorthwindDataService : System.Web.Services.WebService
    {
        const string _username = "goksel";
        const string _password = "1";
        public AuthHeader Kimlik;

        [WebMethod(EnableSession = true)]
        public string Login(string username, string password)
        {
            string key = string.Empty;
            if (username == _username && password == _password)
            {
                key = Guid.NewGuid().ToString();
                Session["LGN"] = key;
            }
            return key;
        }





        [WebMethod(EnableSession = true)]
        public List<Category> List(string key)
        {
            if (Session["LGN"] != null && Session["LGN"].ToString() == key)
            {
                using (NorthwindEntities db = new NorthwindEntities())
                {
                    return db.Categories.ToList();
                }
            }
            else
            {
                return null;
            }
        }

        [WebMethod]
        [SoapHeader("Kimlik")]
        public string Insert(Category category)
        {
            if (Kimlik.Username == "admin" && Kimlik.Password == "1")
            {
                try
                {
                    using (NorthwindEntities db = new NorthwindEntities())
                    {
                        db.Categories.Add(category);
                        db.SaveChanges();
                    }
                    return "Success";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            else
            {
                return "Yetkisiz";
            }
        }

        [WebMethod]
        public string Delete(int id)
        {
            try
            {
                using (NorthwindEntities db = new NorthwindEntities())
                {
                    Category category = db.Categories.Find(id);
                    db.Categories.Remove(category);
                    db.SaveChanges();
                }
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }

    public class AuthHeader : SoapHeader
    {
        public string Username { get; set; }
        public string Password { get; set; }

    }
}
