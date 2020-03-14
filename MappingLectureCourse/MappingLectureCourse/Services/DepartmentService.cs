using MappingLectureCourse.Data;
using MappingLectureCourse.Interface;
using MappingLectureCourse.Models.ContentViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MappingLectureCourse.Services
{
    public class DepartmentService : IDepartment
    {
        private readonly ApplicationDbContext _context;

        public DepartmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Department>> getAllDepartment(string Search)
        {
            var department = from s in _context.departments
                             select s;

            if (!String.IsNullOrEmpty(Search))
            {
               department = department.Where(s => s.Name.Contains(Search));
            }

            return await department.OrderByDescending(s => s.DepartmentID).ToListAsync();
        }

        public async Task<Department> getDepartmentById(Guid? Id)
        {
            return await _context.departments
                        .SingleOrDefaultAsync(x => x.DepartmentID == Id);
        }

        public async Task<bool> createDepartment(Department department)
        {
            await _context.departments.AddAsync(department);
            
            var created = await _context.SaveChangesAsync();

            return created > 0;
        }

        public async Task<bool> updateDepartment(Guid? Id, Department department)
        {
            var departments = await getDepartmentById(Id);

            departments.Name = department.Name;

            _context.departments.Update(departments);

            var updated = await _context.SaveChangesAsync();

            return updated > 0;
        }

        public async Task<bool> deleteDepartment(Guid? Id)
        {
            var department = await getDepartmentById(Id);

            if (department == null)
                return false;

            _context.departments.Remove(department);

            var deleted = await _context.SaveChangesAsync();

            return deleted > 0;
        }

        public async Task<bool> checkDepartmentExist(Department department)
        {
            return await _context.departments
                        .AnyAsync(s => s.Name.Equals(department.Name));
        }

    }
}
