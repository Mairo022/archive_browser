using Microsoft.AspNetCore.Mvc.RazorPages;
using old_forum.Repos;

namespace old_forum.Pages.Shared;

public class LayoutModel : PageModel
{
    readonly BoardRepository _repo;
    public IEnumerable<Models.Board> Boards = [];
    
    public LayoutModel(ILogger<IndexModel> logger, BoardRepository repo) => _repo = repo;

    public async Task OnGetAsync()
    {
        Boards = await _repo.GetAll();
    }
}