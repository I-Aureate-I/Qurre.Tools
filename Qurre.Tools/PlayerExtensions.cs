using CommandSystem;
using CustomPlayerEffects;
using Mirror.LiteNetLib4Mirror;
using Qurre.API;
using Qurre.API.Controllers.Items;
using Qurre.API.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Qurre.Tools
{
    public static class PlayerExtensions
    {
        public static SpectatorManager SpectatorManager(this Player player) => player.ReferenceHub.spectatorManager;

        public static Player Get(this ICommandSender sender) => Player.Get(sender as CommandSender);

        public static int Ping(this Player player) => LiteNetLib4MirrorServer.GetPing(player.Connection.connectionId);

        public static string AuthenticationToken(this Player player) => player.ClassManager.AuthToken;

        public static float MinHp(this Player player) => player.PlayerStats.StatModules[0].MinValue;

        public static TeamUnitType TeamUnit(this Player player) => (TeamUnitType)player.ClassManager.NetworkCurSpawnableTeamType;

        public static void TeamUnit(this Player player, TeamUnitType newValue) => player.ClassManager.NetworkCurSpawnableTeamType = (byte)newValue;

        public static void ModifyStamina(this Player player, float Amount) => player.ReferenceHub.fpc.ModifyStamina(Amount);

        public static EffectType RandomEffect(this Player player, float duration = 0f, bool addDurationIfActive = false)
        {
            EffectType effectType = (EffectType)Enum.GetValues(typeof(EffectType)).GetValue(UnityEngine.Random.Range(0, Enum.GetValues(typeof(EffectType)).Length));
            player.EnableEffect(effectType, duration, addDurationIfActive);
            return effectType;
        }

        public static int GetAmmoLimit(this Player player, AmmoType type) => InventorySystem.Configs.InventoryLimits.GetAmmoLimit(type.GetItemType(), player.ReferenceHub);

        public static int GetCategoryLimit(this Player player, ItemCategory category) => InventorySystem.Configs.InventoryLimits.GetCategoryLimit(category, player.ReferenceHub);

        public static string RawUserId(this Player player) => player.UserId.Split('@')[0];

        public static bool IsDead(this Player player) => player.Team is Team.RIP;

        public static bool IsDead(this RoleType role) => role.GetTeam() is Team.RIP;

        public static bool IsAlive(this Player player) => !IsDead(player);

        public static bool IsAlive(this RoleType role) => !IsDead(role);

        public static bool IsScp(this Player player) => player.Team is Team.SCP;

        public static bool IsScp(this RoleType role) => role.GetTeam() is Team.SCP;

        public static bool IsMtf(this Player player) => player.Team is Team.MTF;

        public static bool IsMtf(this RoleType role) => role.GetTeam() is Team.MTF;

        public static bool IsChaos(this Player player) => player.Team is Team.CHI;

        public static bool IsChaos(this RoleType role) => role.GetTeam() is Team.CHI;

        public static bool IsReloading(this Player player) => player.ItemInHand is Firearm firearm && !firearm.Base.AmmoManagerModule.Standby;

        public static bool IsChatting(this Player player) => player.Radio.UsingVoiceChat;

        public static bool IsTransmitting(this Player player) => player.Radio.UsingRadio;

        public static bool InPocket(this Player player) => player.Room.Type is RoomType.Pocket;

        public static bool InLcz(this Player player) => player.Room.Zone is ZoneType.Light;

        public static bool InHcz(this Player player) => player.Room.Zone is ZoneType.Heavy;

        public static bool InEz(this Player player) => player.Room.Zone is ZoneType.Office;

        public static bool InWorld(this Player player) => player.Room.Zone is ZoneType.Surface;

        public static bool IsFull(this Player player) => player.AllItems.Count >= 8;

        public static bool IsEmpty(this Player player) => player.AllItems.Count <= 0;

        public static bool HasLightSource(this Player player, bool checkRoomLight = false)
        {
            if (player.ItemInHand is Flashlight flashlight)
                return flashlight.Active;
            else if (player.ItemInHand is Firearm firearm)
                return firearm.FlashlightEnabled;
            else if (checkRoomLight)
                return !player.Room.LightsDisabled;

            return false;
        }


        public static string Name(this Player player) => string.IsNullOrEmpty(player.DisplayNickname) ? player.Nickname : string.IsNullOrWhiteSpace(player.DisplayNickname) ? player.Nickname : player.DisplayNickname;

        public static string ScpNumber(this Player player)
        {
            string scp = player.Role.ToString();

            return scp?.Substring(scp.IndexOf(scp.FirstOrDefault(x => int.TryParse(x.ToString(), out int _)))) ?? "000";
        }

        public static List<PlayerEffect> ActiveEffects(this Player player) => player.PlayerEffectsController.AllEffects.Values.Where(effect => effect.Intensity > 0).ToList();

        public static void OfflineMute(this string userId) => MuteHandler.IssuePersistentMute(userId);

        public static void OfflineMute(this Player player) => OfflineMute(player.UserId);

        public static void OfflineBan(string reason, string issuer, string id, int duration) => BanHandler.IssueBan(new BanDetails
        {
            Reason = reason,
            Issuer = issuer,
            Id = id,
            OriginalName = "Unknown - offline ban",
            IssuanceTime = DateTime.Now.ToLocalTime().Ticks,
            Expires = DateTime.UtcNow.AddSeconds(duration).Ticks
        }, BanHandler.BanType.UserId);

        public static void OfflineBan(string reason, string issuer, int duration, string ip) => BanHandler.IssueBan(new BanDetails
        {
            Reason = reason,
            Issuer = issuer,
            Id = ip,
            OriginalName = "Unknown - offline ban",
            IssuanceTime = DateTime.UtcNow.Ticks,
            Expires = DateTime.UtcNow.AddSeconds(duration).Ticks
        }, BanHandler.BanType.IP);

        public static void ToString(this Player player, bool data = false) => string.Format("{0}{1}{2} {3}", player.Name(), data ? "@" + player.Id : string.Empty, data ? " " + player.UserId : string.Empty, player.Role.ToString());
    }
}
