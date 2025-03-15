using BudgetCareApis.Models.ReqModels.Users;
using BudgetCareApis.Models.ResModels.Base;
using BudgetCareApis.Models.ResModels.User;
using BudgetCareApis.Services.services;
using Microsoft.AspNetCore.Mvc;

namespace BudgetCareApis.Controllers
{


	[ApiController]
	[Route("api/[controller]/[action]")]
	public class PubController : BaseController
	{
		private IUserService _userService;

		private readonly ILogger<PubController> _logger;
		private readonly IConfiguration Configuration;

		public PubController(ILogger<PubController> logger, IUserService user, IConfiguration configuration,  IWebHostEnvironment webHostEnvironment) : base(configuration, webHostEnvironment)
		{
			_logger = logger;
			_userService = user;
			Configuration = configuration;
		}

		[HttpPost]
		public async Task<ActionResult> Register(AuthenticationReqModel req)
		{
			var res = new BaseResponseModel();
			if (string.IsNullOrEmpty(req.Name))
			{
				res.Status = false;
				res.Message = "Name is required";
			}
			else if (string.IsNullOrEmpty(req.Email))
			{
				res.Status = false;
				res.Message = "Email is required";
			}
			else if (string.IsNullOrEmpty(req.Phone))
			{
				res.Status = false;
				res.Message = "Mobile is required";
			}
			else if (string.IsNullOrEmpty(req.Password))
			{
				res.Status = false;
				res.Message = "Password is required";
			}
			//else if (req.UserType < 1)
			//{
			//	res.Status = false;
			//	res.Message = "Please select registration type.";
			//}
			else
			{


				var dbres = await _userService.register(req);
				res = dbres;
				//if (res.Status)
				//{
				//	res.ExtraMessage = GetTempJwt(res.ExtraMessage);
				//}
			}
			return Ok(res);
		}

		[HttpPost]
		public async Task<ActionResult> login(AuthenticationReqModel req)
		{
			var res = new LoginResModel();
			if (string.IsNullOrEmpty(req.Email))
			{
				res.Status = false;
				res.Message = "Email is required";
			}
			//else if (string.IsNullOrEmpty(req.Mobile))
			//{
			//	res.Status = false;
			//	res.Message = "Mobile is required";
			//}
			else if (string.IsNullOrEmpty(req.Password))
			{
				res.Status = false;
				res.Message = "Password is required";
			}
			else
			{
				var dbres = await _userService.login(req);
				res = dbres;
				if (res.Status)
				{
					res.access_Token = GetJwt(res.data);
				}
			}
			return Ok(res);
		}

	}
}
