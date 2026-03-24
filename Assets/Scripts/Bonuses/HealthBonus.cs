using MedievalRunner.Player;

namespace MedievalRunner.Bonuses
{
    public class HealthBonus : BonusBase
    {
        protected override void ApplyEffect(PlayerHealth health)
        {
            if (health == null || Data == null)
            {
                return;
            }

            health.Heal(Data.HealAmount);
        }
    }
}
