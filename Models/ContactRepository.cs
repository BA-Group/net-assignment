using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Microsoft.Data.Sqlite;
using Dapper;

namespace net_assignment.Models
{
    public class ContactRepository : IRepository<Contact, long>
    {
        private readonly string _connStr;
        public ContactRepository(IOptions<DBOptions> opts)
        {
            _connStr = opts.Value.ConnectionString;
        }
        public long Create(Contact contact)
        {
           return 0;
        }
        public Contact Read(long id)
        {
            using (var c = new SqliteConnection(_connStr))
            {
                c.Open();
                return c.QuerySingleOrDefault<Contact>("SELECT * FROM Contact WHERE Id=@Id", new { Id = id });
            }
        }
        public int Update(Contact contact)
        {
            return 0;
        }
        public int Delete(long id)
        {
            return 0;
        }

        public IEnumerable<Contact> FindAll()
        {
            return null;
        }
    }
}