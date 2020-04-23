public class ChargingAttack
{
    public void OnSkill(PlayerStatus _PlayerStatus)
    {
        _PlayerStatus.chargingAttackOn = true;
    }
    public void OffSKill(PlayerStatus _PlayerStatus)
    {
        _PlayerStatus.chargingAttackOn = false;
    }
}
