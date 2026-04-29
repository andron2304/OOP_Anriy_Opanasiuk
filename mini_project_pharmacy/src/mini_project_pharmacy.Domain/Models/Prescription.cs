namespace mini_project_pharmacy.Domain.Models;

public sealed class Prescription
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid CustomerId { get; init; }
    public Guid MedicineId { get; init; }
    public string DoctorFullName { get; init; }
    public DateTime IssueDate { get; init; }
    public DateTime ExpirationDate { get; init; }

    public Prescription(Guid customerId, Guid medicineId, string doctorFullName, DateTime issueDate, DateTime expirationDate)
    {
        CustomerId = customerId;
        MedicineId = medicineId;
        DoctorFullName = doctorFullName;
        IssueDate = issueDate;
        ExpirationDate = expirationDate;
    }

    public bool IsValid(DateTime referenceDate) => referenceDate <= ExpirationDate;
}

