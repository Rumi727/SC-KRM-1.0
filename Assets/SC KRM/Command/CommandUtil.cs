using Brigadier.NET;
using Brigadier.NET.Exceptions;
using SCKRM.Renderer;
using SCKRM.Resource;
using SCKRM.Text;
using System;
using System.Text.RegularExpressions;

namespace SCKRM.Command
{
    public static class CommandLanguage
    {
        public static string SearchLanguage(string key, string nameSpace = "", string language = "")
        {
            string text = ResourceManager.SearchLanguage("command." + key, nameSpace, language);

            if (!string.IsNullOrEmpty(text))
                return text;
            else
                return "command." + key;
        }
    }

    public static class IStringReaderExpansion
    {
        public static NameSpacePathPair ReadNameSpacePathPair(this IStringReader reader)
        {
            string text = reader.ReadString().Replace(".", "/");
            MatchCollection matches = Regex.Matches(text, ":");
            int count = matches.Count;

            if (count == 1)
                return text;
            else if (count == 0)
            {
                if (reader.Cursor < reader.TotalLength && reader.Peek() == ':')
                {
                    reader.Cursor++;
                    return new NameSpacePathPair(text, reader.ReadString().Replace(".", "/"));
                }
                else
                    return new NameSpacePathPair(text);
            }
            else
                throw CommandSyntaxException.BuiltInExceptions.ColonTooMany().CreateWithContext(reader, count, 1);
        }

        public static NameSpaceTypePathPair ReadNameSpaceTypePathPair(this IStringReader reader)
        {
            string text = reader.ReadString().Replace(".", "/");
            MatchCollection matches = Regex.Matches(text, ":");
            int count = matches.Count;

            if (count == 1)
                return text;
            else if (count == 0)
            {
                if (reader.Cursor < reader.TotalLength && reader.Peek() == ':')
                {
                    reader.Cursor++;
                    return new NameSpaceTypePathPair(text, ResourceManager.GetTextureType(reader.ReadString().Replace(".", "/"), out string value), value);
                }
                else
                    return new NameSpaceTypePathPair(ResourceManager.GetTextureType(text, out string value), value);
            }
            else
                throw CommandSyntaxException.BuiltInExceptions.ColonTooMany().CreateWithContext(reader, count, 1);
        }

        public static NameSpaceIndexTypePathPair ReadNameSpaceIndexTypePathPair(this IStringReader reader)
        {
            string text = reader.ReadString().Replace(".", "/");
            MatchCollection matches = Regex.Matches(text, ":");
            int count = matches.Count;

            if (count == 2)
            {
                NameSpaceIndexTypePathPair result = text;
                if (result.index < 0)
                    throw CommandSyntaxException.BuiltInExceptions.ReaderInvalidInt().CreateWithContext(reader, text);

                return result;
            }
            else if (count == 1)
            {
                if (reader.Cursor < reader.TotalLength && reader.Peek() == ':')
                {
                    reader.Cursor++;

                    string nameSpace = ResourceManager.GetNameSpace(text, out string value);
                    int index;
                    if (int.TryParse(value, out int result))
                        index = result;
                    else
                    {
                        if (value.Length == 0)
                            throw CommandSyntaxException.BuiltInExceptions.ReaderExpectedInt().CreateWithContext(reader);
                        else
                            throw CommandSyntaxException.BuiltInExceptions.ReaderInvalidInt().CreateWithContext(reader, value);
                    }

                    string type = ResourceManager.GetTextureType(reader.ReadString().Replace(".", "/"), out value);
                    return new NameSpaceIndexTypePathPair(nameSpace, index, type, value);
                }
                else
                    return text;
            }
            else if (count == 0)
            {
                string nameSpace = text;

                if (reader.Cursor < reader.TotalLength && reader.Peek() == ':')
                {
                    reader.Cursor++;

                    string typePath = reader.ReadString().Replace(".", "/");
                    if (reader.Cursor < reader.TotalLength && reader.Peek() == ':')
                    {
                        reader.Cursor -= typePath.Length;
                        int index = reader.ReadInt();
                        reader.Cursor++;

                        typePath = ResourceManager.GetTextureType(reader.ReadString().Replace(".", "/"), out string value);
                        return new NameSpaceIndexTypePathPair(nameSpace, index, typePath, value);
                    }
                    else
                    {
                        if (typePath.Contains(':'))
                        {
                            string indexText = ResourceManager.GetNameSpace(typePath, out typePath);
                            int index;
                            if (int.TryParse(indexText, out int result))
                                index = result;
                            else
                            {
                                if (indexText.Length == 0)
                                    throw CommandSyntaxException.BuiltInExceptions.ReaderExpectedInt().CreateWithContext(reader);
                                else
                                    throw CommandSyntaxException.BuiltInExceptions.ReaderInvalidInt().CreateWithContext(reader, typePath);
                            }

                            return new NameSpaceIndexTypePathPair(nameSpace, index, ResourceManager.GetTextureType(typePath, out string value), value);
                        }
                        else
                            return new NameSpaceIndexTypePathPair(nameSpace, ResourceManager.GetTextureType(typePath, out string value), value);
                    }
                }
                else
                    return new NameSpaceIndexTypePathPair(ResourceManager.GetTextureType(text, out string value), value);
            }
            else
                throw CommandSyntaxException.BuiltInExceptions.ColonTooMany().CreateWithContext(reader, count, 2);
        }

        public static Arguments.PosSwizzle ReadPosSwizzle(this IStringReader reader)
        {
            Arguments.PosSwizzle posSwizzle = Arguments.PosSwizzle.none;

            Check();
            reader.Cursor++;
            Check();
            reader.Cursor++;
            Check();

            return posSwizzle;

            void Check()
            {
                if (reader.Peek() == 'x')
                {
                    if (!posSwizzle.HasFlag(Arguments.PosSwizzle.x))
                    {
                        if (!posSwizzle.HasFlag(Arguments.PosSwizzle.none))
                            posSwizzle |= Arguments.PosSwizzle.x;
                        else
                            posSwizzle = Arguments.PosSwizzle.x;
                    }
                    else
                        CommandSyntaxException.BuiltInExceptions.InvalidPosSwizzle();
                }
                else if (reader.Peek() == 'y')
                {
                    if (!posSwizzle.HasFlag(Arguments.PosSwizzle.y))
                    {
                        if (!posSwizzle.HasFlag(Arguments.PosSwizzle.none))
                            posSwizzle |= Arguments.PosSwizzle.y;
                        else
                            posSwizzle = Arguments.PosSwizzle.y;
                    }
                    else
                        CommandSyntaxException.BuiltInExceptions.InvalidPosSwizzle();
                }
                else if (reader.Peek() == 'z')
                {
                    if (posSwizzle.HasFlag(Arguments.PosSwizzle.z))
                    {
                        if (!posSwizzle.HasFlag(Arguments.PosSwizzle.none))
                            posSwizzle |= Arguments.PosSwizzle.z;
                        else
                            posSwizzle = Arguments.PosSwizzle.z;
                    }
                    else
                        CommandSyntaxException.BuiltInExceptions.InvalidPosSwizzle();
                }
                else
                    CommandSyntaxException.BuiltInExceptions.InvalidPosSwizzle();
            }
        }
    }

    public static class BuiltInExceptionsExpansion
    {
#pragma warning disable IDE0060 // 사용하지 않는 매개 변수를 제거하세요.
        public static Dynamic2CommandExceptionType ColonTooMany(this IBuiltInExceptionProvider e)
        {
            return new Dynamic2CommandExceptionType((found, max) =>
            {
                string message = CommandLanguage.SearchLanguage("colon_too_many").Replace("%found%", found.ToString()).Replace("%max%", max.ToString());
                return new LiteralMessage(message);
            });
        }

        public static SimpleCommandExceptionType InvalidPosSwizzle(this IBuiltInExceptionProvider e)
        {
            string message = CommandLanguage.SearchLanguage("reader_invalid_pos_swizzle");
            return new SimpleCommandExceptionType(new LiteralMessage(message));
        }
#pragma warning restore IDE0060 // 사용하지 않는 매개 변수를 제거하세요.

        public static CommandSyntaxException GetCustomException(this CommandSyntaxException exception)
        {
            exception.CustomMessage = GetCustomExceptionMessage(exception);
            return exception;
        }

        static readonly FastString exceptionFastString = new FastString();
        public static string GetCustomExceptionMessage(this CommandSyntaxException exception)
        {
            string here = CommandLanguage.SearchLanguage("context.here");
            string parseError = CommandLanguage.SearchLanguage("context.parse_error");
            string message = exception.RawMessage().String;

            parseError = parseError.Replace("%message%", message);
            parseError = parseError.Replace("%pos%", exception.Cursor.ToString());

            if (exception.Input == null || exception.Cursor < 0)
                return message;

            exceptionFastString.Clear();
            int num = exception.Input.Length.Min(exception.Cursor);
            if (num > CommandSyntaxException.ContextAmount)
                exceptionFastString.Append("...");

            int num2 = 0.Max(num - CommandSyntaxException.ContextAmount);
            exceptionFastString.Append(exception.Input.Substring(num2, num - num2));
            exceptionFastString.Append(here);

            return parseError.Replace("%text%", exceptionFastString.ToString());
        }
    }
}
