using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StayHub.Data.DBModels;
using StayHub.Data.ViewModels;
using StayHub.Services;
namespace StayHub;
public class Startup
{


    readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;

    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        //In Swagger Add JWT Token Authorization         
        services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                 {
                   new OpenApiSecurityScheme {  Reference = new OpenApiReference
                         {
                          Type = ReferenceType.SecurityScheme,
                          Id = "Bearer"
                          }
                        },
                   new string[] {}
                  }
                });
        });
        services.AddDbContext<StayHubContext>(options =>
                  options.UseSqlServer("Server=sql8004.site4now.net;Database=db_aa454a_stayhub;Persist Security Info=True;Encrypt=False;user id=db_aa454a_stayhub_admin;password=St@yHub13"));
        services.AddControllersWithViews().AddNewtonsoftJson(
          options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

        //Interfaces 
        services.AddScoped<EmailService>();
        services.AddScoped<AccountService>();
        services.AddScoped<EventService>();
        services.AddScoped<RoomService>();
        services.AddScoped<InRoomService>();
        services.AddScoped<GymService>();
        services.AddScoped<SpaService>();
        services.AddScoped<PaymentService>(); 
        services.AddScoped<GuestService>(); 
        services.AddScoped<StaffService>();
        services.AddScoped<BookingService>();
        services.AddScoped<CartService>();
        services.AddScoped<TicketService>();
        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddCors(options =>
        {
            options.AddPolicy(name: MyAllowSpecificOrigins,
                              builder =>
                              {
                                  builder.WithOrigins(new string[]
           {
                "http://localhost:3000",
                "https://localhost:3000",
                "http://localhost:5173",
                "https://localhost:5173",
                "http://localhost:8084",
                "https://localhost:8084",
                "http://localhost:8082",
                "https://localhost:8082"
           }).AllowAnyMethod().AllowAnyHeader().AllowCredentials();
           });
        });

        //jwt authentication

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
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("StayHubKey25102023139")),
                ValidateIssuer = false,
                ValidateAudience = false,

            };
        });


        services.AddAuthorization(auth =>
        {
            auth.AddPolicy(Roles.All, b => b.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser());

            auth.AddPolicy(Roles.Admin, b => b.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .RequireRole(Roles.Admin));

            auth.AddPolicy(Roles.Staff, b => b.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .RequireRole(Roles.Staff));

            auth.AddPolicy(Roles.Guest, b => b.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .RequireRole(Roles.Guest));



        });

        //Default Table Data
        // CreateDefaultsAsync(services.BuildServiceProvider()).GetAwaiter().GetResult();


    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {

        if (env.IsDevelopment())
        {

            app.UseSwagger();


            app.UseDeveloperExceptionPage();
        }

        app.UseStaticFiles();

        // app.UseHttpsRedirection();


        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Stella Api v1");


        });



        app.UseRouting();

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseCors(MyAllowSpecificOrigins);

        //app.UseMiddleware<JWTMiddleware>();


        app.UseEndpoints(endpoints =>
        {


            endpoints.MapControllers();
        });



    }



}
