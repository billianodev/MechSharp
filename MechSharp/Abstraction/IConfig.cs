namespace MechSharp.Abstraction;

public interface IConfig
{
    public float KeypackVolume { get; set; }
    public float MousepackVolume { get; set; }
    public bool IsKeypackEnabled { get; set; }
    public bool IsKeyUpEnabled { get; set; }
    public bool IsRandomEnabled { get; set; }
    public bool IsMousepackEnabled { get; set; }
}
