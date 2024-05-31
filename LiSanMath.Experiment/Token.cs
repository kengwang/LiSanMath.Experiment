namespace LiSanMath.Experiment;

public class Token
{
    public bool Reverse { get; set; }
    public int TokenIndex { get; set; }
    public string RawToken { get; set; }
    
    public TokenCompoundType CompoundType { get; set; }
    
    public Token(string rawToken)
    {
        RawToken = rawToken;
    }
    
    public virtual bool Calculate(List<bool> table)
    {
        var intermediate = table[TokenIndex];
        return Reverse ? !intermediate : intermediate;
    }
}