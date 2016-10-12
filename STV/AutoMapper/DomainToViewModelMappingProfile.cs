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
            CreateMap<Usuario, UsuarioEditVM>();
            CreateMap<Alternativa, AlternativaVM>();
            CreateMap<Unidade, UnidadeVM>();
            CreateMap<Atividade, AtividadeVM>();
            CreateMap<Atividade, AtividadeVM2>();
            CreateMap<Usuario, RelatorioUsuario>();
            CreateMap<Curso, DetalhesCurso>();
            CreateMap<Material, MaterialVM>();
            CreateMap<Curso, CursoVM>();
        }
    }
}