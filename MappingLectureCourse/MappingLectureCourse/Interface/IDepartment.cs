using MappingLectureCourse.Models.ContentViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MappingLectureCourse.Interface
{
    public interface IDepartment
    {
        Task<List<Department>> getAllDepartment(string Search);

        Task<Department> getDepartmentById(Guid? Id);

        Task<bool> createDepartment(Department department);

        Task<bool> updateDepartment(Guid? Id, Department department);

        Task<bool> deleteDepartment(Guid? Id);

        Task<bool> checkDepartmentExist(Department department);
    }
}
