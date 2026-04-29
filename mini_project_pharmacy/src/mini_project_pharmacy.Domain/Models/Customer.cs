namespace mini_project_pharmacy.Domain.Models;

public sealed class Customer
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string FullName { get; init; }
    public bool IsInsured { get; init; }
    public string ContactEmail { get; init; }

    public Customer(string fullName, bool isInsured, string contactEmail)
    {
        FullName = fullName;
        IsInsured = isInsured;
        ContactEmail = contactEmail;
    }
}

