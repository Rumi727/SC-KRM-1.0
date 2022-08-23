using Brigadier.NET;
using Brigadier.NET.Exceptions;
using SCKRM.Renderer;
using SCKRM.Resource;
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
                throw CommandSyntaxException.BuiltInExceptions.ColonTooMany().CreateWithContext(reader, 1, count);
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
                throw CommandSyntaxException.BuiltInExceptions.ColonTooMany().CreateWithContext(reader, 1, count);
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
                throw CommandSyntaxException.BuiltInExceptions.ColonTooMany().CreateWithContext(reader, 2, count);
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
#pragma warning restore IDE0060 // 사용하지 않는 매개 변수를 제거하세요.
    }
}
