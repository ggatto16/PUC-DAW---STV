using AutoMapper;
using STV.Models;
using STV.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STV.Mappers
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Curso, cursoVM>();
            });

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Usuario, UsuarioVM>();
            });
        }
    }

}
