using EventOrganizerAPI.Entities;
using EventOrganizerAPI.Persistance;

namespace EventOrganizerAPI.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly EventOrganizerDbContext _dbContext;

        public DbInitializer(EventOrganizerDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Initialize()
        {
            if(_dbContext.Database.CanConnect())
            {
                if(!_dbContext.Users.Any())
                {
                    var user = GetUser();
                    _dbContext.Users.Add(user);
                    _dbContext.SaveChanges();
                }
            }
        }

        private User GetUser()
        {
            var user = new User()
            {
                City = "Tychy",
                PasswordHashed = "123456789",
                DateOfBirth = new DateTime(2001, 12, 7),
                Email = "test@wp.pl",
                FirstName = "TestFN",
                LastName = "TestLN",

            };

            return user;
        }
    }
}
