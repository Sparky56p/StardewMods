﻿using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewValley;

namespace FishingMinigames
{
    public class MinigameMessage
    {
        public long multiplayerID;
        public string stage;
        public float voicePitch;
        public bool drawTool;
        public bool drawAttachments;
        public int whichFish;
        public int fishQuality;
        public int maxFishSize;
        public float fishSize;
        public float itemSpriteSize;
        public int count;
        public bool recordSize;
        public bool furniture;
        public Rectangle sourceRect;
        public int x;
        public int y;
        public Color ambientColor;


        public MinigameMessage()
        {
            this.multiplayerID = -1L;
            this.stage = null;
            this.sourceRect = new Rectangle();
        }

        public MinigameMessage(Farmer whichPlayer, string stage, float voice, bool drawTool, bool drawAttachments, int whichFish, int fishQuality, int maxFishSize, float fishSize, float itemSpriteSize, int count, bool recordSize, bool furniture, Rectangle sourceRect, int x, int y, Color ambientColor)
        {
            this.multiplayerID = whichPlayer.UniqueMultiplayerID;
            this.stage = stage;
            this.voicePitch = voice;
            this.drawTool = drawTool;
            this.drawAttachments = drawAttachments;
            this.whichFish = whichFish;
            this.fishQuality = fishQuality;
            this.maxFishSize = maxFishSize;
            this.fishSize = fishSize;
            this.itemSpriteSize = itemSpriteSize;
            this.count = count;
            this.recordSize = recordSize;
            this.furniture = furniture;
            this.sourceRect = sourceRect;
            this.x = x;
            this.y = y;
            this.ambientColor = ambientColor;
        }
    }


    class MinigameColor
    {
        public Color color;
        public Vector2 pos = new Vector2(0f);
        public int whichSlider = 0;
    }
    class DummyMenu : StardewValley.Menus.IClickableMenu
    {
        public DummyMenu()
        {
            //this is just to prevent other mods from interfering with minigames
        }
    }


    //[HarmonyPatch(typeof(Tool), "getDescription")]
    class HarmonyPatches
    {
        public static void getDescription_Nets(ref string __result, ref Tool __instance)
        {
            if (__instance is StardewValley.Tools.FishingRod && __instance.UpgradeLevel != 1)//bamboo+ (except training)
            {
                string desc = Game1.content.LoadString("Strings\\StringsFromCSFiles:FishingRod.cs.14042");

                desc += "\n" + FishingMinigames.ModEntry.AddEffectDescriptions(__instance.Name);

                if (__instance.UpgradeLevel > 1)//fiber/iridium
                {
                    if (__instance.attachments[0] != null)
                    {
                        desc += "\n\n" + __instance.attachments[0].DisplayName + ((__instance.attachments[0].Quality == 0) ? "" : " (" + FishingMinigames.ModEntry.translate.Get("Mods.Infinite") + ")")
                               + ":\n" + FishingMinigames.ModEntry.AddEffectDescriptions(__instance.attachments[0].Name);
                    }
                    if (__instance.attachments[1] != null)
                    {
                        desc += "\n\n" + __instance.attachments[1].DisplayName + ((__instance.attachments[1].Quality == 0) ? "" : " (" + FishingMinigames.ModEntry.translate.Get("Mods.Infinite") + ")")
                               + ":\n" + FishingMinigames.ModEntry.AddEffectDescriptions(__instance.attachments[1].Name);
                    }
                }
                if (desc.EndsWith("\n")) desc = desc.Substring(0, desc.Length - 1);
                __result = Game1.parseText(desc, Game1.smallFont, desc.Length * 10);
            }
        }
    }
}
