﻿using Microsoft.AspNetCore.Http;
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

        // Helper functions
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

        // HTTP calls
        [HttpGet("Random")]
        public async Task<IActionResult> getRandomWords()
        {
            int minChars = 142;
            int maxChars = 144;
            int totalCharCount = 0;
            List<string> selectedWords = [];
            List<string> randomWords = await dbContext.Words
                .OrderBy(w => EF.Functions.Random())
                .Select(w => w.Word1)
                .Take(200)
                .ToListAsync();

            foreach(string word in randomWords)
            {
                //Add 1 to denote spaces
                string workingWord = word + "1";
                totalCharCount += workingWord.Length;

                if (totalCharCount > maxChars)
                {
                    break;
                }

                selectedWords.AddRange(workingWord.Select(c => c.ToString()));
                
                if(totalCharCount >= minChars && totalCharCount <= maxChars)
                {
                    break;
                }
            }

            //return WordTestObjects list
            return Ok(convertToWordTestObjects(selectedWords));

        }

    }
}
