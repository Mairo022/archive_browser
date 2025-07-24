using Microsoft.AspNetCore.Mvc.RazorPages;
using old_forum.Models;
using old_forum.Repos;
using static old_forum.Utils.StringUtils;

namespace old_forum.Pages;

public class Board : PageModel
{
    readonly TopicRepository _tRepo;
    readonly BoardRepository _bRepo;
    public IEnumerable<TopicWithUserTotal> Topics { get; set; } = [];
    public IEnumerable<Models.Board> Boards = [];

    const int PageSize = 30;
    public int CurPage { get; set; }
    public int BoardId { get; set; }
    public string? BoardName { get; set; }

    public string? Author;
    public string? Poster;
    public string? Topic;
    public string? Start;
    public Order Order;

    public List<int> Paging { get; set; } = [];
    
    public Board(ILogger<IndexModel> logger, TopicRepository tRepo, BoardRepository bRepo) 
    {
        _tRepo = tRepo;
        _bRepo = bRepo;
    }
    
    public async Task OnGetAsync(int id, int pg, string? start, string? author, string? poster, string? topic, Order order)
    {
        if (pg == 0) pg = 1;
        
        BoardId = id;
        CurPage = pg;
        
        Start = start;
        Author = author;
        Poster = poster;
        Topic = topic;
        Order = order;
        
        var offset = (pg-1) * PageSize;

        Topics = string.IsNullOrEmpty(poster)
            ? await _tRepo.Get(BoardId, offset, PageSize, start, author, topic, order)
            : await _tRepo.GetByPoster(BoardId, offset, PageSize, start, author, topic, order, poster);
        
        Boards = await _bRepo.GetAll();
        BoardName = FirstCharToUpper(Boards
            .Where(board => board.Id == BoardId)
            .Select(board => board.Name)
            .FirstOrDefault() ?? "Board");
        
        Paging = CreatePaging(pg, PageSize, Topics.FirstOrDefault()?.totalRows ?? 0);
    }

    static List<int> CreatePaging(int page, int pageSize, int total)
    {
        const int pagesToSide = 3;
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

public enum Order
{
    DESC,
    ASC
}