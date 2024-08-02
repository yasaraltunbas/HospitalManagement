using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using Business;
using Business.Abstract;
using Business.Concrete;
using DataAccess;
using HospitalManagement.DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace HospitalManagement
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

		
		public void ConfigureServices(IServiceCollection services)
		{
            services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost3000",
                    builder => builder.WithOrigins("http://localhost:3000")
                                      .AllowAnyMethod()
                                      .AllowAnyHeader());
            });


            services.AddControllers();
            services.AddSwaggerGen(setup =>
			{
				var jwtSecurityScheme = new OpenApiSecurityScheme
				{
					BearerFormat = "JWT",
					Name = "JWT Authentication",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.Http,
					Scheme = JwtBearerDefaults.AuthenticationScheme,
					Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

					Reference = new OpenApiReference
					{
						Id = JwtBearerDefaults.AuthenticationScheme,
						Type = ReferenceType.SecurityScheme
					}
				};

				setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

				setup.AddSecurityRequirement(new OpenApiSecurityRequirement
			{
				{ jwtSecurityScheme, Array.Empty<string>() }
			});

			});
			services.AddDbContext<HospitalDbContext>(options =>
            {
                options.UseNpgsql("Host=localhost;Port=5432;User ID=mosstar;password=mosstar123;Database=hospitaldb");
            });

			services.AddScoped<IPatientService, PatientManager>();
            services.AddScoped<IDoctorService, DoctorManager>();
            services.AddScoped<IAppointmentService, AppointmentManager>();
            services.AddScoped<IAdmissionService, AdmissionManager>();
            services.AddScoped<IDepartmentService, DepartmentManager>();
            services.AddScoped<IMedicalRecordService, MedicalRecordManager>();
            services.AddScoped<IExceptionLogService, ExceptionLogManager>();
            services.AddScoped<ExceptionLogManager>();
			services.AddScoped<IUnitOfWorks, UnitOfWorks>();
            services.AddScoped<IUserService, UserManager>();

			services.AddHttpContextAccessor();

			var key = Encoding.ASCII.GetBytes(Configuration["Jwt:Key"]);

			services.AddAuthentication(x =>
			{
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(x =>
			{
				x.RequireHttpsMetadata = false;
				x.SaveToken = true;
				x.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = false,
					ValidateAudience = false
				};
			});


			var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new HospitalAutoMapperProfile()); });
			var mapper = mappingConfig.CreateMapper();
			services.AddSingleton(mapper);
			services.AddControllers().AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });
		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI v1"));
            }
            app.UseCors("AllowLocalhost3000");

            app.UseHttpsRedirection();


            app.UseRouting();
			app.UseAuthentication();

			app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
			HospitalDbContext.Seed(app);

		}
	}
}