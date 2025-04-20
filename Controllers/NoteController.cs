using BudgetCareApis.Models.ReqModels;
using BudgetCareApis.Models.ResModels.Base;
using BudgetCareApis.Services.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BudgetCareApis.Models.ResModels.Notes;
using BudgetCareApis.Models.Dtos.Notes;
using Microsoft.IdentityModel.Tokens;

namespace BudgetCareApis.Controllers
{

	[Authorize]
	[ApiController]
	[Route("api/[controller]/[action]")]
	public class NoteController : BaseController
	{
		private IUserService _userService;

		private readonly ILogger<PubController> _logger;
		private readonly IConfiguration Configuration;

		public NoteController(ILogger<PubController> logger, IUserService user, IConfiguration configuration, IWebHostEnvironment webHostEnvironment) : base(configuration, webHostEnvironment)
		{
			_logger = logger;
			_userService = user;
			Configuration = configuration;
		}

		[HttpPost]
		public async Task<IActionResult> GetUserNotes(GetTransReqModel req)
		{
			var res = new GetNotesResModel();
			var loginuserId = getLoggedinUserId();
			res = await _userService.getNotes(req, loginuserId);
			return Ok(res);
		}

		[HttpGet]
		public async Task<IActionResult> delNote(int noteId)
		{
			var res = new BaseResponseModel();
			//var loginuserId = getLoggedinUserId();
			res = await _userService.delNote(noteId);
			return Ok(res);
		}

		[HttpPost]
		public async Task<IActionResult> addOrUpdateNote(NoteDataModel req)
		{
			var res = new BaseResponseModel();
			 if (req.Title.ToString().IsNullOrEmpty())
			{
				res.Status = false;
				res.Message = "Description is required";
			}
			else if (req.NoteBookId == 0)
			{
				res.Status = false;
				res.Message = "Category is required";
			}
			else if (req.Details.IsNullOrEmpty())
			{
				res.Status = false;
				res.Message = "Date is required";
			}
			else
			{

				var loginuserId = getLoggedinUserId();
				req.UserId = loginuserId;
				res = await _userService.addUpdateNote(req);
			}
			return Ok(res);
		}
	}

}


