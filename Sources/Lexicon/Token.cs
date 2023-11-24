using Sources.ValueObjects;

namespace Sources.Lexicon;

public class Token
{
  private TokenType _type;
  private string? _stringValue;
  
  public Token(TokenType type, string stringValue)
  {
    _type = type;
    _stringValue = stringValue;
  }

  public Token()
  {
    
  }

  public override string ToString()
  {
    return $"Token [TYPE={_type}, STRING_VALUE={_stringValue}]";
  }
}
