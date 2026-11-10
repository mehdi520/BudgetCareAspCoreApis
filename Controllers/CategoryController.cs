using BudgetCareApis.Models.Dtos.Category;
using BudgetCareApis.Models.Dtos.Notes;
using BudgetCareApis.Models.ReqModels.Notes;
using BudgetCareApis.Models.ResModels;
using BudgetCareApis.Models.ResModels.Base;
using BudgetCareApis.Models.ResModels.Notes;
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
		private readonly INoteService _noteService;

		private readonly ILogger<PubController> _logger;
		private readonly IConfiguration Configuration;

		public CategoryController(ILogger<PubController> logger, IUserService user, INoteService noteService, IConfiguration configuration, IWebHostEnvironment webHostEnvironment) : base(configuration, webHostEnvironment)
		{
			_logger = logger;
			_userService = user;
			_noteService = noteService;
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



		// ---------------- Notebooks (Smart Notes) ----------------

		/// <summary>
		/// User's notebooks with note counts.
		/// Optional: pageNo/pageSize for paging (pageNo = 0 returns all), search filters by title.
		/// Response includes currentPage, pageSize, totalPage, totalCount.
		/// </summary>
		[HttpGet]
		public async Task<IActionResult> GetUserNotesCats(int pageNo = 0, int pageSize = 10, string? search = null)
		{
			var req = new GetNoteBooksReqModel { pageNo = pageNo, pageSize = pageSize, search = search };
			var res = await _noteService.GetNoteBooks(req, getLoggedinUserId());
			return Ok(res);
		}

		/// <summary>Deletes a notebook with all of its notes and their attachment files.</summary>
		[HttpGet]
		public async Task<IActionResult> delNoteCat(int catId)
		{
			var res = await _noteService.DeleteNoteBook(catId, getLoggedinUserId());
			return Ok(res);
		}

		[HttpPost]
		public async Task<IActionResult> addOrUpdateNoteCat(NoteBookModel req)
		{
			req.UserId = getLoggedinUserId();
			var res = await _noteService.AddUpdateNoteBook(req);
			return Ok(res);
		}

	}
	
}
