

namespace GameEngine
{
    public class EffectCondition_NoCondition : EffectCondition
    {

        public override bool IsMet(EffectContext context)
        {
            return true;
        }
    }
}