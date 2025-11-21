using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TerrarAI.Content.Commands
{
	public class AICommand : ModCommand
	{
		public override string Command => "ai";

		public override string Description => "Send commands to the AI agent";

		public override CommandType Type => CommandType.Chat;

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			caller.Reply("ping", Color.Green);
		}
	}
}

