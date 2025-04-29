namespace BudgetCareApis.Models.Dtos.Notes
{
	public class NoteBookModel
	{
		public int NoteBookId { get; set; }

		public int UserId { get; set; }
		public int TotalNotes { get; set; }

		public string Title { get; set; } = null!;

		public string IconColor { get; set; } = null!;

	}
}
