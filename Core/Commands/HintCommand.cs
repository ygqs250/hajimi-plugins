using CommandSystem;
using CustomItems.API;
using InventorySystem.Disarming;
using LabApi.Features.Wrappers;
using RemoteAdmin;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

namespace CustomItems.Core.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
[CommandHandler(typeof(GameConsoleCommandHandler))]
internal class HintCommand : ICommand, IUsageProvider
{
    public string Command => "hint";

    public string[] Aliases => ["hint"];

    public string Description => "send hint to your screen";

    public string[] Usage => ["text"];

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        response = "no";
        if (arguments.Count == 0) return false;
            if (sender is PlayerCommandSender p)
        {
            Player player = Player.Get(p);
            Log.Info("cmd:"+arguments.At(0));
            player.SendHint(arguments.At(0),9999);
            response = "ok";
            return true;
        }
            return false;
    }
}
//123\n123\n123\n123\n123\n123\n123\n123\n123\n123\n123\n123\n123\n123\n123\n123\n123\n123\n123\n123\n123\n123\n123\n123\n123\n123\n123\n123\n123n123\n123\n123n123\n123\n123