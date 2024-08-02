
using Core.Domain;
using Domain;
using HospitalManagement.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq.Expressions;

namespace HospitalManagement.DataAccess
{
    public class HospitalDbContext : DbContext
    {
		public HospitalDbContext(DbContextOptions<HospitalDbContext> options) : base(options)
		{

		}



		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

			modelBuilder.Entity<Patient>(x =>
			{
				x.ToTable("patients");
			});

			modelBuilder.Entity<Doctor>(x =>
			{
				x.ToTable("doctors");
			});

			modelBuilder.Entity<Admission>(x =>
			{
				x.ToTable("admissions");
			});

			modelBuilder.Entity<Department>(x =>
			{
				x.ToTable("departments");
			});

			modelBuilder.Entity<MedicalRecord>(x =>
			{
				x.ToTable("medical_records");
			});
			modelBuilder.Entity<Appointment>(x =>
			{
				x.ToTable("appointments");
			});
			modelBuilder.Entity<ExceptionLog>(x =>
			{
				x.ToTable("exceptionlogs");
			});
			modelBuilder.Entity<User>(x =>
			{
				x.ToTable("users");
			});


			base.OnModelCreating(modelBuilder);


			foreach (var entityType in modelBuilder.Model.GetEntityTypes())
			{
				if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
				{
					var parameter = Expression.Parameter(entityType.ClrType, "e");
					var body = Expression.Equal(
												Expression.Property(parameter, nameof(BaseEntity.IsDeleted)),
																		Expression.Constant(false));

					modelBuilder.Entity(entityType.ClrType).HasQueryFilter(Expression.Lambda(body, parameter));
				}
			}

			modelBuilder.Entity<Department>().HasData(
		   new Department { Id = 1, DepartmentName = "Nöroloji", Location = "A Blok", Fee = 200 },
		   new Department { Id = 2, DepartmentName = "İç Hastalıkları", Location = "A Blok", Fee = 150 },
		   new Department { Id = 3, DepartmentName = "Kadın Doğum", Location = "A Blok", Fee = 250 },
		   new Department { Id = 4, DepartmentName = "Çocuk Sağlığı", Location = "A Blok", Fee = 100 },
		   new Department { Id = 5, DepartmentName = "Göz Hastalıkları", Location = "A Blok", Fee = 150 },
		   new Department { Id = 6, DepartmentName = "Kulak Burun Boğaz", Location = "A Blok", Fee = 150 },
		   new Department { Id = 7, DepartmentName = "Ortopedi", Location = "A Blok", Fee = 200 },
		   new Department { Id = 8, DepartmentName = "Cildiye", Location = "A Blok", Fee = 150 },
		   new Department { Id = 9, DepartmentName = "Üroloji", Location = "A Blok", Fee = 200 },
		   new Department { Id = 10, DepartmentName = "Genel Cerrahi", Location = "A Blok", Fee = 250 }
		   );

            modelBuilder.Entity<User>().HasData(
          new User { Id = 1, Email = "1@1.com", FirstName = "Ahmet", LastName = "Yılmaz", PhoneNumber = "1234567890", PasswordHash = "1", Role = UserRole.Doctor, Address = "Address 1" },
          new User { Id = 2, Email = "2@2.com", FirstName = "Mehmet", LastName = "Kaya", PhoneNumber = "1234567891", PasswordHash = "2", Role = UserRole.Doctor, Address = "Address 2" },
          new User { Id = 3, Email = "3@3.com", FirstName = "Ayşe", LastName = "Demir", PhoneNumber = "1234567892", PasswordHash = "3", Role = UserRole.Doctor, Address = "Address 3" },
          new User { Id = 4, Email = "4@4.com", FirstName = "Fatma", LastName = "Çelik", PhoneNumber = "1234567893", PasswordHash = "4", Role = UserRole.Doctor, Address = "Address 4" },
          new User { Id = 5, Email = "5@5.com", FirstName = "Ali", LastName = "Can", PhoneNumber = "1234567894", PasswordHash = "5", Role = UserRole.Doctor, Address = "Address 5" },
          new User { Id = 6, Email = "6@6.com", FirstName = "Zeynep", LastName = "Şahin", PhoneNumber = "1234567895", PasswordHash = "6", Role = UserRole.Doctor, Address = "Address 6" },
          new User { Id = 7, Email = "7@7.com", FirstName = "Hasan", LastName = "Koç", PhoneNumber = "1234567896", PasswordHash = "7", Role = UserRole.Doctor, Address = "Address 7" },
          new User { Id = 8, Email = "8@8.com", FirstName = "Emine", LastName = "Aydın", PhoneNumber = "1234567897", PasswordHash = "8", Role = UserRole.Doctor, Address = "Address 8" },
          new User { Id = 9, Email = "9@9.com", FirstName = "Murat", LastName = "Öz", PhoneNumber = "1234567898", PasswordHash = "9", Role = UserRole.Doctor, Address = "Address 9" },
          new User { Id = 10, Email = "10@10.com", FirstName = "Selin", LastName = "Yıldız", PhoneNumber = "1234567899", PasswordHash = "10", Role = UserRole.Doctor, Address = "Address 10" }
      );


            modelBuilder.Entity<Doctor>().HasData(
                new Doctor { Id = 1, UserId = 1, DepartmentId = 1 },
                new Doctor { Id = 2, UserId = 2, DepartmentId = 2 },
                new Doctor { Id = 3, UserId = 3, DepartmentId = 3 },
                new Doctor { Id = 4, UserId = 4, DepartmentId = 4 },
                new Doctor { Id = 5, UserId = 5, DepartmentId = 5 },
                new Doctor { Id = 6, UserId = 6, DepartmentId = 6 },
                new Doctor { Id = 7, UserId = 7, DepartmentId = 7 },
                new Doctor { Id = 8, UserId = 8, DepartmentId = 8 },
                new Doctor { Id = 9, UserId = 9, DepartmentId = 9 },
                new Doctor { Id = 10, UserId = 10, DepartmentId = 10 }
            );
        }
		public static void Seed(IApplicationBuilder app)
		{
			using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope())
			{
				var context = serviceScope?.ServiceProvider.GetRequiredService<HospitalDbContext>();
				
			}
		}

		public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Admission> Admissions { get; set; }
		public DbSet<ExceptionLog> ExceptionLogs { get; set; }

		public DbSet<User> Users { get; set; }
	
    }
}