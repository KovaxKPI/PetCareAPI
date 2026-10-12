namespace PetCareAPI.Domain.Models;

public class Pet
{
    public Guid PetId { get; } = Guid.CreateVersion7();

    public string UserId { get; set; } = null!;
    
    public string Name { get; set; } = null!;
    
    public int Age { get; set; }
    
    public string Gender { get; set; } = null!;

    public string ImageVersionId { get; set; } = null!;
    
    public List<Appointment>? Appointments { get; set; }
}