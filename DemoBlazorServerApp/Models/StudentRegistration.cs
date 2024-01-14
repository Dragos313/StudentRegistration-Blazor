namespace DemoBlazorServerApp.Models
{
    public class StudentRegistration
    {
        public long StudentId { get; set; }
        public string FullName { get; set; }

        public long UserId { get; set; }
        public string Email { get; set; }
        public string Specialization { get; set; }

        public long SpecId { get; set; }
        public string Tax { get; set; }
        public string Years { get; set; }
        public string RegisterDate { get; set; }
    }
}
