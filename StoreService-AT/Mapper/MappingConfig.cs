using AutoMapper;
using StoreService_AT.Model.Entities;
using StoreService_AT.Model.VOs;

namespace StoreService_AT.Mapper
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config => {
                config.CreateMap<StoreVO, Store>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
