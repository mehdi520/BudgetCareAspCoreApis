using BudgetCareApis.Models.Dtos.bond;
using BudgetCareApis.Models.ResModels.Base;

namespace BudgetCareApis.Models.ResModels.bond
{
	public class GetBondTypesResModel : BaseResponseModel
	{
        public List<BondTypeDataModel> data { get; set; }
    }
}
