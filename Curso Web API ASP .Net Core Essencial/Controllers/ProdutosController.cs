using Curso_Web_API_ASP_.Net_Core_Essencial.Models.Entitys;
using Curso_Web_API_ASP_.Net_Core_Essencial.Models.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext dbContext;
        public ProdutosController(AppDbContext context)
        {
            dbContext = context;
        }

        /// <summary>
        /// Ação que retorna todos os produtos.
        /// </summary>
        /// <returns>Lista de produtos.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoEntity>>> GetAsync()
        {
            try
            {
                return await dbContext.Tb_Produtos.Include(x => x.Categoria).AsNoTracking().ToListAsync(); // Tolist() é para criar uma lista em memórIA. 
                                                                                                           // O método AsNoTracking() é para aumentar a peformace em consultas, desabilita o rastreamento de uma consulta que verifica de alguma coisda mudou.
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao tentar obter todos os produtos. \n{ex.Message}");
            }
        }


        /// <summary>
        /// Ação que retorna um produto em específico, o usuário precisa adicionar um "<paramref name="id"/>", sendo um inteiro >= a 1.
        /// </summary>
        /// <returns>O produto cujo o "<paramref name="id"/>" foi adicionado.</returns>
        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public async Task<ActionResult<ProdutoEntity>> GetAsync(int id)
        {
            try
            {
                var produto = await dbContext.Tb_Produtos.AsNoTracking().FirstOrDefaultAsync(i => i.ProdutoId == id);
                if (produto == null)
                {
                    return NotFound($"O produto com o id={id}, não foi encontrado.");
                }
                else
                {
                    return produto;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao tentar obter o produtos. \n{ex.Message}");
            }

        }

        /// <summary>
        /// Ação que adicionado um produto ao banco de dados, para isso o usuário deve adicionar os dados no corpo da requisisão.
        /// </summary>
        /// <param name="produto"></param>
        /// <returns>Retorna a URL do produto adicionado.</returns>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] ProdutoEntity produto)
        {
            try
            {
                await dbContext.Tb_Produtos.AddAsync(produto);
                await dbContext.SaveChangesAsync();

                return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao tentar adicionar o produtos. \n{ex.Message}");
            }

        }

        /// <summary>
        /// Ação para atualizar um produto através de um Id específico, onde o usuário entrará com esse Id, sendo um inteiro >= a 1.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="produto"></param>
        /// <returns>Se o produto menssagem de erro (Produto com Id não existente), ou sucesso.</returns>
        [HttpPut("{id:int:min(1)}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] ProdutoEntity produto)
        {
            try
            {
                if (id != produto.ProdutoId)
                {
                    return BadRequest($"O Id={id} passado é diferente do id da requisição.");
                }
                else
                {
                    dbContext.Entry(produto).State = EntityState.Modified;
                    await dbContext.SaveChangesAsync();
                    return Ok("Sucesso. Produto atualizado.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao tentar atualizar o produtos. \n{ex.Message}");
            }

        }

        /// <summary>
        /// Ação que deleta um produto em específico. O usuário deve informar o "<paramref name="id"/>" inteiro >= a 1, cujo o produto deve ser excluído.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Se o produto por encontrado retornará o produto deletado, ou se o produto não for encontrado será emitida ima menssagem de erro.</returns>
        [HttpDelete("{id:int:min(1)}")]
        public async Task<ActionResult<ProdutoEntity>> DeleteAsync(int id)
        {
            try
            {
                var produto = await dbContext.Tb_Produtos.FirstOrDefaultAsync(p => p.ProdutoId == id);

                if (produto == null)
                {
                    return NotFound("Produto não encontrado.");
                }
                else
                {
                    dbContext.Tb_Produtos.Remove(produto);
                    await dbContext.SaveChangesAsync();
                    return produto;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao tentar deletar o produtos. \n{ex.Message}");
            }

        }
    }
}
