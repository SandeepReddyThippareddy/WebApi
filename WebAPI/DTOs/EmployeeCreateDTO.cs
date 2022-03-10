using Newtonsoft.Json;

namespace Web_API.DTOs
{
    public class EmployeeCreateDTO
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("data")]
        public EmployeeDataDTO Data { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
