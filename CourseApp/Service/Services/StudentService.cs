using Core.Entities;
using Course.Service.Exceptions;
using Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Pustok.Helpers;
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
    public class StudentService : IStudentService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public StudentService(AppDbContext appDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _context = appDbContext;
            _webHostEnvironment = webHostEnvironment;
        }
        public int Create(StudentCreateDto createDto)
        {
            Group group = _context.Groups.Include(x => x.Students).FirstOrDefault(x => x.Id == createDto.GroupId && !x.IsDeleted);

            if (group == null)
                throw new NotFoundException();

            if (_context.Students.Any(x => x.Email.ToUpper() == createDto.Email.ToUpper() && !x.IsDeleted))
                throw new DublicateEntityException();

            Student student = new()
            {
                FullName = createDto.FullName,
                Email = createDto.Email,
                BirthDay = createDto.BirthDate,
                GroupId = createDto.GroupId,
            };

            if (createDto.FormFile != null)
            {
                student.Image = FileManager.Save(createDto.FormFile, _webHostEnvironment.WebRootPath, "image");
            }


            _context.Students.Add(student);
            _context.SaveChanges();
            return student.Id;
        }


        public int Delete(int id)
        {
            var data = _context.Students.FirstOrDefault(x=>x.Id == id && !x.IsDeleted);

            if (data == null)
                throw new NotFoundException();

            data.IsDeleted = true;
            _context.SaveChanges();
            return data.Id; 
        }

        public List<StudentGetDto> GetAll()
        {
            return _context.Students.Where(x=>!x.IsDeleted).Select(x => new StudentGetDto
            {
                FullName = x.FullName,
                BirthDate = x.BirthDay,
                Image = x.Image,
            }).ToList();
        }

        public StudentGetDto GetById(int id)
        {
            var data = _context.Students.FirstOrDefault(x=>x.Id == id && !x.IsDeleted);

            if (data == null)
                throw new NotFoundException();


            StudentGetDto student = new()
            {
                FullName = data.FullName,
                BirthDate = data.BirthDay
            };
            return student;
        }

        public int Update(StudentUpdateDto updateDto, int Id)
        {
            Group group = _context.Groups.Include(x => x.Students).FirstOrDefault(x => x.Id == updateDto.GroupId && !x.IsDeleted);

            if (group == null)
                throw new NotFoundException();


            var entity = _context.Students.FirstOrDefault(x=>x.Id ==Id && !x.IsDeleted);
            if (entity == null)
                throw new NotFoundException();
            if (entity.Email != updateDto.Email && _context.Students.Any(x => x.Email == updateDto.Email && !x.IsDeleted))
                throw new DublicateEntityException();
            entity.FullName = updateDto.FullName;
            entity.Email = updateDto.Email;
            entity.BirthDay = updateDto.BirthDate;
            entity.GroupId = updateDto.GroupId;


            _context.SaveChanges();
            return entity.Id;
        }
    }
}
