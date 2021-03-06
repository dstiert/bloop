﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Bloop.Infrastructure.Storage;

namespace Bloop.Plugin.Program
{
    [Serializable]
    public class ProgramCacheStorage : BinaryStorage<ProgramCacheStorage>
    {
        public List<Program> Programs = new List<Program>();

        protected override string ConfigFolder
        {
            get { return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); }
        }

        protected override string ConfigName
        {
            get { return "ProgramIndexCache"; }
        }
    }
}