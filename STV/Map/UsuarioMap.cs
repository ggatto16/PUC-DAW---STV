using STV.Models;
using System.Data.Entity.ModelConfiguration;

namespace STV.Map
{
    public class UsuarioMap : EntityTypeConfiguration<Usuario>
    {
        public UsuarioMap()
        {
            ToTable("Usuario");
            HasKey(x => x.Idusuario);

            HasRequired(x => x.Departamento).WithMany(x => x.Usuarios)
                .Map(m => m.MapKey("Iddepartamento"));

        }
    }
}