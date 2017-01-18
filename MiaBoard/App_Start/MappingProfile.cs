using AutoMapper;
using MiaBoard.Dtos;
using MiaBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiaBoard.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Domain to Dto
            Mapper.CreateMap<DataSource, DataSourceDto>();
            Mapper.CreateMap<Dashboard, DashboardDto>();

            // Dto to Domain
            Mapper.CreateMap<DataSourceDto, DataSource>()
               .ForMember(c => c.Id, opt => opt.Ignore());
            
            Mapper.CreateMap<DashboardDto, Dashboard>()
                .ForMember(c => c.Id, opt => opt.Ignore());
        }

    }
}