using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace DeadCells.Common.Buffs;

public class Stun : ModBuff
{
    public const int FrameCount = 4;
    public const int AnimationSpeed = 6;
    public override void SetStaticDefaults()
    {
        // DisplayName.SetDefault("Stun");
        // Description.SetDefault("Not able to move.");
        Main.debuff[Type] = true;
        Main.buffNoSave[Type] = true;
        BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
    }

    public override void Update(Player player, ref int buffIndex)
    {
        player.controlJump = false;
        player.controlDown = false;
        player.controlLeft = false;
        player.controlRight = false;
        player.controlUp = false;
        player.controlUseItem = false;
        player.controlUseTile = false;
        player.controlThrow = false;
        player.gravDir = 1f;
        player.velocity.Y = player.velocity.Y + player.gravity;
        player.pulley = false;
        if (player.velocity.Y > player.maxFallSpeed)
        {
            player.velocity.Y = player.maxFallSpeed;
        }
        base.Update(player, ref buffIndex);
    }
    public override void Update(NPC npc, ref int buffIndex)
    {
        //需要大修
        npc.velocity = Vector2.Zero;
        npc.noGravity = false;



        base.Update(npc, ref buffIndex);
    }
    public override bool PreDraw(SpriteBatch spriteBatch, int buffIndex, ref BuffDrawParams drawParams)
    {
        return true;
    }

}
