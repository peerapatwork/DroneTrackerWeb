using DroneTrackerWeb.Models;
using DroneTrackerWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace DroneTrackerWeb.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TrackerApiController : ControllerBase
{
    private readonly StereoSystemConfig _config;
    private readonly StereoTriangulatorService _triangulator;
    // Constructor: ให้ ASP.NET Core หยิบส่งช่างคำนวณและคลาสค่ากลางมาให้ใช้งาน
    public TrackerApiController(StereoTriangulatorService triangulator, StereoSystemConfig config)
    {
        _triangulator = triangulator;
        _config = config;
    }

    // Endpoint ที่ 1: คำนวณเดินหน้า (ลากเมาส์พิกัด X, Y ➔ ได้ค่า d1, d2)
    [HttpGet("simulate")]
    public IActionResult SimulateForward(double x, double y)
    {
        var offsets = _triangulator.CalculateImageOffsets(x, y);
        return Ok(new
        {
            TargetX = x,    
            TargetY = y,
            OffsetTop = offsets.d1,
            OffsetBottom = offsets.d2
        });
    }
    // Endpoint ที่ 2: คำนวณย้อนกลับ (ส่ง d1, d2 ➔ พิสูจน์หาพิกัด X, Y และมุมทั้งหมด)
    [HttpPost("triangulate")]
    public IActionResult TriangulateReverse([FromBody] ImageOffsetInput input)
    {
        var result = _triangulator.CalculateReverseTriangulation(input.OffsetTop, input.OffsetBottom);
        return Ok(result);
    }
}