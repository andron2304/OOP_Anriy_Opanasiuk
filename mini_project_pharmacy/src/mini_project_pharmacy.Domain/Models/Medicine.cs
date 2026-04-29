namespace mini_project_pharmacy.Domain.Models;

public sealed class Medicine
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; init; } = string.Empty;
    public string Dosage { get; init; } = string.Empty;
    public string Category { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public bool IsPrescriptionOnly { get; init; }
    public DateTime ExpiryDate { get; init; }

    public Medicine() { }

    public Medicine(string name, string dosage, string category, decimal price, bool isPrescriptionOnly, DateTime expiryDate)
    {
        Name = name;
        Dosage = dosage;
        Category = category;
        Price = price;
        IsPrescriptionOnly = isPrescriptionOnly;
        ExpiryDate = expiryDate;
    }

    public bool IsExpired(DateTime referenceDate) => ExpiryDate < referenceDate;
}

