using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogBackend.Models;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

[Route("api/v1/articles")]
[ApiController]
public class ArticlesController : ControllerBase
{
    private readonly BlogDbContext _context;

    public ArticlesController(BlogDbContext context)
    {
        _context = context;
    }

    // Get all articles (Open to all)
    [HttpGet]
    public async Task<IActionResult> GetArticles()
    {
        var articles = await _context.Articles.ToListAsync();
        return Ok(articles);
    }

    // Get a single article by ID (Open to all)
    [HttpGet("{id}")]
    public async Task<IActionResult> GetArticle(int id)
    {
        var article = await _context.Articles.FindAsync(id);
        if (article == null) return NotFound();
        return Ok(article);
    }

    // Create a new article (Requires authentication)
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateArticle([FromBody] Article article)
    {
        var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
        article.AuthorId = userId;
        _context.Articles.Add(article);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetArticle), new { id = article.Id }, article);
    }

    // Update an article (Requires authentication & authorization)
    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateArticle(int id, [FromBody] Article updatedArticle)
    {
        var article = await _context.Articles.FindAsync(id);
        if (article == null) return NotFound();

        var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
        if (article.AuthorId != userId) return Forbid();

        article.Title = updatedArticle.Title;
        article.Content = updatedArticle.Content;
        await _context.SaveChangesAsync();

        return Ok("Article updated successfully");
    }

    // Delete an article (Requires authentication & authorization)
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteArticle(int id)
    {
        var article = await _context.Articles.FindAsync(id);
        if (article == null) return NotFound();

        var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
        if (article.AuthorId != userId) return Forbid();

        _context.Articles.Remove(article);
        await _context.SaveChangesAsync();

        return Ok("Article deleted successfully");
    }
}
