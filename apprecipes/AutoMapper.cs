using apprecipes.DataTransferObject;
using apprecipes.Models;
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
                });
                mapper = configuration.CreateMapper();
                autoMapperInit = false;
            }
        }
    }   
}
