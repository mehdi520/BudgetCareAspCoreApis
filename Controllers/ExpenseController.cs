using BudgetCareApis.Models.Dtos.Category;
using BudgetCareApis.Models.ResModels.Base;
using BudgetCareApis.Models.ResModels;
using BudgetCareApis.Models.ResModels.User;
using BudgetCareApis.Services.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BudgetCareApis.Models.ReqModels;
using BudgetCareApis.Models.Dtos;
using Microsoft.IdentityModel.Tokens;

namespace BudgetCareApis.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]/[action]")]
	public class ExpenseController : BaseController
	{
		private IUserService _userService;

		private readonly ILogger<PubController> _logger;
		private readonly IConfiguration Configuration;

		public ExpenseController(ILogger<PubController> logger, IUserService user, IConfiguration configuration, IWebHostEnvironment webHostEnvironment) : base(configuration, webHostEnvironment)
		{
			_logger = logger;
			_userService = user;
			Configuration = configuration;
		}

		[HttpPost]
		public async Task<IActionResult> GetUserExpense(GetTransReqModel req)
		{
			var res = new GetTransResModel();
			var loginuserId = getLoggedinUserId();
			res = await _userService.getExpense(req,loginuserId);
			return Ok(res);
		}

		[HttpGet]
		public async Task<IActionResult> delExpense(int expenseId)
		{
			var res = new BaseResponseModel();
			//var loginuserId = getLoggedinUserId();
			res = await _userService.delExpense(expenseId);
			return Ok(res);
		}

		[HttpPost]
		public async Task<IActionResult> addOrUpdateExpense(TransDataModel req)
		{
			var res = new BaseResponseModel();
			if (req.Amount < 1)
			{
				res.Status = false;
				res.Message = "Amount is required";
			}
			else if (req.Desciption.IsNullOrEmpty())
			{
				res.Status = false;
				res.Message = "Description is required";
			}
			else if (req.CatId == 0)
			{
				res.Status = false;
				res.Message = "Category is required";
			}
			else if (req.Date == default(DateOnly))
			{
				res.Status = false;
				res.Message = "Date is required";
			}
			else
			{

				var loginuserId = getLoggedinUserId();
				req.UserId = loginuserId;
				res = await _userService.addUpdateExpense(req);
			}

			return Ok(res);
		}
	}

}
