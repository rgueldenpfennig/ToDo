using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo.Application.Models;
using Todo.Domain;
using Todo.Persistence;

namespace Todo.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/todo")]
    [ApiController]
    public class TodoRecordsController : ControllerBase
    {
        private readonly TodoDbContext _dbContext;

        public TodoRecordsController(TodoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Gets all ToDo records.
        /// </summary>
        /// <param name="cancellationToken">Token to support cancellation.</param>
        /// <response code="200">Returns all found ToDo records.</response>
        /// <response code="404">No record has been found.</response>
        [HttpGet]
        [ProducesResponseType(typeof(TodoRecord[]), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var todoRecords = await _dbContext.TodoRecords.ToListAsync(cancellationToken);
            if (!todoRecords.Any()) return NotFound();

            return Ok(todoRecords);
        }

        /// <summary>
        /// Gets a ToDo record by its identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="cancellationToken">Token to support cancellation.</param>
        /// <response code="200">Returns the associated ToDo record.</response>
        /// <response code="404">ToDo record not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TodoRecord), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var todoRecord = await _dbContext.TodoRecords.FirstOrDefaultAsync(tr => tr.Id == id, cancellationToken);
            if (todoRecord is null) return NotFound();

            return Ok(todoRecord);
        }

        /// <summary>
        /// Creates a new ToDo record.
        /// </summary>>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/todo
        ///     {
        ///        "title": "Finish me"
        ///     }
        ///
        /// </remarks>
        /// <param name="record">ToDo record that should be created.</param>
        /// <param name="cancellationToken">Token to support cancellation.</param>
        /// <response code="201">Returns the newly created ToDo record.</response>
        /// <response code="400">Invalid payload.</response>
        [HttpPost]
        [ProducesResponseType(typeof(TodoRecord), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] CreateTodoRecord record, CancellationToken cancellationToken)
        {
            var todoRecord = TodoRecordBuilder.New.WithTitle(record.Title).Build();
            _dbContext.Add(todoRecord);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = todoRecord.Id }, todoRecord);
        }
    }
}
