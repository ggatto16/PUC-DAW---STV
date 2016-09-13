using AutoMapper;
using STV.Models;
using STV.ViewModels;

namespace STV.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<Usuario, UsuarioVM>();
            CreateMap<Alternativa, AlternativaVM>();
            CreateMap<Unidade, UnidadeVM>();
            CreateMap<Atividade, AtividadeVM>();
            CreateMap<Usuario, RelatorioUsuario>();
            CreateMap<Curso, DetalhesCurso>();
        }
    }
}