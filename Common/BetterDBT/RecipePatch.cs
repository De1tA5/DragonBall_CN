using DBZMODPORT.Tiles;
using Terraria;
using Terraria.ModLoader;

namespace DragonBall_CN.Common.BetterDBT
{
    public class RecipePatch : ModSystem
    {
        public override void AddRecipes()
        {
            //由于dbzcalamity不再更新，需要换成dbzcalamityUTD
            if (!ModLoader.TryGetMod("BetterDBT", out Mod mod) ||
                !ModLoader.TryGetMod("CalamityMod", out Mod cal) ||
                !ModLoader.TryGetMod("dbzcalamityUTD", out Mod dbzcal))
                return;

            if (!DBCPatchConfig.Instance.GokuItemsRecipeFix)
                return;

            Recipe recipe;

            //修复神之魂配方错误
            if (mod.TryFind<ModItem>("GodSoul", out ModItem godSoul) &&
                cal.TryFind<ModItem>("CosmiliteBar", out ModItem cosmiliteBar) &&
                dbzcal.TryFind<ModItem>("GodlyKiCrystal", out ModItem godlyKiCrystal))
            {
                recipe = Recipe.Create(godSoul.Type)
                    .AddIngredient(cosmiliteBar.Type, 10)
                    .AddIngredient(godlyKiCrystal.Type, 35)
                    .AddTile(ModContent.TileType<KaiTable>())
                    .Register();
            }

            //修复噬魂幽花之魂配方错误
            if (mod.TryFind<ModItem>("PolterghastSoul", out ModItem polterghastSoul) &&
                cal.TryFind<ModItem>("RuinousSoul", out ModItem ruinousSoul) &&
                dbzcal.TryFind<ModItem>("WarmKiCrystal", out ModItem warmKiCrystal))
            {
                recipe = Recipe.Create(polterghastSoul.Type)
                    .AddIngredient(ruinousSoul.Type, 10)
                    .AddIngredient(warmKiCrystal.Type, 35)
                    .AddTile(ModContent.TileType<KaiTable>())
                    .Register();
            }

            //修复至尊女巫之魂配方错误
            if (mod.TryFind<ModItem>("WitchSoul", out ModItem witchSoul) &&
                cal.TryFind<ModItem>("AshesofAnnihilation", out ModItem ashesofAnnihilation) &&
                dbzcal.TryFind<ModItem>("UnlimitedKiCrystal", out ModItem unlimitedKiCrystal))
            {
                recipe = Recipe.Create(witchSoul.Type)
                    .AddIngredient(ashesofAnnihilation.Type, 10)
                    .AddIngredient(unlimitedKiCrystal.Type, 35)
                    .AddTile(ModContent.TileType<KaiTable>())
                    .Register();
            }

        }
    }
}
