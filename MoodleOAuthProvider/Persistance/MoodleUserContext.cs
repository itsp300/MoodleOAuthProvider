using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MoodleOAuthProvider.Persistance
{
    public class MoodleUserContext : DbContext
    {
        public MoodleUserContext(DbContextOptions<MoodleUserContext> options) : base(options)
        {

        }

        public DbSet<MoodleUser> Users { get; set; }

        public override int SaveChanges()
        {
            //prevent commiting to db
            throw new InvalidOperationException("This context is read-only.");
        }

        internal bool ValidateCredentials(string username, string password)
        {
            return Users.Where(u => u.Username == username && u.VerifyPassword(password)).Count() != 0;
        }

        internal Task<MoodleUser> FindBySubjectAsync(ClaimsPrincipal subject)
        {
            return Users.FirstOrDefaultAsync(u => u.Username == subject.Claims.First(x=>x.Type == "sub").Value);
        }

        internal MoodleUser FindBySubject(ClaimsPrincipal subject)
        {
            return Users.FirstOrDefault(u => u.Username == (subject.Claims.First(x => x.Type == "sub").Value));
        }

        internal MoodleUser FindByUsername(string username)
        {
            return Users.FirstOrDefault(u => u.Username == username);
        }
    }
}
