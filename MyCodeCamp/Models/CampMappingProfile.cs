﻿using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using MyCodeCamp.Data.Entities;

namespace MyCodeCamp.Models
{
    public class CampMappingProfile : Profile
    {
        public CampMappingProfile()
        {
            CreateMap<Camp, CampModel>()
                .ForMember(c => c.StartDate,
                       opt => opt.MapFrom(camp => camp.EventDate))
                .ForMember(c => c.EndDate,
                           opt => opt.ResolveUsing(camp => camp.EventDate.AddDays(camp.Length)))
                .ForMember(c => c.Url,
                           opt => opt.ResolveUsing<CampUrlResolver>())
                .ReverseMap()
                .ForMember(m => m.EventDate, 
                           opt => opt.MapFrom(model => model.StartDate))
                .ForMember(m => m.Length, 
                           opt => opt.ResolveUsing(model => (model.EndDate - model.StartDate).Days + 1 ))
                .ForMember(m => m.Location,
                           opt => opt.ResolveUsing(c => new Location()
	            {
			        Address1 = c.LocationAddress1,
	                Address2 = c.LocationAddress2,
	                Address3 = c.LocationAddress3,
	                CityTown = c.LocationCityTown,
	                StateProvince = c.LocationStateProvince,
	                PostalCode = c.LocationPostalCode,
	                Country = c.LocationCountry
	            }));

            CreateMap<Speaker, SpeakerModel>()
            .ForMember(s => s.Url, opt => opt.ResolveUsing<SpeakerUrlResolver>())
            .ReverseMap();

            CreateMap<Speaker, Speaker2Model>()
                .IncludeBase<Speaker, SpeakerModel>()
                .ForMember(s => s.BadgeName, opt => opt.ResolveUsing(s => $"{s.Name} (@{s.TwitterName})"));

            CreateMap<Talk, TalkModel>()
                .ForMember(s => s.Url, opt => opt.ResolveUsing<TalkUrlResolver>())
                .ForMember(s => s.Links, opt => opt.ResolveUsing<TalkLinksResolver>())
                .ReverseMap();
        }
    }
}
