using Core.Entities;
using Course.Service.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Service.Dtos;
using Service.Exceptions;
using Service.Services;
using Service.Services.Interfaces;

namespace CourseApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService student)
        {
            _studentService = student;
        }

        [HttpPost("")]
        public IActionResult Create(StudentCreateDto student)
        {
            try
            {
                return StatusCode(201, new { id = _studentService.Create(student) });
            }
            catch (DublicateEntityException e) 
            {
                return Conflict();
            }
            catch (NotFoundException e)
            {
                return NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, "Bilinmedik bir xeta bas verdi");
            }
        }

        [HttpGet("")]
        public ActionResult<List<StudentGetDto>> GetAll()
        {
            return Ok(_studentService.GetAll());
        }


        [HttpGet("{id}")]
        public ActionResult GetById(int id)
        {
            try
            {
                var data = _studentService.GetById(id);
                return StatusCode(200, data);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("{id}")]
        public ActionResult Update(StudentUpdateDto updateDto, int id)
        {
            try
            {
                return StatusCode(201, new { id = _studentService.Update(updateDto, id) });
            }
            catch (DublicateEntityException e)
            {
                return Conflict();
            }
            catch (NotFoundException e)
            {
                return Conflict();
            }
            catch (Exception e)
            {
                return StatusCode(500, "Bilinmedik bir xeta bas verdi");
            }
        }



        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var data = _studentService.Delete(id);
                return StatusCode(200, data);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
    }
}
