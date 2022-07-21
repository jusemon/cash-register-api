using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CashRegister.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class TaskController : ControllerBase
  {
    private readonly Models.CashRegisterContext _context;

    public TaskController(Models.CashRegisterContext context)
    {
      _context = context;
    }

    // GET: api/Task
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Models.Task>>> GetTasks()
    {
      if (_context.Tasks == null)
      {
        return NotFound();
      }
      return await _context.Tasks.ToListAsync();
    }

    // GET: api/Task/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Models.Task>> GetTask(long id)
    {
      if (_context.Tasks == null)
      {
        return NotFound();
      }
      var task = await _context.Tasks.FindAsync(id);

      if (task == null)
      {
        return NotFound();
      }

      return task;
    }

    // PUT: api/Task/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTask(long id, Models.Task task)
    {
      if (id != task.TaskId)
      {
        return BadRequest();
      }

      _context.Entry(task).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!TaskExists(id))
        {
          return NotFound();
        }
        else
        {
          throw;
        }
      }

      return NoContent();
    }

    // POST: api/Task
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Models.Task>> PostTask(Models.Task task)
    {
      if (_context.Tasks == null)
      {
        return Problem("Entity set 'CashRegisterContext.Tasks'  is null.");
      }
      _context.Tasks.Add(task);
      await _context.SaveChangesAsync();

      return CreatedAtAction(nameof(GetTask), new { id = task.TaskId }, task);
    }

    // DELETE: api/Task/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(long id)
    {
      if (_context.Tasks == null)
      {
        return NotFound();
      }
      var task = await _context.Tasks.FindAsync(id);
      if (task == null)
      {
        return NotFound();
      }

      _context.Tasks.Remove(task);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    private bool TaskExists(long id)
    {
      return (_context.Tasks?.Any(e => e.TaskId == id)).GetValueOrDefault();
    }
  }
}
