using apprecipes.DataAccess.Entity;
using apprecipes.DataTransferObject.Object;
using AutoMapper;

namespace apprecipes
{
    public static class AutoMapper
    {
        private static bool autoMapperInit = true;
        public static IMapper mapper;
        
        public static void Start()
        {
            if (autoMapperInit)
            {
                MapperConfiguration configuration = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Image, DtoImage>().MaxDepth(3);
                    cfg.CreateMap<DtoImage, Image>().MaxDepth(3);

                    cfg.CreateMap<Authentication, DtoAuthentication>().MaxDepth(1);
                    cfg.CreateMap<DtoAuthentication, Authentication>().MaxDepth(1);
                    
                    cfg.CreateMap<User, DtoUser>().MaxDepth(2)
                        .ForMember(dest => dest.ChildDtoAthentication, opt => opt.MapFrom(src => src.ChildAthentication));
                    cfg.CreateMap<DtoUser, User>().MaxDepth(3)
                        .ForMember(dest => dest.ChildAthentication, opt => opt.MapFrom(src => src.ChildDtoAthentication));

                    
                });
                mapper = configuration.CreateMapper();
                autoMapperInit = false;
            }
        }
    }   
}
