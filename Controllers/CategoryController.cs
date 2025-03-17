using BudgetCareApis.Models.Dtos.Category;
using BudgetCareApis.Models.ResModels;
using BudgetCareApis.Models.ResModels.Base;
using BudgetCareApis.Models.ResModels.User;
using BudgetCareApis.Services.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetCareApis.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]/[action]")]
	public class CategoryController : BaseController
	{
		private IUserService _userService;

		private readonly ILogger<PubController> _logger;
		private readonly IConfiguration Configuration;

		public CategoryController(ILogger<PubController> logger, IUserService user, IConfiguration configuration, IWebHostEnvironment webHostEnvironment) : base(configuration, webHostEnvironment)
		{
			_logger = logger;
			_userService = user;
			Configuration = configuration;
		}

		[HttpGet]
		public async Task<IActionResult> GetUserCats()
		{
			var res = new GetUserCatsResModel();
			var loginuserId = getLoggedinUserId();
			res = await _userService.getCatagories(loginuserId);
			return Ok(res);
		}

		[HttpGet]
		public async Task<IActionResult> delCat(int catId)
		{
			var res = new BaseResponseModel();
			//var loginuserId = getLoggedinUserId();
			res = await _userService.delCatagory(catId);
			return Ok(res);
		}

		[HttpPost]
		public async Task<IActionResult> addOrUpdateCat(CategoryDataModel req)
		{
			var res = new BaseResponseModel();
			var loginuserId = getLoggedinUserId();
			req.UserId = loginuserId;
			res = await _userService.addUpdateCatagories(req);
			return Ok(res);
		}
	}
	
}
