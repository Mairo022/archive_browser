using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using old_forum.Db;
using old_forum.Models;
using old_forum.Repos;

namespace old_forum.Pages;

public class IndexModel : PageModel
{
    readonly ILogger<IndexModel> _logger;
    readonly BoardRepository _repo;
    public IEnumerable<Models.Board> Boards = [];

    public static string Message { get; set; } = "CARS ARE FAST";
    
    public IndexModel(ILogger<IndexModel> logger, BoardRepository repo) => _repo = repo;

    public async Task OnGetAsync()
    {
        Boards = await _repo.GetAll();
    }
}