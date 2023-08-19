using KotorDotNET.ResourceContainers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotorDotNET.Common
{
    /// <summary>
    /// A class designed to centralize fetching resources from a specified installation
    /// of the games.
    /// </summary>
    public class Installation
    {
        //public Chitin Chitin { get; private set; }
        public string ChitinPath { get; private set; }
        //public TalkTable TalkTable { get; private set; }
        public string TalkTablePath { get; private set; }
        public CapsuleFolder Modules { get; private set; }
        public string ModulesPath { get; private set; }
        public CapsuleFolder Rims { get; private set; }
        public string RimsPath { get; private set; }
        public CapsuleFolder Lips { get; private set; }
        public string LipsPath { get; private set; }
        public Capsule TexturesHigh { get; private set; }
        public Capsule TexturesMed { get; private set; }
        public Capsule TexturesLow { get; private set; }
        public Capsule TexturesGUI { get; private set; }
        public string TexturePacksPath { get; private set; }
        public ResourceFolder Override { get; private set; }
        public string OverridePath { get; private set; }
        public ResourceFolder Music { get; private set; }
        public string MusicPath { get; private set; }
        public ResourceFolder Sounds { get; private set; }
        public string SoundsPath { get; private set; }
        public ResourceFolder Voices { get; private set; }
        public string VoicesPath { get; private set; }


        public Installation(string rootPath)
        {
            // TODO
        }
    }
}
