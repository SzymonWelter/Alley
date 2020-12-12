namespace Alley.Utils.Helpers
{
    public static class MethodHelper
    {
        private const char Separator = '/';
        private const int ServiceIndex = 1;
        private const int MethodIndex = 2;

        public static void SplitMethodFullName(string methodFullName, out string serviceName, out string methodName)
        {
            var splittedFullName = methodFullName.Split(Separator);
            serviceName = splittedFullName[ServiceIndex];
            methodName = splittedFullName[MethodIndex];
        }
    }
}