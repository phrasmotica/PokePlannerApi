using System;
using System.Runtime.InteropServices;

namespace PokePlannerApi.Data.Util
{
    /// <summary>
    /// Helper class for access host platform information.
    /// </summary>
    public static class EnvHelper
    {
        /// <summary>
        /// Returns whether the host platform is MacOS.
        /// </summary>
        public static bool IsMacOs => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

        /// <summary>
        /// Returns whether the host platform is MacOS.
        /// </summary>
        public static bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        /// <summary>
        /// Returns the environment variable with the given name.
        /// </summary>
        public static string GetVariable(string name)
        {
            if (IsMacOs)
            {
                return Environment.GetEnvironmentVariable(name);
            }

            if (IsWindows)
            {
                return Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Machine);
            }

            throw new NotImplementedException();
        }
    }
}
