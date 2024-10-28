using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;
using TyperV1API.Models;

namespace TyperV1API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordsController : ControllerBase
    {
        private TyperV1Context dbContext = new TyperV1Context();

        //Helper functions

        static List<WordTestObject> convertToWordTestObjects(List<string> strs)
        {
            List<WordTestObject> wordTestObjects = new List<WordTestObject>();
            List<char> currentWordChars = new List<char>();
            int startIndex = 0;

            for(int i = 0; i < strs.Count; i++)
            {
                char s = char.Parse(strs[i]);

                if(s == '1')
                {
                    if(currentWordChars.Count > 0)
                    {
                        wordTestObjects.Add(new WordTestObject{ chars = currentWordChars, startIndex = startIndex});
                        currentWordChars = new List<char>();
                    }
                    startIndex = i + 1;
                }
                else
                {
                    if (currentWordChars.Count == 0)
                    {
                        startIndex = i;
                    }
                    currentWordChars.Add(s);
                }
            }
            if(currentWordChars.Count > 0)
            {
                wordTestObjects.Add(new WordTestObject { chars = currentWordChars, startIndex = startIndex });
            }
            return wordTestObjects;
        } 

        //HTTP calls
        [HttpGet("Random")]
        public async Task<IActionResult> getRandomWords()
        {
            int minChars = 142;
            int maxChars = 144;
            int totalCharCount = 0;

            Random random = new Random();

            List<string> selectedWords = new List<string>();

            List<Word> words = await dbContext.Words.ToListAsync();

            List<Word> randomWords = words.OrderBy(w => random.Next()).ToList();

            foreach(Word word in randomWords)
            {
                //Add 1 to denote spaces
                string workingWord = word.Word1 + "1";
                totalCharCount += workingWord.Length;
                if (totalCharCount > maxChars)
                {
                    break;
                }

                //Add characters of word + its corresponding space to selectedWords
                for (int i = 0; i < workingWord.Length; i++) 
                {
                    selectedWords.Add(workingWord[i].ToString());
                }
                
                if(totalCharCount >= minChars && totalCharCount <= maxChars)
                {
                    break;
                }
            }

            //return WordTestObjects list
            return Ok(convertToWordTestObjects(selectedWords));

        }

        //[HttpGet("Random")]
        //public async Task<IActionResult> getRandomWords()
        //{
        //    int minChars = 142;
        //    int maxChars = 144;
        //    int totalCharCount = 0;

        //    Random random = new Random();

        //    List<string> selectedWords = new List<string>();

        //    List<Word> words = await dbContext.Words.ToListAsync();

        //    List<Word> randomWords = words.OrderBy(w => random.Next()).ToList();

        //    foreach (Word word in randomWords)
        //    {
        //        totalCharCount += word.Length;
        //        if (totalCharCount > maxChars)
        //        {

        //            break;
        //        }

        //        selectedWords.Add(word.Word1);

        //        if (totalCharCount >= minChars && totalCharCount <= maxChars)
        //        {
        //            break;
        //        }
        //    }

        //    return Ok(selectedWords);

        //}


    }
}
