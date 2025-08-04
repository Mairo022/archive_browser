using archive_browser.Repos;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace archive_browser.Pages.Shared;

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