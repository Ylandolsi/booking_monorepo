using Booking.Common;
using Booking.Common.Results;

namespace Booking.Modules.Users.Domain.ValueObjects;

public class Status : ValueObject
{
    private Status()
    {
    }

    public Status(bool isMentor)
    {
        IsMentor = isMentor;
        IsActive = isMentor;
    }

    public bool IsMentor { get; private set; }
    public bool IsActive { get; private set; }

    public static Status CreateMentee()
    {
        return new Status(false);
    }

    public static Status CreateMentor()
    {
        return new Status(true);
    }

    public Result BecomeMentor()
    {
        if (IsMentor)
            return Result.Failure(StatusErrors.AlreadyMentor);

        IsMentor = true;
        IsActive = true;
        return Result.Success();
    }

    public Result ToggleActivation()
    {
        if (!IsMentor)
            return Result.Failure(StatusErrors.OnlyMentorsCanToggleActivation);

        IsActive = !IsActive;
        return Result.Success();
    }

    public Result Activate()
    {
        if (!IsMentor)
            return Result.Failure(StatusErrors.OnlyMentorsCanToggleActivation);

        if (IsActive)
            return Result.Failure(StatusErrors.AlreadyActive);

        IsActive = true;
        return Result.Success();
    }

    public Result Deactivate()
    {
        if (!IsMentor)
            return Result.Failure(StatusErrors.OnlyMentorsCanToggleActivation);

        if (!IsActive)
            return Result.Failure(StatusErrors.AlreadyInactive);

        IsActive = false;
        return Result.Success();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return IsMentor;
        yield return IsActive;
    }

    public override string ToString()
    {
        return $"Mentor: {IsMentor}, Active: {IsActive}";
    }
}

public static class StatusErrors
{
    public static readonly Error AlreadyMentor = Error.Problem(
        "Status.AlreadyMentor",
        "User is already a mentor");

    public static readonly Error OnlyMentorsCanToggleActivation = Error.Problem(
        "Status.OnlyMentorsCanToggleActivation",
        "Only mentors can be activated or deactivated");

    public static readonly Error AlreadyActive = Error.Problem(
        "Status.AlreadyActive",
        "User is already active");

    public static readonly Error AlreadyInactive = Error.Problem(
        "Status.AlreadyInactive",
        "User is already inactive");
}