using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using NutriFitLogBackend.Domain.DTOs.Nutrition;
using NutriFitLogBackend.Domain.DTOs.Trainings;
using NutriFitLogBackend.Domain.DTOs.Users;
using NutriFitLogBackend.Domain.Entities.Nutrition;
using NutriFitLogBackend.Domain.Entities.Trainings;
using NutriFitLogBackend.Domain.Entities.Users;

namespace NutriFitLogBackend.Infrastructure.Mapper;

[ExcludeFromCodeCoverage]
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Select(r => Enum.GetName(typeof(UserRole), r))));
        
        CreateMap<Food, FoodDto>().ReverseMap()
            .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => Enum.GetName(typeof(UnitOfMeasure), src.Unit)));
        
        CreateMap<Exercise, ExerciseDto>().ReverseMap();
        
        CreateMap<Training, TrainingDto>()
            .ForMember(dest => dest.Exercises, opt => opt.MapFrom(src => src.Exercises));

        CreateMap<TrainingExercise, TrainingExerciseDto>()
            .ForMember(dest => dest.Exercise, opt => opt.MapFrom(src => src.Exercise))
            .ForMember(dest => dest.Sets, opt => opt.MapFrom(src => src.Sets));
        
        CreateMap<Set, SetDto>();

        CreateMap<Exercise, ExerciseDto>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => Enum.GetName(typeof(ExerciseType), src.Type)));
        
        CreateMap<DayPart, DayPartDto>();
        
        CreateMap<MealFood, MealFoodDto>();
        
        CreateMap<Meal, MealDto>();
    }
}