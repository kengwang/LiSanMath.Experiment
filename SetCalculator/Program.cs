using Spectre.Console;

var prompt = new SelectionPrompt<int>()
    .Title("请选择要执行的操作:")
    .UseConverter((i =>
    {
        return i switch
        {
            1 => "交集",
            2 => "并集",
            3 => "差集",
            4 => "幂集",
            5 => "求 m 元子集",
            _ => "退出"
        };
    }))
    .AddChoices(1, 2, 3, 4, 5, 0);

var result = AnsiConsole.Prompt(prompt);
if (result != 0)
{
    var rawSetA = AnsiConsole.Ask<string>("请输入集合 A: ");
    var setA = ParseSet(rawSetA);
    if (result < 4)
    {
        var rawSetB = AnsiConsole.Ask<string>("请输入集合 B: ");
        var setB = ParseSet(rawSetB);
        var setResult = result switch
        {
            1 => setA.Intersect(setB).ToHashSet(),
            2 => setA.Union(setB).ToHashSet(),
            3 => setA.Except(setB).ToHashSet(),
            _ => throw new ArgumentOutOfRangeException()
        };
        AnsiConsole.Markup("结果为: ");
        setResult.ShowResult();
    }
    else
    {
        switch (result)
        {
            case 4:
                AnsiConsole.Markup("结果为: ");
                setA.ToList().PowerSet().ShowResult();
                break;
            case 5:
                var m = AnsiConsole.Ask<int>("请输入 m 的值: ");
                AnsiConsole.Markup("结果为: ");
                setA.ToList().PowerSetOfWhereNumber(m).ShowResult();
                break;
        }
    }
}

AnsiConsole.MarkupLine("");
AnsiConsole.MarkupLine("[red]感谢使用, 再见[/]");



/*********************        FUNCTION AREA          *************************/

HashSet<string> ParseSet(string rawSet)
{
    string curItem = "";
    var set = new HashSet<string>();
    bool inBracket = false;
    if (rawSet[0] is '{')
    {
        rawSet = rawSet[1..];
    }

    foreach (var c in rawSet)
    {
        if (c is '}')
        {
            // 这个既可以结尾标识符, 也可以是集合中的集合元素的结尾标识符, 两个处理逻辑一致
            if (inBracket) curItem += c;
            if (!string.IsNullOrWhiteSpace(curItem))
            {
                set.Add(curItem.Trim());
                curItem = "";
            }

            inBracket = false;
            continue;
        }

        if (inBracket)
        {
            curItem += c;
            continue;
        }

        if (c is '{')
        {
            curItem += c;
            inBracket = true;
            continue;
        }

        if (c is ',' or '，')
        {
            if (!string.IsNullOrWhiteSpace(curItem))
            {
                set.Add(curItem.Trim());
                curItem = "";
            }

            continue;
        }

        curItem += c;
    }


    return set;
}

public static class HashSetExtensions
{
    public static HashSet<HashSet<T>> PowerSet<T>(this List<T> originalSet)
    {
        HashSet<HashSet<T>> sets = [];
        // get all subsets
        for (int i = 0; i < (1 << originalSet.Count); i++)
        {
            List<T> list = [];
            for (int j = 0; j < originalSet.Count; j++)
            {
                if ((i & (1 << j)) != 0)
                {
                    list.Add(originalSet[j]);
                }
            }

            sets.Add([..list]);
        }
        return sets;
    }

    public static HashSet<HashSet<T>> PowerSetOfWhereNumber<T>(this List<T> originalSet, int m)
    {
        HashSet<HashSet<T>> sets = [];
        // get all subsets
        for (int i = 0; i < (1 << originalSet.Count); i++)
        {
            List<T> list = [];
            for (int j = 0; j < originalSet.Count; j++)
            {
                if ((i & (1 << j)) != 0)
                {
                    list.Add(originalSet[j]);
                }
            }

            if (list.Count == m)
            {
                sets.Add([..list]);
            }
        }
        return sets;
    }
    
    public static void ShowResult(this HashSet<string> setResult)
    {
        AnsiConsole.Markup("{");
        bool first = true;
        foreach (var resItem in setResult)
        {
            if (!first)
            {
                AnsiConsole.Markup(",");
            }

            first = false;
            AnsiConsole.Markup($"[green]{resItem}[/]");
        }

        AnsiConsole.Markup("}");
    }
    
    public static void ShowResult(this HashSet<HashSet<string>> setResult)
    {
        AnsiConsole.Markup("{");
        bool first = true;
        foreach (var resItem in setResult)
        {
            if (!first)
            {
                AnsiConsole.Markup(",");
            }

            first = false;
            resItem.ShowResult();
        }

        AnsiConsole.Markup("}");
    }
}