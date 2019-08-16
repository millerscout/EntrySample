using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bank.Entries.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Entry.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransferController : ControllerBase
    {
        private ITransferService transferService;

        public TransferController(ITransferService transferService)
        {
            this.transferService = transferService;

        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Bank.Entries.Core.DTOs.TransferDTO transferDTO)
        {
            if (await transferService.InternalTransfer(transferDTO))
            {
                return Ok();
            }

            return BadRequest();


        }

    }
}
