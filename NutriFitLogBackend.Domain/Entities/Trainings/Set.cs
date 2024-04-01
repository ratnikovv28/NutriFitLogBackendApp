namespace NutriFitLogBackend.Domain.Entities.Trainings;

public class Set
{
    public long Id { get; set; }
    
    // Универсальные поля для всех типов упражнений
    public long? Repetitions { get; set; } // Повторы, используются для силовых и на выносливость
    public double? Weight { get; set; } // Вес, используется для силовых упражнений
    public TimeSpan? Duration { get; set; } // Продолжительность, используется для бега и упражнений на выносливость
    public double? Distance { get; set; } // Расстояние, используется для бега
    
    public long TrainingExerciseId { get; set; }
    public TrainingExercise? TrainingExercise { get; set; }
}