using Amazon.LexRuntimeV2;
using Microsoft.AspNetCore.Mvc;
using Amazon.LexRuntimeV2.Model;
using LexBotBroker.LexModel;
using System.Net;
using Microsoft.AspNetCore.Cors;
using System.Text.Json.Serialization;
using System.Net.Http.Json;
using Newtonsoft.Json;


namespace LexBotBroker
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowSpecificOrigin")]
    public class LexBot2Controller : ControllerBase
    {
      private readonly IAmazonLexRuntimeV2 _lexClient;
      private readonly IConfiguration _configuration;
      private readonly ILogger _logger;
      private static bool OngoingConversation = false;

    public LexBot2Controller(ILogger<LexBot2Controller> logger, IAmazonLexRuntimeV2 lexClient, IConfiguration configuration)
        {
            _lexClient = lexClient;
            _configuration = configuration;
            _logger = logger;
        }

      [HttpPost]
      public async Task<IActionResult> Post([FromBody] LexRequest request)
      {
      try
      {
          _logger.LogInformation("Entering the LexBot2Controller");
          _logger.LogInformation($"Lex Request: {request.InputText.ToString()}");
          string botId, botAliasId;
          var botLanuage = _configuration.GetValue<string>("LexBotSettings:BotLanguage");
          var botSessionId = _configuration.GetValue<string>("LexBotSettings:SessionId");

          RecognizeTextRequest lexRequest;

          if (!request.InputText.Contains("time", StringComparison.OrdinalIgnoreCase) && 
              (request.InputText.Contains("Navigate", StringComparison.OrdinalIgnoreCase) || 
              request.InputText.Contains("Go to", StringComparison.OrdinalIgnoreCase) ||
              request.InputText.Contains("Goto", StringComparison.OrdinalIgnoreCase)))
          {
              botId = _configuration.GetValue<string>("LexBotSettings:NewBotId");
              botAliasId = _configuration.GetValue<string>("LexBotSettings:NewBotAliasId");
          }
          else
          {
              botId = _configuration.GetValue<string>("LexBotSettings:BotId");
              botAliasId = _configuration.GetValue<string>("LexBotSettings:BotAliasId");
          }

          _logger.LogInformation($"We are using the BotId: {botId} and BotAliasId: {botAliasId}");

          lexRequest = new RecognizeTextRequest
          {
              BotAliasId = botAliasId,
              BotId = botId,
              LocaleId = botLanuage,
              SessionId = botSessionId,
              Text = request.InputText,
                    
          };

          var response = await _lexClient.RecognizeTextAsync(lexRequest);
          if (response.HttpStatusCode != HttpStatusCode.OK)
          {
              return StatusCode((int)response.HttpStatusCode);
          }

          var intent = response.SessionStateValue.Intent;
          var intentName = intent.Name;
          var intentState = intent.State;
          var content = response.Messages.FirstOrDefault()?.Content;

          LexResponse lexResponse = new LexResponse
          {
              Content = content,
              ContentType = "text" // Default content type
          };

          if (intentState == "Fulfilled" && (intentName == "CreateStrategy" || intentName == "Navigation"))
          {
              lexResponse.ContentType = "Url";
          }
          else if (intentState == "Failed")
          {
              lexResponse.ContentType = "text";
          }

          _logger.LogInformation($"Lex Response is {JsonConvert.SerializeObject(lexResponse)}");
          return Ok(lexResponse);

      }
      catch (Exception ex)
      {
        throw ex;
      }
    }
    }
  }
