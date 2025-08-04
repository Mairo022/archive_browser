using archive_browser.Models;
using archive_browser.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace archive_browser.Pages;

public class IndexModel(BoardRepository repo, SavedPostsRepository spRepo) : PageModel
{
    public IEnumerable<Models.Board> Boards = [];

    public IEnumerable<SavedPostWithDetails> SavedPosts { get; set; } = [];

    [BindProperty]
    public int SavedPostId { get; set; }
    
    public async Task OnGetAsync()
    {
        Boards = await repo.GetAll();
        SavedPosts = await spRepo.GetAllWithDetails();
    }
    
    public async Task<IActionResult> OnPostDeletePost()
    {
        return await spRepo.DeleteOne(SavedPostId) ? StatusCode(201) : StatusCode(400);
    }
}