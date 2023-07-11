using Microsoft.AspNetCore.Mvc;
using OpenAI.Interfaces;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;

namespace OpenAITranslator.Controllers;

[ApiController]
[Route("translate")]
public class TranslateController : ControllerBase
{
    private readonly IOpenAIService _openAIService;

    public TranslateController(IOpenAIService openAIService)
    {
        _openAIService = openAIService;
    }

    [HttpPost]
    public async Task<IActionResult> Post(TranslateRequest request)
    {
        try
        {
            var result = await _openAIService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
            {
                Model = Models.ChatGpt3_5Turbo,
                Messages = new List<ChatMessage>
                { 
                    ChatMessage.FromSystem($"Translate the following {request.Source} text to {request.Target}:"),
                    ChatMessage.FromUser(request.Text),
                },
            });

            if (!result.Successful)
            {
                throw new Exception(result.Error?.Message ?? "Translation failed.");
            }

            var response = new TranslateResponse(
                request.Source,
                request.Target,
                request.Text,
                result.Choices.FirstOrDefault()?.Message.Content ?? "");

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { ex.GetBaseException().Message });
        }
    }
}

public record TranslateRequest(string Source, string Target, string Text);

public record TranslateResponse(string Source, string Target, string Text, string Result);