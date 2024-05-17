using NutriFitLogBackend.Domain.Entities.Users;

namespace NutriFitLogBackend.Domain.Repositories.Users;

public interface IStudentTrainerRepository
{
    void DeleteRelationShip(StudentTrainer studentTrainer);
    Task<StudentTrainer> GetRelationShip(long studentId, long trainerId);
    Task<StudentTrainer> AddRelationShip(StudentTrainer studentTrainer);
    void UpdateRelationShip(StudentTrainer studentTrainer);
}