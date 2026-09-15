namespace DroneTrackerWeb.Models;

public class StereoSystemConfig
{
    public double Baseline { get; set; } = 20.0; //เส้นระยะห่างระหว่างกล้อง 2 ตัว
    public double ConvergenceDistance { get; set; } = 100.0; //ระยะที่จุดกึ่งกลางกล้องทั้ง 2 ตัวตัดกัน
    public double FocalLength { get; set; } = 100.4988; //ระยะโฟกัสภาพ

    public double Camera1Height { get; set; } = 0.0; // ความสูงกล้อง 1 (m)
    public double Camera2Height { get; set; } = 0.0; // ความสูงกล้อง 2 (m)
    public double Focus1Height { get; set; } = 0.0;  // ความสูงจุดโฟกัสกล้อง 1 (m)
    public double Focus2Height { get; set; } = 0.0;  // ความสูงจุดโฟกัสกล้อง 2 (m)
}