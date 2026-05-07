using Ecommerce.Data;
using Ecommerce.DTOs.TaskDtos;
using Ecommerce.Model;
using Ecommerce.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TaskController : ControllerBase
{
    private readonly TaskManageDataContext _context;
    private readonly ILogservice _logservice;
    public TaskController(TaskManageDataContext context, ILogservice logservice)
    {
        _context = context;
        _logservice = logservice;
    }

    // ✅ Create Task
    [HttpPost("createtask")]
    public async Task<IActionResult> CreateTask(CreateTaskDto dto)
    {
        var userId = int.Parse(User.FindFirst("UserId").Value);

        var task = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            Status = "Pending",
            UserId = userId
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
        await _logservice.AddLog("Task Created", userId);
        return Ok("Task Created");
       
    }
    [HttpPost("StatusChange")]
    public async Task<IActionResult> StatusChange(StatusDto dto)
    {
        var userId = int.Parse(User.FindFirst("UserId").Value);
        var task =await _context.Tasks.FindAsync(dto.Id);
        if(task == null)
        {
            return NotFound("Task not found");
        }
        task.Status = dto.Status;
        
        
        await _context.SaveChangesAsync();
        await _logservice.AddLog("Task Status Updated", userId);
        return Ok("status change");
    }

    // ✅ Get My Tasks
    [HttpGet("my")]
    public async Task<IActionResult> GetMyTasks()
    {
        var userId = int.Parse(User.FindFirst("UserId").Value);

        var tasks = await _context.Tasks
            .Where(t => t.UserId == userId)
            .Select(t => new TaskResponseDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Status = t.Status
            })
            .ToListAsync();

        return Ok(tasks);
    }

    // ✅ Admin: Get All Tasks
    [HttpGet("all")]
    [Authorize(Roles = "5")]
    public async Task<IActionResult> GetAllTasks()
    {
        var tasks = await _context.Tasks
            .Select(t => new TaskResponseDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Status = t.Status
            })
            .ToListAsync();

        return Ok(tasks);
    }
}
