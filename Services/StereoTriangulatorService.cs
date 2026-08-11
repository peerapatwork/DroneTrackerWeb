using DroneTrackerWeb.Models;

namespace DroneTrackerWeb.Services;

public class StereoTriangulatorService
{
    private readonly StereoSystemConfig _config;
    public StereoTriangulatorService(StereoSystemConfig config)
    {
        _config = config;
    }

    // ฟังก์ชันที่ 1: คำนวณหาค่า d1, d2 เมื่อผู้ใช้ลากจุดโดรน (X, Y) ในภาพที่ 1
    public (double d1, double d2) CalculateImageOffsets(double targetX, double targetY)
    {
        // 1. ตำแหน่งกล้องบน (0, +10) และกล้องล่าง (0, -10)
        double c1Y = _config.Baseline / 2.0;
        double c2Y = -_config.Baseline / 2.0;
        // 2. มุมจริงจากกล้องไปยังเป้าหมาย (True Target Angles)
        double angle1 = Math.Atan2(targetY - c1Y, targetX);
        double angle2 = Math.Atan2(targetY - c2Y, targetX);
        // 3. มุมหรี่ตาเข้าหากันของกล้อง (Convergence Angles)
        double conv1 = Math.Atan2(-c1Y, _config.ConvergenceDistance);
        double conv2 = Math.Atan2(-c2Y, _config.ConvergenceDistance);
        // 4. มุมเบี่ยงเบนบนระนาบภาพ
        double phi1 = angle1 - conv1;
        double phi2 = angle2 - conv2;
        // 5. แปลงเป็นระยะมิลลิเมตร/พิกเซลบนภาพ (d1, d2)
        double d1 = Math.Tan(phi1) * _config.FocalLength;
        double d2 = Math.Tan(phi2) * _config.FocalLength;
        return (Math.Round(d1, 2), Math.Round(d2, 2));
    }
    // ฟังก์ชันที่ 2: พิสูจน์สูตรคำนวณย้อนกลับหาพิกัด (X, Y) เพื่อนำไปวาดในภาพที่ 2
    public TriangulationResults CalculateReverseTriangulation(double d1, double d2)
    {
        double theta1Conv = Math.Atan2(-_config.Baseline / 2.0, _config.ConvergenceDistance);
        double theta2Conv = Math.Atan2(+_config.Baseline / 2.0, _config.ConvergenceDistance);
        double phi1 = Math.Atan2(d1, _config.FocalLength);
        double phi2 = Math.Atan2(d2, _config.FocalLength);
        double beta1 = theta1Conv + phi1;
        double beta2 = theta2Conv + phi2;
        double sinDiff = Math.Sin(beta2 - beta1);
        double t1 = (_config.Baseline * Math.Cos(beta2)) / sinDiff;
        double calcX = t1 * Math.Cos(beta1);
        double calcY = (_config.Baseline / 2.0) + (t1 * Math.Sin(beta1));
        return new TriangulationResults
        {
            DroneX = Math.Round(calcX, 2),
            DroneY = Math.Round(calcY, 2),
            RayAngleTopDeg = Math.Round(beta1 * 180.0 / Math.PI, 2),
            RayAngleBottomDeg = Math.Round(beta2 * 180.0 / Math.PI, 2),
            IntersectionAngleDeg = Math.Round(sinDiff * 180.0 / Math.PI, 2),
            DistanceFromBaseline = Math.Round(Math.Sqrt(calcX * calcX + calcY * calcY), 2)
        };
    }
}
