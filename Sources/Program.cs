using Sources.Exceptions;
using Sources.Lexicon;

var scanner = new Scanner("input.isi");

try
{
  Token token;
  
  do
  {
    token = scanner.NextToken();

    if (token is not null)
      Console.WriteLine(token);

  } while (token is not null);
}
catch (LexicalException err)
{
  Console.ForegroundColor = ConsoleColor.Red;
  Console.Error.WriteLine($"[ERROR]: {err.Message}");
  Console.ResetColor();
}
catch (Exception err)
{
  Console.ForegroundColor = ConsoleColor.Red;
  Console.Error.WriteLine($"Generic [ERROR]: {err.Message}");
  Console.ResetColor();
}
