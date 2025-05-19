using BudgetCareApis.Data;
using BudgetCareApis.Data.Entities;
using BudgetCareApis.Models.Dtos.bond;
using BudgetCareApis.Models.Dtos.Category;
using BudgetCareApis.Models.ReqModels.bond;
using BudgetCareApis.Models.ResModels;
using BudgetCareApis.Models.ResModels.Base;
using BudgetCareApis.Models.ResModels.bond;
using BudgetCareApis.Services.services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BudgetCareApis.Services.repository
{


	public class BondService : IBondService
	{
		private readonly BudgetCareDBContext _context;

		public BondService(BudgetCareDBContext context)
		{
			_context = context;
		}

		public async Task<GetAnalyzeUploadNewDrawResModel> analyzeUploadNewDrawResult(CreateNewDrawReqModel req)
		{
			var res = new GetAnalyzeUploadNewDrawResModel();
			try
			{
				await _context.Database.ExecuteSqlRawAsync("DELETE FROM DrawAnalyze");

				string jsonString = JsonConvert.SerializeObject(req);
				var analyze = new DrawAnalyze();
				analyze.Json = jsonString;
				var id = _context.DrawAnalyzes.Add(analyze);
				_context.SaveChanges();
				res.data = analyze;
				res.Status = true;
				res.Message = "Anaylze successfully.";
			}
			catch (Exception ex)
			{
				res.Status = false;
				res.Message = ex.Message;
			}
			return res;
		}

		public async Task<GetDrawResultResModel> getDrawResult(int id)
		{
			var res = new GetDrawResultResModel();
			try
			{
				var draw =  _context.BondsDraws.Where(x => x.ScheduleId == id).FirstOrDefault();
				if (draw != null)
				{
					CreateNewDrawReqModel data = new CreateNewDrawReqModel();
					data.first_prize_worth = draw.FirstPrizeWorth;
					data.second_prize_worth = draw.SecondPrizeWorth;
					data.third_prize_worth = draw.ThirdPrizeWorth;
					data.draw_date = draw.DrawDate;
					data.draw_no = draw.DrawNo;
					data.scheduleId = draw.ScheduleId;
					data.draw_id = draw.DrawId;

					var bounds = _context.DrawWinsBonds.Where(x => x.DrawId == draw.DrawId).ToList();
					var list = new List<DrawWinsBondDataModel>();
					foreach (var item in bounds)
					{
						var bond = new DrawWinsBondDataModel();
						bond.Position = item.Position;
						bond.DrawId = item.DrawId;
						bond.BoundNo = item.BoundNo;
						bond.Id = item.Id;
						list.Add(bond);

					}
					
					data.bonds = list;

					res.data = data;
					res.Status = true;
					res.Message = "Anaylze successfully.";
				}
				else
				{
					res.Status = false;
					res.Message = "Draw result is not announced yet or Data is ot available.";
				}
				
			}
			catch (Exception ex)
			{
				res.Status = false;
				res.Message = ex.Message;
			}
			return res;
		}

		public Task<GetDrawSchedulesByYearResModel> getDrawSchedulesByYear(int yearId)
		{
			var res = new GetDrawSchedulesByYearResModel();
			try
			{
				var user = _context.ScheduleBonds.Where(x => x.YearId == yearId).ToList();
				var catList = new List<DrawSchedulesByYearDataModel>();
				foreach (var cat in user)
				{
					var userData = new DrawSchedulesByYearDataModel();
					userData.Id = cat.Id;
					userData.YearId = cat.YearId;
					userData.Amount = cat.Amount;
					userData.IsPremium = cat.IsPremium;
					userData.Day = cat.Day;
					userData.Date = cat.Date;
					userData.Place = cat.Place;
					userData.IsAnnounced = cat.IsAnnounced;
					catList.Add(userData);
				}

				res.data = catList;
				res.Status = true;
				res.Message = "fetch successfully";

			}
			catch (Exception ex)
			{
				res.Status = false;
				res.Message = ex.Message;
			}

			return Task.FromResult(res);
		}

		public Task<GetRecordAvailYearsResModel> getRecordsAvailableYears()
		{
			var res = new GetRecordAvailYearsResModel();
			try
			{

				var user = _context.BondsRecordsYears.OrderByDescending(x => x.Year).ToList();
				var catList = new List<RecordAvailYearsDataModel>();
				foreach (var cat in user)
				{
					var userData = new RecordAvailYearsDataModel();
					userData.Id = cat.Id;
					userData.Year = cat.Year;
					catList.Add(userData);
				}

				res.data = catList;
				res.Status = true;
				res.Message = "fetch successfully";

			}
			catch (Exception ex)
			{
				res.Status = false;
				res.Message = ex.Message;
			}

			return Task.FromResult(res);
		}

		public Task<BaseResponseModel> importDrawResult(int id)
		{
			var res = new BaseResponseModel();
			try
			{
				var drawdataJson = _context.DrawAnalyzes.Find(id);
				if (drawdataJson != null)
				{
					
					CreateNewDrawReqModel drawData = JsonConvert.DeserializeObject<CreateNewDrawReqModel>(drawdataJson.Json);
					var existingDraw = _context.BondsDraws.Where(x => x.ScheduleId == drawData.scheduleId).FirstOrDefault();
					if (existingDraw != null)
					{
						_context.Database.ExecuteSqlRawAsync("DELETE FROM BondsDraws where schedule_id=" + existingDraw.ScheduleId);
						_context.Database.ExecuteSqlRawAsync("DELETE FROM DrawWinsBonds where draw_id=" + existingDraw.DrawId);
					}

					var draw = new BondsDraw();
					draw.DrawNo = drawData.draw_no;
					draw.ScheduleId = drawData.scheduleId;
					draw.DrawDate = drawData.draw_date;
					draw.FirstPrizeWorth = drawData.first_prize_worth;
					draw.SecondPrizeWorth = drawData.second_prize_worth;
					draw.ThirdPrizeWorth = drawData.third_prize_worth;

					_context.BondsDraws.Add(draw);
					_context.SaveChanges();

					foreach (var item in drawData.bonds)
					{
						var bond = new DrawWinsBond();
						bond.BoundNo = item.BoundNo;
						bond.Position = item.Position;
						bond.DrawId = draw.DrawId;

						_context.DrawWinsBonds.Add(bond);
						_context.SaveChanges();

					}

					res.Status = true;
					res.Message = "Imported successfully";

					 _context.Database.ExecuteSqlRawAsync("DELETE FROM DrawAnalyze");

				}
				else
				{
					res.Status = false;
					res.Message = "No Anayzed Draw found.";
				}
			}
			catch (Exception ex)
			{
				res.Status = false;
				res.Message = ex.Message;
			}

			return Task.FromResult(res);
		}
	}
}
