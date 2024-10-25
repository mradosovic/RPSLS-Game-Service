using Infrastructure.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RPSLS_Game.Presentation.DTOs;

namespace RPSLS_Game.Presentation.Controllers
{
    [ApiController]
    [Route("")]
    public class GameController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GameController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("choices")]
        public async Task<IActionResult> GetChoices()
        {
            try
            {
                var choices = await _mediator.Send(new GetChoicesQuery());
                var choiceDtos = choices.Select(c => new ChoiceDto
                {
                    Id = (int)c,
                    Name = c.ToString()
                });

                if (choiceDtos == null || !choices.Any())
                {
                    return NotFound("No choices available.");
                }

                return Ok(choiceDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving choices.");
            }
        }

        [HttpGet("choice")]
        public async Task<IActionResult> GetRandomChoice()
        {
            try
            {
                var randomChoice = await _mediator.Send(new GetRandomChoiceQuery());

                return Ok(new RandomChoiceDto((int)randomChoice, randomChoice.ToString()));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving a random choice.");
            }
        }

        [HttpPost("play")]
        public async Task<IActionResult> Play([FromBody] PlayRequestDTO request)
        {
            try
            {
                var playCommand = new PlayCommand(request.Player);
                var result = await _mediator.Send(playCommand);

                if (result == null)
                {
                    return NotFound("Unable to complete the game with the provided data.");
                }

                return Ok(new PlayResultDto(result.Results, result.Player, result.Computer));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while playing the game.");
            }
        }

    }
}
