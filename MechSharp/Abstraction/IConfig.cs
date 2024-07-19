namespace MechSharp.Abstraction;

public interface IConfig : IRuntimeConfig
{
    public bool IsMuted { get; set; }
    public bool IsKeypackEnabled { get; set; }
    public bool IsKeyUpEnabled { get; set; }
    public bool IsMousepackEnabled { get; set; }
}
