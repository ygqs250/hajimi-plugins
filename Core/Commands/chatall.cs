using CommandSystem;
using CustomItems.API;
using InventorySystem.Disarming;
using LabApi.Features.Wrappers;
using PlayerStatsSystem;
using RemoteAdmin;
using System;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CustomItems.Core.Commands;

//[CommandHandler(typeof(RemoteAdminCommandHandler))]
[CommandHandler(typeof(ClientCommandHandler))]
internal class ChatAllCommand : ICommand, IUsageProvider
{
    public string Command => "ChatAll";

    public string[] Aliases => ["bc"];

    public string Description => "全体聊天";

    public string[] Usage => ["聊天内容"];

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {

        if (arguments.Count == 0)
        {
            response = this.DisplayCommandUsage() + "\n";
            return false;
        }
        else
        {
            if (sender is PlayerCommandSender p)
            {
                Player player = Player.Get(p);
                if (player.Faction == PlayerRoles.Faction.Unclassified)
                {
                    response = "死人不能发送全体信息" + "\n";
                    return false;
                }
                StringBuilder message = new StringBuilder(); ; 
                for (int i = 0; i < arguments.Count; i++)
                {
                    message.Append(arguments.At(i));
                }
                foreach (Player re in Player.List)
                {
                    Wanjia.Get(re).hintUIManager.AddChatMessageAll(player.Nickname, message.ToString(),player.Faction);
                }
                response = "发送成功";
                return true;
            }
        }
        response = "发送失败";
        return false;

    }

}