﻿namespace BudgetCareApis.Models.ReqModels.Users
{
	public class AuthenticationReqModel
	{
		public string? Name { get; set; }
		public string Email { get; set; }
		public string? Phone { get; set; }
		public string Password { get; set; }

	}
}
