using AutoMapper;
using CarRental.DTOs;
using CarRental.Models;
using CarRental.Models.DTOs;

namespace CarRental
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapping for User
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, UserLoginDTO>().ReverseMap();
            CreateMap<User, UserRegistrationDTO>().ReverseMap();
            CreateMap<User, UserUpdateDTO>().ReverseMap();

            // Mapping for Car
            CreateMap<Car, CarCreateDTO>().ReverseMap();
            CreateMap<Car, CarReadDTO>().ReverseMap();
            CreateMap<Car, CarListDTO>().ReverseMap();
            CreateMap<Car, CarUpdateDTO>().ReverseMap();

            // Mapping for Reservation
            CreateMap<Reservation, ReservationDTO>().ReverseMap();
            CreateMap<Reservation, CreateReservationDTO>().ReverseMap();

            // Mapping for Payment
            CreateMap<Payment, PaymentDTO>().ReverseMap();
            CreateMap<CreatePaymentDTO, Payment>();

            // Mapping for Review
            CreateMap<Review, ReviewDTO>().ReverseMap();
            CreateMap<Review, CreateReviewDTO>().ReverseMap();

            // Mapping for Admin Report
            CreateMap<AdminReport, AdminReportCreateDTO>().ReverseMap();
            CreateMap<AdminReport, AdminReportReadDTO>().ReverseMap();


            // Mapping for Password Reset
            CreateMap<PasswordReset, PasswordResetDTO>().ReverseMap();
            CreateMap<PasswordReset, PasswordResetRequestDTO>().ReverseMap();
            CreateMap<PasswordReset, PasswordResetResponseDTO>().ReverseMap();
            CreateMap<PasswordReset, PasswordResetConfirmDTO>().ReverseMap();
            CreateMap<PasswordReset, PasswordResetVerifyDTO>().ReverseMap();
                    }
    }
}
