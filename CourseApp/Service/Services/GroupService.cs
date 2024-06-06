using Core.Entities;
using Course.Service.Exceptions;
using Data;
using Service.Dtos;
using Service.Exceptions;
using Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class GroupService : IGroupService
    {
        private readonly AppDbContext _context;

        public GroupService(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
        public int Create(GroupCreateDto createDto)
        {
            if (_context.Groups.Any(x => x.No == createDto.No && !x.IsDeleted))
                throw new DublicateEntityException();

            Group group = new()
            {
                No = createDto.No,
                Limit = createDto.Limit,
            };


            _context.Groups.Add(group);
            _context.SaveChanges();
            return group.Id;
        }

        public List<GroupGetDto> GetAll()
        {
            return _context.Groups.Where(x=>!x.IsDeleted).Select(x=> new GroupGetDto
            {
                Id = x.Id,
                No = x.No,
                Limit=x.Limit
            }).ToList();
        }

        public GroupGetDto GetById(int id)
        {
            var data = _context.Groups.FirstOrDefault(x=>x.Id == id && !x.IsDeleted);

            if(data == null)
            {
                throw new NotFoundException();
            }
            GroupGetDto dto = new()
            {
                No = data.No,
                Limit = data.Limit,
                Id = id
            };
            return dto;
        }

        public int Update(GroupUpdateDto updateDto, int id)
        {
            var entity = _context.Groups.FirstOrDefault(x => x.Id == id && !x.IsDeleted);

            if (entity == null) throw  new NotFoundException();

            if (entity.No != updateDto.No && _context.Groups.Any(x => x.No == updateDto.No && !x.IsDeleted))
                throw new DublicateEntityException();

            entity.No = updateDto.No;
            entity.Limit = updateDto.Limit;

            _context.SaveChanges();
            return entity.Id;
        }

        public int Delete(int id)
        {
            var data = _context.Groups.FirstOrDefault(x => x.Id == id && !x.IsDeleted);

            if (data == null)
                throw new NotFoundException();

            data.IsDeleted = true;
            _context.SaveChanges();
            return data.Id;
        }
    }
}
