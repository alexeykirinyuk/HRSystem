namespace HRSystem.Domain.Attributes.Base
{
    public interface IAttribute
    {
        int Id { get; set; }

        string EmployeeLogin { get; set; }

        int AttributeInfoId { get; set; }

        AttributeInfo AttributeInfo { get; set; }
        
        AttributeType Descriminator { get; set; }
    }
}