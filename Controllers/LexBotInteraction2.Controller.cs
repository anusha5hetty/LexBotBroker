using Amazon.LexRuntimeV2;
using Microsoft.AspNetCore.Mvc;
using Amazon.LexRuntimeV2.Model;
using LexBotBroker.LexModel;
using System.Net;
using Microsoft.AspNetCore.Cors;


namespace LexBotBroker
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowSpecificOrigin")]
    public class LexBot2Controller : ControllerBase
    {
      private readonly IAmazonLexRuntimeV2 _lexClient;
      private readonly IConfiguration _configuration;

      public LexBot2Controller(IAmazonLexRuntimeV2 lexClient, IConfiguration configuration)
        {
            _lexClient = lexClient;
            _configuration = configuration;
        }

      [HttpPost]
      public async Task<IActionResult> Post([FromBody] LexRequest request)
      {
            try
            {
                const string GlobalOptionScreen = "http://localhost/planview/AdminApplication/editglobaloptions.aspx"; 
                const string ManageAttributeScreen = "http://localhost/planview/PPM/ManageAttributes.aspx";
                const string ConfiguredScreen = "http://localhost/planview/ConfiguredScreens/AdminConfiguredScreens.aspx";

                RecognizeTextRequest lexRequest;
                SetRecognizeTextLexRequest(request, out lexRequest);

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

                if (intentState == "Fulfilled" && intentName == "Strategy")
                {
                    lexResponse.ContentType = "text";
                }
                else if (intentState == "Fulfilled"&& intentName == "Navigation")
                {
                    //lexResponse.Content = ManageAttributeScreen;
                    lexResponse.ContentType = "Url";
                }
                else if (intentState == "Failed")
                {
                    lexResponse.ContentType = "text";
                }

                return Ok(lexResponse);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void SetRecognizeTextLexRequest(LexRequest request, out RecognizeTextRequest lexRequest)
        {
            var botId = _configuration.GetValue<string>("LexBotSettings:BotId");
            var botAliasId = _configuration.GetValue<string>("LexBotSettings:BotAliasId");
            var botLanuage = _configuration.GetValue<string>("LexBotSettings:BotLanguage");
            var botSessionId = _configuration.GetValue<string>("LexBotSettings:SessionId");

            lexRequest = new RecognizeTextRequest
            {
                BotAliasId = botAliasId,
                BotId = botId,
                LocaleId = botLanuage,
                SessionId = botSessionId,
                Text = request.InputText
            };
        }
    }
  }
