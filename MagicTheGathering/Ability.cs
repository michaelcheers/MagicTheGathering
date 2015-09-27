namespace MagicTheGathering
{
    internal abstract class Ability
    {
        string text;
        CardReference reference;
        public abstract AbilityInstance CreateInstance();
    }
}