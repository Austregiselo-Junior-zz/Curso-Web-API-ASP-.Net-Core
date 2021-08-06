using Curso_Web_API_ASP_.Net_Core_Essencial.Models.Entitys;
using Curso_Web_API_ASP_.Net_Core_Essencial.Models.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Curso_Web_API_ASP_.Net_Core_Essencial.Controllers
{
  //  [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[Controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext Db_Context;
        public CategoriasController(AppDbContext contexto)
        {
            Db_Context = contexto;
        }

        /// <summary>
        /// Ação que retorna todas as categorias..  
        /// </summary>
        /// <returns>Lista de catgorias.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaEntity>>> GetAsync()
        {
            try
            {
                return await Db_Context.Tb_Categorias.Include(x => x.Produtos).AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao tentar obter todas as categorias. \n{ex.Message}");
            }

        }

        /// <summary>
        /// Ação que retorna a categoria com base no "<paramref name="id"/>" um inteiro >= a 1, passado pelo usuário.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Se a categoria fo achada, ela será retornada, mas caso a categoria cujo "Id" informado não exista será retornado erro.</returns>
        [HttpGet("{id:int:min(1)}", Name = "ObterCategoria")]
        public async Task<ActionResult<CategoriaEntity>> GetAsync(int id)
        {
            try
            {
                var categoria = await Db_Context.Tb_Categorias.AsNoTracking().FirstOrDefaultAsync(i => i.CategoriaId == id);

                if (categoria == null)
                {
                    return NotFound($"O categoria com o id={id}, não foi encontrado.");
                }
                else
                {
                    return categoria;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao tentar atualizar a categoria. \n{ex.Message}");
            }

        }

        /// <summary>
        /// Ação que adiciona um produto ao banco de dados. Os dados a serem adicionados devem ser passados no corpo da requisição.
        /// </summary>
        /// <param name="categoria"></param>
        /// <returns>Retorna a rota em que o produto pode ser encontrado.</returns>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] CategoriaEntity categoria)
        {
            try
            {
               await Db_Context.Tb_Categorias.AddAsync(categoria);
               await Db_Context.SaveChangesAsync();

                return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoria);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao tentar adicionar uma categoria. \n{ex.Message}");
            }

        }

        /// <summary>
        /// Ação em que a categora é atualizada com base no "<paramref name="id"/>" um inteiro >= a 1" passado e no "<paramref name="categoria"/>" adicionado.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="categoria"></param>
        /// <returns>Se o Id da categoria passada for igual ao Id em que está no banco ele será atualizado, caso negativo retornará erro.</returns>
        [HttpPut("{id:int:min(1)}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] CategoriaEntity categoria)
        {
            try
            {
                if (id != categoria.CategoriaId)
                {
                    return NotFound($"O Id={id} passado é diferente do id da requisição.");
                }
                else
                {
                   Db_Context.Entry(categoria).State = EntityState.Modified;
                   await Db_Context.SaveChangesAsync();
                    return Ok("Sucesso. Categoria atualizado.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao tentar atualizar uma categoria. \n{ex.Message}");
            }
        }

        /// <summary>
        /// Ação para deletar uma categoria com base no "<paramref name="id"/>" um inteiro >= a 1" passado pelo usuário.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Caso o Id passado não corresponder a um Id do banco de dados será retonado um erro, mas em caso aformativo será retornado a categoroa deletada.</returns>
        [HttpDelete("{id:int:min(1)}")]
        public async Task<ActionResult<CategoriaEntity>> DeleteAsync(int id)
        {
            try
            {
                var categoria = await Db_Context.Tb_Categorias.FirstOrDefaultAsync(i => i.CategoriaId == id);

                if (categoria == null)
                {
                    return NotFound($"Categoria não encontrada.");
                }
                else
                {
                    Db_Context.Tb_Categorias.Remove(categoria);
                    await Db_Context.SaveChangesAsync();
                    return categoria;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao tentar deletar a categoria. \n{ex.Message}");
            }

        }
    }
}
