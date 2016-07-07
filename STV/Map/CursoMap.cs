using STV.Models;
using System.Data.Entity.ModelConfiguration;

namespace STV.Map
{
    public class CursoMap : EntityTypeConfiguration<Curso>
    {
        public CursoMap()
        {
            ToTable("Curso");
            HasKey(x => x.Idcurso);

            HasRequired(x => x.Usuario).WithMany(x => x.Cursos)
                .Map(m => m.MapKey("Idusuario"));

            HasMany(x => x.Departamentos)
                .WithMany(x => x.Cursos)
                .Map(m =>
                {
                    m.MapLeftKey("IdcursoS");
                    m.MapRightKey("Iddepartamento");
                    m.ToTable("CursoDepartamento");
                });

        }
    }
}