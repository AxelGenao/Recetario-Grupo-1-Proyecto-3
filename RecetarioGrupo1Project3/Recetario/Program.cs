using Antlr4.Runtime;

var input = CharStreams.fromPath(@"..\..\..\Recetas.txt");

Console.WriteLine("Los SQL INSERT son:");

var lexer = new RecetarioLexer(input);
var tokenStream = new CommonTokenStream(lexer);
var parser = new RecetarioParser(tokenStream);
var tree = parser.recetario();
var recetario = new Recetario.Recetario();
recetario.Visit(tree);

int n = 0;
foreach (string line in recetario.Output)
{
    n++;
    Console.WriteLine(line);
}
Console.WriteLine("\nDele a alguna tecla para salir.");
Console.ReadKey();