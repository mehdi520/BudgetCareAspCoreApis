namespace BudgetCareApis.Models.Dtos.Category
{
	public class CategoryDataModel
	{
		public int Id { get; set; }

		public string Title { get; set; } = null!;

		public string? Description { get; set; }

		public bool IsDeleted { get; set; }

		public int? UserId { get; set; }

		public DateTime? CreatedAt { get; set; }

		public DateTime? UpdatedAt { get; set; }

	}
}
