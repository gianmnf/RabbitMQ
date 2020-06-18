using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using back_modelo.BLL;
using back_modelo.Utils;
using back_modelo.BLL.Exceptions;
using back_modelo.DAL.Models;
using back_modelo.DAL.DAO;
using back_modelo.Extensions.Responses;
using AutoMapper;

namespace back_modelo.Controllers
{
    // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class PessoaController:ControllerBase
    {
        private readonly IPessoaBLL _pessoaBll;
        private ILoggerManager _logger;
        private IMapper _mapper;


        public PessoaController(IPessoaBLL pessoaBll, ILoggerManager logger, IMapper mapper,IMBDAO mb)
        {
            _pessoaBll = pessoaBll;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("ObterPessoas")]
        public ActionResult<List<Pessoa>> ObterPessoas()
        {
            var cores = _pessoaBll.ObterPessoas();

            if (cores == null)
            {
                return NotFound(new ApiResponse(404, "Não há pessoas cadastradas."));
            }
            
            List<Pessoa> listaPessoas = new List<Pessoa>();

            foreach (var item in cores)
            {
                listaPessoas.Add(_mapper.Map<Pessoa>(item));
            }

            return Ok(new ApiOkResponse(listaPessoas));
        }

        [HttpGet("ObterPessoaPorId/{idPessoa}")]
        public ActionResult<Pessoa> ObterPessoaPorId(string idPessoa)
        {
            var cor = _pessoaBll.ObterPessoaPorId(idPessoa);

            if (cor == null)
            {
                return NotFound(new ApiResponse(404, $"Pessoa com o id {idPessoa}, não foi encontrada."));
            }            

            return Ok(new ApiOkResponse(_mapper.Map<Pessoa>(cor)));
        }

        [HttpGet("ObterPessoaPorCPF/{cpf}")]
        public ActionResult<Pessoa> ObterPessoaPorCPF(string cpf)
        {
            var cor = _pessoaBll.ObterPessoaPorCPF(cpf);

            if (cor == null)
            {
                return NotFound(new ApiResponse(404, $"Pessoa com o CPF {cpf}, não foi encontrada."));
            }            

            return Ok(new ApiOkResponse(_mapper.Map<Pessoa>(cor)));
        }

        [HttpGet("ObterPessoasPorCor/{cor}")]
        public ActionResult<List<Pessoa>> ObterPessoasPorCor(string cor)
        {
            var cores = _pessoaBll.ObterPessoasPorCor(cor);

            if (cores == null)
            {
                return NotFound(new ApiResponse(404, "Não há pessoas cadastradas com essa cor."));
            }
            
            List<Pessoa> listaPessoas = new List<Pessoa>();

            foreach (var item in cores)
            {
                listaPessoas.Add(_mapper.Map<Pessoa>(item));
            }

            return Ok(new ApiOkResponse(listaPessoas));
        }

        [HttpGet("ObterTotalPessoas")]
        public int ObterTotalPessoas()
        {
            return _pessoaBll.ObterTotalPessoas();
        }

        [HttpPost("InserirPessoa")]
        public IActionResult InserirPessoa(Pessoa novaPessoa)
        {
            try
            {
                _pessoaBll.InserirPessoa(novaPessoa);
                return Ok(new ApiResponse(200, "Pessoa inserida com sucesso."));
            }   
            catch (Exception)
            {
                return BadRequest(new ApiResponse(405, "Já existe uma pessoa com esse CPF."));
            }
        }

        [HttpPut("AtualizarPessoa/{idPessoa}")]
        public IActionResult AtualizarPessoa(string idPessoa, Pessoa novaPessoa)
        {
            try
            {
                _pessoaBll.AtualizarPessoa(idPessoa, novaPessoa);
                return Ok(new ApiResponse(200, $"{novaPessoa.Nome} atualizado(a) com sucesso."));
            }
            catch (NotFoundException)
            {
                return NotFound(new ApiResponse(404, "Pessoa não encontrada."));
            }
            catch (Exception)
            {
                return BadRequest(new ApiResponse(405, "Já existe uma pessoa com esse CPF."));
            }
        }
        
        [HttpDelete("DeletarPessoa/{idPessoa}")]
        public IActionResult DeletarPessoa(string idPessoa)
        {
            try
            {
                _pessoaBll.DeletarPessoa(idPessoa);
                return Ok(new ApiResponse(200, $"Pessoa removida com sucesso."));
            }
            catch (NotFoundException) 
            {
                return NotFound(new ApiResponse(404, "Pessoa não encontrada."));
            }
        }
    }
}
