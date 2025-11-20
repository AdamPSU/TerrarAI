using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrarAI.Content.Projectiles
{
	public class AgentPet : ModProjectile
	{
		public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.DynamiteKitten;

		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = Main.projFrames[ProjectileID.DynamiteKitten];
			Main.projPet[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.DynamiteKitten);
			AIType = ProjectileID.DynamiteKitten;
			DrawOriginOffsetY = -16;
		}
	}
}

