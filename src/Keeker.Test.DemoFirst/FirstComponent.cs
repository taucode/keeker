using Keeker.Test.DemoFirst.Logging;

namespace Keeker.Test.DemoFirst
{
    public class FirstComponent
    {
        public void DoFirst()
        {
            FirstHelper.GetLogger().Info("First says hello!");
        }
    }
}
