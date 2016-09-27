using AutoMapper;
using STV.Models;
using STV.ViewModels;

namespace STV.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<UsuarioVM, Usuario>();
            CreateMap<AlternativaVM, Alternativa>();
            CreateMap<UnidadeVM, Unidade>();
            CreateMap<AtividadeVM, Atividade>();
            CreateMap<MaterialVM, Material>();
        }
    }
}