using Microsoft.AspNetCore.Mvc;

using net_assignment.Models;

namespace net_assignment.Contollers
{
    [Route("contacts/")]
    public class ContactController : Controller
    {

        private readonly IRepository<Contact, long> _contactRepository;

        public ContactController(IRepository<Contact, long> contactRepository)
        {
            _contactRepository = contactRepository;
        }

        [HttpPost]
        public IActionResult Create([FromBody]Contact contact)
        {
            return new StatusCodeResult(501);
        }
        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody]Contact contact)
        {
            return new StatusCodeResult(501);
        }

        [HttpGet]
        public IActionResult List()
        {
            return new StatusCodeResult(501);
        }
        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var c = _contactRepository.Read(id);
            if(c == null){
                return NotFound();
            }else{
                return Ok(c);
            }
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            return new StatusCodeResult(501);
        }
    }
}