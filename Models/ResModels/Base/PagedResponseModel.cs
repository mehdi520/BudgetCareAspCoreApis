namespace BudgetCareApis.Models.ResModels.Base
{
	/// <summary>Common paging info returned by every list endpoint.</summary>
	public class PagedResponseModel : BaseResponseModel
	{
		public int CurrentPage { get; set; }
		public int PageSize { get; set; }
		/// <summary>Total number of pages.</summary>
		public int TotalPage { get; set; }
		/// <summary>Total number of records matching the filter/search.</summary>
		public int TotalCount { get; set; }

		public void SetPaging(int currentPage, int pageSize, int totalCount)
		{
			CurrentPage = currentPage;
			PageSize = pageSize;
			TotalCount = totalCount;
			TotalPage = pageSize > 0 ? (int)Math.Ceiling(totalCount / (double)pageSize) : 1;
			if (TotalPage < 1) TotalPage = 1;
		}
	}
}
