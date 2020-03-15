using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MappingLectureCourse.Data;
using MappingLectureCourse.Interface;
using MappingLectureCourse.Models.ContentViewModel;
using Microsoft.EntityFrameworkCore;


namespace MappingLectureCourse.Services
{
    public class SessionService : ISessions
    {
        private readonly ApplicationDbContext _context;

        public SessionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> createSession(Session session)
        {
            await _context.sessions.AddAsync(session);

            var created = await _context.SaveChangesAsync();

            return created > 0;
        }

        public async Task<bool> deleteSession(Guid? Id)
        {
            var session = await getSessionById(Id);

            if (session == null)
                return false;

            _context.sessions.Remove(session);

            var deleted = await _context.SaveChangesAsync();

            return deleted > 0;
        }

        public async Task<List<Session>> getAllSession()
        { 
            return await _context.sessions.OrderByDescending(s => s.SessionName).ToListAsync();
        }

        public async Task<Session> getSessionById(Guid? Id)
        {
            return await _context.sessions
                        .SingleOrDefaultAsync(x => x.SessionID == Id);
        }

        public async Task<bool> updateSession(Session session)
        {
            _context.sessions.Update(session);

            var updated = await _context.SaveChangesAsync();

            return updated > 0;
        }

        public async Task<bool> checkSessionExist(Session session)
        {
            return await _context.sessions
                        .AnyAsync(s => s.SessionName.Equals(session.SessionName));
        }

    }
}