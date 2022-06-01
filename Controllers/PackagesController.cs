using DevTracker.API.Models;
using DevTracker.API.Entities;
using Microsoft.AspNetCore.Mvc;
using DevTracker.API.Persistence.Repository;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace DevTracker.API.Controllers
{
    [ApiController]
    [Route("api/packages")]
    public class PackagesController : ControllerBase
    {
        private readonly IPackageRepository _repository;
        private readonly ISendGridClient _client;

        public PackagesController(IPackageRepository repository, ISendGridClient client)
        {
            _client = client;
            _repository = repository;
        }

        /// <summary>
        /// GET api/packages/XXXX-XXXX-XXXX-XXXX
        /// Return one object(package)
        /// </summary>
        /// <returns></returns>
        [HttpGet("{code}")]
        public IActionResult GetByCode(string code)
        {
            var package = _repository.GetByCode(code);

            if (code == null)
            {
                return NotFound();
            }

            return Ok(package);
        }

        /// <summary>
        /// GET api/packages
        /// Return all objects(packages)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            var packages = _repository.GetAll();
            return Ok(packages);
        }

        /// <summary>
        /// POST api/packages
        /// Cadaster new object(package)
        /// </summary>
        /// <remarks>
        ///  {
        ///   "title": "Novo Pacote de Software 3",
        ///   "weight": 10.5,
        ///   "senderName": "Pedro 3",
        ///   "senderEmail": "pedroeternalss@gmail.com"
        ///  } 
        /// </remarks>
        /// <param name="model">Data of package</param>
        /// <returns>New object</returns>
        /// <response code="201">Cadaster was with successe</response>
        /// <response code="400">Data incorrects</response>
        [HttpPost]
        public async Task<IActionResult> Post(AddPackageInputModel model)
        {
            if (model.Title?.Length < 10)
            {
                return BadRequest("Title length must be at least 10 character.");
            }

            var package = new Package(model.Title, model.Weight);
            _repository.Add(package);

            var message = new SendGridMessage
            {
                From = new EmailAddress("pedroeternalss@gmail.com", "pedroeternalss@gmail.com"),
                Subject = "Your Package was dispatched.",
                PlainTextContent = $"Your package with code {package.Code} was dispatched."
            };
            message.AddTo(model.SenderEmail, model.SenderName);

            await _client.SendEmailAsync(message);

            return CreatedAtAction("GetByCode", new { code = package.Code }, package);
        }

        /// <summary>
        /// POST api/packages/XXXX-XXXX-XXXX-XXXX/updates
        /// Update a package
        /// </summary>
        /// <returns></returns>
        [HttpPost("{code}/updates")]
        public IActionResult PostUpdate(string code, AddPackageUpdateInputModel model)
        {
            var package = _repository.GetByCode(code);

            if (package == null)
            {
                return NotFound();
            }
            package.AddUpdate(model.Status, model.Delivered);
            _repository.Update(package);

            return NoContent();
        }
    }
}