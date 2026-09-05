using Antlr4.Runtime;
using Kotor.NET.Common;
using Kotor.NET.Encapsulations;
using Kotor.NET.Patcher;
using Kotor.NET.Patcher.ForUTI;
using Kotor.NET.PatchingLanguage.Visitor;

try
{
    string text = """ 
    edit appearance
        target row where "label" is "Creature_Tauntaun" 
        copy row where "label" is "Creature_Dewback" 
        assign cell set "race" to "c_tauntaun"
    end edit

    edit creature "c_tauntaun"
        copy from template "c_dewback"
        assign uint16 set "Appearance_Type" to 123
        set appearance from label "Creature_Dewback"
    end edit
    """;

    string uti = """
    edit item "w_e11"
        create or replace
        from key
        to override

        set base item to label "Blaster_Rifle"
        set name to "E11 Blaster"
    
        add property
            set property name to 1
        end

        add property
            set property name to 2
        end
    end edit
    """;

    var installation = new Installation(
        @"C:\Program Files (x86)\Steam\steamapps\common\swkotor\",
        GameEngine.K2,
        Platform.Windows);


    AntlrInputStream inputStream = new AntlrInputStream(uti);
    KotorPatchingLanguageLexer speakLexer = new KotorPatchingLanguageLexer(inputStream);
    CommonTokenStream commonTokenStream = new CommonTokenStream(speakLexer);
    KotorPatchingLanguageParser parser = new KotorPatchingLanguageParser(commonTokenStream);
    var context = parser.script();
    KotorPatchingLanguageVisitor visitor = new KotorPatchingLanguageVisitor();
    var patch = ((List<object>)visitor.Visit(context)).OfType<PatchUTI>().ToList();
    patch.First().Apply(installation, new());
}
catch (Exception ex)
{
    Console.WriteLine("Error: " + ex);
}
