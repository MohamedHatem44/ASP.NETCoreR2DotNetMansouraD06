namespace ASP.NETCoreD06.Models
{
    public class Employee
    {
        /*------------------------------------------------------------------*/
        public int Id { get; set; }
        public required string Name { get; set; }
        public int Age { get; set; }
        public decimal Salary { get; set; }
        public string? ImageUrl { get; set; } = string.Empty;
        /*------------------------------------------------------------------*/
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; } = null!;
        /*------------------------------------------------------------------*/
    }
}
