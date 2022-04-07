﻿using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace GearonArsenal.Content.Projectiles {
    public class MagicMissile : ModProjectile {

        private bool Power = false;
        public override void SetDefaults() {
            Projectile.width = 20;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.light = 0.8f;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.damage = 20;
            Projectile.timeLeft = 600;
        }

        public override Color? GetAlpha(Color lightColor) => new Color(255, 255, 255, 0);

        //<summary> when the projectile enter in the water, transform to a homing projectile </summary>
        public override void AI() {

            Projectile.rotation = Projectile.velocity.ToRotation();

            if (Projectile.wet) {
                Power = true;
                Projectile.velocity *= 1.01f;
            }

            if (Power) {

                Power = true;
                Projectile.ai[0]++;

                if (Projectile.ai[0] <= 1) {
                    for (int i = 0; i < 40; i++) {
                        Vector2 speed = Main.rand.NextVector2CircularEdge(3f, 3f);
                        Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.BubbleBurst_Blue, speed * 5);
                        d.noGravity = true;
                    }
                }

                for (int i = 0; i < 5; i++) {
                    Vector2 speed = Main.rand.NextVector2CircularEdge(0.5f, 0.5f);
                    Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.BubbleBurst_Blue, speed * 5);
                    d.noGravity = true;
                    d.fadeIn = 0.8f;
                }

                Projectile.scale = 1.5f;




                if (Projectile.ai[0] >= 40 && Projectile.ai[0] <= 60) {


                }

                if (Projectile.ai[0] >= 60) {

                    Empowered();

                    if (Projectile.velocity == Vector2.Zero) {
                        Projectile.Kill();
                    }
                }
            }

            if (Projectile.soundDelay == 0 && Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y) > 2f) {
                Projectile.soundDelay = 10;
                SoundEngine.PlaySound(SoundID.Item9, Projectile.position);
            }
        }
        public override void Kill(int timeLeft) {
            for (int i = 0; i < 40; i++) {
                Vector2 speed = Main.rand.NextVector2CircularEdge(1.5f, 1.5f);
                Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.BubbleBurst_Blue, speed * 5);
                d.noGravity = true;
            }
        }

        public void Empowered() {

            Vector2 move = Vector2.Zero;
            float distance = 400f;
            bool isTarget = false;

            for (int k = 0; k < 200; k++) {
                if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5) {
                    Vector2 newMove = Main.npc[k].Center - Projectile.Center;
                    float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                    if (distanceTo < distance) {
                        move = newMove;
                        distance = distanceTo;
                        isTarget = true;
                    }
                }
            }

            if (isTarget) {
                AdjustMagnitude(ref move);
                Projectile.velocity = (10 * Projectile.velocity + move) / 11f;
                AdjustMagnitude(ref Projectile.velocity);
            }

        }
        private void AdjustMagnitude(ref Vector2 vector) {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 6f) {
                vector *= 6f / magnitude;
            }
        }
    }
}