using AutoMapper;
using NutriFitLogBackend.Domain.DTOs;
using NutriFitLogBackend.Domain.DTOs.Nutrition;
using NutriFitLogBackend.Domain.DTOs.Trainings;
using NutriFitLogBackend.Domain.DTOs.Users;
using NutriFitLogBackend.Domain.Entities.Nutrition;
using NutriFitLogBackend.Domain.Entities.Trainings;
using NutriFitLogBackend.Domain.Entities.Users;

namespace NutriFitLogBackend.Infrastructure.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserDto, User>().ReverseMap();
        
        CreateMap<Food, FoodDto>().ReverseMap();
        
        CreateMap<Exercise, ExerciseDto>().ReverseMap();
        
        CreateMap<Training, TrainingDto>()
            .ForMember(dest => dest.Exercises, opt => opt.MapFrom(src => src.Exercises));

        CreateMap<TrainingExercise, TrainingExerciseDto>()
            .ForMember(dest => dest.Exercise, opt => opt.MapFrom(src => src.Exercise))
            .ForMember(dest => dest.Sets, opt => opt.MapFrom(src => src.Sets));
        
        CreateMap<Set, SetDto>();

        CreateMap<Exercise, ExerciseDto>();
        
        CreateMap<DayPart, DayPartDto>();
        
        CreateMap<MealFood, MealFoodDto>();
    }
}