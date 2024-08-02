using AutoMapper;
using Core.Business.DTO.Admission;
using Core.Business.DTO.Appointment;
using Core.Business.DTO.Department;
using Core.Business.DTO.Doctor;
using Core.Business.DTO.MedicalRecord;
using Core.Business.DTO.Patient;
using DataAccess.User;
using Domain;
using HospitalManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
	public class HospitalAutoMapperProfile : Profile
	{

		public HospitalAutoMapperProfile() {

			//Patient
			CreateMap<Patient, PatientGetDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName));
            CreateMap<PatientCreateDto, Patient>();
			CreateMap<PatientUpdateDto, Patient>();
			CreateMap<Patient, PatientUpdateDto>();
			CreateMap<PatientGetDto, PatientCreateDto>();
			CreateMap<PatientCreateDto, PatientGetDto>();
			CreateMap<Patient, PatientLoginDto>();
			CreateMap<Patient, PatientRegisterDto>();
			CreateMap<PatientLoginDto, Patient>();
			CreateMap<PatientRegisterDto, Patient>();
			//CreateMap<PatientGetDto, PatientUpdateDto>;



			//Appointment
			CreateMap<Appointment, AppointmentGetDto>();
			CreateMap<AppointmentCreateDto, Appointment>();
			CreateMap<AppointmentUpdateDto, Appointment>();
			CreateMap<Appointment, AppointmentUpdateDto>();
			CreateMap<AppointmentGetDto,AppointmentCreateDto>();
			CreateMap<AppointmentCreateDto,AppointmentGetDto>();

			//Doctor
			CreateMap<Doctor, DoctorGetDto>()
				.ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
				.ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName));

			CreateMap<DoctorCreateDto, Doctor>();
			CreateMap<DoctorUpdateDto, Doctor>();
			CreateMap<Doctor, DoctorUpdateDto>();
			CreateMap<DoctorGetDto, DoctorCreateDto>();
			CreateMap<DoctorCreateDto, DoctorGetDto>();
			CreateMap<Doctor, DoctorLoginDto>();
			CreateMap<Doctor, DoctorRegisterDto>();
			CreateMap<DoctorLoginDto, Doctor>();
			CreateMap<DoctorRegisterDto, Doctor>();

			//Medical Record
			CreateMap<MedicalRecord, MedicalRecordGetDto>();
			CreateMap<MedicalRecordCreateDto, MedicalRecord>();
			CreateMap<MedicalRecordUpdateDto, MedicalRecord>();
			CreateMap<MedicalRecord, MedicalRecordUpdateDto>();
			CreateMap<MedicalRecordGetDto, MedicalRecordCreateDto>();
			CreateMap<MedicalRecordCreateDto, MedicalRecordGetDto>();
			CreateMap<MedicalRecord, PatientGetDto>();

			//Admission
			CreateMap<Admission, AdmissionGetDto>();
			CreateMap<AdmissionCreateDto, Admission>();
			CreateMap<AdmissionUpdateDto, Admission>();
			CreateMap<Admission, AdmissionUpdateDto>();
			CreateMap<AdmissionGetDto, AdmissionCreateDto>();
			CreateMap<AdmissionCreateDto, AdmissionGetDto>();
			
			//Department
			CreateMap<Department, DepartmentGetDto>();
			CreateMap<DepartmentCreateDto, Department>();
			CreateMap<DepartmentUpdateDto, Department>();
			CreateMap<Department, DepartmentUpdateDto>();
			CreateMap<DepartmentGetDto, DepartmentCreateDto>();
			CreateMap<DepartmentCreateDto, DepartmentGetDto>();

			//User
			CreateMap<User, Patient>();
			CreateMap<User, Doctor>();
			CreateMap<User, PatientGetDto>();
			CreateMap<User, DoctorGetDto>();
			CreateMap<User, PatientUpdateDto>();
			CreateMap<User, DoctorUpdateDto>();
			CreateMap<User, PatientLoginDto>();
			CreateMap<User, PatientRegisterDto>();
			CreateMap<User, UserLoginDto>();
			CreateMap<User, UserRegisterDto>();
			CreateMap<UserLoginDto, User>();
			CreateMap<UserRegisterDto, User>();
			CreateMap<User, UserGetDto>();
			CreateMap<UserGetDto, User>();
		
		}
	}
}
