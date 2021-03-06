using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AnomalyDetection.Data.Repository;
using System.Threading.Tasks;
using System.Linq;
using AnomalyDetection.Data.Model.Queue;
using AnomalyDetection.Data.Model.Api;
using System;

namespace AnomalyDetection.Manager.Controllers
{
    public class CrudController<T> : ControllerBase where T : ApiCrudModel
    {
        protected readonly ILogger<CrudController<T>> _logger;
        protected readonly ICrudRepository<T> _repository;

        public CrudController(ILogger<CrudController<T>> logger, ICrudRepository<T> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("GET: GetAll()");

            var result = await _repository.GetAllAsync().ConfigureAwait(false);

            if (result?.Any() != true) // Null or empty
            {
                return NoContent();
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(T item)
        {
            _logger.LogInformation($"POST: Add({item})");

            if (item is null)
            {
                return BadRequest();
            }

            var result = await _repository.AddAsync(item).ConfigureAwait(false);

            // return Created(new Uri($"{Request?.Scheme}://{Request?.Host}{Request?.Path}/{item.Id}"), result);

            await ProcessCrudEvent(new CrudEvent<T>() { Action = CrudAction.Create, Item = result}).ConfigureAwait(false);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation($"GET: GetById({id})");

            var result = await _repository.GetByIdAsync(id).ConfigureAwait(false);

            return result is not null ? Ok(result) : NotFound();
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> EditById(int id, T item)
        {
            _logger.LogInformation($"PATCH: EditById({id})");

            if (item is null)
            {
                return BadRequest();
            }

            var result = await _repository.EditAsync(id, item).ConfigureAwait(false);

            await ProcessCrudEvent(new CrudEvent<T>() { Action = CrudAction.Edit, Item = result}).ConfigureAwait(false);

            return result is not null ? Ok(result) : NotFound();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            _logger.LogInformation($"DELETE: DeleteById({id})");

            var result = await _repository.DeleteByIdAsync(id).ConfigureAwait(false);

            await ProcessCrudEvent(new CrudEvent<T>() { Action = CrudAction.Delete, Item = result}).ConfigureAwait(false);

            return result is not null ? Ok() : NotFound();
        }

        protected virtual Task ProcessCrudEvent(CrudEvent<T> crudEvent)
        {
            return Task.CompletedTask;
        }

        public Task EditById<T>(int? id, T item) where T : ApiCrudModel
        {
            throw new NotImplementedException();
        }
    }
}