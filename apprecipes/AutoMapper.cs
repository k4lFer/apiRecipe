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

                    cfg.CreateMap<Category, DtoCategory>().MaxDepth(3);
                    cfg.CreateMap<DtoCategory, Category>().MaxDepth(3);
                    
                    cfg.CreateMap<Recipe, DtoRecipe>().MaxDepth(3)
                        .ForMember(dest => dest.images, opt => opt.MapFrom(src => src.ChildImages))
                        .ForMember(dest => dest.videos, opt => opt.MapFrom(src => src.ChildVideos))
                        .ForMember(dest => dest.rating, opt => opt.MapFrom(src => src.ChildRating));
                    
                    cfg.CreateMap<DtoRecipe, Recipe>().MaxDepth(3)
                        .ForMember(dest => dest.ChildImages, opt => opt.MapFrom(src => src.images))
                        .ForMember(dest => dest.ChildVideos, opt => opt.MapFrom(src => src.videos));
                    
                    cfg.CreateMap<Like, DtoLike>().MaxDepth(3);
                    cfg.CreateMap<DtoLike, Like>().MaxDepth(3);
                    
                    cfg.CreateMap<Rating, DtoRating>().MaxDepth(3);
                    cfg.CreateMap<DtoRating, Rating>().MaxDepth(3);
                });
                mapper = configuration.CreateMapper();
                _initMapper = false;
            }
        }
    }   
}
