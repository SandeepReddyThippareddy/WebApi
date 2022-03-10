using AutoMapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Web_API.DTOs;

namespace Web_API.SyncDataService.Http
{
    public class HttpCommandDataClient : IEmployeeDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public HttpCommandDataClient(HttpClient httpClient, IMapper mapper)
        {
            _httpClient = httpClient;
            _mapper = mapper;
        }
        public async Task<EmployeeCreateDTO> GetDataFromEmployeeApi(int id)
        {

            var response = await _httpClient.GetAsync($"employee/{id}");

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Request to Employee API was OK");

                using (HttpContent content = response.Content)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    var res = JsonConvert.DeserializeObject<EmployeeCreateDTO>(responseBody);

                    return res;
                }
            }
            else
            {
                Console.WriteLine(response.Headers);
                Console.WriteLine(response.Content);
                Console.WriteLine(response.StatusCode);
                Console.WriteLine("Request to Employee API was not OK");
            }

            return new EmployeeCreateDTO();

        }
    }
}
