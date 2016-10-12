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
            CreateMap<UsuarioEditVM, Usuario>();
            CreateMap<AlternativaVM, Alternativa>();
            CreateMap<UnidadeVM, Unidade>();
            CreateMap<AtividadeVM, Atividade>();
            CreateMap<AtividadeVM2, Atividade>();
            CreateMap<MaterialVM, Material>();
            CreateMap<CursoVM, Curso>();
        }
    }
}