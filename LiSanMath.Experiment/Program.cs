using LiSanMath.Experiment;
using Spectre.Console;

AnsiConsole.MarkupLine("离散数学 [green bold]求取命题公式真值表[/]");
AnsiConsole.MarkupLine("by: [yellow bold]Kengwang[/]");

string rawExpression = AnsiConsole.Ask<string>("请输入表达式: ");
var elements = new List<string>();
var tokenChain = Tokenizer.Tokenize(rawExpression, elements);
var table = new List<bool>();
// Fill table with false
for (var i = 0; i < elements.Count; i++)
{
    table.Add(false);
}

var consoleTable = new Table();
consoleTable.AddColumn("序号");
foreach (var element in elements)
{
    consoleTable.AddColumn(element);
}
consoleTable.AddColumn("结果");
List<int> maxValues = [];
List<int> minValues = [];

var canBeTrue = false;
for (var i = 0; i < Math.Pow(2, elements.Count); i++)
{
    var binary = Convert.ToString(i, 2).PadLeft(elements.Count, '0');
    for (var j = 0; j < elements.Count; j++)
    {
        table[j] = binary[j] == '1';
    }

    var result = tokenChain.Calculate(table);
    if (result)
    {
        canBeTrue = true;
        minValues.Add(i);
    }
    else
    {
        maxValues.Add(i);
    }
    var rowResult = new List<string>();
    rowResult.Add($"[blue]{i}[/]");
    table.Select(x => x ? "[green]T[/]" : "[red]F[/]").ToList().ForEach(x => rowResult.Add(x));
    rowResult.Add(result ? "[green bold]T[/]" : "[red bold]F[/]"); 
    consoleTable.AddRow(rowResult.ToArray());
}

AnsiConsole.Write(consoleTable);

AnsiConsole.MarkupLine($"[{(canBeTrue ? "green" : "red")}]表达式为{(canBeTrue ? "可满足" : "不可满足")}式[/]");
AnsiConsole.Markup("主析取范式为: ");
bool isStart = true;
foreach (var minValue in minValues)
{
    if (!isStart)
    {
        AnsiConsole.Markup(" | ");
    }
    else
    {
        isStart = false;
    }
    AnsiConsole.Markup($"[green]m{minValue}[/]");
}
AnsiConsole.MarkupLine("");

AnsiConsole.Markup("主合取范式为: ");
isStart = true;
foreach (var maxValue in maxValues)
{
    if (!isStart)
    {
        AnsiConsole.Markup(" & ");
    }
    else
    {
        isStart = false;
    }
    AnsiConsole.Markup($"[green]M{maxValue}[/]");
}
