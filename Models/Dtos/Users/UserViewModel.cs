namespace BudgetCareApis.Models.Dtos.Users
{
	public class UserViewModel
	{
		public int Id { get; set; }

		public string Name { get; set; } = null!;

		public string Email { get; set; } = null!;

		public string Phone { get; set; } = null!;

		public string Password { get; set; } = null!;

		public string? Image { get; set; }

		public DateTimeOffset CreatedAt { get; set; }

		public DateTimeOffset UpdatedAt { get; set; }
	}
}
