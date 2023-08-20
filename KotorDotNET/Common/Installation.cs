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
        public string GamePath { get; private set; }
        public Chitin Chitin { get; private set; }
        public string ChitinPath { get; private set; }
        public TalkTable TalkTable { get; private set; }
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

        public Installation(string gameDirectory)
        {
            // TODO - path handling
            GamePath = gameDirectory;

            ChitinPath = gameDirectory + "/chitin.key";
            Chitin = new Chitin(gameDirectory);

            TalkTablePath = gameDirectory + "/dialog.tlk";
            TalkTable = new TalkTable(TalkTablePath);

            ModulesPath = gameDirectory + "/modules";
            Modules = new CapsuleFolder(ModulesPath);

            RimsPath = gameDirectory + "/rims";
            Rims = new CapsuleFolder(RimsPath);

            LipsPath = gameDirectory + "lips";
            Lips = new CapsuleFolder(LipsPath);

            TexturePacksPath = gameDirectory + "TexturePacks/";
            TexturesLow = new Capsule(TexturePacksPath + "/swpc_tex_tpc.erf");
            TexturesMed = new Capsule(TexturePacksPath + "/swpc_tex_tpb.erf");
            TexturesHigh = new Capsule(TexturePacksPath + "/swpc_tex_tpa.erf");
            TexturesGUI = new Capsule(TexturePacksPath + "/swpc_tex_gui.erf");

            OverridePath = gameDirectory + "/override";
            Override = new ResourceFolder(OverridePath);

            MusicPath = gameDirectory + "/streammusic";
            Music = new ResourceFolder(MusicPath);

            SoundsPath = gameDirectory + "/streamsounds";
            Sounds = new ResourceFolder(SoundsPath);

            VoicesPath = gameDirectory + "/streamwaves";
            Voices = new ResourceFolder(VoicesPath);
        }
    }
}
