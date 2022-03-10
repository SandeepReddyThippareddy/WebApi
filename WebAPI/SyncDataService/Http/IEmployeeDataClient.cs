using System.Threading.Tasks;
using Web_API.DTOs;

namespace Web_API.SyncDataService.Http
{
    public interface IEmployeeDataClient
    {
        Task<EmployeeCreateDTO> GetDataFromEmployeeApi(int id);
    }
}
