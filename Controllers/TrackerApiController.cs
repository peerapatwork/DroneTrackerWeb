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

    // Endpoint คำนวณเดินหน้า (ลากเมาส์พิกัด X, Y ➔ ได้ค่า d1, d2)
    [HttpGet("simulate")]
    public IActionResult SimulateForward(double x, double y, double z)
    {
        var offsets = _triangulator.CalculateImageOffsets3D(x, y, z);
        return Ok(new
        {
            TargetX = x,
            TargetY = y,
            TargetZ = z,
            OffsetTop = offsets.d1y,
            OffsetBottom = offsets.d2y,
            OffsetZ1 = offsets.dz1,
            OffsetZ2 = offsets.dz2
        });
    }

    // Endpoint คำนวณย้อนกลับ 3D (ส่ง d1y, d2y, dz -> ได้พิกัด 3D และ Distance3D)
    [HttpPost("triangulate3d")]
    public IActionResult TriangulateReverse3D([FromBody] ImageOffsetInput input)
    {
        var result = _triangulator.CalculateReverseTriangulation3D(input.OffsetTop, input.OffsetBottom, input.OffsetZ);
        return Ok(result);
    }
}