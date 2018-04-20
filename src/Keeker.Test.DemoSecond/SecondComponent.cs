using Keeker.Test.DemoSecond.Logging;

namespace Keeker.Test.DemoSecond
{
    public class SecondComponent
    {
        public void DoSecond()
        {
            SecondHelper.GetLogger().Info("Second says hello!");
        }
    }
}
