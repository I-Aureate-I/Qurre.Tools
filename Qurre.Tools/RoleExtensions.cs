using Qurre.API;
using ColorType = UnityEngine.Color;
using LeadingTeamType = LeadingTeam;

namespace Qurre.Tools
{
    public static class RoleExtensions
    {
        public static ColorType Color(this RoleType role) => role is RoleType.None ? ColorType.white : CharacterClassManager._staticClasses.Get(role).classColor;

        public static LeadingTeamType LeadingTeam(this Team team)
        {
            switch (team)
            {
                case Team.CDP:
                case Team.CHI:
                    return LeadingTeamType.ChaosInsurgency;
                case Team.MTF:
                case Team.RSC:
                    return LeadingTeamType.FacilityForces;
                case Team.SCP:
                    return LeadingTeamType.Anomalies;
                default:
                    return LeadingTeamType.Draw;
            }
        }

        public static LeadingTeamType LeadingTeam(this RoleType role) => role.GetTeam().LeadingTeam();
    }
}
