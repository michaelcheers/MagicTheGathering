namespace MagicTheGathering
{
    internal abstract class Ability
    {
        string text;
        CardReference reference;
        public AbilityInstance CreateInstance()
        {
            return new AbilityInstance(this);
        }
    }
}