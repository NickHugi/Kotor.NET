using Antlr4.Runtime;
using Kotor.NET.Patcher;
using Kotor.NET.PatchingLanguage;

try
{
    string text = """ 
	edit appearance
		target row where "label" is "Creature_Tauntaun" 
		copy row where "label" is "Creature_Dewback" 
		assign cell set "race" to "c_tauntaun"
	end edit

	edit creature "c_tauntaun"
		target override 
		copy from template "c_dewback"
		set appearance 0
	end edit
	""";


    AntlrInputStream inputStream = new AntlrInputStream(text);
    KotorPatchingLanguageLexer speakLexer = new KotorPatchingLanguageLexer(inputStream);
    CommonTokenStream commonTokenStream = new CommonTokenStream(speakLexer);
    KotorPatchingLanguageParser parser = new KotorPatchingLanguageParser(commonTokenStream);
    var context = parser.script();
    KotorPatchingLanguageVisitor visitor = new KotorPatchingLanguageVisitor();
    var patch = ((List<object>)visitor.Visit(context)).OfType<IPatch>().ToList();
    patch.First().Apply(new());
}
catch (Exception ex)
{
    Console.WriteLine("Error: " + ex);
}
