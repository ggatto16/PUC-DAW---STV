using AutoMapper;
using STV.AutoMapper;
using STV.Models;
using STV.ViewModels;

namespace STV.Mappers
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<DomainToViewModelMappingProfile>();
                cfg.AddProfile<ViewModelToDomainMappingProfile>();
            });
        }
    }

}
