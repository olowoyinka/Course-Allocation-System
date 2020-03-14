using MappingLectureCourse.Models.MappingViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MappingLectureCourse.Interface
{
    public interface IResearchArea
    {
        Task<List<ResearchArea>> getAllResearchArea(int Id, string search);

        Task<ResearchArea> getResearchAreaById(Guid? Id);

        Task<bool> createResearchArea(ResearchArea researchArea);

        Task<bool> updateResearchArea(ResearchArea researchArea);

        Task<bool> deleteResearchArea(Guid? Id);

        Task<bool> checkResearchAreaExist(ResearchArea researchArea, Guid DepartmentID);
    }
}
