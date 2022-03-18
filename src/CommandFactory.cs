using System;
using System.Collections.Generic;
using System.CommandLine;

namespace Loinc.Cli;
class CommandFactory
{
    public static IEnumerable<LoincCommand> BuildCommandList()
    {
        foreach (var type in GetCommandTypes())
        {
            if (type == null)
                continue;

            LoincCommand? command = type.GetConstructor(Type.EmptyTypes)?.Invoke(null) as LoincCommand;
            if (command == null)
            {
                throw new ArgumentException("Invalid command type");
            }
            yield return command;
        }
    }

    static IEnumerable<Type> GetCommandTypes()
    {
        yield return typeof(CodeSystemCommand);
        yield return typeof(ValueSetCommand);
        yield return typeof(LoginCommand);
    }
} 