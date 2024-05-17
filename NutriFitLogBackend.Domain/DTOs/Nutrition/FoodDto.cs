namespace NutriFitLogBackend.Domain.DTOs.Nutrition;

public class FoodDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string PictureUrl { get; set; }
    public string Unit { get; set; }
}