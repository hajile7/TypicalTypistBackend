using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TyperV1API.Models;

namespace TyperV1API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private TyperV1Context dbContext = new TyperV1Context();

        private Uploader uploader = new Uploader();


        //DTO Conversions
        static UserDTO convertUserDTO(User u)
        {
            return new UserDTO
            {
                UserId = u.UserId,
                FirstName = u.FirstName,
                LastName = u.LastName,
                UserName = u.UserName,
                Email = u.Email,
                Image = convertImageDTO(u.Image),
            };
        }

        static ImageDTO convertImageDTO(Image i)
        {
            if (i == null)
            {
                return null;
            }

            return new ImageDTO
            {
                ImageId = i.ImageId,
                ImagePath = i.ImagePath
            };

        }

        //API Calls

        [HttpGet("{userId}")]
        public IActionResult getUser(int userId)
        {
            User result = dbContext.Users.Include(i => i.Image).Where(u => u.Active == true).FirstOrDefault(u => u.UserId == userId);

            if(result == null || result.Active == false)
            {
                return NotFound("User not found");
            }
            
            return Ok(convertUserDTO(result));
        }

        [HttpGet("Login")]
        public IActionResult Login(string username, string password)
        {
            User result = dbContext.Users.Include(i => i.Image).Where(u => u.Active == true).FirstOrDefault(u => u.UserName == username && u.Password == password);

            if(result == null || result.Active == false)
            {
                return NotFound();
            }
            return Ok(convertUserDTO(result));
        }

        [HttpPost]
        public IActionResult createUser([FromForm] PostUserDTO u)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (dbContext.Users.Any(a => a.UserName == u.UserName && a.Active == true))
            {
                return BadRequest(u.UserName + " is already in use");
            }

            User newUser = new User();

            newUser.UserName = u.UserName;
            newUser.Password = u.Password;
            newUser.FirstName = u.FirstName;
            newUser.LastName = u.LastName;
            newUser.Email = u.Email;
            newUser.Active = true; //do I need this?

            if (u.Image != null)
            {
                Image newImage = uploader.getImage(u.Image, "Users");
                if (newImage != null)
                {
                    newUser.ImageId = newImage.ImageId;
                    newUser.Image = dbContext.Images.Find(newUser.ImageId);
                }
            }
            else
            {
                newUser.ImageId = 101;
                newUser.Image = dbContext.Images.Find(newUser.ImageId);
            }

            dbContext.Users.Add(newUser);
            dbContext.SaveChanges();

            return Ok(convertUserDTO(newUser));

        }

        [HttpPut("{id}")]
        public IActionResult updateUser([FromForm]putUserDTO u, int id)
        {
            User updateUser = dbContext.Users.Include(i => i.Image).FirstOrDefault(u => u.UserId == id);

            if (updateUser == null || updateUser.Active == false)
            {
                return NotFound("User Not Found");
            }
            if (u.FirstName != null)
            {
                updateUser.FirstName = u.FirstName;
            }
            if (u.LastName != null)
            {
                updateUser.LastName = u.LastName;
            }
            if (u.UserName != null)
            {
                if (dbContext.Users.Any(o => o.UserName == u.UserName && u.UserName != updateUser.UserName && o.Active == true))
                {
                    return BadRequest();
                }
                updateUser.UserName = u.UserName;

            }
            if (u.Email != null) {
                if (dbContext.Users.Any(o => o.Email == u.Email && u.Email != updateUser.Email && o.Active == true)) 
                { 
                    return BadRequest(); 
                }
                updateUser.Email = u.Email;
            }
            if (u.Image != null)
            {
                Image newImage = uploader.getImage(u.Image, "Users");
                if (newImage != null)
                {
                    if (updateUser.Image != null && System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), updateUser.Image.ImagePath)) && updateUser.Image.ImagePath != "DefaultProfPic\\V1DefaultProfPic.webp")
                    {
                        System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), updateUser.Image.ImagePath));
                        dbContext.Images.Remove(updateUser.Image);
                    }
                    updateUser.ImageId = newImage.ImageId;
                    updateUser.Image = dbContext.Images.Find(updateUser.ImageId);
                }
            }

            dbContext.Users.Update(updateUser);
            dbContext.SaveChanges();

            return Ok(convertUserDTO(updateUser));

        }

        [HttpDelete("{id}")]
        public IActionResult removeUser(int id)
        {
            User result = dbContext.Users.Find(id);

            if (result == null || result.Active == false)
            {
                return NotFound("User Not Found");
            }

            result.Active = false;

            if(result.Image != null)
            {
                System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), result.Image.ImagePath));
            }

            result.ImageId = 101;
            result.Image = dbContext.Images.Find(result.ImageId);

            dbContext.Users.Update(result);
            dbContext.SaveChanges();

            return NoContent();

        }


        
        


    }
}
