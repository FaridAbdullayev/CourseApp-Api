using Service.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Service.Services.Interfaces
{
    public interface IGroupService
    {
        int Create(GroupCreateDto createDto);
        List<GroupGetDto> GetAll();
        GroupGetDto GetById(int id);
        int Update(GroupUpdateDto updateDto,int Id);
        int Delete(int id);
    }
}
