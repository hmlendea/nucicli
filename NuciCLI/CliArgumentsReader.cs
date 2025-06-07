using System;
using System.Linq;
using NuciExtensions;

namespace NuciCLI
{
    public static class CliArgumentsReader
    {
        public static bool HasOption(string[] args, params string[] optionVariants)
            => optionVariants.Any(args.Contains);

        public static string GetOptionValue(string[] args, params string[] optionVariants)
        {
            if (EnumerableExt.IsNullOrEmpty(args))
            {
                throw new ArgumentNullException(nameof(args));
            }

            for (int i = 0; i < args.Length; i++)
            {
                string[] arg = args[i].Split('=');
                string argName = arg[0];
                string argValue = arg.Length == 2 ? arg[1] : null;

                if (optionVariants.Contains(argName))
                {
                    if (argValue is not null)
                    {
                        return argValue;
                    }

                    if (i + 1 >= args.Length ||
                        args[i + 1].StartsWith('-'))
                    {
                        throw new ArgumentNullException($"The value for the '{argName}' option argument is missing");
                    }

                    return args[i + 1];
                }
            }

            throw new ArgumentException("The specified option is not present in the arguments list");
        }

        public static string TryGetOptionValue(string[] args, params string[] optionVariants)
        {
            try
            {
                return GetOptionValue(args, optionVariants);
            }
            catch
            {
                return null;
            }
        }
    }
}
