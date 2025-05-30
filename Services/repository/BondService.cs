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
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace BudgetCareApis.Services.repository
{


	public class BondService : IBondService
	{
		private readonly BudgetCareDBContext _context;

		public BondService(BudgetCareDBContext context)
		{
			_context = context;
		}

		public Task<GetBondTypesResModel> getBondTypes()
		{
			var res = new GetBondTypesResModel();
			try
			{

				var user = _context.BondTypes.OrderByDescending(x => x.BondType1).ToList();
				var catList = new List<BondTypeDataModel>();
				foreach (var cat in user)
				{
					var userData = new BondTypeDataModel();
					userData.TypeId = cat.TypeId;
					userData.BondType = cat.BondType1;
					userData.IsPermium = cat.IsPermium;

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

		public Task<GetDrawsResModel> getDrawsByBondType(int bondType)
		{
			var res = new GetDrawsResModel();
			try
			{
				var user = _context.BondsDraws.Where(x => x.BondTypeId == bondType)
					.Include(y=>y.BondType)
					.ToList();
				var catList = new List<DrawDataModel>();
				foreach (var cat in user)
				{
					var userData = new DrawDataModel();
					userData.DrawId = cat.DrawId;
					userData.BondTypeId = cat.BondTypeId;
					userData.BondType = cat.BondType.BondType1;
					userData.IsResultAnnounced = cat.IsResultAnnounced;
					userData.Day = cat.Day;
					userData.DrawDate = cat.DrawDate;
					userData.Place = cat.Place;
					userData.FirstPrizeWorth = cat.FirstPrizeWorth;
					userData.SecondPrizeWorth = cat.SecondPrizeWorth;
					userData.ThirdPrizeWorth = cat.ThirdPrizeWorth;
					userData.DrawNo = cat.DrawNo ?? 0;
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

		public Task<BaseResponseModel> addUpdateDrawsByBondType(DrawDataModel req)
		{
			var res = new BaseResponseModel();
			try
			{
				if (req.DrawId > 0)
				{
					var draw = _context.BondsDraws.Find(req.DrawId);
					if (draw == null)
					{
						res.Status = false;
						res.Message = "Draw not found";
					}
					else
					{

						draw.DrawDate = req.DrawDate;
						draw.Place = req.Place;
						draw.FirstPrizeWorth = req.FirstPrizeWorth;
						draw.ThirdPrizeWorth = req.ThirdPrizeWorth;
						draw.DrawNo = req.DrawNo;
						draw.SecondPrizeWorth = req.SecondPrizeWorth;
						draw.BondTypeId = req.BondTypeId;
						draw.Day = req.Day;
						draw.IsResultAnnounced = draw.IsResultAnnounced;
						_context.SaveChanges();
						res.Status = true;
						res.Message = "Draw updated successfully";

					}
				}
				else
				{ 
				
					BondsDraw draw = new BondsDraw();
					draw.DrawDate = req.DrawDate;
					draw.Place = req.Place;
					draw.FirstPrizeWorth = req.FirstPrizeWorth;
					draw.ThirdPrizeWorth= req.ThirdPrizeWorth;
					draw.DrawNo = req.DrawNo;
					draw.SecondPrizeWorth = req.SecondPrizeWorth;
					draw.BondTypeId = req.BondTypeId;
					draw.Day = req.Day;
					draw.IsResultAnnounced = false;

					_context.BondsDraws.Add(draw);
					_context.SaveChanges();
					res.Status = true;
					res.Message = "Draw created successfully";
				
				}
			}
			catch (Exception ex)
			{
				res.Status = false;
				res.Message = ex.ToString();
			}

			return Task.FromResult(res);
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
				res.totalBonds = req.bonds.Count;
				res.totalFirst = req.bonds.Where(x => x.Position == 1).ToList().Count;
				res.totalSecond = req.bonds.Where(x => x.Position == 2).ToList().Count;
				res.totalThird = req.bonds.Where(x => x.Position == 3).ToList().Count;

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
				var draw =  _context.BondsDraws.Where(x => x.DrawId == id).FirstOrDefault();
				if (draw != null && draw.IsResultAnnounced)
				{
					CreateNewDrawReqModel data = new CreateNewDrawReqModel();
					data.first_prize_worth = draw.FirstPrizeWorth;
					data.second_prize_worth = draw.SecondPrizeWorth;
					data.third_prize_worth = draw.ThirdPrizeWorth;
					data.draw_date = draw.DrawDate;
					data.draw_no = draw.DrawNo ?? 0;
					//data.scheduleId = draw.ScheduleId;
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

	
		//public async Task<BaseResponseModel> importDrawResult(int id)
		//{
		//	var res = new BaseResponseModel();
		//	try
		//	{
		//		var drawdataJson = _context.DrawAnalyzes.Find(id);
		//		if (drawdataJson != null)
		//		{
					
		//			CreateNewDrawReqModel drawData = JsonConvert.DeserializeObject<CreateNewDrawReqModel>(drawdataJson.Json);
		//			var existingDraw = _context.BondsDraws.Where(x => x.DrawId == drawData.draw_id).FirstOrDefault();
		//			if (existingDraw != null)
		//			{
		//				//_context.Database.ExecuteSqlRawAsync("DELETE FROM BondsDraws where draw_id=" + existingDraw.DrawId);
		//			  await	_context.Database.ExecuteSqlRawAsync("DELETE FROM DrawWinsBonds where draw_id=" + existingDraw.DrawId);


		//				existingDraw.DrawNo = drawData.draw_no;
		//				//draw.ScheduleId = drawData.scheduleId;
		//				existingDraw.DrawDate = drawData.draw_date;
		//				existingDraw.FirstPrizeWorth = drawData.first_prize_worth;
		//				existingDraw.SecondPrizeWorth = drawData.second_prize_worth;
		//				existingDraw.ThirdPrizeWorth = drawData.third_prize_worth;
		//				existingDraw.IsResultAnnounced = true;

		//				_context.SaveChanges();

		//				foreach (var item in drawData.bonds)
		//				{
		//					var bond = new DrawWinsBond();
		//					bond.BoundNo = item.BoundNo;
		//					bond.Position = item.Position;
		//					bond.DrawId = existingDraw.DrawId;

		//					_context.DrawWinsBonds.Add(bond);

		//				}
		//				await _context.SaveChangesAsync(); // ✅ Save all in bulk

		//				res.Status = true;
		//				res.Message = "Imported successfully";

		//				await _context.Database.ExecuteSqlRawAsync("DELETE FROM DrawAnalyze");
		//			}
		//			else
		//			{
		//				res.Status = false;
		//				res.Message = "Draw not found. Please create draw first";
		//			}
		//		}
		//		else
		//		{
		//			res.Status = false;
		//			res.Message = "No Anayzed Draw found.";
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		res.Status = false;
		//		res.Message = ex.Message;
		//	}

		//	return res;
		//}
		public async Task<BaseResponseModel> importDrawResult(int id)
		{
			var res = new BaseResponseModel();
			try
			{
				var drawdataJson = _context.DrawAnalyzes.Find(id);

				if (drawdataJson == null)
				{
					res.Status = false;
					res.Message = "No analyzed draw found.";
					return res;
				}

				// Deserialize JSON into draw object
				var drawData = JsonConvert.DeserializeObject<CreateNewDrawReqModel>(drawdataJson.Json);

				var existingDraw = _context.BondsDraws.FirstOrDefault(x => x.DrawId == drawData.draw_id);
				if (existingDraw == null)
				{
					res.Status = false;
					res.Message = "Draw not found. Please create the draw first.";
					return res;
				}

				// Delete existing bond results
				await _context.Database.ExecuteSqlRawAsync($"DELETE FROM DrawWinsBonds WHERE draw_id = {existingDraw.DrawId}");

				// Update existing draw info
				existingDraw.DrawNo = drawData.draw_no;
				existingDraw.DrawDate = drawData.draw_date;
				existingDraw.FirstPrizeWorth = drawData.first_prize_worth;
				existingDraw.SecondPrizeWorth = drawData.second_prize_worth;
				existingDraw.ThirdPrizeWorth = drawData.third_prize_worth;
				existingDraw.IsResultAnnounced = true;

				_context.BondsDraws.Update(existingDraw);
				await _context.SaveChangesAsync();

				// Prepare bonds
				var validBonds = new List<DrawWinsBond>();
				var skippedBonds = new List<DrawWinsBondDataModel>();
				const int batchSize = 500;

				foreach (var bond in drawData.bonds)
				{
					bool isValid = !string.IsNullOrWhiteSpace(bond.BoundNo)
								   && bond.BoundNo.Length == 6
								   && Regex.IsMatch(bond.BoundNo, @"^\d{6}$");

					if (isValid)
					{
						validBonds.Add(new DrawWinsBond
						{
							BoundNo = bond.BoundNo,
							Position = bond.Position,
							DrawId = existingDraw.DrawId
						});
					}
					else
					{
						skippedBonds.Add(bond); // collect skipped for log
					}
				}

				// Batch insert
				for (int i = 0; i < validBonds.Count; i += batchSize)
				{
					var batch = validBonds.Skip(i).Take(batchSize).ToList();
					_context.DrawWinsBonds.AddRange(batch);
					await _context.SaveChangesAsync();
				}

				// Clean DrawAnalyze table (optional)
				await _context.Database.ExecuteSqlRawAsync("DELETE FROM DrawAnalyze");

				// Summary message
				res.Status = true;
				res.Message = $"Imported {validBonds.Count} bond records successfully.";
				if (skippedBonds.Count > 0)
				{
					res.Message += $" Skipped {skippedBonds.Count} invalid bond(s).";
					Console.WriteLine("Skipped bonds:");
					foreach (var skip in skippedBonds)
					{
						Console.WriteLine($" - BoundNo: {skip.BoundNo}, Position: {skip.Position}");
					}
				}
			}
			catch (Exception ex)
			{
				res.Status = false;
				res.Message = $"Error importing draw result: {ex.Message}";
				Console.WriteLine("Import Error: " + ex);
			}

			return res;
		}


	}
}
