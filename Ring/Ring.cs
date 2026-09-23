using System.Collections.Generic;

namespace GameEngine
{
    public class Ring
    {
        private readonly Dictionary<RingPosition, RingObject> _ringObjects = new Dictionary<RingPosition, RingObject>();

        public bool TryAddObject(
            RingObject ringObject,
            RingPosition position)
        {
            Guard.NotNull(ringObject, nameof(ringObject));

            if (_ringObjects.ContainsKey(position))
                return false;

            _ringObjects.Add(position, ringObject);

            return true;
        }

        public bool TryAddObject(RingObject ringObject)
        {
            Guard.NotNull(ringObject, nameof(ringObject));

            if (!TryGetFreePosition(out RingPosition position))
                return false;

            return TryAddObject(ringObject, position);
        }

        public bool TryGetObject(
            RingPosition position,
            out RingObject ringObject)
        {
            return _ringObjects.TryGetValue(
                position,
                out ringObject
            );
        }

        public bool TryRotate(RotationDirection direction)
        {
            if (_ringObjects.Count == 0)
                return false;

            Dictionary<RingPosition, RingObject> rotatedObjects = new Dictionary<RingPosition, RingObject>();

            foreach (KeyValuePair<RingPosition, RingObject> entry in _ringObjects)
            {
                RingPosition newPosition =
                    GetRotatedPosition(entry.Key, direction);

                rotatedObjects[newPosition] = entry.Value;
            }

            _ringObjects.Clear();

            foreach (KeyValuePair<RingPosition, RingObject> entry in rotatedObjects)
            {
                _ringObjects.Add(entry.Key, entry.Value);
            }

            return true;
        }

        private bool TryGetFreePosition(out RingPosition position)
        {
            RingPosition[] positions =
            {
                RingPosition.Top,
                RingPosition.Right,
                RingPosition.Bottom,
                RingPosition.Left
            };

            foreach (RingPosition candidate in positions)
            {
                if (!_ringObjects.ContainsKey(candidate))
                {
                    position = candidate;
                    return true;
                }
            }

            position = default;
            return false;
        }

        internal bool TryReposition(
            RingPosition firstPosition,
            RingPosition secondPosition)
        {
            if (firstPosition == secondPosition)
                return false;

            bool hasFirstObject =
                _ringObjects.TryGetValue(
                    firstPosition,
                    out RingObject firstObject);

            bool hasSecondObject =
                _ringObjects.TryGetValue(
                    secondPosition,
                    out RingObject secondObject);

            if (!hasFirstObject && !hasSecondObject)
                return false;

            _ringObjects.Remove(firstPosition);
            _ringObjects.Remove(secondPosition);

            if (hasFirstObject)
            {
                _ringObjects.Add(
                    secondPosition,
                    firstObject);
            }

            if (hasSecondObject)
            {
                _ringObjects.Add(
                    firstPosition,
                    secondObject);
            }

            return true;
        }

        internal bool TryGetPosition(
            RingObject ringObject,
            out RingPosition position)
        {
            Guard.NotNull(ringObject, nameof(ringObject));

            foreach (KeyValuePair<RingPosition, RingObject> entry
                     in _ringObjects)
            {
                if (entry.Value.Id != ringObject.Id)
                    continue;

                position = entry.Key;
                return true;
            }

            position = default;
            return false;
        }

        internal bool TryMoveObject(
                RingObject ringObject,
                RingPosition targetPosition)
        {
            Guard.NotNull(ringObject, nameof(ringObject));

            if (!TryGetPosition(
                    ringObject,
                    out RingPosition currentPosition))
            {
                return false;
            }

            if (currentPosition == targetPosition)
                return false;

            if (_ringObjects.ContainsKey(targetPosition))
                return false;

            _ringObjects.Remove(currentPosition);

            _ringObjects.Add(
                targetPosition,
                ringObject);

            return true;
        }

        private RingPosition GetRotatedPosition(
            RingPosition position,
            RotationDirection direction)
        {
            if (direction == RotationDirection.Clockwise)
            {
                return position switch
                {
                    RingPosition.Top => RingPosition.Right,
                    RingPosition.Right => RingPosition.Bottom,
                    RingPosition.Bottom => RingPosition.Left,
                    RingPosition.Left => RingPosition.Top,
                    _ => position
                };
            }

            return position switch
            {
                RingPosition.Top => RingPosition.Left,
                RingPosition.Left => RingPosition.Bottom,
                RingPosition.Bottom => RingPosition.Right,
                RingPosition.Right => RingPosition.Top,
                _ => position
            };
        }
    }
}