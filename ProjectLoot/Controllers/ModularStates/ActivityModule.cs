using ANLG.Utilities.States;

namespace ProjectLoot.Controllers.ModularStates;

public class UpdateModule<T> : IUpdate
{
    private readonly Action<T> _activity;
    private readonly T _arg1;

    public UpdateModule(Action<T> activity, T arg1)
    {
        _activity = activity;
        _arg1     = arg1;
    }

    public void Update()
    {
        _activity(_arg1);
    }
}

public class UpdateModule<T1, T2> : IUpdate
{
    public required Action<T1, T2> Activity { get; init; }
    public required T1 Arg1 { get; init; }
    public required T2 Arg2 { get; init; }

    public void Update()
    {
        Activity(Arg1, Arg2);
    }
}
