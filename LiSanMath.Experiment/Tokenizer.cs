using System.Text.RegularExpressions;

namespace LiSanMath.Experiment;

public static class Tokenizer
{
    public static Dictionary<char, TokenCompoundType> CompoundTokens = new()
    {
        { '&', TokenCompoundType.And },
        { '|', TokenCompoundType.Or },
        { '-', TokenCompoundType.Implication },
        { '+', TokenCompoundType.Equivalence },
        { '(', TokenCompoundType.LeftParenthesis },
        { ')', TokenCompoundType.RightParenthesis }
    };

    public static string TokenRegexStr = @"(\&|\||\-|\+|\(|\)|\!)";
    public static Regex TokenRegexRegex = new(TokenRegexStr);
    

    public static Token Tokenize(string rawExpression, List<string> elements)
    {
        List<TokenChain> targetTokenChains = [new TokenChain(rawExpression)];
        var rawTokenList = TokenRegexRegex.Split(rawExpression).ToList();
        var populateBracket = 0;
        var patternInBracket = "";
        var action = TokenCompoundType.Original;
        var isReverse = false;
        rawTokenList = rawTokenList.Where(x => !string.IsNullOrWhiteSpace(x)).Select(t=>t.Trim()).ToList();
        for (var rawTokenIndex = 0; rawTokenIndex < rawTokenList.Count; rawTokenIndex++)
        {
            var ans = rawTokenList[rawTokenIndex];

            // 判断是否为括号
            if (TokenRegexRegex.IsMatch(ans))
            {
                if (ans is "(")
                {
                    populateBracket++;
                    continue;
                }

                if (ans is ")")
                {
                    if (populateBracket <= 0)
                    {
                        throw new Exception("括号不匹配");
                    }

                    populateBracket--;
                    var bracketedToken = new TokenPart(patternInBracket);
                    bracketedToken.LoadFrom(patternInBracket, elements);
                    bracketedToken.Reverse = isReverse;
                    targetTokenChains.Last().Tokens.Add(bracketedToken);
                    isReverse = false;
                    patternInBracket = "";
                }
            }

            if (populateBracket >= 1)
            {
                patternInBracket += ans;
                continue;
            }

            if (TokenRegexRegex.IsMatch(ans))
            {
                if (ans != "!")
                {
                    if (ans is "+" or "-")
                    {
                        var nchain = new TokenChain(ans);
                        nchain.Tokens.Add(targetTokenChains.Last().Tokens.Last());
                        targetTokenChains.Last().Tokens.RemoveAt(targetTokenChains.Last().Tokens.Count - 1);
                        targetTokenChains.Add(nchain);
                    }
                    else
                    {
                        if (targetTokenChains.Count > 1)
                        {
                            targetTokenChains[^2].Tokens.Add(targetTokenChains.Last());
                            targetTokenChains.RemoveAt(targetTokenChains.Count - 1);
                        }
                    }
                    action = CompoundTokens[ans[0]];
                }
                else
                {
                    isReverse = !isReverse;
                }

                continue;
            }


            if (!elements.Contains(ans))
            {
                elements.Add(ans);
            }

            var index = elements.IndexOf(ans);
            var token = new Token(ans)
            {
                CompoundType = action,
                Reverse = isReverse,
                TokenIndex = index
            };
            targetTokenChains.Last().Tokens.Add(token);
            isReverse = false;
            
            
            while (targetTokenChains.Count > 1)
            {
                targetTokenChains[^2].Tokens.Add(targetTokenChains.Last());
                targetTokenChains.RemoveAt(targetTokenChains.Count - 1);
            }
        }
        
        while (targetTokenChains.Count > 1)
        {
            targetTokenChains[^2].Tokens.Add(targetTokenChains.Last());
            targetTokenChains.RemoveAt(targetTokenChains.Count - 1);
        }
        
        return targetTokenChains.Last();
    }
}

public enum TokenCompoundType
{
    // 原样
    Original,

    // 合取
    And,

    // 析取
    Or,

    // 蕴含
    Implication,

    // 等价
    Equivalence,

    // 左括号
    LeftParenthesis,

    // 右括号
    RightParenthesis
}