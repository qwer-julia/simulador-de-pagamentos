using AutoMapper;
using BenefitsManager.Models;

namespace BenefitsManager.Services
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Taxpayer, Taxpayer>();
        }
    }
}
