using BudgetCareApis.Models.Dtos;
using BudgetCareApis.Models.Dtos.Users;
using BudgetCareApis.Models.ReqModels.Users;
using BudgetCareApis.Models.ResModels;
using BudgetCareApis.Models.ResModels.Base;
using BudgetCareApis.Models.ResModels.User;
using BudgetCareApis.Services.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BudgetCareApis.Controllers
{

	[Authorize]
	[ApiController]
	[Route("api/[controller]/[action]")]
	public class UserController : BaseController
	{
		private IUserService _userService;

		private readonly ILogger<PubController> _logger;
		private readonly IConfiguration Configuration;

		public UserController(ILogger<PubController> logger, IUserService user, IConfiguration configuration,  IWebHostEnvironment webHostEnvironment) : base(configuration, webHostEnvironment)
		{
			_logger = logger;
			_userService = user;
			Configuration = configuration;
		}

		[HttpGet]
		public async Task<IActionResult> GetProfile()
		{
			var res = new GetUserResModel();
			 var loginuserId = getLoggedinUserId();
			res = await _userService.getUserById(loginuserId);
			return Ok(res);
		}

		[HttpPost]
		public async Task<IActionResult> updateProfile(UserViewModel req)
		{
			var res = new BaseResponseModel();
			if (string.IsNullOrEmpty(req.Name))
			{
				res.Status = false;
				res.Message = "Name is required";
			}
			else if (string.IsNullOrEmpty(req.Phone))
			{
				res.Status = false;
				res.Message = "Phone is required";
			}
			else
			{

				var loginuserId = getLoggedinUserId();
				req.Id = loginuserId;
				res = await _userService.updateProfile(req);
			}
			return Ok(res);
		}

		[HttpPost]
		public async Task<IActionResult> changePass(ChnagePassReqModel req)
		{
			var res = new BaseResponseModel();
			if (string.IsNullOrEmpty(req.oldPass))
			{
				res.Status = false;
				res.Message = "Current password is required";
			}
			else if (string.IsNullOrEmpty(req.newPass))
			{
				res.Status = false;
				res.Message = "New Password is required";
			}
			else
			{

				var loginuserId = getLoggedinUserId();
				req.userId = loginuserId;
				res = await _userService.changePassword(req);
			}
			return Ok(res);
		}

		[HttpGet]
		public async Task<IActionResult> summary()
		{
			var res = new GetTotalsResModel();
			var loginuserId = getLoggedinUserId();
			res = await _userService.getUserSummary(loginuserId);
			return Ok(res);
		}


		[HttpGet]
		public async Task<IActionResult> graphSummary()
		{
			var res = new GetGraphResModel();
			var loginuserId = getLoggedinUserId();
			res = await _userService.getUserGraphData(loginuserId);
			return Ok(res);
		}
	}
}
