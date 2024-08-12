using apprecipes.DataAccess.Entity;
using apprecipes.DataTransferObject.Object;
using AutoMapper;

namespace apprecipes
{
    public static class AutoMapper
    {
        private static bool _initMapper = true;
        public static IMapper mapper;
        
        public static void Start()
        {
            if (_initMapper)
            {
                MapperConfiguration configuration = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Authentication, DtoAuthentication>().MaxDepth(3);
                    cfg.CreateMap<DtoAuthentication, Authentication>().MaxDepth(3);
                    
                    cfg.CreateMap<Image, DtoImage>().MaxDepth(3);
                    cfg.CreateMap<DtoImage, Image>().MaxDepth(3);
                    
                    cfg.CreateMap<Video, DtoVideo>().MaxDepth(3);
                    cfg.CreateMap<DtoVideo, Video>().MaxDepth(3);

                    cfg.CreateMap<User, DtoUser>().MaxDepth(3)
                        .ForMember(dest => dest.authetication, opt => opt.MapFrom(src => src.ChildAthentication));
                    cfg.CreateMap<DtoUser, User>().MaxDepth(3)
                        .ForMember(dest => dest.ChildAthentication, opt => opt.MapFrom(src => src.authetication));

                    cfg.CreateMap<Recipe, DtoRecipe>().MaxDepth(3)
                        //.ForMember(dest => dest.ParentDtoCategory, opt => opt.MapFrom(src => src.ParentCategory))
                        .ForMember(dest => dest.images, opt => opt.MapFrom(src => src.ChildImages))
                        .ForMember(dest => dest.videos, opt => opt.MapFrom(src => src.ChildVideos));
                    
                    cfg.CreateMap<DtoRecipe, Recipe>().MaxDepth(3)
                        //.ForMember(dest => dest.ParentCategory, opt => opt.MapFrom(src => src.ParentDtoCategory))
                        .ForMember(dest => dest.ChildImages, opt => opt.MapFrom(src => src.images))
                        .ForMember(dest => dest.ChildVideos, opt => opt.MapFrom(src => src.videos));
                });
                mapper = configuration.CreateMapper();
                _initMapper = false;
            }
        }
    }   
}
