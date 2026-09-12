using Hjson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Terraria.Localization;
using Terraria.ModLoader;

namespace DragonBall_CN.Common.DBTBalanceRevived
{
    public class LocalizationPatchSystem : ModSystem
    {
        internal static string GetLocalizationFile(string mod, string targetMod)
        {
            string languageName = "";

            if (GameCulture.FromCultureName(GameCulture.CultureName.Chinese).IsActive)
                languageName = "zh-Hans";
            else
                languageName = "en-US";

            return $"LocalizationPatch/{mod}/{languageName}_Mods.{targetMod}.hjson";
        }

        internal static void LoadLocalizationFile(string origMod, string targetMod)
        {
            string filePath = GetLocalizationFile(origMod, targetMod);
            Mod mod = ModContent.GetInstance<DragonBall_CN>();
            byte[] fileBytes;
            string jsonText;

            try
            {
                fileBytes = mod.GetFileBytes(filePath);
            }
            catch (Exception e)
            {
                mod.Logger.Error($"无法读取本地化文件：{filePath}\n{e}");
                return;
            }

            string hjsonText = Encoding.UTF8.GetString(fileBytes);


            try
            {
                jsonText = JsonValue.Parse(hjsonText).ToString(Stringify.Plain);
            }
            catch (Exception e)
            {
                mod.Logger.Error($"无法解析 HJSON 文件：{filePath}\n{e}");
                return;
            }

            Dictionary<string, Dictionary<string, string>> flattenedTranslations = new(StringComparer.Ordinal);

            try
            {
                using JsonDocument document = JsonDocument.Parse(jsonText);
                FlattenLocalization(document.RootElement, string.Empty, flattenedTranslations);
            }
            catch (Exception exception)
            {
                mod.Logger.Error($"无法展开本地化补丁文件：{filePath}\n{exception}");
                return;
            }

            if (flattenedTranslations.Count == 0)
            {
                mod.Logger.Warn($"本地化补丁没有包含有效文本：{filePath}");
                return;
            }

            try
            {
                string flattenedJson = JsonSerializer.Serialize(flattenedTranslations);
                LanguageManager.Instance.LoadLanguageFromFileTextJson(flattenedJson, canCreateCategories: false);
            }
            catch (Exception e)
            {
                mod.Logger.Error($"无法注册本地化文件：{filePath}\n{e}");
            }
        }
        internal static void FlattenLocalization(JsonElement element, string prefix, Dictionary<string, Dictionary<string, string>> result)
        {
            if (element.ValueKind == JsonValueKind.Object)
            {
                foreach (JsonProperty property in element.EnumerateObject())
                {
                    string childKey = string.IsNullOrEmpty(prefix) ? property.Name : $"{prefix}.{property.Name}";
                    FlattenLocalization(property.Value, childKey, result);
                }
                return;
            }

            if (element.ValueKind != JsonValueKind.String)
                return;

            string? value = element.GetString();

            if (string.IsNullOrEmpty(value))
                return;

            int separatorIndex = prefix.LastIndexOf('.');

            if (separatorIndex <= 0 || separatorIndex >= prefix.Length - 1)
                return;


            string category = prefix[..separatorIndex];

            string key = prefix[(separatorIndex + 1)..];

            if (!result.TryGetValue(category, out Dictionary<string, string>? values))
            {
                values = new Dictionary<string, string>(StringComparer.Ordinal);
                result.Add(category, values);
            }

            values[key] = value;
        }
    }
}

