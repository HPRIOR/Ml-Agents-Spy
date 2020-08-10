using System.Collections;
using Interfaces;
using UnityEngine;

namespace Agents
{
    public class PatrolGuardTile : IPatrolGuardTile
    {
        private readonly GameObject _surrogateGameObject;
        

        public PatrolGuardTile(GameObject surrogateGameObject, Vector3 tilePosition, (int x, int y) coords)
        {
            _surrogateGameObject = surrogateGameObject;
            Position = tilePosition;
            Coords = coords;
        }

        public bool RecentlyVisitedByGuard { get; set; }

        public Vector3 Position { get; }
        public (int x, int y) Coords { get; }
        
        public static bool operator ==(PatrolGuardTile p1, PatrolGuardTile p2)
            => p2 != null
               && p1 != null
               && p1.Coords.x == p2.Coords.x
               && p1.Coords.y == p2.Coords.y
               && p1.Position == p2.Position;

        public static bool operator !=(PatrolGuardTile p1, PatrolGuardTile p2) => !(p1 == p2);

        private bool Equals(PatrolGuardTile other)
        {
            return Equals(_surrogateGameObject, other._surrogateGameObject) &&
                   RecentlyVisitedByGuard == other.RecentlyVisitedByGuard && Position.Equals(other.Position) &&
                   Coords.Equals(other.Coords);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((PatrolGuardTile) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _surrogateGameObject != null ? _surrogateGameObject.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ RecentlyVisitedByGuard.GetHashCode();
                hashCode = (hashCode * 397) ^ Position.GetHashCode();
                hashCode = (hashCode * 397) ^ Coords.GetHashCode();
                return hashCode;
            }
        }
    }
}