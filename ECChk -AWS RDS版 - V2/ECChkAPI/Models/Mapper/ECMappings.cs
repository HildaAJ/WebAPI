using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace ECChkAPI.Models.Mapper
{
    public class ECMappings : Profile
    {
        public ECMappings()
        {
            CreateMap<IFECCUTF, IFECCUTFDto>().ReverseMap();
        }

    }
}
