using Amazon.LexRuntimeV2;
using Microsoft.AspNetCore.Mvc;
using Amazon.LexRuntimeV2.Model;


namespace LexBotBroker
{
    [Route("api/[controller]")]
    [ApiController]
    public class LexBot2Controller : ControllerBase
    {
      private readonly IAmazonLexRuntimeV2 _lexClient;

      public LexBot2Controller(IAmazonLexRuntimeV2 lexClient)
      {
        _lexClient = lexClient;
      }

      [HttpPost]
      public async Task<IActionResult> Post([FromBody] LexRequest request)
      {
      var lexRequest = new RecognizeTextRequest
      {
        BotAliasId = "",
        BotId = "",
        LocaleId = "en_US", // or your bot's locale
        SessionId = "testSession", // or any unique session ID
        Text = request.InputText
      };

      var response = await _lexClient.RecognizeTextAsync(lexRequest);
        return Ok(response);
      }
    }
  }
