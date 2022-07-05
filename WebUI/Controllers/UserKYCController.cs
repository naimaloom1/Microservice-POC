using Microsoft.AspNetCore.Http;
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

namespace WebUI.Controllers
{
    public class UserKYCController : Controller
    {
        private IConfiguration _configuration;
        string apiUrl = "";
        HttpClient client;
        public UserKYCController(IConfiguration iconfig)
        {
            _configuration = iconfig;
            // apiUrl = _configuration.GetValue<string>("baseurl:KYCService");
            apiUrl = _configuration.GetValue<string>("baseurl:APIGetway");
            client = new HttpClient();
            client.BaseAddress = new Uri(apiUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<IActionResult> PendingKYC()
        {
            List<User> users = new List<User>();
            HttpResponseMessage responseMessage = await client.GetAsync(apiUrl + "/UserKYC/GetPendingApproval");
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                users = JsonConvert.DeserializeObject<List<User>>(responseData);
            }
            return View(users);
        }
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User model = new User();
            HttpResponseMessage response = client.GetAsync(apiUrl + "/UserKYC/GetUser/" + id).Result;
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
        public ActionResult Edit(int id, IFormCollection collection)
        {
            if (ModelState.IsValid)
            {
                UserKYC uk = new UserKYC();
                uk.KYCStatus = "Approval Pending";
                uk.CreatedOn = DateTime.Today;
                uk.UpdatedOn = DateTime.Today;
                uk.CreatedBy = "HR";
                uk.UpdatedBy = "HR";
                uk.UserId = id;
                string data = JsonConvert.SerializeObject(uk);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync(apiUrl+ "/UserKYC", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("PendingKYC");
                }
                return View();
            }
            return View();
        }
        public async Task<IActionResult> KYCApproval()
        {
            List<UserViewModel> users = new List<UserViewModel>();
            HttpResponseMessage responseMessage = await client.GetAsync(apiUrl + "/UserKYC/GetApprovalPending");
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                users = JsonConvert.DeserializeObject<List<UserViewModel>>(responseData);
            }
            return View(users);
        }
        public ActionResult ApprovalEdit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UserViewModel model = new UserViewModel();
            HttpResponseMessage response = client.GetAsync(apiUrl + "/UserKYC/GetUser/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                model = JsonConvert.DeserializeObject<UserViewModel>(data);
            }

            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApprovalEdit(int id, IFormCollection collection)
        {
            if (Convert.ToInt32(collection["UserKYCId"]) > 0)
            {
                UserKYC uk = new UserKYC();
                uk.KYCStatus = collection["KYCStatus"]== "Approve" ? "Approved" : "Rejected";
                uk.UpdatedOn = DateTime.Today;
                uk.UpdatedBy = "HR";
                uk.CreatedOn = DateTime.Today;
                uk.CreatedBy = "HR";
                uk.UserKYCId = Convert.ToInt32(collection["UserKYCId"]);
                uk.UserId = id;
                string data = JsonConvert.SerializeObject(uk);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PutAsync(apiUrl + "/UserKYC/" + id, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("KYCApproval");
                }
                return View();
            }
            return View();
        }
        public async Task<IActionResult> KYCStatus()
        {
            List<UserViewModel> users = new List<UserViewModel>();
            HttpResponseMessage responseMessage = await client.GetAsync(apiUrl + "/UserKYC/GetApprovalStatus");
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                users = JsonConvert.DeserializeObject<List<UserViewModel>>(responseData);
            }
            return View(users);
        }

    }
}
