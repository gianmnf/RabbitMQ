using AutoMapper;
using back_modelo.DAL.DTO;
using back_modelo.DAL.Models;

namespace back_modelo
{
    public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration()
        {
            CreateMap<PessoaDTO, Pessoa>().
                AfterMap((dto, model) => model.IdPessoa = dto.IdPessoa);

            CreateMap<CorDTO, Cor>().
                AfterMap((dto, model) => model.IdCor = dto.IdCor);
        }
    }
}