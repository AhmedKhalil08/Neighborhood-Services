using Microsoft.AspNetCore.Mvc;
using Neighborhood.Services.Application.Reviews.DTOs;
using Neighborhood.Services.Application.Reviews.Interfaces;

namespace Neighborhood.Services.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewsController : ControllerBase
{
    private readonly IReviewService _reviewService;

    public ReviewsController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }


    // Create Review
    [HttpPost]
    public async Task<IActionResult> Create(CreateReviewDto dto)
    {
        await _reviewService.CreateReviewAsync(dto);

        return Ok("Review created successfully");
    }


    // Get All Reviews
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var reviews = await _reviewService.GetAllAsync();

        return Ok(reviews);
    }


    // Get Review By Id
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var review = await _reviewService.GetByIdAsync(id);

        if (review is null)
            return NotFound("Review not found");

        return Ok(review);
    }


    // Delete Review (Soft Delete)
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _reviewService.DeleteAsync(id);

        return Ok("Review deleted successfully");
    }
}