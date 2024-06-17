using ConsoleApp1;
using Kotor;
using Kotor.NET.Common.Geometry;
using Kotor.NET.Common.Item;
using Kotor.NET.Formats.KotorBWM;
using Kotor.NET.Formats.KotorMDL;
using Kotor.NET.Resources.KotorUTP;
using MapBuilder.Data;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGLES;
using Silk.NET.Windowing;


//Viewer viewer = new();
//viewer.Run();

var map = new TerrainData(50, 50);
var mdl = map.GenerateModel();

///new MDLBinaryReader(@"C:\Users\hugin\Desktop\temp\test.mdl", @"C:\Users\hugin\Desktop\temp\test.mdx").Read();
new MDLBinaryWriter(@"C:\Users\hugin\Desktop\temp\test.mdl", @"C:\Users\hugin\Desktop\temp\test.mdx").Write(mdl);

