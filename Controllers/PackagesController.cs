using DevTracker.API.Models;
using DevTracker.API.Entities;
using Microsoft.AspNetCore.Mvc;
using DevTracker.API.Persistence.Repository;

namespace DevTracker.API.Controllers
{
    [ApiController]
    [Route("api/packages")]
    public class PackagesController : ControllerBase
    {
        private readonly IPackageRepository _repository;
        public PackagesController(IPackageRepository repository)
        {
            _repository = repository;

        }

        /// <summary>
        /// Return one object(package)
        /// GET api/packages/XXXX-XXXX-XXXX-XXXX
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
        /// Return all objects(packages)
        /// GET api/packages
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
        /// <param name="package"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post(AddPackageInputModel model)
        {
            if (model.Title?.Length < 10)
            {
                return BadRequest("Title length must be at least 10 character.");
            }

            var package = new Package(model.Title, model.Weight);
            _repository.Add(package);

            return CreatedAtAction("GetByCode", new { code = package.Code }, package);
        }

        /// <summary>
        /// PUT api/packages/XXXX-XXXX-XXXX-XXXX/updates
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