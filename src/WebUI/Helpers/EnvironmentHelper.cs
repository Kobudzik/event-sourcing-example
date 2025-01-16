namespace EventSourcingExample.WebUI.Helpers
{
    public static class EnvironmentHelper
    {
        public static bool IsDebug =>
#if DEBUG
            true;
#else
            false;
#endif
    }
}
