using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using back_modelo.BLL;
using back_modelo.Utils;
using back_modelo.BLL.Exceptions;
using back_modelo.DAL.Models;
using back_modelo.Extensions.Responses;
using AutoMapper;

namespace back_modelo.Controllers
{
    // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class CorController:ControllerBase
    {
        private readonly ICorBLL _corBll;
        private ILoggerManager _logger;
        private IMapper _mapper;
        
        public CorController(ICorBLL corBll, ILoggerManager logger, IMapper mapper)
        {
            _corBll = corBll;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("ObterCores")]
        public ActionResult<List<Cor>> ObterCores()
        {
            var cores = _corBll.ObterCores();

            if (cores == null)
            {
                return NotFound(new ApiResponse(404, "Não há cores cadastradas."));
            }
            
            List<Cor> listaCores = new List<Cor>();

            foreach (var item in cores)
            {
                listaCores.Add(_mapper.Map<Cor>(item));
            }

            return Ok(new ApiOkResponse(listaCores));
        }

        [HttpGet("ObterCoresPorId/{idCor}")]
        public ActionResult<Cor> ObterCorPorId(string idCor)
        {
            var cor = _corBll.ObterCorPorId(idCor);

            if (cor == null)
            {
                return NotFound(new ApiResponse(404, $"Cor com o id->{idCor} não foi encontrada."));
            }            

            return Ok(new ApiOkResponse(_mapper.Map<Cor>(cor)));
        }

        [HttpGet("ObterCorPorNome/{nomeCor}")]
        public ActionResult<Cor> ObterCorPorNome(string nomeCor)
        {
            try
            {
                return Ok(_corBll.ObterCorPorNome(nomeCor));
            }
            catch (ArgumentException)
            {
                return BadRequest(new ApiResponse(406, "Nome não pode ser vazio."));
            }
            catch(NotFoundException) 
            {
                return NotFound(new ApiResponse(404, $"Cor com o nome->{nomeCor} não foi encontrado(a)."));
            }
        }

        [HttpPost("InserirCor")]
        public IActionResult InserirCor([FromBody]Cor cor)
        {
            try
            {
                _corBll.InserirCor(_mapper.Map<Cor>(cor));
                return Ok(new ApiResponse(200, "Cor inserida com sucesso."));
            }
            catch (ArgumentException)
            {
                return BadRequest(new ApiResponse(406, "Nome não pode ser vazio."));
            }
            catch (Exception)
            {
                return BadRequest(new ApiResponse(405, "Já existe uma cor com esse nome."));
            }
        }

        [HttpPut("AtualizarCor/{idCor}")]
        public IActionResult AtualizarCor(string idCor, Cor novaCor)
        {
            try
            {
                _corBll.AtualizarCor(idCor, novaCor);
                return Ok(new ApiResponse(200, $"{novaCor.NomeCor} atualizado(a) com sucesso."));
            }
            catch (Exception)
            {
                return BadRequest(new ApiResponse(405, "Já existe uma cor com esse nome."));
            }
        }
        
        [HttpDelete("DeletarCor/{idCor}")]
        public IActionResult DeletarCor(string idCor)
        {
            try
            {
                _corBll.DeletarCor(idCor);  
                return Ok(new ApiResponse(200, $"Cor removida com sucesso."));
            }
            catch (NotFoundException)
            {
                return NotFound(new ApiResponse(404, $"Cor com o id {idCor}, não foi encontrada."));
            }
            catch (FormatException)
            {
                return NotFound(new ApiResponse(406, $"Id informado {idCor}, não é válido."));
            }
            catch (Exception)
            {
                return BadRequest(new ApiResponse(403, "Essa cor está vinculada a uma pessoa."));
            }
        }
    }
}
