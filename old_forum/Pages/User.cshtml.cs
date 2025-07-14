using Microsoft.AspNetCore.Mvc.RazorPages;
using old_forum.Repos;

namespace old_forum.Pages;

public class User(BoardRepository bRepo, PostRepository pRepo, UserRepository uRepo) : PageModel
{
    public IEnumerable<Models.Board> Boards = [];
    public string Username = string.Empty;

    public async Task OnGetAsync(int id = -1)
    {
        Boards = await bRepo.GetAll();
        var user = uRepo.GetById(id);
    }
}