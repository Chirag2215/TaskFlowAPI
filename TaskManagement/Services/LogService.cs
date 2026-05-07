using System;
using Ecommerce.Data;
using Ecommerce.Services;
public class LogService : ILogservice
{
    private readonly TaskManageDataContext _context;

    public LogService (TaskManageDataContext context)
    {
        _context = context;
    }

    public async Task AddLog(string action, int userId)
    {
        var log = new Log
        {
            Action = action,
            UserId = userId,
            TimeStamp = DateTime.Now
        };

        _context.Logs.Add(log);
        await _context.SaveChangesAsync();
    }
}
