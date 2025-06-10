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
		public async Task<IActionResult> GetBondTypes()
		{
			var res = new GetBondTypesResModel();
			res = await _bondService.getBondTypes();
			return Ok(res);
		}


		[HttpGet]
		public async Task<IActionResult> GetDrawsByBondType(int bondType)
		{
			var res = new GetDrawsResModel();
			res = await _bondService.getDrawsByBondType(bondType);
			return Ok(res);
		}

		[HttpPost]
		public async Task<IActionResult> AddUpdateDrawsByBondType(DrawDataModel req)
		{
			var res = new BaseResponseModel();
			res = await _bondService.addUpdateDrawsByBondType(req);
			return Ok(res);
		}

	

		//[HttpPost]
		//public async Task<IActionResult> AnalyzeUploadNewDrawResult([FromForm] CreateNewDrawReqModel req)
		//{
		//	var res = new GetAnalyzeUploadNewDrawResModel();

		//	try
		//	{
		//		// Input validation
		//		if (req.draw_no < 1)
		//		{
		//			res.Status = false;
		//			res.Message = "Draw number is required.";
		//		}
		//		else if (req.draw_id < 1)
		//		{
		//			res.Status = false;
		//			res.Message = "Draw Id is required.";
		//		}
		//		else if (req.draw_date == DateTime.MinValue)
		//		{
		//			res.Status = false;
		//			res.Message = "Draw date is required.";
		//		}
		//		else if (string.IsNullOrWhiteSpace(req.first_prize_worth))
		//		{
		//			res.Status = false;
		//			res.Message = "First prize worth is required.";
		//		}
		//		else if (string.IsNullOrWhiteSpace(req.second_prize_worth))
		//		{
		//			res.Status = false;
		//			res.Message = "Second prize worth is required.";
		//		}
		//		else if (string.IsNullOrWhiteSpace(req.third_prize_worth))
		//		{
		//			res.Status = false;
		//			res.Message = "Third prize worth is required.";
		//		}
		//		else if (req.file == null || req.file.Length == 0)
		//		{
		//			res.Status = false;
		//			res.Message = "File is required.";
		//		}
		//		else
		//		{
		//			// Excel file processing
		//			using var stream = req.file.OpenReadStream();
		//			using var workbook = new ClosedXML.Excel.XLWorkbook(stream);
		//			var worksheet = workbook.Worksheets.First();

		//			string firstPrize = null;
		//			var secondPrizes = new List<string>();
		//			var allSixDigitNumbers = new HashSet<string>();

		//			foreach (var row in worksheet.RowsUsed())
		//			{
		//				foreach (var cell in row.Cells())
		//				{
		//					var cellValue = cell.GetFormattedString().Trim();

		//					// Match exact 6-digit values including leading zeroes
		//					if (Regex.IsMatch(cellValue, @"^\d{6}$"))
		//					{
		//						allSixDigitNumbers.Add(cellValue);
		//					}
		//					else if (cellValue.Contains("1 Prize of 1st", StringComparison.OrdinalIgnoreCase))
		//					{
		//						var nextRow = row.RowBelow();
		//						firstPrize = nextRow?.CellsUsed()
		//							.Select(c => c.GetFormattedString().Trim())
		//							.FirstOrDefault(val => Regex.IsMatch(val, @"^\d{6}$"));
		//					}
		//					else if (cellValue.Contains("2nd Winner", StringComparison.OrdinalIgnoreCase))
		//					{
		//						var nextRow = row.RowBelow();
		//						var prizeValues = nextRow?.CellsUsed()
		//							.Select(c => c.GetFormattedString().Trim())
		//							.Where(val => Regex.IsMatch(val, @"^\d{6}$"))
		//							.ToList();

		//						if (prizeValues != null)
		//							secondPrizes.AddRange(prizeValues);
		//					}
		//				}
		//			}

		//			if (string.IsNullOrWhiteSpace(firstPrize))
		//			{
		//				res.Status = false;
		//				res.Message = "Valid first prize bond number (6 digits) not found.";
		//				return Ok(res);
		//			}

		//			// Third prizes = all valid 6-digit numbers excluding first and second prizes
		//			var thirdPrizes = allSixDigitNumbers
		//				.Except(secondPrizes)
		//				.Where(x => x != firstPrize)
		//				.ToList();

		//			req.bonds = new List<DrawWinsBondDataModel>();

		//			// Add first prize
		//			req.bonds.Add(new DrawWinsBondDataModel
		//			{
		//				BoundNo = firstPrize,
		//				Position = 1
		//			});

		//			// Add second prizes
		//			req.bonds.AddRange(
		//				secondPrizes.Distinct()
		//					.Select(x => new DrawWinsBondDataModel
		//					{
		//						BoundNo = x,
		//						Position = 2
		//					}));

		//			// Add third prizes
		//			req.bonds.AddRange(
		//				thirdPrizes
		//					.Select(x => new DrawWinsBondDataModel
		//					{
		//						BoundNo = x,
		//						Position = 3
		//					}));

		//			req.file = null; // Clear uploaded file reference

		//			// Call your analysis service
		//			res = await _bondService.analyzeUploadNewDrawResult(req);
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		res.Status = false;
		//		res.Message = ex.ToString();
		//	}

		//	return Ok(res);
		//}

		[HttpPost]
		public async Task<IActionResult> AnalyzeUploadNewDrawResult([FromForm] CreateNewDrawReqModel req)
		{
			var res = new GetAnalyzeUploadNewDrawResModel();

			try
			{
				// Input validation
				if (req.draw_no < 1)
				{
					res.Status = false;
					res.Message = "Draw number is required.";
				}
				else if (req.draw_id < 1)
				{
					res.Status = false;
					res.Message = "Draw Id is required.";
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
					// Excel file processing
					using var stream = req.file.OpenReadStream();
					using var workbook = new ClosedXML.Excel.XLWorkbook(stream);
					var worksheet = workbook.Worksheets.First();

					var firstPrizes = new List<string>();
					var secondPrizes = new List<string>();
					var allSixDigitNumbers = new HashSet<string>();

					foreach (var row in worksheet.RowsUsed())
					{
						foreach (var cell in row.Cells())
						{
							var cellValue = cell.GetFormattedString().Trim();

							// Collect all 6-digit numbers
							if (Regex.IsMatch(cellValue, @"^\d{6}$"))
							{
								allSixDigitNumbers.Add(cellValue);
							}
							else if ((cellValue.Contains("1 Prize of 1st", StringComparison.OrdinalIgnoreCase)) || cellValue.Contains("2 Prize of 1st", StringComparison.OrdinalIgnoreCase))
							{
								var currentRow = row.RowBelow();
								while (currentRow != null && currentRow.CellsUsed().Any())
								{
									var prizeValues = currentRow.CellsUsed()
										.Select(c => c.GetFormattedString().Trim())
										.Where(val => Regex.IsMatch(val, @"^\d{6}$"))
										.ToList();

									if (prizeValues.Any())
										firstPrizes.AddRange(prizeValues);
									else
										break;

									currentRow = currentRow.RowBelow();
								}
							}
							else if (cellValue.Contains("2nd Winner", StringComparison.OrdinalIgnoreCase))
							{
								var currentRow = row.RowBelow();
								while (currentRow != null && currentRow.CellsUsed().Any())
								{
									var prizeValues = currentRow.CellsUsed()
										.Select(c => c.GetFormattedString().Trim())
										.Where(val => Regex.IsMatch(val, @"^\d{6}$"))
										.ToList();

									if (prizeValues.Any())
										secondPrizes.AddRange(prizeValues);
									else
										break;

									currentRow = currentRow.RowBelow();
								}
							}
						}
					}

					if (firstPrizes.Count == 0)
					{
						res.Status = false;
						res.Message = "Valid first prize bond numbers (6 digits) not found.";
						return Ok(res);
					}

					// Third prizes = all 6-digit numbers excluding first and second prizes
					var thirdPrizes = allSixDigitNumbers
						.Except(firstPrizes)
						.Except(secondPrizes)
						.ToList();

					req.bonds = new List<DrawWinsBondDataModel>();

					// Add first prizes
					req.bonds.AddRange(
						firstPrizes.Select(x => new DrawWinsBondDataModel
						{
							BoundNo = x,
							Position = 1
						}));

					// Add second prizes
					req.bonds.AddRange(
						secondPrizes.Distinct()
							.Select(x => new DrawWinsBondDataModel
							{
								BoundNo = x,
								Position = 2
							}));

					// Add third prizes
					req.bonds.AddRange(
						thirdPrizes
							.Select(x => new DrawWinsBondDataModel
							{
								BoundNo = x,
								Position = 3
							}));

					req.file = null; // Clear uploaded file reference

					// Call your analysis service
					res = await _bondService.analyzeUploadNewDrawResult(req);
				}
			}
			catch (Exception ex)
			{
				res.Status = false;
				res.Message = ex.ToString();
			}

			return Ok(res);
		}


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

		[Authorize]
		[HttpGet]
		public async Task<IActionResult> GetUserBonds(int typeId)
		{
			var res = new GetBondsResModel();
			var loginuserId = getLoggedinUserId();
			res = await _bondService.getUserBonds(loginuserId,typeId);
			return Ok(res);

		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> AddUpdateUserBond(BondDataModel req)
		{
			var res = new BaseResponseModel();
			var loginuserId = getLoggedinUserId();
			res = await _bondService.addUpdateUserBond(loginuserId, req);
			return Ok(res);

		}

		[Authorize]
		[HttpGet]
		public async Task<IActionResult> DeleteUserBond(int bondId )
		{
			var res = new BaseResponseModel();
			var loginuserId = getLoggedinUserId();

			res = await _bondService.deleteUserBond(loginuserId, bondId);
			return Ok(res);

		}



		[HttpGet]
		public async Task<IActionResult> DrawWinCheckSyncByDraw(int drawId)
		{
			var res = new BaseResponseModel();
			var loginuserId = getLoggedinUserId();

			res = await _bondService.DrawWinCheckSyncByDraw(drawId);
			return Ok(res);

		}

		[Authorize]
		[HttpGet]
		public async Task<IActionResult> GetUserWonBonds(string status)
		{
			var res = new GetUserWonBondsResModel();
			var loginuserId = getLoggedinUserId();

			res = await _bondService.GetUserWonBonds(loginuserId,status);
			return Ok(res);

		}

		[Authorize]
		[HttpGet]
		public async Task<IActionResult> UpdateUserWonBondStatus(string status, int wonId)
		{
			var res = new BaseResponseModel();
			var loginuserId = getLoggedinUserId();

			res = await _bondService.UpdateUserWonBondStatus(loginuserId, status, wonId);
			return Ok(res);

		}
	}
}
