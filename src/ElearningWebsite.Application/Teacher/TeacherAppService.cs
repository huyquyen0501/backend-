using Abp.Application.Services.Dto;
using Abp.Domain.Uow;
using ElearningWebsite.Entities;
using ElearningWebsite.Teacher.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningWebsite.Teacher
{
    public class TeacherAppService:AppServiceBase
    {

        public async Task<PagedResultDto<GetAllCourseOutputDto>> GetAllPaging(GetAllCourseInput input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var b = WorkScope.GetAll<Course>().Select(s => s).ToList();
                var a = from C in WorkScope.GetAll<Course>().Where(s => s.Title.Contains(input.NameOfCourse, StringComparison.OrdinalIgnoreCase))

                        select new GetAllCourseOutputDto
                        {
                            Creator = C.Creator.FullName,
                            DateCreator = C.CreationTime,
                            LastModify = C.LastModificationTime,
                            Tilte = C.Title,
                            Description = C.Details,
                            //TypeOfCourses=d.CourseType.Deltail.ToList()
                        };
                var TotalCount = await a.CountAsync();
                var result = await a.Skip(input.PageSize - 1).Take(input.PageSize).ToListAsync();
                return new PagedResultDto<GetAllCourseOutputDto>(TotalCount, result);

            }
        }
    }
}
