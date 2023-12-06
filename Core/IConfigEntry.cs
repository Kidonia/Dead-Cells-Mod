using System;
using Terraria.ModLoader;

namespace DeadCells.Core
{
    public interface IConfigEntry
    {
        Type ValueType { get; }

        string Name { get; }

        string Category { get; }

        object? Value { get; set; }

        object? LocalValue { get; set; }

        object? RemoteValue { get; set; }

        object DefaultValue { get; }

        ConfigSide Side { get; }

        void Initialize(Mod mod);
    }

}
