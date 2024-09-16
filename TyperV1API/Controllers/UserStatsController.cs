﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TyperV1API.Models;

namespace TyperV1API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserStatsController : ControllerBase
    {
        private TyperV1Context dbContext = new TyperV1Context();

        //DTO Converstions

        static UserTypingTestDTO convertToTestDTO(UserTypingTest test)
        {
            return new UserTypingTestDTO
            {
                userId = test.UserId,
                CharCount = test.CharCount,
                incorrectCount = test.IncorrectCount,
                Mode = test.Mode,
                Speed = test.Wpm,
                Accuracy = test.Accuracy
            };
        }

        [HttpPost("tests")]
        public async Task<IActionResult> sendTestResults(UserTypingTestDTO testDTO)
        {

            if(testDTO == null)
            {
                return BadRequest(new { Message = "Invalid test data." });
            }

            UserTypingTest typingTest = new UserTypingTest();

            typingTest.UserId = testDTO.userId;
            typingTest.TestDate = DateTime.Now;
            typingTest.CharCount = testDTO.CharCount;
            typingTest.IncorrectCount = testDTO.incorrectCount;
            typingTest.Mode = testDTO.Mode;
            typingTest.Wpm = testDTO.Speed;
            typingTest.Accuracy = testDTO.Accuracy;

            dbContext.UserTypingTests.Add(typingTest);
            await dbContext.SaveChangesAsync();

            return Ok();
            
        }

        [HttpPost("bigraphs")]
        public async Task<IActionResult> sendBigraphResults (userBigraphStatDTO[] statArr)
        {

            if (statArr == null)
            {
                return BadRequest(new { Message = "Inavlid bigraph data." });
            }

            foreach (var bigraphStatDTO in statArr)
            {
                var existingStat = await dbContext.UserBigraphStats.FirstOrDefaultAsync(stat =>
                stat.UserId == bigraphStatDTO.UserId && stat.Bigraph == bigraphStatDTO.Bigraph);

                if (existingStat == null)
                {
                    UserBigraphStat userBigraph = new UserBigraphStat();

                    userBigraph.UserId = bigraphStatDTO.UserId;
                    userBigraph.StartingKey = bigraphStatDTO.Bigraph[0].ToString();
                    userBigraph.Bigraph = bigraphStatDTO.Bigraph;
                    userBigraph.TotalTyped = bigraphStatDTO.Quantity;
                    userBigraph.Accuracy = bigraphStatDTO.Accuracy;
                    userBigraph.Speed = bigraphStatDTO.Speed;

                    dbContext.Add(userBigraph);

                }
                else
                {
                    existingStat.TotalTyped += bigraphStatDTO.Quantity;
                    existingStat.Accuracy = (existingStat.Accuracy + bigraphStatDTO.Accuracy) / 2;
                    existingStat.Speed = (existingStat.Speed + bigraphStatDTO.Speed) / 2;
                }

            }

            await dbContext.SaveChangesAsync();
            return Ok();

        }

        [HttpPost("keys")]
        public async Task<IActionResult> sendKeyResults(UserKeyStatDTO[] statArr)
        {

            if (statArr == null)
            {
                return BadRequest(new { Message = "Inavlid key data." });
            }

            foreach (var userKeyStatDTO in statArr)
            {
                var existingStat = await dbContext.UserKeyStats.FirstOrDefaultAsync(stat =>
                stat.UserId == userKeyStatDTO.UserId && stat.Key == userKeyStatDTO.Key);

                if (existingStat == null)
                {
                    UserKeyStat keyStat = new UserKeyStat();

                    keyStat.UserId = userKeyStatDTO.UserId;
                    keyStat.Key = userKeyStatDTO.Key;
                    keyStat.TotalTyped = userKeyStatDTO.TotalTyped;
                    keyStat.Accuracy = userKeyStatDTO.Accuracy;
                    keyStat.Speed = userKeyStatDTO.Speed;

                    dbContext.Add(keyStat);

                }
                else
                {
                    existingStat.TotalTyped += userKeyStatDTO.TotalTyped;
                    existingStat.Accuracy = (existingStat.Accuracy + userKeyStatDTO.Accuracy) / 2;
                    existingStat.Speed = (existingStat.Speed + userKeyStatDTO.Speed) / 2;
                }

            }

            await dbContext.SaveChangesAsync();
            return Ok();

        }

        [HttpGet("UserTests")]
        public async Task<IActionResult> getTestList(int userId)
        {
            List<UserTypingTest> result = await dbContext.UserTypingTests.Where(t => t.UserId == userId).ToListAsync();

            if (result.Count == 0)
            {
                return NotFound(new { Message = "No typing tests found for user." });
            }

            List<UserTypingTestDTO> formattedResult = result.Select(t => convertToTestDTO(t)).ToList();

            return Ok(formattedResult);

        }

        [HttpGet("UserBigraphs")]
        public async Task<IActionResult> getBigraphStats(int userId)
        {
            List<UserBigraphStat> result = await dbContext.UserBigraphStats.Where(t => t.UserId == userId).ToListAsync();

            if (result.Count == 0)
            {
                return NotFound(new { Message = "No bigraph data found for user." });
            }

            return Ok(result);

        }

        [HttpGet("UserKeys")]
        public async Task<IActionResult> getKeyStats(int userId)
        {
            List<UserKeyStat> result = await dbContext.UserKeyStats.Where(t => t.UserId == userId).ToListAsync();

            if (result.Count == 0)
            {
                return NotFound(new { Message = "No bigraph data found for user." });
            }

            return Ok(result);

        }
    }
}
