using ANLG.Utilities.States;

namespace ProjectLoot.Controllers.ModularStates;

public class ActivityModule<T> : IActivity
{
    private readonly Action<T> _activity;
    private readonly T _arg1;

    public ActivityModule(Action<T> activity, T arg1)
    {
        _activity = activity;
        _arg1     = arg1;
    }

    public void CustomActivity()
    {
        _activity(_arg1);
    }
}

public class ActivityModule<T1, T2> : IActivity
{
    public required Action<T1, T2> Activity { get; init; }
    public required T1 Arg1 { get; init; }
    public required T2 Arg2 { get; init; }

    public void CustomActivity()
    {
        Activity(Arg1, Arg2);
    }
}
