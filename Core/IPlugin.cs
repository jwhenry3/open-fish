namespace OpenFish.Core
{
    public interface IPlugin
    {
        public string GetName();
        public string GetDescription();
        public string[] GetDependencies();
    }
}