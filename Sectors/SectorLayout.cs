using System.Collections.Generic;

namespace GameEngine
{
    internal sealed class SectorLayout
    {

        private readonly Dictionary<RingPosition, Sector> _sectors = new Dictionary<RingPosition, Sector>();


        public SectorLayout()
        {

        }


        internal bool TrySetSector(
            Sector sector,
            RingPosition position)
        {
            if (sector == null)
                return false;

            _sectors[position] = sector;
            return true;
        }

        internal bool TryGetSector(
            RingPosition position,
            out Sector sector)
        {
            return _sectors.TryGetValue(
                position,
                out sector
            );
        }

        internal bool TryRemoveSector(
            RingPosition position)
        {
            return _sectors.Remove(position);
        }
    }
}