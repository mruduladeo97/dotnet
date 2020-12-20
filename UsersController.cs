using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserAuthentication.Models;

namespace UserAuthentication.Controllers
{
    public class UsersController : Controller
    {


        private bool checkSession()
        {
            if ((string)Session["UserName"] != null)
                return true;
            return false;
        }

        private bool checkCookieRememberMe()
        {
            HttpCookie objCookie = Request.Cookies["UserCredentials"];
            bool flag = false;
            if (objCookie != null)
            {
                try
                {
                    SqlConnection cn = new SqlConnection();
                    cn.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=Project;Integrated Security=True;Pooling=False";
                    cn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "select * from UserData where UserName = @UserName and Password = @Password;";
                    cmd.Parameters.AddWithValue("@UserName", objCookie.Values["UserName"]);
                    cmd.Parameters.AddWithValue("@Password", objCookie.Values["Password"]);
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        if (!checkSession())
                        {
                            Session["UserName"] = dr["UserName"];
                        }
                        flag = true;
                    }
                    else
                        flag = false;

                }
                catch
                {
                    flag = false;
                }

            }
            return flag;
        }


        // GET: Users
        public ActionResult Index()
        {
            if (checkCookieRememberMe() || checkSession())
            {
                return RedirectToAction("Home");
            }
            return View();
        }


        // GET: Users/Register
        public ActionResult Register()
        {
            UserData userObj = new UserData();
            List<SelectListItem> citylist = new List<SelectListItem>();

            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=Project;Integrated Security=True;Pooling=False";
            cn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from City;";

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                citylist.Add(new SelectListItem { Text = dr["City_Name"].ToString(), Value = dr["CityID"].ToString() });
            }
            userObj.CitiesList = citylist;
            dr.Close();
            cn.Close();
            return View(userObj);

        }

        // POST: Users/register
        [HttpPost]
        public ActionResult Register(UserData userObj)

        {
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=Project;Integrated Security=True;Pooling=False";

            cn.Open();
            SqlTransaction tr = cn.BeginTransaction();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.Transaction = tr;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Insert into UserData (UserName, Password, Full_Name, EmailId, CityID, PhoneNo)values(@UserName, @Password, @Full_Name, @EmailId, @CityID, @PhoneNo);";
            cmd.Parameters.AddWithValue("@UserName", userObj.UserName);
            cmd.Parameters.AddWithValue("@Password", userObj.Password);
            cmd.Parameters.AddWithValue("@Full_Name", userObj.Full_Name);
            cmd.Parameters.AddWithValue("@EmailId", userObj.EmailId);
            cmd.Parameters.AddWithValue("@CityID", userObj.CityID);
            cmd.Parameters.AddWithValue("@PhoneNo", userObj.Phone);
            try
            {
                // TODO: Add insert logic here

                cmd.ExecuteNonQuery();
                tr.Commit();
                return RedirectToAction("Index");

            }
            catch
            {
                tr.Rollback();
                return View();
            }
            finally
            {
                cn.Close();

            }
        }

        // GET: Users/Edit/5
        public ActionResult Login()
        {
            if (checkCookieRememberMe() || checkSession())
            {
                return RedirectToAction("Home");
            }

            UserData userObj = new UserData();
            return View(userObj);

        }

        // POST: Users/Edit/5
        [HttpPost]
        public ActionResult Login(UserData userObj)
        {
            if (checkCookieRememberMe() || checkSession())
            {
                return RedirectToAction("Home");
            }
            try
            {
                // TODO: Add update logic here

                SqlConnection cn = new SqlConnection();
                cn.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=Project;Integrated Security=True;Pooling=False";
                cn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select * from UserData where UserName = @UserName and Password = @Password;";
                cmd.Parameters.AddWithValue("@UserName", userObj.UserName);
                cmd.Parameters.AddWithValue("@Password", userObj.Password);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    Session["UserName"] = dr["UserName"];

                    if (userObj.isActive)
                    {
                        HttpCookie objCookie = new HttpCookie("UserCredentials");
                        objCookie.Expires = DateTime.Now.AddDays(1);
                        objCookie.Values["UserName"] = userObj.UserName;
                        objCookie.Values["Password"] = userObj.Password;
                        Response.Cookies.Add(objCookie);
                    }
                }
                cn.Close();
                return RedirectToAction("Home");
            }
            catch
            {
                return View();
            }
        }


        // GET:
        public ActionResult Logout()
        {
            if (!checkCookieRememberMe() && !checkSession())
            {
                return RedirectToAction("Index");
            }
            Session.Abandon();
            HttpCookie objCookie = Request.Cookies["UserCredentials"];
            if (objCookie != null)
            {
                objCookie.Expires = DateTime.Now.AddDays(-1);
                objCookie.Values["UserName"] = "";
                objCookie.Values["Password"] = "";
                Response.Cookies.Add(objCookie);

            }

            return RedirectToAction("Index");
        }



        // GET: Users/Details/5
        public ActionResult Home()
        {

            if (!checkCookieRememberMe() && !checkSession())
            {
                return RedirectToAction("Index");
            }
            UserData userObj = new UserData();

            try
            {
                SqlConnection cn = new SqlConnection();
                cn.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=Project;Integrated Security=True;Pooling=False";

                cn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select * from UserData where UserName = @UserName";
                cmd.Parameters.AddWithValue("@UserName", (string)Session["UserName"]);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    userObj.UserName = (string)dr["UserName"];
                    userObj.Full_Name = (string)dr["Full_Name"];
                    userObj.EmailId = (string)dr["EmailId"];
                    userObj.Phone = (string)dr["PhoneNo"];
                    ViewBag.user = userObj;
                }
                cn.Close();

            }
            catch
            {

            }
            return View();

        }

        // GET: Users/Edit/5
        public ActionResult Update()
        {
            if (!checkCookieRememberMe() && !checkSession())
            {
                return RedirectToAction("Index");
            }
            #region
            UserData userObj = new UserData();
            List<SelectListItem> citylist = new List<SelectListItem>();

            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=Project;Integrated Security=True;Pooling=False";

            cn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from City;";

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                citylist.Add(new SelectListItem { Text = dr["City_Name"].ToString(), Value = dr["CityID"].ToString() });
            }
            userObj.CitiesList = citylist;
            dr.Close();
            #endregion



            #region
            cmd.CommandText = "select * from UserData where UserName = @UserName;";
            cmd.Parameters.AddWithValue("@UserName", (string)Session["UserName"]);
            dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                userObj.UserName = (string)dr["UserName"];
                userObj.Full_Name = (string)dr["Full_Name"];
                userObj.EmailId = (string)dr["EmailId"];
                userObj.Phone = (string)dr["PhoneNo"];
                userObj.CityID = (int)dr["CityID"];
            }

            dr.Close();

            cn.Close();

            #endregion
            return View(userObj);
        }

        // POST: Users/Edit/5
        [HttpPost]
        public ActionResult Update(UserData userObj)
        {
            if (!checkCookieRememberMe() && !checkSession())
            {
                return RedirectToAction("Index");
            }
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=Project;Integrated Security=True;Pooling=False";

            cn.Open();
            SqlTransaction tr = cn.BeginTransaction();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.Transaction = tr;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Update  UserData set Password=@Password, Full_Name=@Full_Name , EmailId=@EmailId, CityID=@CityID, PhoneNo=@PhoneNo where UserName =@UserName ;";
            cmd.Parameters.AddWithValue("@UserName", userObj.UserName);
            cmd.Parameters.AddWithValue("@Password", userObj.Password);
            cmd.Parameters.AddWithValue("@Full_Name", userObj.Full_Name);
            cmd.Parameters.AddWithValue("@EmailId", userObj.EmailId);
            cmd.Parameters.AddWithValue("@CityID", userObj.CityID);
            cmd.Parameters.AddWithValue("@PhoneNo", userObj.Phone);
            try
            {
                cmd.ExecuteNonQuery();
                tr.Commit();
                return RedirectToAction("Logout");

            }
            catch
            {
                tr.Rollback();
                return View();
            }
            finally
            {
                cn.Close();

            }
        }


    }
}