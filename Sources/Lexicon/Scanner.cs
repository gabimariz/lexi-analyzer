using System.Text.RegularExpressions;
using Sources.Exceptions;
using Sources.ValueObjects;

namespace Sources.Lexicon;

public class Scanner
{
  private readonly char[]? _content;
  private int _state;
  private int _position;

  public Scanner(string filename)
  {
    try
    {
      var stringContent = File.ReadAllText(filename);
      
      Console.WriteLine("DEBUG ---------------------");
      Console.WriteLine(stringContent);
      Console.WriteLine("---------------------------");

      _content = stringContent.ToCharArray();
      _position = 0;
    }
    catch (Exception err)
    {
      Console.WriteLine(err);
    }
  }

  public Token NextToken()
  {
    string term = "";

    if (IsEof())
      return null!;

    _state = 0;
    while (true)
    {
      var currentCharacter = NextCharacter();

      Token token;
      switch (_state)
      {
        case 0:
          if (IsCharacter(currentCharacter))
          {
            term += currentCharacter;
            _state = 1;
          }
          else if (IsDigit(currentCharacter))
          {
            _state = 3;
            term += currentCharacter;
          }
          else if (IsSpace(currentCharacter))
          {
            _state = 0;
          }
          else if (IsOperator(currentCharacter))
          {
            _state = 5;
          }
          else
          {
            throw new LexicalException("Unrecognized Symbol");
          }

          break;
        case 1:
          if (IsCharacter(currentCharacter) || IsDigit(currentCharacter))
          {
            _state = 1;
            term += currentCharacter;
          }
          else if (IsSpace(currentCharacter) || IsOperator(currentCharacter))
          {
            _state = 2;
          }
          else
          {
            throw new LexicalException("Malformed Identifier");
          }

          break;
        case 2:
          Back();
          token = new(TokenType.Identifier, term);

          return token;
        case 3:
          if (IsDigit(currentCharacter))
          {
            _state = 3;
            term += currentCharacter;
          }
          else if (!IsCharacter(currentCharacter))
          {
            _state = 4;
          }
          else
          {
            throw new LexicalException("Unrecognized Number");
          }

          break;
        case 4:
          token = new(TokenType.Number, term);
          Back();

          return token;
        case 5:
          term += currentCharacter;
          token = new(TokenType.Operator, term);

          return token;
      }
    }
  }

  private bool IsCharacter(char character) => Regex.IsMatch(character.ToString(), "[a-z|A-Z]");

  private bool IsDigit(char character) => Regex.IsMatch(character.ToString(), "[0-9]");

  private bool IsOperator(char character) => Regex.IsMatch(character.ToString(), "[>|<|=|!]");

  private bool IsSpace(char character) => Regex.IsMatch(character.ToString(), @"[\s|\t|\n|\r]");

  private bool IsEof() => _position == _content!.Length;
  
  private void Back() => _position--;

  private char NextCharacter() => _content![_position++];
}
