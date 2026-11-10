namespace BudgetCareApis.Models.ResModels.Base
{
	/// <summary>Response for create/update calls; Id is the saved record's id.</summary>
	public class SaveResponseModel : BaseResponseModel
	{
		public int Id { get; set; }
	}
}
