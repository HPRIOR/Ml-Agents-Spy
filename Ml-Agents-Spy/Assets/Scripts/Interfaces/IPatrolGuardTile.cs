namespace Interfaces
{
    public interface IPatrolGuardTile : ITile
    {
        bool RecentlyVisitedByGuard { get; set; }
    }
}