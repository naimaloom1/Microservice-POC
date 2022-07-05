using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebUI.Data;
using WebUI.Models;
using X.PagedList;

namespace WebUI.Controllers
{
    public class UsersController : Controller
    {
        private IConfiguration _configuration;
        string apiUrl = "";
        HttpClient client;
        public UsersController(IConfiguration iconfig)
        {
            _configuration = iconfig;
            // apiUrl = _configuration.GetValue<string>("baseurl:UserService");
            apiUrl = _configuration.GetValue<string>("baseurl:APIGetway");
            client = new HttpClient();
            client.BaseAddress = new Uri(apiUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<IActionResult> Index()
        {
            List<User>? users = new List<User>();
            HttpResponseMessage responseMessage = await client.GetAsync(apiUrl + "/User");
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                users = JsonConvert.DeserializeObject<List<User>>(responseData);
                return View(users);
            }
            return View(users);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,MobileNo,Title,FirstName,LastName,Email,Sex,Role,PasswordHash,Status,CreatedOn,CreatedBy,UpdatedOn,UpdatedBy")] User user)
        {
            if (ModelState.IsValid)
            {
                user.Status = true;
                user.CreatedOn = DateTime.Today;
                user.CreatedBy = "HR";
                string data = JsonConvert.SerializeObject(user);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync(apiUrl + "/User", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                return View();
            }
            return View();
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User model = new User();
            HttpResponseMessage response = client.GetAsync(apiUrl + "/User/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                model = JsonConvert.DeserializeObject<User>(data);
            }

            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,MobileNo,Title,FirstName,LastName,Email,Sex,Role,PasswordHash,Status,CreatedOn,CreatedBy,UpdatedOn,UpdatedBy")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    user.Status = true;
                    user.UpdatedOn = DateTime.Today;
                    user.CreatedOn = DateTime.Today;
                    user.CreatedBy = "HR";
                    user.UpdatedBy = "HR";
                    string data = JsonConvert.SerializeObject(user);
                    StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = client.PutAsync(apiUrl + "/User/" + user.UserId, content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    return View("Index", user);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

            }
            return View("Index", user);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User user = new User();
            HttpResponseMessage response = client.GetAsync(apiUrl + "/User/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                user = JsonConvert.DeserializeObject<User>(data);
            }
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (id == null)
            {
                return Problem("Entity set 'WebUIContext.User'  is null.");
            }
            User user = new User();
            HttpResponseMessage response = client.GetAsync(apiUrl + "/User" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                user = JsonConvert.DeserializeObject<User>(data);
            }
            if (user != null)
            {
                HttpResponseMessage resp = client.DeleteAsync(apiUrl + "/User/" + id).Result;
                if (resp.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }

            return RedirectToAction(nameof(Index));
        }
        private bool UserExists(int id)
        {
            if (id == null)
            {
                return false;
            }

            User model = new User();
            HttpResponseMessage response = client.GetAsync(apiUrl + "/User/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            if (model == null)
            {
                return false;
            }
            return true;
        }
    }
}
