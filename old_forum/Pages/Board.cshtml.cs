using Microsoft.AspNetCore.Mvc.RazorPages;
using old_forum.Models;
using old_forum.Repos;

namespace old_forum.Pages;

public class Board : PageModel
{
    readonly TopicRepository _tRepo;
    readonly BoardRepository _bRepo;
    public IEnumerable<TopicWithUserTotal> Topics { get; private set; } = [];
    public IEnumerable<Models.Board> Boards = [];

    const int PageSize = 60;
    public int CurPage { get; set; }
    public int BoardId { get; set; }

    public List<int> Paging { get; set; } = [];
    
    public Board(ILogger<IndexModel> logger, TopicRepository tRepo, BoardRepository bRepo) 
    {
        _tRepo = tRepo;
        _bRepo = bRepo;
    }
    
    public async Task OnGetAsync(int id, int pg = 1)
    {
        BoardId = id;
        CurPage = pg;
        
        var offset = (pg-1) * PageSize;
        
        Topics = await _tRepo.Get(BoardId, offset, PageSize);
        Boards = await _bRepo.GetAll();
        
        Paging = CreatePaging(pg, PageSize, Topics.FirstOrDefault()?.totalRows ?? 0);
    }

    static List<int> CreatePaging(int page, int pageSize, int total)
    {
        const int pagesToSide = 2;
        var paging = new List<int>(pagesToSide * 2 + 1 + 2);
        
        if (total == 0) return paging;
        
        var totalPages = (int)Math.Ceiling((double)total / pageSize);
        paging.Add(1);
        
        if (totalPages == 1) return paging;
        
        var start = page - pagesToSide > 1 ? page - pagesToSide : 2;
        var end = page + pagesToSide >= totalPages ? totalPages : page + pagesToSide;
        
        for (var i = start; i <= end; i++) paging.Add(i);
        
        if (totalPages == end) return paging;
        
        paging.Add(totalPages);
        return paging;
    }
}