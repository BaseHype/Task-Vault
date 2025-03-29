using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController(AppDbContext context) : ControllerBase
    {
        // GET All tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Domain.Task>>> GetTasks()
        {
            return await context.Tasks.ToListAsync();
        }

        // GET Task by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Domain.Task>> GetTask(int id)
        {
            var task = await context.Tasks.FindAsync(id);
            return task == null ? NotFound() : task;
        }

        // POST Task
        [HttpPost]
        public async Task<ActionResult<Domain.Task>> CreateTask(Domain.Task task)
        {
            context.Tasks.Add(task);
            await context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }

        // PUT Task
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, Domain.Task task)
        {
            if (id != task.Id)
            {
                return BadRequest();
            }
            context.Entry(task).State = EntityState.Modified;
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TaskExists(id))
            {
                return NotFound();
            }
            return NoContent();
        }

        // DELETE Task
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            context.Tasks.Remove(task);
            await context.SaveChangesAsync();
            return NoContent();
        }

        private bool TaskExists(int id)
        {
            return context.Tasks.Any(e => e.Id == id);
        }
    }
}
