using BudgetCareApis.Data.Entities;
using BudgetCareApis.Models.Dtos.bond;
using BudgetCareApis.Models.ReqModels.bond;
using BudgetCareApis.Models.ResModels;
using BudgetCareApis.Models.ResModels.Base;
using BudgetCareApis.Models.ResModels.bond;
using BudgetCareApis.Services.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;

namespace BudgetCareApis.Controllers
{

	//[Authorize]
	[ApiController]
	[Route("api/[controller]/[action]")]
	public class BondsController : BaseController
	{
		private IBondService _bondService;

		private readonly ILogger<PubController> _logger;
		private readonly IConfiguration Configuration;

		public BondsController(ILogger<PubController> logger, IBondService bond, IConfiguration configuration, IWebHostEnvironment webHostEnvironment) : base(configuration, webHostEnvironment)
		{
			_logger = logger;
			_bondService = bond;
			Configuration = configuration;
		}

		[HttpGet]
		public async Task<IActionResult> GetRecordsAvailableYears()
		{
			var res = new GetRecordAvailYearsResModel();
			res = await _bondService.getRecordsAvailableYears();
			return Ok(res);
		}


		[HttpGet]
		public async Task<IActionResult> GetScheduleDrawByYear(int yearId)
		{
			var res = new GetDrawSchedulesByYearResModel();
			res = await _bondService.getDrawSchedulesByYear(yearId);
			return Ok(res);
		}

		[HttpPost]
		public async Task<IActionResult> AnalyzeUploadNewDrawResult([FromForm] CreateNewDrawReqModel req)
		{
			var res = new GetAnalyzeUploadNewDrawResModel();

			if (req.draw_no < 1)
			{
				res.Status = false;
				res.Message = "Draw number is required.";
			}
			else if (req.scheduleId < 1)
			{
				res.Status = false;
				res.Message = "Bond Type is required.";
			}
			else if (req.draw_date == DateTime.MinValue)
			{
				res.Status = false;
				res.Message = "Draw date is required.";
			}
			else if (string.IsNullOrWhiteSpace(req.first_prize_worth))
			{
				res.Status = false;
				res.Message = "First prize worth is required.";
			}
			else if (string.IsNullOrWhiteSpace(req.second_prize_worth))
			{
				res.Status = false;
				res.Message = "Second prize worth is required.";
			}
			else if (string.IsNullOrWhiteSpace(req.third_prize_worth))
			{
				res.Status = false;
				res.Message = "Third prize worth is required.";
			}
			else if (req.file == null || req.file.Length == 0)
			{
				res.Status = false;
				res.Message = "File is required.";
			}
			else
			{
				using var reader = new StreamReader(req.file.OpenReadStream());
				var content = await reader.ReadToEndAsync();

				// First prize (must be exactly 6 digits)
				var firstPrizeMatch = Regex.Match(content, @"First Prize.*?\n(\d{6})", RegexOptions.Singleline);
				var firstPrize = firstPrizeMatch.Success ? firstPrizeMatch.Groups[1].Value : null;

				if (string.IsNullOrWhiteSpace(firstPrize))
				{
					res.Status = false;
					res.Message = "Valid first prize bond number (6 digits) not found.";
					return Ok(res);
				}

				// Second prize numbers (extract and filter only 6-digit)
				var secondMatch = Regex.Match(content, @"Second Prize.*?\n([\d\s\t\r\n]+)", RegexOptions.Singleline);
				var secondNumbers = secondMatch.Success
					? secondMatch.Groups[1].Value
						.Replace("\r", "")
						.Replace("\n", "")
						.Split(new[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries)
						.Where(x => Regex.IsMatch(x, @"^\d{6}$"))
						.Distinct()
						.ToList()
					: new List<string>();

				// All 6-digit numbers in the file
				var allBonds = Regex.Matches(content, @"\b\d{6}\b")
					.Select(m => m.Value)
					.Distinct()
					.ToList();

				// Third prize numbers (exclude first and second)
				var thirdNumbers = allBonds
					.Except(secondNumbers)
					.Where(x => x != firstPrize)
					.ToList();

				// Build bond list safely
				req.bonds = new List<DrawWinsBondDataModel>();

				
				req.bonds.Add(new DrawWinsBondDataModel { BoundNo = firstPrize, Position = 1 });

				req.bonds.AddRange(
					secondNumbers
						.Where(x => int.TryParse(x, out _))
						.Select(x => new DrawWinsBondDataModel
						{
							BoundNo = x,
							Position = 2
						})
				);

				req.bonds.AddRange(
					thirdNumbers
						.Where(x => int.TryParse(x, out _))
						.Select(x => new DrawWinsBondDataModel
						{
							BoundNo = x,
							Position = 3
						})
				);

				req.file = null;

				res = await _bondService.analyzeUploadNewDrawResult(req);
			}

			return Ok(res);
		}


		//[HttpPost]
		//public async Task<IActionResult> AnalyzeUploadNewDrawResult([FromForm] CreateNewDrawReqModel req)
		//{
		//	var res = new GetAnalyzeUploadNewDrawResModel();

		//	if (req.draw_no < 1)
		//	{
		//		res.Status = false;
		//		res.Message = "Draw number is required.";
		//	}
		//	else if (req.scheduleId < 1)
		//	{
		//		res.Status = false;
		//		res.Message = "Bond Type is required.";
		//	}
		//	else if (req.draw_date == DateTime.MinValue)
		//	{
		//		res.Status = false;
		//		res.Message = "Draw date is required.";
		//		return Ok(res);
		//	}
		//	else if (req.first_prize_worth.IsNullOrEmpty())
		//	{
		//		res.Status = false;
		//		res.Message = "First prize worth is required.";
		//	}
		//	else if (req.second_prize_worth.IsNullOrEmpty())
		//	{
		//		res.Status = false;
		//		res.Message = "Second prize worth is required.";
		//	}
		//	else if (req.third_prize_worth.IsNullOrEmpty())
		//	{
		//		res.Status = false;
		//		res.Message = "Third prize worth is required.";
		//	}
		//	else if (req.file == null || req.file.Length == 0)
		//	{
		//		res.Status = false;
		//		res.Message = "File is required.";
		//	}
		//	else
		//	{

		//		using var reader = new StreamReader(req.file.OpenReadStream());
		//		var content = await reader.ReadToEndAsync();


		//		// First prize number
		//		var firstPrize = Regex.Match(content, @"First Prize.*?\n(\d{5,6})", RegexOptions.Singleline).Groups[1].Value;

		//		// Second prize numbers
		//		var secondMatch = Regex.Match(content, @"Second Prize.*?\n([\d\s]+)", RegexOptions.Singleline);
		//		//var secondNumbers = secondMatch.Success
		//		//    ? secondMatch.Groups[1].Value.Split(' ', StringSplitOptions.RemoveEmptyEntries)
		//		//    : Array.Empty<string>();

		//		var secondNumbers = secondMatch.Success
		//? secondMatch.Groups[1].Value
		//	.Replace("\r", "")  // Remove carriage returns
		//	.Replace("\n", "")  // Remove newlines
		//	.Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries)  // Split by tab, remove empty entries
		//: Array.Empty<string>();


		//		// All 5-6 digit numbers in file
		//		var allBonds = Regex.Matches(content, @"\b\d{5,6}\b")
		//	.Select(m => m.Value)
		//	.Distinct()
		//	.ToList();

		//		// Third prize numbers (exclude first and second)
		//		var thirdNumbers = allBonds
		//			.Except(secondNumbers)
		//			.Where(x => x != firstPrize)
		//			.ToList();

		//		// Build bond list
		//		req.bonds = new List<DrawWinsBond>
		//{
		//	new DrawWinsBond { BoundNo = int.Parse(firstPrize), Position = 1 }
		//};

		//		// Add second prize bonds
		//		req.bonds.AddRange(secondNumbers.Select(b => new DrawWinsBond
		//		{
		//			BoundNo = int.Parse(b),
		//			Position = 2
		//		}));

		//		// Add third prize bonds
		//		req.bonds.AddRange(thirdNumbers.Select(b => new DrawWinsBond
		//		{
		//			BoundNo = int.Parse(b),
		//			Position = 3
		//		}));

		//		// Remove file from request before sending
		//		req.file = null;


		//		res = await _bondService.analyzeUploadNewDrawResult(req);
		//	}

		//	return Ok(res);
		//}

		[HttpGet]
		public async Task<IActionResult> ImportDrawResult(int id)
		{
			var res = new BaseResponseModel();
			res = await _bondService.importDrawResult(id);
			return Ok(res);

		}

		[HttpGet]
		public async Task<IActionResult> GetDrawResult(int id)
		{
			var res = new GetDrawResultResModel();
			res = await _bondService.getDrawResult(id);
			return Ok(res);

		}
	}
}
