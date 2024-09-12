using Amazon.Lex;
using Amazon.Lex.Model;
using Amazon.LexModelBuildingService;
using Amazon.LexModelsV2;
using Amazon.LexModelsV2.Model;
using Amazon.LexModelBuildingService.Model;

using Microsoft.AspNetCore.Mvc;
using Amazon.Runtime.Internal;


namespace LexBotBroker
{
  [Route("api/[controller]")]
  [ApiController]
  public class LexBotController : ControllerBase
  {
    private readonly IAmazonLex _lexClient;
    private readonly IAmazonLexModelBuildingService _lexModelBuildingService;


    public LexBotController(IAmazonLex lexClient, IAmazonLexModelBuildingService lexModelBuildingService)
    {
      _lexClient = lexClient;
      _lexModelBuildingService = lexModelBuildingService;
    }

    //[HttpGet("aliases/")]
    //public async Task<IActionResult> GetBotAliases()
    //{
    //  var client = new AmazonLexModelsV2Client();

    //  var request = new ListBotAliasesRequest
    //  {
    //    BotId = "",
    //    MaxResults = 10 // Adjust as needed
    //  };

    //  var response = await client.ListBotAliasesAsync(request);
    //  foreach (var alias in response.BotAliasSummaries)
    //  {
    //    Console.WriteLine($"Alias Name: {alias.BotAliasName}, Alias Id: {alias.BotAliasId}");
    //  }

    //  return Ok(response);
    //}

    //[HttpGet("aliases2/")]
    //public async Task<IActionResult> GetBotAliases2()
    //{
    //  var lexRequest = new GetBotAliasesRequest
    //  {
    //    BotName = "PortfoliosActions",
    //    MaxResults = 10 // Adjust as needed
    //  };

    //  var response = await _lexModelBuildingService.GetBotAliasesAsync(lexRequest);
    //  return Ok(response.BotAliases);
    //}

    //[HttpPost]
    //public async Task<IActionResult> Post([FromBody] LexRequest request)
    //{
    //  var lexRequest = new PostTextRequest
    //  {
    //    BotAlias = "",
    //    BotName = "",
    //    UserId = "UserId",
    //    InputText = request.InputText
    //  };

    //  var response = await _lexClient.PostTextAsync(lexRequest);
    //  return Ok(response.Message); 
    //}
  }

  public class LexRequest
  {
    public string InputText { get; set; }
  }

}
