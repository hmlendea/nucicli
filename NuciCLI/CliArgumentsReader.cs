using System;
using System.Linq;

namespace NuciCLI
{
    public static class CliArgumentsReader
    {
        public static bool HasOption(string[] args, params string[] optionVariants)
        {
            return optionVariants.Any(args.Contains);
        }

        public static string GetOptionValue(string[] args, params string[] optionVariants)
        {
            for (int i = 0; i < args.Length; i++)
            {
                string[] arg = args[i].Split('=');
                string argName = arg[0];
                string argValue = arg.Length == 2 ? arg[1] : null;

                if (optionVariants.Contains(argName))
                {
                    if (!(argValue is null))
                    {
                        return argValue;
                    }

                    if (i + 1 >= args.Length ||
                        args[i + 1].StartsWith("-"))
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
            string value;

            try
            {
                value = GetOptionValue(args, optionVariants);
            }
            catch
            {
                value = null;
            }

            return value;
        }
    }
}
