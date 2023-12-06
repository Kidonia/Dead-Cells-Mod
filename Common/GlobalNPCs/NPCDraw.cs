using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DeadCells.Common.Buffs;
using DeadCells.Core;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace DeadCells.Common.GlobalNPCs;
//用于绘制NPC头顶的各种DeBuff，同时也能提供参数供使用
public class NPCDrawBuff : GlobalNPC
{
    public override bool InstancePerEntity => true;    //这个必须写，没有为什么，不然加载不了
    public int bleednum;
    public bool oil;

    public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        //参考灾厄绘制DeBuff图标
        IList<Texture2D> buffTextureList = new List<Texture2D>();//列表存放各种图标素材

        //绘制流血图标
        for (int i = 0; i < bleednum && i < 5; i++)//按顺序，先绘制流血图标
        {
            buffTextureList.Add(ModContent.Request<Texture2D>("DeadCells/Assets/BuffIcons/BleedAffect", (AssetRequestMode)2).Value);
        }
        if (npc.HasBuff<Oil>()) buffTextureList.Add(ModContent.Request<Texture2D>("DeadCells/Assets/BuffIcons/OilAffect", (AssetRequestMode)2).Value);


        //下面实现绘制
        int totalLength = buffTextureList.Count * 7;

        float drawPosX =  (buffTextureList.Count < 5) ? totalLength + 5.7f : buffTextureList.Count * 4.72f;

        /*
        即
        if (buffTextureList.Count < 5)
        {
            drawPosX = totalLength / 2 + 5.7f;
        }
        else
        {
            drawPosX = totalLength / 2 - 2.25f * buffTextureList.Count;
        }
        */

        //不要动
        float drawPosY = TextureAssets.Npc[npc.type].Value.Height / Main.npcFrameCount[npc.type] / 2 * npc.scale + npc.gfxOffY + 26f;

        for (int j = 0; j < buffTextureList.Count; j++)
        {
            if (j != 0)
            {
                drawPosX = (buffTextureList.Count < 5) ? drawPosX - 14f : drawPosX - 6.4f;
                /*
                即
                if (buffTextureList.Count < 5)
                {
                    drawPosX = drawPosX - 14f;
                }
                else
                {
                    drawPosX = drawPosX - 6.4f;
                }
                */
            }
            Texture2D tex = buffTextureList.ElementAt(j);
            spriteBatch.Draw(tex, npc.Center - screenPos - new Vector2(drawPosX, drawPosY), null, Color.White, 0f, default, 1f, 0, 0f);
        }



        if(bleednum == 5)
        {
            Texture2D texture = ModContent.Request<Texture2D>("DeadCells/fxEffects/BigRedCircle", (AssetRequestMode)2).Value;//在周围画个圆
            Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition - texture.Size() / 2f, new Color(95, 0, 0, 0));//画圆
        }

        return true;
    }

}
