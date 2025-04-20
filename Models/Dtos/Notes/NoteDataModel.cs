namespace BudgetCareApis.Models.Dtos.Notes
{
	public class NoteDataModel
	{
		public int NoteId { get; set; }

		public int NoteBookId { get; set; }

		public int UserId { get; set; }

		public DateTime CreatedAt { get; set; }

		public string Title { get; set; } = null!;

		public string Details { get; set; } = null!;
	}
}
