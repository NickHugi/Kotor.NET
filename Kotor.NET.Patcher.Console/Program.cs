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

    string copy =
    """
    copy files to override
        "iw_blstrrfl_007.tga"
    end copy
    """;

    string utc = """
    edit creature "c_stormtrooper"
        create or replace
        from key
        to override

        set appearance to label "Sith_Soldier_02"
        set computer use to 10
        set demolitions to 11

        add feat 13

        add equipment to right weapon
            set resref to "w_e11"
            set dropable to true
        end add
        add item to inventory
            set resref to "w_e11"
            set dropable to false
        end add
        set new class
            set class to 2
            set level to 13
            add power 5
        end set
    end edit
    """;

    var installation = new Installation(
        @"C:\Program Files (x86)\Steam\steamapps\common\swkotor\",
        GameEngine.K2,
        Platform.Windows);


    AntlrInputStream inputStream = new AntlrInputStream(utc);
    KotorPatchingLanguageLexer speakLexer = new KotorPatchingLanguageLexer(inputStream);
    CommonTokenStream commonTokenStream = new CommonTokenStream(speakLexer);
    KotorPatchingLanguageParser parser = new KotorPatchingLanguageParser(commonTokenStream);
    var context = parser.script();
    KotorPatchingLanguageVisitor visitor = new KotorPatchingLanguageVisitor();
    var patch = ((List<object>)visitor.Visit(context)).OfType<IPatch>().ToList();
    patch.First().Apply(installation, new(), @"C:\Users\hugin\FotE\ModData\E11 Blaster");
}
catch (Exception ex)
{
    Console.WriteLine("Error: " + ex);
}
