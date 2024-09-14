using Microsoft.AspNetCore.Http;
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

        [HttpPost]
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
    }
}
