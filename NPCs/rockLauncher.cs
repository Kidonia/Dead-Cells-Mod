using DeadCells.Core;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.GameContent;
using Terraria;
using Microsoft.Xna.Framework;

namespace DeadCells.NPCs;

public class rockLauncher : DC_BasicNPC
{
    private Dictionary<int, DCAnimPic> AtkDic = new();
    private Dictionary<int, DCAnimPic> AtkLoadDic = new();
    private Dictionary<int, DCAnimPic> idleDic = new();
    private Dictionary<int, DCAnimPic> hitDic = new();
    private Dictionary<int, DCAnimPic> stunDic = new();
    private Dictionary<int, DCAnimPic> stunIdleDic = new();
    private Dictionary<int, DCAnimPic> fallDic = new();
    private int atkCharge;
    private int noticeTime = 32;//初始发现玩家后的僵直时间

    private bool drawFX;
    private bool drawGlow;

    public override void SetDefaults()
    {
        AtkDic = AssetsLoader.rockLauncherDic["atk"];
        AtkLoadDic = AssetsLoader.rockLauncherDic["atkLoad"];
        idleDic = AssetsLoader.rockLauncherDic["idle"];
        hitDic = AssetsLoader.rockLauncherDic["hit"];
        fallDic = AssetsLoader.rockLauncherDic["fall"];
        stunDic = AssetsLoader.rockLauncherDic["stun"];
        stunIdleDic = AssetsLoader.rockLauncherDic["stunIdle"];

        NPC.lifeMax = 400;
        NPC.width = 32;
        NPC.height = 64;
        NPC.damage = 1;
        NPC.defense = 8;
        NPC.scale = 1.7f;
        NPC.friendly = false;
    }
    public override void AI()
    {
        float distance = Vector2.Distance(player.Center, NPC.Center);
        Main.NewText(NPC.ai[3]);
        switch (NPC.ai[0])
        {
            case 0:
                ChooseCorrectFrame(idleDic); break;
            case 1:
                ChooseCorrectFrame(stunDic); break;     
            case 2:
                ChooseCorrectFrame(fallDic); break;
            case 3:
                ChooseCorrectFrame(hitDic); break;
        }
        if (NPC.ai[0] == 3 && NPC.ai[3] == hitDic.Count - 4)
            NPC.ai[0] = 0;
    }

    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        spriteBatch.Draw(TextureAssets.Npc[Type].Value, NPC.Center - Main.screenPosition - NPC.frame.Size() * NPC.scale / 2, NPC.frame, Color.White, 0f, Vector2.Zero, NPC.scale, (SpriteEffects)(-(NPC.direction - 1) / 2), 0);
        DrawGlowTexture(AssetsLoader.rockLauncherGlowTex, spriteBatch, new(1, 252, 187));

        return false;
    }
    public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
    {
        if (NPC.ai[0] == 0f)
        {
            NPC.ai[0] = 3;
        }
            base.OnHitByItem(player, item, hit, damageDone);
    }
    public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
    {
        if (NPC.ai[0] == 0f)
        {
            NPC.ai[0] = 3;
        }
        base.OnHitByProjectile(projectile, hit, damageDone);
    }
    public override void HitEffect(NPC.HitInfo hit)
    {
        AddDCNPCHitEffect(AssetsLoader.enm_zmb_die, 5660545, 12303527);
    }
}
