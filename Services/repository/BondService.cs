using BudgetCareApis.Data;
using BudgetCareApis.Data.Entities;
using BudgetCareApis.Models.Dtos.bond;
using BudgetCareApis.Models.Dtos.Category;
using BudgetCareApis.Models.ReqModels.bond;
using BudgetCareApis.Models.ResModels;
using BudgetCareApis.Models.ResModels.Base;
using BudgetCareApis.Models.ResModels.bond;
using BudgetCareApis.Services.services;
using DocumentFormat.OpenXml.Spreadsheet;
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

		public Task<GetBondsResModel> getUserBonds(int userId, int typeId)
		{ 
			var res = new GetBondsResModel();
			try { 

				List<UserBond> userBonds = new List<UserBond>();

				if (typeId > 0)
				{
					userBonds = _context.UserBonds.Where(x => x.UserId == userId && x.IsDeleted == false && x.BondType == typeId)
						.Include(x=>x.BondTypeNavigation).ToList();

				}
				else
				{
					userBonds = _context.UserBonds.Where(x => x.UserId == userId && x.IsDeleted == false).Include(x => x.BondTypeNavigation).ToList();

				}
				var list = new List<BondDataModel>();
				foreach (var item in userBonds)
				{
					var bond = new BondDataModel();
					bond.UserId = item.UserId;
					bond.BondNumber = item.BondNumber;
					bond.BondType = item.BondType;
					bond.BondTypeName = item.BondTypeNavigation.BondType1;
					bond.BondId = item.BondId;
					bond.CreatedAt = item.CreatedAt;
					list.Add(bond);
				}

				res.Status = true;
				res.data = list;
				res.Message = "Bond list";
			}
			catch (Exception ex) { 
				res.Status = false;
				res.Message = ex.Message;
			
			}

			return Task.FromResult(res);
		}

		public Task<BaseResponseModel> addUpdateUserBond(int userId, BondDataModel req)
		{
			var res = new BaseResponseModel();
			try
			{
				if (req.BondId > 0)
				{
					var bond = _context.UserBonds.Where(x=>x.UserId == userId && x.BondId == req.BondId && x.IsDeleted == false).FirstOrDefault();
					if (bond != null)
					{
						var isExist = _context.UserBonds.Where(x => x.UserId == userId && x.IsDeleted == false && x.BondNumber == req.BondNumber && x.BondId != req.BondId).FirstOrDefault();
						if (isExist != null)
						{
							res.Status = false;
							res.Message = "Bond number already exist";
						}
						else
						{
							
							bond.BondType = req.BondType;
							bond.BondNumber = req.BondNumber;
							bond.CreatedAt = req.CreatedAt ?? DateTime.Now;
							bond.UserId = userId;
							bond.IsDeleted = false;
							_context.SaveChanges();

							res.Status = true;
							res.Message = "Bond updated successfully";
						}

					}
					else
					{
						res.Status = false;
						res.Message = "Bond not found";
					}
				}
				else
				{
					var isExist = _context.UserBonds.Where(x => x.UserId == userId && x.IsDeleted == false && x.BondType == req.BondType && x.BondNumber == req.BondNumber).FirstOrDefault();
					if (isExist != null)
					{
						res.Status = false;
						res.Message = "Bond number already exist";
					}
					else
					{
						var bond = new UserBond();
						bond.BondType = req.BondType;
						bond.BondNumber = req.BondNumber;
						bond.CreatedAt = req.CreatedAt ?? DateTime.Now;
						bond.UserId = userId;
						bond.IsDeleted = false;

						_context.UserBonds.Add(bond);
						_context.SaveChanges();

						res.Status = true;
						res.Message = "Bond saved successfully";
					}
				
				}
			}
			catch (Exception ex)
			{
				res.Status = false;
				res.Message = ex.Message;

			}

			return Task.FromResult(res);
		}

		public Task<BaseResponseModel> deleteUserBond(int userId, int bondId)
		{
			var res = new BaseResponseModel();
			try
			{

				var bond = _context.UserBonds.Where(y => y.UserId == userId && y.IsDeleted == false && y.BondId == bondId).FirstOrDefault();
				if (bond != null)
				{
					bond.IsDeleted = true;
					_context.SaveChanges();
					res.Status = true;
					res.Message = "Bond deleted successfully";

				}
				else
				{
					res.Status = false;
					res.Message = "Bond nt found";
				}
			}
			catch (Exception ex)
			{
				res.Status = false;
				res.Message = ex.Message;

			}

			return Task.FromResult(res);
		}

		public async Task<BaseResponseModel> DrawWinCheckSyncByDraw(int drawId)
		{
			var res = new BaseResponseModel();
			try
			{
				
					// 1. Get the draw details
					var draw = await _context.BondsDraws
						.Include(d => d.DrawWinsBonds)
						.FirstOrDefaultAsync(d => d.DrawId == drawId);

					if (draw == null)
					{
						res.Status = false;
						res.Message = "Draw not found.";
						return res;
					}

					// 2. Get winning bond numbers for this draw
					var winningBonds = draw.DrawWinsBonds.Select(wb => new { wb.BoundNo, wb.Position }).ToList();

					if (!winningBonds.Any())
					{
						res.Status = false;
						res.Message = "No winning bonds for this draw.";
						return res;
					}

					// 3. Get all user bonds that match this draw's bond type
					var userBonds = await _context.UserBonds
						.Where(ub => ub.BondType == draw.BondTypeId && !ub.IsDeleted)
						.ToListAsync();

					var currentTime = DateTime.UtcNow;

				// 4. Compare user bonds with winning bonds
				foreach (var userBond in userBonds)
				{
					// Find *all* win matches (though ideally only one should exist per bond number)
					var matchedWins = winningBonds
						.Where(w => w.BoundNo.Trim() == userBond.BondNumber.Trim())
						.ToList();

					foreach (var matchedWin in matchedWins)
					{
						// Check if this specific user-win entry already exists
						bool alreadyExists = await _context.UserWonBonds.AnyAsync(uwb =>
							uwb.BondId == userBond.BondId &&
							uwb.DrawId == drawId &&
							uwb.UserId == userBond.UserId &&
							uwb.Position == matchedWin.Position);

						if (!alreadyExists)
						{
							var wonBond = new UserWonBond
							{
								BondId = userBond.BondId,
								DrawId = drawId,
								UserId = userBond.UserId,
								Position = matchedWin.Position,
								Status = "Pending",
								CreatedAt = currentTime,
								UpdatedAt = currentTime
							};

							await _context.UserWonBonds.AddAsync(wonBond);
						}
					}
				}

				await _context.SaveChangesAsync();

					res.Status = true;
					res.Message = "User bond wins synced successfully.";
				
			}
			catch (Exception ex)
			{
				res.Status = false;
				res.Message = $"Error: {ex.Message}";
			}

			return res;
		}

		public Task<GetUserWonBondsResModel> GetUserWonBonds(int userId, string status)
		{
			var res = new GetUserWonBondsResModel();
			try
			{

				var bonds = _context.UserWonBonds.Where(y => y.UserId == userId && y.Status == status)
					.Include(x=>x.Bond)
					.Include(x=>x.Draw)
					.ThenInclude(d => d.BondType)
					.ToList();
				var list = new List<WonBondDataModel>();
				foreach (var item in bonds)
				{
					var bond = new WonBondDataModel();
					bond.Status = item.Status;
					bond.WonId = item.WonId;
					bond.BondId = item.BondId;
					bond.CreatedAt = item.CreatedAt;
					bond.UpdatedAt = item.UpdatedAt;
					bond.DrawId = item.DrawId;
					bond.Position = item.Position;
					if (item.Bond != null)
					{
						var bondData = new BondDataModel();
						bondData.BondId = item.Bond.BondId;
						bondData.BondNumber = item.Bond.BondNumber;
						bondData.BondType = item.Bond.BondType;
						bondData.CreatedAt = item.Bond.CreatedAt;

						bond.Bond = bondData;
						
					}

					if (item.Draw != null)
					{
						var drawData = new DrawDataModel();

						drawData.DrawId = item.Draw.DrawId;
						drawData.BondTypeId = item.Draw.BondTypeId;
						drawData.BondType = item.Draw.BondType.BondType1;
						drawData.Place = item.Draw.Place;

						drawData.Day = item.Draw.Day;
						drawData.DrawDate = item.Draw.DrawDate;
						drawData.DrawNo = item.Draw.DrawNo;
						drawData.FirstPrizeWorth = item.Draw.FirstPrizeWorth;
						drawData.SecondPrizeWorth = item.Draw.SecondPrizeWorth;
						drawData.ThirdPrizeWorth = item.Draw.ThirdPrizeWorth;

						bond.Draw = drawData;
					}


					list.Add(bond);
				}
				res.Status = true;
				res.data = list;
				res.Message = "";
				
			}
			catch (Exception ex)
			{
				res.Status = false;
				res.Message = ex.Message;

			}

			return Task.FromResult(res);
		}

		public Task<BaseResponseModel> UpdateUserWonBondStatus(int userId, string status, int wonId)
		{
			var res = new BaseResponseModel();
			try
			{

				var bond = _context.UserWonBonds.Where(y => y.UserId == userId && y.WonId == wonId ).FirstOrDefault();
				if (bond != null)
				{
					bond.Status = status;
					_context.SaveChanges();
					res.Status = true;
					res.Message = "Bond updated successfully";

				}
				else
				{
					res.Status = false;
					res.Message = "Bond not found";
				}
			}
			catch (Exception ex)
			{
				res.Status = false;
				res.Message = ex.Message;

			}

			return Task.FromResult(res);
		}

		public Task<GetUserBondSummaryResModel> GetUserBondsSummary(int userId)
		{
			var res = new GetUserBondSummaryResModel
			{
				data = new UserBondSummaryDataModel()
			};

			try
			{
				// Get user's non-deleted bonds with BondType navigation loaded
				var userBonds = _context.UserBonds
					.Where(x => x.UserId == userId && !x.IsDeleted)
					.Include(x => x.BondTypeNavigation) // Ensure BondType is loaded
					.ToList();

				// Group and summarize by BondType
				var bondSummaryList = userBonds
					.GroupBy(b => new { b.BondTypeNavigation.TypeId, b.BondTypeNavigation.BondType1 })
					.Select(g => new UserBondCountByType
					{
						bondType = g.Key.BondType1,
						bondCount = g.Count(),
						bondWorth = g.Count() * 100 // Example fixed worth per bond
					}).ToList();

				res.data.bonds = bondSummaryList;
				res.data.TotalBond = bondSummaryList.Sum(b => b.bondCount);
				res.data.TotalWorth = bondSummaryList.Sum(b => b.bondWorth);
				res.Status = true;
				res.Message = "Success";
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
