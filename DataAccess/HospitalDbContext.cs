
using Core.Domain;
using Domain;
using HospitalManagement.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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