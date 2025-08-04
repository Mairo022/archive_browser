using archive_browser.Models;
using archive_browser.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace archive_browser.Pages;

public class Topic(BoardRepository bRepo, PostRepository pRepo, SavedPostsRepository spRepo) : PageModel
{
    public int BoardId { get; set; }
    public int TopicId { get; set; }
    
    public IEnumerable<Models.Board> Boards = [];
    public IEnumerable<Post> Posts = [];
    public IEnumerable<SavedPost> SavedPosts = [];
    
    [BindProperty]
    public int PostId { get; set; }

    public async Task OnGetAsync(int id, int boardId)
    {
        BoardId = boardId;
        TopicId = id;
        Boards = await bRepo.GetAll();
        Posts = await pRepo.GetByTopicId(id);
        SavedPosts = await spRepo.GetAll();
    }

    public async Task<IActionResult> OnPostSavePost()
    {
        return await spRepo.InsertOne(PostId) ? StatusCode(201) : StatusCode(400);
    }
}