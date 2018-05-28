namespace HRSystem.Web.Dtos
{
    public class EmployeeAttributeDto
    {
        public int Id { get; set; }
        public AttributeInfoDto AttributeInfo { get; set; }
        public string Value { get; set; }
    }
}