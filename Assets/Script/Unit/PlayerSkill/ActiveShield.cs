public class ActiveShield
{
    public void OnSkill(PlayerStatus _PlayerStatus, Skill _Skill)
    {
        _PlayerStatus.IncreaseShield(_Skill.skillValue);
    }
    public void OffSkill(PlayerStatus _PlayerStatus)
    {
        _PlayerStatus.ShieldReset();
    }
}
