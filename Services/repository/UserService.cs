using BudgetCareApis.Data;
using BudgetCareApis.Data.Entities;
using BudgetCareApis.Models.Dtos;
using BudgetCareApis.Models.Dtos.Category;
using BudgetCareApis.Models.Dtos.Notes;
using BudgetCareApis.Models.Dtos.Users;
using BudgetCareApis.Models.ReqModels;
using BudgetCareApis.Models.ReqModels.Users;
using BudgetCareApis.Models.ResModels;
using BudgetCareApis.Models.ResModels.Base;
using BudgetCareApis.Models.ResModels.Notes;
using BudgetCareApis.Models.ResModels.User;
using BudgetCareApis.Services.services;
using Microsoft.EntityFrameworkCore;


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

		public Task<BaseResponseModel> addUpdateNote(NoteDataModel req)
		{
			var res = new BaseResponseModel();
			try
			{
				if (req.NoteId == 0)
				{
					var existingCat = _context.Notes.Where(x => x.Title == req.Title && x.UserId == req.UserId).FirstOrDefault();
					if (existingCat != null)
					{
						res.Status = false;
						res.Message = "Already exist same category.Please use different title";

					}
					else
					{
						var cat = new Note();
						cat.Title = req.Title;
						cat.NoteBookId = req.NoteBookId;
						cat.Details = req.Details;
						cat.UserId = req.UserId;
						cat.CreatedAt = DateTime.Now;
						_context.Add(cat);
						_context.SaveChanges();

						res.Status = true;
						res.Message = "Note saved successfully";
					}
				}
				else
				{
					var existingCat = _context.Notes.Find(req.NoteId);
					if (existingCat != null && existingCat.UserId == req.UserId)
					{
						var sameTitleCat = _context.Notes.Where(x => x.Title == req.Title  && x.UserId == req.UserId && x.NoteId != req.NoteId).FirstOrDefault();
						if (sameTitleCat != null)
						{
							res.Status = false;
							res.Message = "Already exist same title.Please use different title";

						}
						else
						{

							existingCat.Title = req.Title;
							existingCat.Details = req.Details;
							_context.SaveChanges();

							res.Status = true;
							res.Message = "Note updated successfully";
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

		public Task<BaseResponseModel> addUpdateNoteCatagories(NoteBookModel req)
		{
			var res = new BaseResponseModel();
			try
			{
				if (req.NoteBookId == 0)
				{
					var existingCat = _context.NoteBooks.Where(x => x.Title == req.Title  && x.UserId == req.UserId).FirstOrDefault();
					if (existingCat != null)
					{
						res.Status = false;
						res.Message = "Already exist same note book.Please use different title";

					}
					else
					{
						var cat = new NoteBook();
						cat.UserId = req.UserId;
						cat.Title = req.Title;
						cat.IconColor = req.IconColor;
						_context.Add(cat);
						_context.SaveChanges();

						res.Status = true;
						res.Message = "Notebook saved successfully";
					}
				}
				else
				{
					var existingCat = _context.NoteBooks.Find(req.NoteBookId);
					if (existingCat != null && existingCat.UserId == req.UserId)
					{
						var sameTitleCat = _context.NoteBooks.Where(x => x.Title == req.Title  && x.UserId == req.UserId && x.NoteBookId != req.NoteBookId).FirstOrDefault();
						if (sameTitleCat != null)
						{
							res.Status = false;
							res.Message = "Already exist same Title.Please use different title";

						}
						else
						{

							existingCat.Title = req.Title;
							existingCat.IconColor = req.IconColor;
							_context.SaveChanges();

							res.Status = true;
							res.Message = "Notebook updated successfully";
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

		public Task<BaseResponseModel> changePassword(ChnagePassReqModel req)
		{
			var res = new BaseResponseModel();
			try
			{
				var user = _context.Users.Find(req.userId);
				if (user != null)
				{
					if (user.Password != req.oldPass)
					{
						res.Status = false;
						res.Message = "Current password is not correct.";
					}
					else
					{
						user.Password = req.newPass;
						user.UpdatedAt = DateTime.Now;
						_context.SaveChanges();
						res.Status = true;
						res.Message = "Password changed successfully";
					}
					

				}
			}
			catch (Exception ex)
			{
				res.Status = false;
				res.Message = ex.ToString();

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

		public Task<BaseResponseModel> delNote(int note_id)
		{
			var res = new BaseResponseModel();
			try
			{

				var user = _context.Notes.Find(note_id);

				if (user == null)
				{
					res.Status = false;
					res.Message = "Invalid Credentials. Please try again with correct credentials.";
				}

				else
				{
					_context.Remove(user);
					_context.SaveChanges();
					res.Status = true;
					res.Message = "Note deleted successfully";
				}
			}
			catch (Exception ex)
			{
				res.Status = false;
				res.Message = ex.Message;
			}

			return Task.FromResult(res);
		}

		public Task<BaseResponseModel> delNoteCatagory(int note_id)
		{
			var res = new BaseResponseModel();
			try
			{

				var user = _context.NoteBooks.Find(note_id);

				if (user == null)
				{
					res.Status = false;
					res.Message = "Invalid Credentials. Please try again with correct credentials.";
				}

				else
				{
					_context.Remove(user);
					_context.SaveChanges();
					res.Status = true;
					res.Message = "Notebook deleted successfully";
				}
			}
			catch (Exception ex)
			{
				res.Status = false;
				res.Message = ex.Message;
			}

			return Task.FromResult(res);
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

		public Task<GetUserNoteBookResModel> getNoteCatagories(int user_id)
		{
			var res = new GetUserNoteBookResModel();
			try
			{

				var user = _context.NoteBooks.Where(x => x.UserId == user_id)
					.Include(x=> x.Notes)
					.ToList();

				if (user == null)
				{
					res.Status = false;
					res.Message = "Invalid Credentials. Please try again with correct credentials.";
				}

				else
				{
					var catList = new List<NoteBookModel>();
					foreach (var cat in user)
					{
						var userData = new NoteBookModel();
						userData.NoteBookId = cat.NoteBookId;
						userData.Title = cat.Title;
						userData.IconColor = cat.IconColor;
						userData.UserId = cat.UserId;
						userData.TotalNotes = cat.Notes.Count;
						
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

		public async Task<GetNotesResModel> getNotes(GetTransReqModel req, int user_id)
		{
			var res = new GetNotesResModel();
			if (req.pageNo < 1) req.pageNo = 1;
			if (req.pageSize < 1) req.pageSize = 10;
			try
			{
				var query = _context.Notes.AsQueryable();
				query = query.Where(i => i.UserId == user_id);
				//query = query.Where(i => i.Date >= req.startDate && i.Date <= req.endDate);
				if (req.categoryId > 0)
				{
					query = query.Where(i => i.NoteBookId == req.categoryId);
				}

				var totalRecords = await query.CountAsync();
				var totalPages = (int)Math.Ceiling(totalRecords / (double)req.pageSize);

				var incomes = await query
					.OrderBy(i => i.CreatedAt)  // Sorting by CreatedAt (ascending order)
					.Skip((req.pageNo - 1) * req.pageSize)  // Skip items based on page number
					.Take(req.pageSize)
					.Include(x=> x.NoteBook)// Take the specified page size
					.ToListAsync();

				var catList = new List<NoteDataModel>();
				foreach (var cat in incomes)
				{
					var userData = new NoteDataModel();
					userData.NoteId = cat.NoteId;
					userData.Title = cat.Title;
					userData.Details = cat.Details;
					userData.CreatedAt = cat.CreatedAt;
					userData.NoteBookId = cat.NoteBookId;
					userData.NoteBookName = cat.NoteBook.Title;
					userData.UserId = cat.UserId;
					catList.Add(userData);
				}

				res.data = catList;

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

		public async Task<GetGraphResModel> getUserGraphData(int userId)
		{
			var res = new GetGraphResModel();

			try
			{
				var currentYear = DateTime.Now.Year;
				// Get the income data grouped by month for the current year
				var incomeData = await _context.Incomes
					.Where(i => i.UserId == userId && i.Date.Year == currentYear)
					.GroupBy(i => i.Date.Month)
					.Select(g => new
					{
						Month = g.Key,
						TotalIncome = g.Sum(i => i.Amount)
					})
					.ToListAsync();

				// Get the expense data grouped by month for the current year
				var expenseData = await _context.Expenses
					.Where(e => e.UserId == userId && e.Date.Year == currentYear)
					.GroupBy(e => e.Date.Month)
					.Select(g => new
					{
						Month = g.Key,
						TotalExpense = g.Sum(e => e.Amount)
					})
					.ToListAsync();
				var graphData = new List<GraphDataModel>();
				for (int month = 1; month <= 12; month++)
				{
					var income = incomeData.FirstOrDefault(i => i.Month == month)?.TotalIncome ?? 0m;
					var expense = expenseData.FirstOrDefault(e => e.Month == month)?.TotalExpense ?? 0m;

					graphData.Add(new GraphDataModel
					{
						month = month,
						totalIncome = income,
						totalExpens = expense
					});
				}

				res.Status = true;
				res.Message = "";
				res.data = graphData;

			}
			catch (Exception ex)
			{

				res.Status = false;
				res.Message = ex.Message;
			}

			return res;
		}

		public async Task<GetTotalsResModel> getUserSummary(int userId)
		{
			var res = new GetTotalsResModel();
			try
			{
				var currentYear = DateTime.Now.Year;
				var currentMonth = DateTime.Now.Month;
				var currentDate = DateTime.Now.Date;

				// Get the total income for the current month
				var totalThisMonthIncome = await _context.Incomes
					.Where(i => i.UserId == userId && i.Date.Year == currentYear && i.Date.Month == currentMonth)
					.SumAsync(i => i.Amount);

				// Get the total expense for the current month
				var totalThisMonthExpense = await _context.Expenses
					.Where(e => e.UserId == userId && e.Date.Year == currentYear && e.Date.Month == currentMonth)
					.SumAsync(e => e.Amount);

				// Get the total income for the current year
				var totalThisYearIncome = await _context.Incomes
					.Where(i => i.UserId == userId && i.Date.Year == currentYear)
					.SumAsync(i => i.Amount);

				// Get the total expense for the current year
				var totalThisYearExpense = await _context.Expenses
					.Where(e => e.UserId == userId && e.Date.Year == currentYear)
					.SumAsync(e => e.Amount);

				res.data = new TotalsDataModel
				{
					totalThisMonthIncome = totalThisMonthIncome,
					totalThisMonthExpense = totalThisMonthExpense,
					totalThisYearIncome = totalThisYearIncome,
					totalThisYearExpense = totalThisYearExpense
				};

				res.Status = true;
				res.Message = "";
				

			}
			catch (Exception ex)
			{
				res.Status = false;
				res.Message = ex.Message;
			}
			return res;
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

		public Task<BaseResponseModel> updateProfile(UserViewModel req)
		{
			var res = new BaseResponseModel();
			try
			{
				var user = _context.Users.Find(req.Id);
				if (user != null)
				{
					user.Name = req.Name;
					user.Phone = req.Phone;
					user.UpdatedAt = DateTime.Now;
					_context.SaveChanges();
					res.Status = true;
					res.Message = "Profile saved successfully";

				}
			}
			catch (Exception ex)
			{
				res.Status = false;
				res.Message = ex.ToString();
			
			}
			return Task.FromResult(res);

		}
	}
}
