using MappingLectureCourse.Models.ContentViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MappingLectureCourse.Interface
{
    public interface ISessions
    {
        Task<List<Session>> getAllSession();

        Task<Session> getSessionById(Guid? Id);

        Task<bool> createSession(Session session); 

        Task<bool> updateSession(Session session);
         
        Task<bool> deleteSession(Guid? Id);

        Task<bool> checkSessionExist(Session session);
    }
}
