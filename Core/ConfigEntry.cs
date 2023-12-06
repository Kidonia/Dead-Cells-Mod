using System;
using System.Collections.Generic;
using System.Configuration.Internal;
using System.Runtime.CompilerServices;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace DeadCells.Core
{
    public class ConfigEntry<T> : IConfigEntry
    {
        private readonly Func<T> defaultValueGetter;

        private T? localValue;

        private T? remoteValue;
        private class CategoryData
        {
            public readonly Dictionary<string, IConfigEntry> EntriesByName = new Dictionary<string, IConfigEntry>();
        }
        private static readonly Dictionary<string, IConfigEntry> entriesByName = new Dictionary<string, IConfigEntry>();
        private static readonly Dictionary<string, CategoryData> categoriesByName = new Dictionary<string, CategoryData>();
        public string Name { get; }

        public string Category { get; }

        public ConfigSide Side { get; }

        public LocalizedText? DisplayName { get; internal set; }

        public LocalizedText? Description { get; internal set; }

        public Mod? Mod { get; private set; }

        public Type ValueType => typeof(T);

        public T DefaultValue => defaultValueGetter();

        public T? LocalValue
        {
            get
            {
                return ModifyGetValue(localValue);
            }
            set
            {
                localValue = ModifySetValue(value);
            }
        }

        public T? RemoteValue
        {
            get
            {
                return ModifyGetValue(remoteValue);
            }
            set
            {
                remoteValue = ModifySetValue(value);
            }
        }

        public T? Value
        {
            get
            {
                if (Side == ConfigSide.Both && Main.netMode == 1)
                {
                    return RemoteValue;
                }
                return LocalValue;
            }
            set
            {
                if (Side == ConfigSide.Both && Main.netMode == 1)
                {
                    RemoteValue = value;
                }
                else
                {
                    LocalValue = value;
                }
            }
        }

        object? IConfigEntry.Value
        {
            get
            {
                return Value;
            }
            set
            {
                Value = (T)value;
            }
        }

        object? IConfigEntry.LocalValue
        {
            get
            {
                return LocalValue;
            }
            set
            {
                LocalValue = (T)value;
            }
        }

        object? IConfigEntry.RemoteValue
        {
            get
            {
                return RemoteValue;
            }
            set
            {
                RemoteValue = (T)value;
            }
        }

        object IConfigEntry.DefaultValue => DefaultValue;

        internal static void RegisterEntry(IConfigEntry entry)
        {
            entriesByName.Add(entry.Name, entry);
            if (!categoriesByName.TryGetValue(entry.Category, out var category))
            {
                category = (categoriesByName[entry.Category] = new CategoryData());
            }
            category.EntriesByName.Add(entry.Name, entry);
        }

        public ConfigEntry(ConfigSide side, string category, string name, Func<T> defaultValueGetter)
        {
            Name = name;
            Category = category;
            Side = side;
            this.defaultValueGetter = defaultValueGetter;
            RemoteValue = DefaultValue;
            LocalValue = DefaultValue;
            RegisterEntry(this);
        }

        protected virtual T? ModifyGetValue(T? value)
        {
            return value;
        }

        protected virtual T? ModifySetValue(T? value)
        {
            return value;
        }

        public void Initialize(Mod mod)
        {
            Mod = mod;
            DefaultInterpolatedStringHandler val = default(DefaultInterpolatedStringHandler);
            val.AppendLiteral("Configuration.");
            val.AppendFormatted(Category);
            val.AppendLiteral(".");
            val.AppendFormatted(Name);
            val.AppendLiteral(".DisplayName");
            DisplayName = Language.GetOrRegister(mod, val.ToStringAndClear());
            val.AppendLiteral("Configuration.");
            val.AppendFormatted(Category);
            val.AppendLiteral(".");
            val.AppendFormatted(Name);
            val.AppendLiteral(".Description");
            Description = Language.GetOrRegister(mod, val.ToStringAndClear());
        }

        public static implicit operator T?(ConfigEntry<T> configEntry)
        {
            return configEntry.Value;
        }
    }
    //roll
}
