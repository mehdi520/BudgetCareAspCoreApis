	using BudgetCareApis.Data;
	using BudgetCareApis.Services.repository;
	using BudgetCareApis.Services.services;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.AspNetCore.Authentication.JwtBearer;
	using Microsoft.IdentityModel.Tokens;
	using System.Text;


	var builder = WebApplication.CreateBuilder(args);
	builder.Services.AddCors(option =>

		option.AddPolicy("AllowAll",policy => {

			policy
			.AllowAnyOrigin()
			.AllowAnyMethod()
			.AllowAnyHeader();
			
	
		})
		);

	// Add DbContext with SQL Server connection string.
	builder.Services.AddDbContext<BudgetCareDBContext>(options =>
	{
		options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
		// Uncomment for lazy loading proxies if needed
		// options.UseLazyLoadingProxies(true);
	});


	// Configure Authentication using JWT Bearer.
	builder.Services.AddAuthentication(options =>
	{
		options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
		options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
	})
	.AddJwtBearer(o =>
	{
		o.TokenValidationParameters = new TokenValidationParameters
		{
			ValidIssuer = builder.Configuration["Jwt:Issuer"],
			ValidAudience = builder.Configuration["Jwt:Audience"],
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = false,
			ValidateIssuerSigningKey = true
		};
	});

	// Add HTTP Context Accessor.
	builder.Services.AddHttpContextAccessor();

	// Add scoped services for repositories and services.
	builder.Services.AddScoped<IUserService, UserService>();

	// Add services to the container.

	builder.Services.AddControllers();
	// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
	builder.Services.AddEndpointsApiExplorer();
	builder.Services.AddSwaggerGen();

	var app = builder.Build();
	app.UseCors("AllowAll");
	// Configure the HTTP request pipeline.
	if (app.Environment.IsDevelopment())
	{
		app.UseSwagger();
		app.UseSwaggerUI();
	}

	app.UseHttpsRedirection();

	app.UseAuthorization();

	app.MapControllers();

	app.Run();
