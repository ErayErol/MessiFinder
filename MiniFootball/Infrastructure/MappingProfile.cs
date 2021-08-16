﻿namespace MiniFootball.Infrastructure
{
    using AutoMapper;
    using Data.Models;
    using Models.Fields;
    using Models.Games;
    using Services.Fields;
    using Services.Games.Models;
    using Services.Users;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Game, GameListingServiceModel>();
            CreateMap<Game, GameDeleteServiceModel>();

            CreateMap<Game, GameDetailsServiceModel>()
                .ForMember(gDSM => gDSM.UserId, cfg => cfg.MapFrom(g => g.Admin.UserId))
                .ForMember(gDSM => gDSM.GameId, cfg => cfg.MapFrom(g => g.Id));

            CreateMap<Game, GameDeleteServiceModel>()
                .ForMember(gDSM => gDSM.UserId, cfg => cfg.MapFrom(g => g.Admin.UserId))
                .ForMember(gDSM => gDSM.GameId, cfg => cfg.MapFrom(g => g.Id));

            CreateMap<GameDetailsServiceModel, GameEditServiceModel>()
                .ForMember(gE => gE.GameId, cfg => cfg.MapFrom(gD => gD.GameId))
                .ForMember(gE => gE.FieldName, cfg => cfg.MapFrom(gD => gD.FieldName));

            CreateMap<CreateGameSecondStepViewModel, CreateGameLastStepViewModel>();

            CreateMap<Field, GameFieldListingServiceModel>();
            CreateMap<Field, FieldListingServiceModel>();
            CreateMap<Field, FieldDetailServiceModel>();
            CreateMap<Field, FieldDeleteServiceModel>();
            CreateMap<FieldDetailServiceModel, FieldCreateFormModel>();
            CreateMap<FieldDetailServiceModel, FieldEditFormModel>();

            CreateMap<User, UserDetailsServiceModel>();
        }
    }
}
