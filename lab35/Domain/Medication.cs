namespace lab35.Domain;

public class Medication
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public decimal BasePrice { get; set; }
    public bool RequiresPrescription { get; set; }
    public string Status { get; set; } = "InStock"; // InStock, Sold, Reserved

    // Бізнес-правило: Розрахунок ціни зі знижкою
    public decimal GetFinalPrice(IDiscountStrategy strategy) 
    {
        if (BasePrice <= 0) throw new ArgumentException("Ціна має бути більше 0.");
        return strategy.Apply(BasePrice);
    }
}