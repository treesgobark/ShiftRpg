namespace ProjectLoot.Components.Interfaces;

public interface IHitstopComponent
{
    bool IsStopped { get; set; }
    TimeSpan RemainingHitstopTime { get; set; }

    void Stop();
    void Resume();
}