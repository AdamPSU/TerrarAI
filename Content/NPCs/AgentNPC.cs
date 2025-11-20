using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrarAI.Content.NPCs
{
	public class AgentNPC : ModNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Zombie];
		}

		public override void SetDefaults()
		{
			NPC.CloneDefaults(NPCID.Zombie);
			NPC.friendly = true;
			AnimationType = NPCID.Zombie;
		}

		public override bool CheckActive()
		{
			return false;
		}
	}
}

