using BudgetCareApis.Models.ResModels.User;
using BudgetCareApis.Services.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
	}
}
