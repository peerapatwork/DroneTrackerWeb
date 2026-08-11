using System;
namespace DroneTrackerWeb.Models;

public class TriangulationResults
{
    public double DroneX { get; set; }            // พิกัด X (เมตร)
    public double DroneY { get; set; }            // พิกัด Y (เมตร)
    public double RayAngleTopDeg { get; set; }    // มุมลำแสงกล้องบน (องศา)
    public double RayAngleBottomDeg { get; set; } // มุมลำแสงกล้องล่าง (องศา)
    public double IntersectionAngleDeg { get; set; } // มุมตัดกัน (องศา)
    public double DistanceFromBaseline { get; set; } // ระยะห่างตรงจากกล้อง (เมตร)
    public double ErrorDistance { get; set; }     // ความคลาดเคลื่อน (เมตร)
    public string ZoneDescription { get; set; } = string.Empty; // ข้อความอธิบาย
}
