using Microsoft.EntityFrameworkCore;
using NutriFitLogBackend.Domain.Entities.Users;
using NutriFitLogBackend.Domain.Repositories.Users;
using NutriFitLogBackend.Infrastructure.Database;

namespace NutriFitLogBackend.Infrastructure.Repositories.Users;

public class StudentTrainerRepository : IStudentTrainerRepository
{
    private readonly NutriFitLogContext _dbContext;

    public StudentTrainerRepository(NutriFitLogContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void DeleteRelationShip(StudentTrainer studentTrainer)
    {
        _dbContext.StudentTrainer.Remove(studentTrainer);
    }

    public Task<StudentTrainer> GetRelationShip(long studentId, long trainerId)
    {
        return _dbContext.StudentTrainer
            .AsNoTracking()
            .FirstOrDefaultAsync(st => st.StudentId == studentId && st.TrainerId == trainerId);
    }
    
    public async Task<StudentTrainer> AddRelationShip(StudentTrainer studentTrainer)
    {
        await _dbContext.StudentTrainer.AddAsync(studentTrainer);
        return studentTrainer;
    }

    public void UpdateRelationShip(StudentTrainer studentTrainer)
    {
        _dbContext.StudentTrainer.Update(studentTrainer);
    }
}