using Core.IRepository;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BuyersController : ControllerBase
    {
        private readonly IBuyerRepository _buyerRepository;
        public BuyersController(IBuyerRepository buyerRepository)
        {
            _buyerRepository = buyerRepository;
        }

        [HttpGet("BuyerList")]
        public async Task<ActionResult> GetBuyerList()
        {
            try
            {
                var response = await _buyerRepository.GetBuyerListAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
            return Ok();
        }
    }
}
