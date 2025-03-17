using BudgetCareApis.Data;
using BudgetCareApis.Data.Entities;
using BudgetCareApis.Models.Dtos;
using BudgetCareApis.Models.Dtos.Category;
using BudgetCareApis.Models.Dtos.Users;
using BudgetCareApis.Models.ReqModels;
using BudgetCareApis.Models.ReqModels.Users;
using BudgetCareApis.Models.ResModels;
using BudgetCareApis.Models.ResModels.Base;
using BudgetCareApis.Models.ResModels.User;
using BudgetCareApis.Services.services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace BudgetCareApis.Services.repository
{
	public class UserService : IUserService
	{
		private readonly BudgetCareDBContext _context;
		//private CommonRepository _commonRepository;
		//public readonly PasswordHelper _passwordHelper;

		public UserService(BudgetCareDBContext context)
		{
			_context = context;
		}

		public Task<BaseResponseModel> addUpdateCatagories(CategoryDataModel req)
		{
			var res = new BaseResponseModel();
			try
			{
				if (req.Id == 0)
				{
					var existingCat = _context.Categories.Where(x => x.Title == req.Title && x.IsDeleted == false && x.UserId == req.UserId).FirstOrDefault();
					if (existingCat != null)
					{
						res.Status = false;
						res.Message = "Already exist same category.Please use different title";

					}
					else
					{
						var cat = new Category();
						cat.Title = req.Title;
						cat.IsDeleted = false;
						cat.Description = req.Description;
						cat.UserId = req.UserId;
						cat.CreatedAt = DateTime.Now;
						cat.UpdatedAt = DateTime.Now;
						_context.Add(cat);
						_context.SaveChanges();

						res.Status = true;
						res.Message = "Category saved successfully";
					}
				}
				else
				{
					var existingCat = _context.Categories.Find(req.Id);
					if (existingCat != null && existingCat.UserId == req.UserId) {
						var sameTitleCat = _context.Categories.Where(x => x.Title == req.Title && x.IsDeleted == false && x.UserId == req.UserId && x.Id != req.Id).FirstOrDefault();
						if (sameTitleCat != null)
						{
							res.Status = false;
							res.Message = "Already exist same category.Please use different title";

						}
						else
						{

							existingCat.Title = req.Title;
							existingCat.Description = req.Description;
							existingCat.UpdatedAt = DateTime.Now;
							_context.SaveChanges();

							res.Status = true;
							res.Message = "Category updated successfully";
						}
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

		public Task<BaseResponseModel> addUpdateExpense(TransDataModel req)
		{
			var res = new BaseResponseModel();
			try
			{
				if (req.Id == 0)
				{
					
						var cat = new Expense();
						cat.Amount = req.Amount;
						cat.Description = req.Desciption;
						cat.CatId = req.CatId;
						cat.Date = req.Date;
						cat.UserId = req.UserId;
						cat.CreatedAt = DateTime.Now;
						cat.UpdatedAt = DateTime.Now;
						_context.Add(cat);
						_context.SaveChanges();

						res.Status = true;
						res.Message = "Expense saved successfully";
					
				}
				else
				{
					var existingCat = _context.Expenses.Find(req.Id);
					if (existingCat != null)
					{
						

							existingCat.Amount = req.Amount;
						existingCat.Description = req.Desciption;

						existingCat.UpdatedAt = DateTime.Now;
							_context.SaveChanges();

							res.Status = true;
							res.Message = "Expense updated successfully";
						
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

		public Task<BaseResponseModel> addUpdateIncome(TransDataModel req)
		{
			var res = new BaseResponseModel();
			try
			{
				if (req.Id == 0)
				{

					var cat = new Income();
					cat.Amount = req.Amount;
					cat.Desciption = req.Desciption;
					cat.CatId = req.CatId;
					cat.Date = req.Date;
					cat.UserId = req.UserId;
					cat.CreatedAt = DateTime.Now;
					cat.UpdatedAt = DateTime.Now;
					_context.Add(cat);
					_context.SaveChanges();

					res.Status = true;
					res.Message = "Income saved successfully";

				}
				else
				{
					var existingCat = _context.Incomes.Find(req.Id);
					if (existingCat != null)
					{


						existingCat.Amount = req.Amount;
						existingCat.Desciption = req.Desciption;
						existingCat.Date = req.Date;

						existingCat.UpdatedAt = DateTime.Now;
						_context.SaveChanges();

						res.Status = true;
						res.Message = "Income updated successfully";

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

		public Task<BaseResponseModel> delCatagory(int cat_id)
		{
			var res = new BaseResponseModel();
			try
			{

				var user = _context.Categories.Find(cat_id);

				if (user == null)
				{
					res.Status = false;
					res.Message = "Category not found";
				}

				else
				{
					user.IsDeleted = true;
					user.UpdatedAt = DateTime.Now;
					_context.SaveChanges();
					res.Status = true;
					res.Message = "Category deleted successfully";
				}
			}
			catch (Exception ex)
			{
				res.Status = false;
				res.Message = ex.Message;
			}

			return Task.FromResult(res);
		}

		public async Task<BaseResponseModel> delExpense(int incomeId)
		{
			var res = new BaseResponseModel();
			try
			{
				var ico = _context.Expenses.Find(incomeId);
				_context.Expenses.Remove(ico);
				_context.SaveChanges();

				res.Status = true;
				res.Message = "Expense deleted successfully";
			}
			catch (Exception ex)
			{
				res.Status = false;
				res.Message = ex.Message.ToString();
			}
			return res;
		}

		public async Task<BaseResponseModel> delIncome(int incomeId)
		{
			var res = new BaseResponseModel();
			try
			{
				var ico = _context.Incomes.Find(incomeId);
				_context.Incomes.Remove(ico);
				_context.SaveChanges();

				res.Status = true;
				res.Message = "Income deleted successfully";
			}
			catch (Exception ex)
			{
				res.Status = false;
				res.Message = ex.Message.ToString();
			}
			return res;
		}

		public Task<GetUserCatsResModel> getCatagories(int user_id)
		{
			var res = new GetUserCatsResModel();
			try
			{

				var user = _context.Categories.Where(x=> x.UserId == user_id && x.IsDeleted == false).ToList();

				if (user == null)
				{
					res.Status = false;
					res.Message = "Invalid Credentials. Please try again with correct credentials.";
				}

				else
				{
					var catList = new List<CategoryDataModel>();
					foreach (var cat in user)
					{
						var userData = new CategoryDataModel();
						userData.Id = cat.Id;	
						userData.Title = cat.Title;
						userData.Description = cat.Description;
						userData.UserId = cat.UserId;
						userData.CreatedAt = cat.CreatedAt;
						userData.UpdatedAt = cat.UpdatedAt;
						catList.Add(userData);
					}
				
					res.data = catList;
					res.Status = true;
					res.Message = "fetch successfully";
				}
			}
			catch (Exception ex)
			{
				res.Status = false;
				res.Message = ex.Message;
			}

			return Task.FromResult(res);
		}

		public async Task<GetTransResModel> getExpense(GetTransReqModel req, int user_id)
		{
			var res = new GetTransResModel();
			if (req.pageNo < 1) req.pageNo = 1;
			if (req.pageSize < 1) req.pageSize = 10;
			try
			{
				var query = _context.Expenses.AsQueryable();
				query = query.Where(i => i.UserId == user_id);
				query = query.Where(i => i.Date >= req.startDate && i.Date <= req.endDate);
				if (req.categoryId > 0)
				{
					query = query.Where(i => i.CatId == req.categoryId);
				}

				var totalRecords = await query.CountAsync();
				var totalPages = (int)Math.Ceiling(totalRecords / (double)req.pageSize);

				var incomes = await query
					.OrderBy(i => i.CreatedAt)  // Sorting by CreatedAt (ascending order)
					.Skip((req.pageNo - 1) * req.pageSize)  // Skip items based on page number
					.Take(req.pageSize)  // Take the specified page size
					.ToListAsync();

				var catList = new List<TransDataModel>();
				foreach (var cat in incomes)
				{
					var userData = new TransDataModel();
					userData.Id = cat.Id;
					userData.Desciption = cat.Description;
					userData.Amount = cat.Amount;
					userData.CreatedAt = cat.CreatedAt;
					userData.Date = cat.Date;
					userData.CatId = cat.CatId;
					userData.UserId = cat.UserId;
					userData.UpdatedAt = cat.UpdatedAt;
					catList.Add(userData);
				}

				res.data = catList;
				res.totalAmount = catList.Sum(item => item.Amount);

				res.TotalPage = totalPages;
				res.Status = true;
				res.Message = "fetch successfully";

			}
			catch (Exception ex)
			{
				res.Status = false;
				res.Message = ex.Message;
			}

			return res;
		}

		public async Task<GetTransResModel> getIncomes(GetTransReqModel req, int user_id)
		{
			var res = new GetTransResModel();
			if (req.pageNo < 1) req.pageNo = 1;
			if (req.pageSize < 1) req.pageSize = 10;
			try
			{
				var query = _context.Incomes.AsQueryable();
				query = query.Where(i => i.UserId == user_id);
				query = query.Where(i => i.Date >= req.startDate && i.Date <= req.endDate);
				if (req.categoryId > 0)
				{
					query = query.Where(i => i.CatId == req.categoryId);
				}

				var totalRecords = await query.CountAsync();
				var totalPages = (int)Math.Ceiling(totalRecords / (double)req.pageSize);

				var incomes = await query
					.OrderBy(i => i.CreatedAt)  // Sorting by CreatedAt (ascending order)
					.Skip((req.pageNo - 1) * req.pageSize)  // Skip items based on page number
					.Take(req.pageSize)  // Take the specified page size
					.ToListAsync();

					var catList = new List<TransDataModel>();
					foreach (var cat in incomes)
					{
						var userData = new TransDataModel();
						userData.Id = cat.Id;
						userData.Desciption = cat.Desciption;
						userData.Amount = cat.Amount;
						userData.CreatedAt = cat.CreatedAt;
					userData.Date = cat.Date;
					userData.CatId = cat.CatId;
					userData.UserId = cat.UserId;
					userData.UpdatedAt = cat.UpdatedAt;

						catList.Add(userData);
					}

					res.data = catList;
				res.totalAmount = catList.Sum(item => item.Amount);
				res.TotalPage = totalPages;                
					res.Status = true;
					res.Message = "fetch successfully";
				
			}
			catch (Exception ex)
			{
				res.Status = false;
				res.Message = ex.Message;
			}

			return res;
		}

		public Task<GetUserResModel> getUserById(int user_id)
		{
			var res = new GetUserResModel();
			try
			{

				var user = _context.Users.Find(user_id);

				if (user == null)
				{
					res.Status = false;
					res.Message = "Invalid Credentials. Please try again with correct credentials.";
				}

				else
				{
					var userData = new UserViewModel();
					userData.Name = user.Name;

					userData.Id = user.Id;
					userData.Email = user.Email;
					userData.Phone = user.Phone;
					userData.Image = user.Image;
					userData.CreatedAt = user.CreatedAt;
					userData.UpdatedAt = user.UpdatedAt;


					res.data = userData;
					res.Status = true;
					res.Message = "Login successfully";
				}
			}
			catch (Exception ex)
			{
				res.Status = false;
				res.Message = ex.Message;
			}

			return Task.FromResult(res);
		}

		public Task<LoginResModel> login(AuthenticationReqModel req)
		{
			var res = new LoginResModel();
			try
			{

				var user = _context.Users.Where(x => x.Email == req.Email && x.Password == req.Password).FirstOrDefault();

				if (user == null)
				{
					res.Status = false;
					res.Message = "Invalid Credentials. Please try again with correct credentials.";
				}
				
				else
				{
					var userData = new UserViewModel();
					userData.Name = user.Name;
				
					userData.Id = user.Id;
					userData.Email = user.Email;

					res.data = userData;
					res.Status = true;
					res.Message = "Login successfully";
				}
			}
			catch (Exception ex)
			{
				res.Status = false;
				res.Message = ex.Message;
			}

			return Task.FromResult(res);
		}

		public async Task<BaseResponseModel> register(AuthenticationReqModel req)
		{
			var res = new BaseResponseModel();

			try
			{
				var user = _context.Users.Where(x => x.Email == req.Email).FirstOrDefault();
				if (user != null)
				{
					res.Status = false;
					res.Message = "Email is already exist.Please try with different email.";

				}
				else
				{

					var u_user = new User();
					u_user.Name = req.Name;
					u_user.Phone = req.Phone;
					u_user.Email = req.Email;
					u_user.Password = req.Password;
					u_user.CreatedAt = DateTime.Now;
					u_user.UpdatedAt = DateTime.Now;


					_context.Add(u_user);
					_context.SaveChanges();
					

					res.Status = true;
					res.ExtraMessage = "Register Successfully";
					
				}
			}
			catch (Exception ex)
			{
				res.Status = false;
				res.Message = ex.ToString();
			}

			return await Task.FromResult(res);
		}
	}
}
