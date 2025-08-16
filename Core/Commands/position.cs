using CommandSystem;
using CustomItems.API;
using InventorySystem.Disarming;
using LabApi.Features.Wrappers;
using RemoteAdmin;
using System;
using System.Linq;
using UnityEngine;

namespace CustomItems.Core.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
[CommandHandler(typeof(GameConsoleCommandHandler))]
internal class PositionCommand : ICommand, IUsageProvider
{
    public string Command => "position";

    public string[] Aliases => ["ps"];

    public string Description => "print player's position";

    public string[] Usage => ["%player%"];

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        response = "position:";
        if (!sender.CheckPermission(PlayerPermissions.GivingItems))
        {
            response = "Permission Denied, required: " + PlayerPermissions.GivingItems;
            return false;
        }
        if (arguments.Count == 0)
        {
            if (sender is PlayerCommandSender p)
            {
                Player player = Player.Get(p);
                Vector3 position = player.Position;
                response= "position:" + position.ToString();
                return true;
            }
        }
        return false;
      
    }

    private bool CheckEligible(Player player) => player.IsAlive && !player.Inventory.IsDisarmed() && (player.Items.Count() < 8);

    // Borrowed from Exiled, but modified to work with LabAPI
    private Player Get(string args)
    {
        if (Player.List.Where(p => p.UserId == args).FirstOrDefault() is Player player)
            return player;
        if (int.TryParse(args, out int id)) return Player.Get(id);

        var lastnameDifference = 3;
        var firstString = args.ToLower();
        Player found = null;

        foreach (Player plr in Player.Dictionary.Values)
        {
            if (!plr.IsOnline || plr.IsHost || plr.Nickname is null) continue;
            if (!plr.Nickname.ToLower().Contains(args.ToLower())) continue;
            var secondString = plr.Nickname;

            var nameDifference = secondString.Length - firstString.Length;
            if (nameDifference < lastnameDifference)
            {
                lastnameDifference = nameDifference;
                found = plr;
            }
        }
        return found;
    }
}