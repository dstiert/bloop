﻿namespace Bloop.Plugin
{
    public class PluginPair
    {
        public IPlugin Plugin { get; set; }
        public PluginMetadata Metadata { get; set; }

        internal long InitTime { get; set; }

        internal long AvgQueryTime { get; set; }

        internal int QueryCount { get; set; }

        public override string ToString()
        {
            return Metadata.Name;
        }
    }
}
