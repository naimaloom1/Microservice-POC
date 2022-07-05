using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KYCService.Database;
using KYCService.Repository;

namespace KYCService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserKYCController : ControllerBase
    {

        private readonly IUserKYC _userRepo;
        public UserKYCController(IUserKYC userRepo)
        {
            _userRepo = userRepo;
        }
        [HttpGet]
        [Route("CheckHealth")]
        public IActionResult Check()
        {
            return Ok("Running and Up!");
        }

        [HttpGet]
        [Route("GetPendingApproval")]
        public IQueryable GetPendingApproval()
        {
            return _userRepo.GetPendingKYC();
        }


        [HttpGet("GetUser/{id}")]
        public UserKYCModel Get(int id)
        {
            return _userRepo.GetKYCUser(id);
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserKYC userKYC)
        {
            if (userKYC == null)
                return BadRequest(ModelState);

            if (!_userRepo.CreateUserKYC(userKYC))
            {
                ModelState.AddModelError("", $"Something went wrong while saving movie record of {userKYC.KYCStatus}");
                return StatusCode(500, ModelState);
            }

            return Ok(userKYC);

        }

        //// PUT api/<UserController>/5
        [HttpPut("{Id:int}")]
        public IActionResult Update(int Id, [FromBody] UserKYC userKYC)
        {
            if (userKYC == null || Id != userKYC.UserId)
                return BadRequest(ModelState);

            if (!_userRepo.UpdateKYC(userKYC))
            {
                ModelState.AddModelError("", $"Something went wrong while updating movie : {userKYC.KYCStatus}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
        ////// GET: api/UserKYC
        [HttpGet]
        [Route("GetApprovalStatus")]
        public IEnumerable<UserKYCModel> GetApprovalStatus()
        {
            return _userRepo.GetKYCStatus();
        }
        [HttpGet]
        [Route("GetApprovalPending")]
        public IEnumerable<UserKYCModel> GetApprovalPending()
        {
            return _userRepo.GetApprovalPending();
        }
    }
}
