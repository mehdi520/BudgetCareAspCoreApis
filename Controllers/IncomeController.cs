using BudgetCareApis.Models.Dtos.Category;
using BudgetCareApis.Models.ResModels.Base;
using BudgetCareApis.Models.ResModels;
using BudgetCareApis.Models.ResModels.User;
using BudgetCareApis.Services.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BudgetCareApis.Models.ReqModels;
using BudgetCareApis.Models.Dtos;

namespace BudgetCareApis.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]/[action]")]
	public class IncomeController : BaseController
	{
		private IUserService _userService;

		private readonly ILogger<PubController> _logger;
		private readonly IConfiguration Configuration;

		public IncomeController(ILogger<PubController> logger, IUserService user, IConfiguration configuration, IWebHostEnvironment webHostEnvironment) : base(configuration, webHostEnvironment)
		{
			_logger = logger;
			_userService = user;
			Configuration = configuration;
		}

		[HttpGet]
		public async Task<IActionResult> GetUserIncome(GetTransReqModel req)
		{
			var res = new GetTransResModel();
			var loginuserId = getLoggedinUserId();
			res = await _userService.getIncomes(req,loginuserId);
			return Ok(res);
		}

		[HttpGet]
		public async Task<IActionResult> delIncome(int incomeId)
		{
			var res = new BaseResponseModel();
			//var loginuserId = getLoggedinUserId();
			res = await _userService.delIncome(incomeId);
			return Ok(res);
		}

		[HttpGet]
		public async Task<IActionResult> addOrUpdateIncome(TransDataModel req)
		{
			var res = new BaseResponseModel();
			//var loginuserId = getLoggedinUserId();
			res = await _userService.addUpdateIncome(req);
			return Ok(res);
		}
	}
	
}
