namespace STV.DAL
{
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using STV.Models;

    public partial class STVDbContext : DbContext
    {
        public STVDbContext()
            : base("name=STVDbContext")
        {
        }

        public virtual DbSet<Departamento> Departamento { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<Curso> Curso { get; set; }
        public virtual DbSet<Unidade> Unidade { get; set; }
        public virtual DbSet<Material> Material { get; set; }
        public virtual DbSet<Atividade> Atividade { get; set; }
        public virtual DbSet<Questao> Questao { get; set; }
        public virtual DbSet<Alternativa> Alternativa { get; set; }
        public virtual DbSet<Arquivo> Arquivo { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Departamento>()
                .ToTable("Departamento")
                .HasKey(x => x.Iddepartamento);


            modelBuilder.Entity<Usuario>()
                .ToTable("Usuario")
                .HasKey(x => x.Idusuario);

            modelBuilder.Entity<Usuario>()
                .HasRequired(x => x.Departamento).WithMany(x => x.Usuarios).HasForeignKey(x => x.Iddepartamento);

            modelBuilder.Entity<Curso>()
                .ToTable("Curso")
                .HasKey(x => x.Idcurso);

            
            modelBuilder.Entity<Curso>()
                .HasRequired(x => x.Instrutor).WithMany(x => x.CursosInstrutor)
                .HasForeignKey(x => x.IdusuarioInstrutor);
                //.Map(m => m.MapKey("IdusuarioInstrutor")).WillCascadeOnDelete(true);

            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Curso>()
                .HasMany(x => x.Departamentos)
                .WithMany(x => x.Cursos)
                .Map(m =>
                {
                    m.MapLeftKey("Idcurso");
                    m.MapRightKey("Iddepartamento");
                    m.ToTable("CursoDepartamento");
                });

            modelBuilder.Entity<Curso>()
                .HasMany(x => x.Usuarios)
                .WithMany(x => x.Cursos)
                .Map(m =>
                {
                    m.MapLeftKey("Idcurso");
                    m.MapRightKey("Idusuario");
                    m.ToTable("CursoUsuario");
                });

            modelBuilder.Entity<Unidade>()
                .ToTable("Unidade")
                .HasKey(x => x.Idunidade);

            modelBuilder.Entity<Unidade>()
                .HasRequired(x => x.Curso).WithMany(x => x.Unidades).HasForeignKey(x => x.Idcurso);
                //.Map(m => m.MapKey("Idcurso"));

            modelBuilder.Entity<Material>()
                .ToTable("Material")
                .HasKey(x => x.Idmaterial);

            modelBuilder.Entity<Material>()
                .HasRequired(x => x.Unidade).WithMany(x => x.Materiais).HasForeignKey(x => x.Idunidade);
            //.Map(m => m.MapKey("Idunidade"));

            modelBuilder.Entity<Arquivo>()
                .ToTable("Arquivo")
                .HasKey(x => x.Idmaterial);

            modelBuilder.Entity<Arquivo>()
                .HasRequired(x => x.Material).WithOptional(x => x.Arquivo).WillCascadeOnDelete(true);

            modelBuilder.Entity<Atividade>()
                .ToTable("Atividade")
                .HasKey(x => x.Idatividade);

            modelBuilder.Entity<Atividade>()
                .HasRequired(x => x.Unidade).WithMany(x => x.Atividades).HasForeignKey(x => x.Idunidade);

            modelBuilder.Entity<Questao>()
                .ToTable("Questao")
                .HasKey(x => x.Idquestao);

            modelBuilder.Entity<Questao>()
                .HasRequired(x => x.Atividade).WithMany(x => x.Questoes).HasForeignKey(x => x.Idatividade);

            modelBuilder.Entity<Alternativa>()
                .ToTable("Alternativa")
                .HasKey(x => x.Idalternativa);

            modelBuilder.Entity<Alternativa>()
                .HasRequired(x => x.Questao).WithMany(x => x.Alternativas).HasForeignKey(x => x.Idquestao);

        }

    }
}
