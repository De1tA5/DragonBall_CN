using Hjson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Terraria.Localization;
using Terraria.ModLoader;

namespace DragonBall_CN.Common.DBTBalanceRevived
{
    //TODO - 支持热重载？ 自动注册LocalizationPatch文件夹下的所有本地化补丁？
    public static class LocalizationPatchHelper
    {
        /// <summary>
        /// 获取游戏语言修改模组的本地化路径
        /// </summary>
        /// <param name="mod">进行改动的模组</param>
        /// <param name="targetMod">被改动本地化的模组</param>
        /// <returns>返回对应语言的本地化文件路径</returns>
        internal static string GetLocalizationFile(string mod, string targetMod)
        {
            string languageName = "";

            if (GameCulture.FromCultureName(GameCulture.CultureName.Chinese).IsActive)
                languageName = "zh-Hans";
            else
                languageName = "en-US";

            return $"LocalizationPatch/{mod}/{languageName}_Patches_Mods.{targetMod}.hjson";
        }

        /// <summary>
        /// 加载补丁本地化文件
        /// </summary>
        /// <param name="origMod">进行改动的模组</param>
        /// <param name="targetMod">被改动本地化的模组</param>
        internal static void LoadLocalizationFile(string origMod, string targetMod)
        {
            string filePath = GetLocalizationFile(origMod, targetMod);
            Mod mod = ModContent.GetInstance<DragonBall_CN>();
            byte[] fileBytes;

            try
            {
                fileBytes = mod.GetFileBytes(filePath);
            }
            catch (Exception e)
            {
                mod.Logger.Error($"[LocalizationPatchHelper]无法读取本地化文件：{filePath}\n{e}");
                return;
            }
            string jsonText;

            string hjsonText = Encoding.UTF8.GetString(fileBytes).TrimStart('\uFEFF');

            try
            {
                string normalizedHjsonText = hjsonText.TrimStart();

                //Hjson文件加上大括号
                if (!normalizedHjsonText.StartsWith("{", StringComparison.Ordinal))
                {
                    normalizedHjsonText = $"{{{normalizedHjsonText}}}";
                }

                //HJSON转json
                jsonText = HjsonValue.Parse(normalizedHjsonText).ToString(Stringify.Plain);
            }
            catch (Exception e)
            {
                mod.Logger.Error($"[LocalizationPatchHelper]无法解析 HJSON 文件：{filePath}\n{e}");
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
                mod.Logger.Error($"[LocalizationPatchHelper]无法展开本地化补丁文件：{filePath}\n{exception}");
                return;
            }

            if (flattenedTranslations.Count == 0)
            {
                mod.Logger.Warn($"[LocalizationPatchHelper]本地化补丁没有包含有效文本：{filePath}");
                return;
            }

            try
            {
                //JSON序列化
                Dictionary<string, Dictionary<string, string>> targetTranslations = new(StringComparer.Ordinal);

                foreach (KeyValuePair<string, Dictionary<string, string>> entry in flattenedTranslations)
                {
                    string targetCategory = $"Mods.{targetMod}.{entry.Key}";
                    targetTranslations[targetCategory] = entry.Value;
                }

                string targetJson = JsonSerializer.Serialize(targetTranslations);
                LanguageManager.Instance.LoadLanguageFromFileTextJson(targetJson, canCreateCategories: false);
            }   
            catch (Exception e)
            {
                mod.Logger.Error($"[LocalizationPatchHelper]无法注册本地化文件：{filePath}\n{e}");
            }
        }

        /// <summary>
        /// 递归遍历本地化JSON，展开拼接键值对
        /// </summary>
        /// <param name="element"></param>
        /// <param name="prefix"></param>
        /// <param name="result"></param>
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

