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

                    cfg.CreateMap<User, DtoUser>().MaxDepth(3)
                        .ForMember(dest => dest.ChildDtoAthentication, opt => opt.MapFrom(src => src.ChildAthentication));
                    cfg.CreateMap<DtoUser, User>().MaxDepth(3)
                        .ForMember(dest => dest.ChildAthentication, opt => opt.MapFrom(src => src.ChildDtoAthentication));

                    cfg.CreateMap<Recipe, DtoRecipe>().MaxDepth(3)
                        .ForMember(dest => dest.ChildDtoImages, opt => opt.MapFrom(src => src.ChildImages))
                        .ForMember(dest => dest.ChildDtoVideos, opt => opt.MapFrom(src => src.ChildVideos));
                    
                    cfg.CreateMap<DtoRecipe, Recipe>().MaxDepth(3)
                        .ForMember(dest => dest.ChildImages, opt => opt.MapFrom(src => src.ChildDtoImages))
                        .ForMember(dest => dest.ChildVideos, opt => opt.MapFrom(src => src.ChildDtoVideos));
                });
                mapper = configuration.CreateMapper();
                _initMapper = false;
            }
        }
    }   
}
