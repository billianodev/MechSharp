namespace MechSharp.Abstraction;

public interface IRuntimeConfig
{
    public float KeypackVolume { get; set; }
    public float MousepackVolume { get; set; }
    public bool IsRandomEnabled { get; set; }
}
