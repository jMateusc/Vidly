using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Vidly.Dtos;
using Vidly.Models;

namespace Vidly.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //relaciona DTO <-> Model Customer    ||| ignora propriedade primaria do ID para fazer alteração.
            Mapper.CreateMap<Customer, CustomerDto>().ForMember(c => c.Id,opt =>opt.Ignore());
            Mapper.CreateMap<CustomerDto, Customer>();

            //relaciona DTO <-> Model Movie       ||| ignora propriedade primaria do ID para fazer alteração.
            Mapper.CreateMap<Movie, MovieDto>().ForMember(m => m.Id, opt => opt.Ignore());
            Mapper.CreateMap<MovieDto, Movie>();
        }
    }
}