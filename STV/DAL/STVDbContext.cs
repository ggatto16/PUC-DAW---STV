namespace STV.DAL
{
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using STV.Models;
    using System.Data.Entity.Infrastructure;

    public partial class STVDbContext : DbContext
    {
        public STVDbContext()
            : base("name=STVDbContext")
        {
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 600;
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
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Nota> Nota { get; set; }
        public virtual DbSet<Resposta> Resposta { get; set; }
        public virtual DbSet<NotaCurso> NotaCurso { get; set; }
        public virtual DbSet<Medalha> Medalha { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

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
                .HasRequired(x => x.Instrutor).WithMany(x => x.CursosGerenciaveis)
                .HasForeignKey(x => x.IdusuarioInstrutor).WillCascadeOnDelete(false);


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

            modelBuilder.Entity<Role>()
                .ToTable("Role")
                .HasKey(x => x.Idrole);

            modelBuilder.Entity<Usuario>()
                .HasMany(x => x.Roles)
                .WithMany(x => x.Usuarios)
                .Map(m =>
                {
                    m.MapLeftKey("Idusuario");
                    m.MapRightKey("Idrole");
                    m.ToTable("UsuarioRole");
                });

            modelBuilder.Entity<Nota>()
                .HasKey(x => new { x.Idusuario, x.Idatividade });

            modelBuilder.Entity<Nota>().HasRequired(x => x.Usuario).WithMany(x => x.Notas)
                .HasForeignKey(x => x.Idusuario).WillCascadeOnDelete(true);

            modelBuilder.Entity<Nota>().HasRequired(x => x.Atividade).WithMany(x => x.Notas)
                .HasForeignKey(x => x.Idatividade).WillCascadeOnDelete(true);

            modelBuilder.Entity<Resposta>()
                .HasKey(x => new { x.Idusuario, x.Idquestao });

            modelBuilder.Entity<Resposta>().HasRequired(x => x.Usuario).WithMany(x => x.Respostas)
                .HasForeignKey(x => x.Idusuario).WillCascadeOnDelete(false);

            modelBuilder.Entity<Resposta>().HasRequired(x => x.Questao).WithMany(x => x.Respostas)
                .HasForeignKey(x => x.Idquestao).WillCascadeOnDelete(false);

            modelBuilder.Entity<NotaCurso>()
                .HasKey(x => new { x.Idusuario, x.Idcurso });

            modelBuilder.Entity<NotaCurso>().HasRequired(x => x.Usuario).WithMany(x => x.NotasCursos)
                .HasForeignKey(x => x.Idusuario).WillCascadeOnDelete(true);

            modelBuilder.Entity<NotaCurso>().HasRequired(x => x.Curso).WithMany(x => x.NotasCurso)
                .HasForeignKey(x => x.Idcurso).WillCascadeOnDelete(true);

            modelBuilder.Entity<Material>()
                .HasMany(x => x.UsuariosConsulta)
                .WithMany(x => x.MateriaisConsultados)
                .Map(m =>
                {
                    m.MapLeftKey("Idmaterial");
                    m.MapRightKey("Idusuario");
                    m.ToTable("MaterialUsuario");
                });

            modelBuilder.Entity<Medalha>()
                .HasKey(x => x.Idmedalha);

            modelBuilder.Entity<Usuario>()
                .HasMany(x => x.Medalhas)
                .WithMany(x => x.Usuarios)
                .Map(m =>
                {
                    m.MapLeftKey("Idusuario");
                    m.MapRightKey("Idmedalha");
                    m.ToTable("UsuarioMedalha");
                });

        }

    }
}
