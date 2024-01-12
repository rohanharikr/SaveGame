namespace SaveGame.Test.Unit
{
    public class BaseTest
    {
        readonly string appWorkingDirPath;

        public BaseTest()
        {
            appWorkingDirPath = Path.GetFullPath(@"../../../../SaveGame/bin/Debug/net8.0-windows");
        }

        [SetUp]
        public void SetUp()
        {
            //Remove DB so we start every test on a clean state
            File.Delete(Path.Combine(appWorkingDirPath, "Data.db"));
        }
    }
}
