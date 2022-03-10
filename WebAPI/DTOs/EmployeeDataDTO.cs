using Newtonsoft.Json;

namespace Web_API.DTOs
{
    public class EmployeeDataDTO
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("employee_name")]
        public string EmployeeName { get; set; }

        [JsonProperty("employee_salary")]
        public long EmployeeSalary { get; set; }

        [JsonProperty("employee_age")]
        public long EmployeeAge { get; set; }

        [JsonProperty("profile_image")]
        public string ProfileImage { get; set; }
    }
}