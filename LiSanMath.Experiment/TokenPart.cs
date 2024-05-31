namespace LiSanMath.Experiment;

public class TokenPart : Token
{
    public List<Token> Tokens { get; set; } = [];
    public TokenPart(string rawToken) : base(rawToken)
    {
        
    }

    public void LoadFrom(string rawToken, List<string> elements)
    {
        Tokens = (Tokenizer.Tokenize(rawToken, elements) as TokenChain)?.Tokens!;
    }

    public override bool Calculate(List<bool> table)
    {
        var result = false;
        if (Tokens is not {Count: > 0}) return result;
        foreach (var token in Tokens)
        {
            var curResult = token.Calculate(table);
            result = token.CompoundType switch
            {
                TokenCompoundType.And => result && curResult,
                TokenCompoundType.Or => result || curResult,
                TokenCompoundType.Implication => !result || curResult,
                TokenCompoundType.Equivalence => result == curResult,
                TokenCompoundType.Original => curResult,
                _ => result
            };
        }

        return Reverse ? !result : result;
    }
}