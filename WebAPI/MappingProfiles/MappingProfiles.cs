using AutoMapper;
using Web_API.DTOs;
using WebAPI.BLL.Models;

namespace Web_API.MappingProfiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Employee, EmployeeReadDTO>();
            CreateMap<EmployeeReadDTO, Employee>();
            CreateMap<EmployeeCreateDTO, Employee>();
        }
    }
}
