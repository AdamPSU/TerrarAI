using Terraria;
using Terraria.ModLoader;

namespace TerrarAI.Content.Buffs
{
	public class AgentPetBuff : ModBuff
	{
		public override string Texture => "Terraria/Images/Buff_" + Terraria.ID.BuffID.DynamiteKitten;

		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;

			int projType = ModContent.ProjectileType<Projectiles.AgentPet>();

			if (player.whoAmI == Main.myPlayer && player.ownedProjectileCounts[projType] <= 0)
			{
				var entitySource = player.GetSource_Buff(buffIndex);
				Projectile.NewProjectile(entitySource, player.Center, Microsoft.Xna.Framework.Vector2.Zero, projType, 0, 0f, player.whoAmI);
			}
		}
	}
}

