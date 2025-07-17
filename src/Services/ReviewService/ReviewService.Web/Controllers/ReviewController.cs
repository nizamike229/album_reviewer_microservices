using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReviewService.Application.Services;
using ReviewService.Domain.Models;
using ReviewService.Mapping;
using ReviewService.Models;

namespace ReviewService.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ReviewController : ControllerBase
{
    private readonly IReviewService _reviewService;

    public ReviewController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    [Authorize]
    [HttpPost]
    [ActionName("publish")]
    public async Task<ActionResult> PublishReview([FromBody] ReviewRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var result = await _reviewService.Post(request.ToReview(userId!));
        return Ok(result);
    }

    [Authorize]
    [HttpGet]
    [ActionName("getAllByMbId")]
    public async Task<ActionResult<List<Review>>> GetAllByMbId([FromQuery]string mbId)
    {
        var result = await _reviewService.GetAll(mbId);
        return Ok(result);
    }
}