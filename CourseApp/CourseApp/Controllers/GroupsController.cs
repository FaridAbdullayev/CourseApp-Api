using Course.Service.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Service.Dtos;
using Service.Exceptions;
using Service.Services.Interfaces;

namespace CourseApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupService _groupService;
        public GroupsController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpPost("")]
        public ActionResult Create(GroupCreateDto createDto)
        {
            try
            {
                return StatusCode(201, new { id = _groupService.Create(createDto) });
            }
            catch (DublicateEntityException e)
            {
                return Conflict();
            }
            catch (Exception e)
            {
                return StatusCode(500, "Bilinmedik bir xeta bas verdi");
            }
        }

        [HttpGet("")]
        public ActionResult<List<GroupGetDto>> GetAll()
        {
            return Ok(_groupService.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult GetById(int id)
        {
            try
            {
                var data = _groupService.GetById(id);
                return StatusCode(200, data);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

        }

        [HttpPut("{id}")]
        public ActionResult Update(GroupUpdateDto groupUpdateDto,int id)
        {
            try
            {
                return StatusCode(201, new { id = _groupService.Update(groupUpdateDto,id) });
            }
            catch (DublicateEntityException e)
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
                var data = _groupService.Delete(id);
                return StatusCode(200, data);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
    }
}
