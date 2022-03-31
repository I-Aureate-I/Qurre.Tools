using Qurre.API;
using Qurre.API.Controllers;
using Qurre.API.Objects;
using System.Collections.Generic;
using System.Linq;

namespace Qurre.Tools
{
    public static class RoomExtensions
    {
        public static IEnumerable<Room> LczRooms => Map.Rooms.Where(x => x.IsLcz());

        public static IEnumerable<Room> HczRooms => Map.Rooms.Where(x => x.IsHcz());

        public static IEnumerable<Room> EzRooms => Map.Rooms.Where(x => x.IsEz());

        public static bool IsLcz(this Room room) => room.Zone is ZoneType.Light;

        public static bool IsHcz(this Room room) => room.Zone is ZoneType.Heavy;

        public static bool IsEz(this Room room) => room.Zone is ZoneType.Office;

        public static bool IsLcz(this RoomType room) => room.ToString().ToUpper().StartsWith("LCZ");

        public static bool IsHcz(this RoomType room) => room.ToString().ToUpper().StartsWith("HCZ");

        public static bool IsEz(this RoomType room) => room.ToString().ToUpper().StartsWith("EZ") || room is RoomType.HczEzCheckpoint;
    }
}
