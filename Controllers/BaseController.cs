using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using static Azure.Core.HttpHeader;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BudgetCareApis.Models.Dtos.Users;

namespace BudgetCareApis.Controllers
{
	[ApiController]
	[Route("api/[controller]/[action]")]

	public abstract class BaseController : ControllerBase
	{
		IConfiguration _configuration;
		//ICommonService _commonService;
		IWebHostEnvironment _webHostEnvironment;
		//public readonly PasswordHelper _passwordHelper;

		public BaseController(IConfiguration configuration,  IWebHostEnvironment webHostEnvironment)
		{
			_configuration = configuration;
			//_commonService = ICommonService;
			_webHostEnvironment = webHostEnvironment;
			//_passwordHelper = new PasswordHelper();

			//var str = _webHostEnvironment.WebRootFileProvider.ToString();
			//var str2 = _webHostEnvironment.WebRootPath.ToString();

		}

		[ApiExplorerSettings(IgnoreApi = true)]
		public string GetJwt(UserViewModel user)
		{
			var issuer = _configuration.GetValue<string>("Jwt:Issuer");
			var audience = _configuration.GetValue<string>("Jwt:Audience");
			var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("Jwt:Key"));

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[]
				{

				new Claim("Id", user.Id.ToString()),
				//new Claim(JwtRegisteredClaimNames.Sub, user.UserType.ToString()),
				new Claim(JwtRegisteredClaimNames.Email, user.Email)

			 }),
				Expires = DateTime.UtcNow.AddMinutes(50),
				Issuer = issuer,
				Audience = audience,
				SigningCredentials = new SigningCredentials
				(new SymmetricSecurityKey(key),
				SecurityAlgorithms.HmacSha512Signature)
			};
			var tokenHandler = new JwtSecurityTokenHandler();
			var token = tokenHandler.CreateToken(tokenDescriptor);
			// var jwtToken = tokenHandler.WriteToken(token);
			var stringToken = tokenHandler.WriteToken(token);
			return stringToken;
		}

		

		[ApiExplorerSettings(IgnoreApi = true)]
		public int getLoggedinUserId()
		{
			var claims = HttpContext.User.Claims;
			if (claims != null)
			{
				var claimsNa = claims.FirstOrDefault(c => c.Type.Contains("Id"));
				if (claimsNa != null)
				{
					var claimsName = claimsNa.Value;
					return int.Parse(claimsName);
				}
				else
				{
					return 0;
				}
			}
			else
			{
				return 0;


			}
		}

	
	}
}
