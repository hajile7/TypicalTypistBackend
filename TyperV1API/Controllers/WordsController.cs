using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TyperV1API.Models;

namespace TyperV1API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordsController : ControllerBase
    {
        private TyperV1Context dbContext = new TyperV1Context();

        [HttpGet("Random")]
        public IActionResult getRandomWords()
        {
            int minChars = 110;
            int maxChars = 115;
            int totalCharCount = 0;

            Random random = new Random();

            List<string> selectedWords = new List<string>();

            List<Word> words = dbContext.Words.ToList();

            List<Word> randomWords = words.OrderBy(w => random.Next()).ToList();

            foreach(Word word in randomWords)
            {
                totalCharCount += word.Length;
                if (totalCharCount > maxChars)
                {
                    break;
                }

                selectedWords.Add(word.Word1);

                if(totalCharCount >= minChars && totalCharCount <= maxChars)
                {
                    break;
                }
            }

            return Ok(selectedWords);

        }

    }
}
