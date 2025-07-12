namespace TeaShopDomain.Reviews;

public class Review
{
    public Guid Id {get; set;}
    
    public required string Title {get; set;}
    
    public required int Rate { get; set; }
    
    public required Guid UserId {get; set;}
    
    public string Text { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
}