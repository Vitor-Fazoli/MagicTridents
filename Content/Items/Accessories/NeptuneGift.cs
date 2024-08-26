﻿using NeptunesTreasure.Content.Dusts;
using NeptunesTreasure.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NeptunesTreasure.Content.Items.Accessories
{
    /// <summary>
    /// Represents the Neptune Gift accessory item.
    /// </summary>
    public class NeptuneGift : ModItem
    {
        /// <summary>
        /// Sets the default properties of the Neptune Gift accessory item.
        /// </summary>
        public override void SetDefaults()
        {
            // Size settings
            Item.width = 28;
            Item.height = 30;

            // Item Properties
            Item.accessory = true;
            Item.rare = ItemRarityID.Lime;
        }

        /// <summary>
        /// Updates the player's accessory effects when equipped with the Neptune Gift.
        /// </summary>
        /// <param name="player">The player wearing the accessory.</param>
        /// <param name="hideVisual">Determines if the accessory's visual effects should be hidden.</param>
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<NeptuneGiftPlayer>().hasNeptuneGift = true;
        }
    }

    /// <summary>
    /// Represents the player's effects when wearing the Neptune Gift accessory.
    /// </summary>
    internal class NeptuneGiftPlayer : ModPlayer
    {
        public bool hasNeptuneGift = false;
        private float timer;
        private int waterType = Utils.Water.GetRandomWater();

        /// <summary>
        /// Resets the player's effects when the accessory is unequipped.
        /// </summary>
        public override void ResetEffects()
        {
            hasNeptuneGift = false;
        }

        /// <summary>
        /// Initializes the timer when the player enters the world with the Neptune Gift equipped.
        /// </summary>
        public override void OnEnterWorld()
        {
            if (hasNeptuneGift)
            {
                timer = 0;
            }
        }

        /// <summary>
        /// Updates the accessory's effects on the player.
        /// </summary>
        public override void PostUpdateMiscEffects()
        {
            const int cooldown = 600;
            const int waterBubbleCount = 40;

            if (hasNeptuneGift)
            {
                if (timer >= cooldown)
                {
                    for (int i = 0; i < waterBubbleCount; i++)
                    {
                        Vector2 speed = Main.rand.NextVector2Circular(0.5f, 0.5f);
                        Dust d = Dust.NewDustPerfect(Player.Center, ModContent.DustType<WaterBubble>(), speed * 5);
                        d.noGravity = true;

                        Lighting.AddLight(Player.position, Water.GetWaterColor().ToVector3());
                    }

                    waterType = Utils.Water.GetRandomWater();
                    timer = 0;
                }

                Main.waterStyle = waterType;
                timer++;
            }
        }
    }
}