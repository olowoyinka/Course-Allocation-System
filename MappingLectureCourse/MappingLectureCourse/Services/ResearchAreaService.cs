using MappingLectureCourse.Data;
using MappingLectureCourse.Interface;
using MappingLectureCourse.Models.MappingViewModel;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MappingLectureCourse.Services
{
    public class ResearchAreaService : IResearchArea
    {
        private readonly ApplicationDbContext _context;

        public ResearchAreaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ResearchArea>> getAllResearchArea(int Id, string search)
        {
            return await _context.researchAreas.ToListAsync();
        }

        public async Task<ResearchArea> getResearchAreaById(Guid? Id)
        {
            return await _context.researchAreas
                            .Include(s => s.Department)
                        .SingleOrDefaultAsync(x => x.ResearchAreaID == Id);
        }

        public async Task<bool> createResearchArea(ResearchArea researchArea)
        {
            await _context.researchAreas.AddAsync(researchArea);

            var created = await _context.SaveChangesAsync();

            return created > 0;
        }

        public async Task<bool> updateResearchArea(ResearchArea researchArea)
        {
            _context.researchAreas.Update(researchArea);

            var updated = await _context.SaveChangesAsync();

            return updated > 0;
        }

        public async Task<bool> deleteResearchArea(Guid? Id)
        {
            var researchArea = await getResearchAreaById(Id);

            if (researchArea == null)
                return false;

            _context.researchAreas.Remove(researchArea);

            var deleted = await _context.SaveChangesAsync();

            return deleted > 0;
        }

        public async Task<bool> checkResearchAreaExist(ResearchArea researchArea,Guid DepartmentID)
        {
            return await _context.researchAreas
                        .AnyAsync(s => s.Name.Equals(researchArea.Name) && s.DepartmentID.Equals(DepartmentID));
        }
    }
}
