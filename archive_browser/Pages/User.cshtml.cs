using archive_browser.Models;
using archive_browser.Repos;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace archive_browser.Pages;

public class User(BoardRepository bRepo, PostRepository pRepo, UserRepository uRepo) : PageModel
{
    public IEnumerable<Models.Board> Boards = [];
    public UserInfo? UserInfo { get; set; }

    public async Task OnGetAsync(int id = -1)
    {
        Boards = await bRepo.GetAll();
        UserInfo = await uRepo.GetUserInfo(id);
    }
}