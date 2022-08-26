using Brigadier.NET;
using Brigadier.NET.Context;
using Brigadier.NET.Exceptions;
using Brigadier.NET.Suggestion;
using Brigadier.NET.Tree;
using SCKRM.Object;
using SCKRM.Text;
using SCKRM.UI;
using SCKRM.UI.Layout;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace SCKRM.Command
{
    public sealed class CommandIntelliSense : UI.UI
    {
        [SerializeField] TMP_InputField chatInputField;

        [SerializeField] TargetSizeFitter backgroundTargetSizeFitter;

        [SerializeField] RectTransform empty;

        [SerializeField] RectTransform autocomplete;
        [SerializeField] TargetSizeFitter autocompleteTargetSizeFitter;
        [SerializeField] Transform autocompleteContent;
        [SerializeField] ChildSizeFitter autocompleteContentChildSizeFitter;

        [SerializeField] RectTransform description;
        [SerializeField] TMP_Text descriptionText;
        [SerializeField] TargetSizeFitter descriptionTargetSizeFitter;
        [SerializeField] BetterContentSizeFitter descriptionTextBetterContentSizeFitter;

        protected override void OnEnable() => tempCaretPosition = -1;

        int tempCaretPosition = -1;
        void Update()
        {
            if (!InitialLoadManager.isInitialLoadEnd)
                return;

            if (tempCaretPosition != chatInputField.caretPosition)
            {
                IntelliSense(chatInputField.text);
                tempCaretPosition = chatInputField.caretPosition;
            }

            descriptionTextBetterContentSizeFitter.max = new Vector2(rectTransform.rect.width, descriptionTextBetterContentSizeFitter.max.y);
        }

        static readonly FastString descriptionFastString = new FastString();
        public async void IntelliSense(string allInput)
        {
            string input = allInput;
            if (input.Length > 0 && chatInputField.caretPosition < allInput.Length)
                input = input.Remove(chatInputField.caretPosition.Clamp(0, allInput.Length));

            Debug.Log(input);

            CommandManager.defaultCommandSource.Initialization();

            CommandDispatcher<DefaultCommandSource> commandDispatcher = CommandManager.commandDispatcher;
            RootCommandNode<DefaultCommandSource> root = commandDispatcher.GetRoot();
            ParseResults<DefaultCommandSource> allTextParseResults = commandDispatcher.Parse(allInput, CommandManager.defaultCommandSource);
            ICollection<CommandNode<DefaultCommandSource>> rootNodes = root.Children;

            descriptionText.text = "";

            IDictionary<CommandNode<DefaultCommandSource>, CommandSyntaxException> exceptions = allTextParseResults.Exceptions;
            if (allTextParseResults.Exceptions.Count > 0)
            {
                LiteralObjectRemove();
                descriptionFastString.Clear();

                var lastException = exceptions.Last();
                foreach (var exception in exceptions)
                {
                    descriptionFastString.Append(exception.Value.GetCustomExceptionMessage());

                    if (!exception.Equals(lastException))
                        descriptionFastString.Append("\n");
                }

                descriptionText.text = descriptionFastString.ToString();
            }
            else
            {
                CommandManager.defaultCommandSource.Initialization();

                ParseResults<DefaultCommandSource> parseResults = commandDispatcher.Parse(input, CommandManager.defaultCommandSource);
                Suggestions suggestions = await commandDispatcher.GetCompletionSuggestions(parseResults);

                if (suggestions.List.Count > 0)
                    LiteralObjectCreate(suggestions.List);
                else if (parseResults.Context.Nodes.Count > 0)
                {
                    LiteralObjectRemove();
                    commandDispatcher.GetSmartUsage(parseResults.Context.Nodes.Last().Node, CommandManager.defaultCommandSource);
                }


                /*CommandContextBuilder<DefaultCommandSource> currentContext = parseResults.Context.LastChild;
                if (currentContext.Nodes.Count > 0)
                {
                    
                    /*CommandNode<DefaultCommandSource> node = currentContext.Nodes.Last().Node;
                    while (node.Redirect != null)
                        node = node.Redirect;

                    LiteralCommandNode<DefaultCommandSource>[] literalNodes = node.Children.OfType<LiteralCommandNode<DefaultCommandSource>>().ToArray();
                    LiteralObjectCreate(literalNodes, input);

                    if (autocompleteTextList.Count <= 0)
                    {
                        descriptionFastString.Clear();

                        IDictionary<CommandNode<DefaultCommandSource>, string> usages = commandDispatcher.GetSmartUsage(node, CommandManager.defaultCommandSource);
                        if (usages.Count > 0)
                        {
                            KeyValuePair<CommandNode<DefaultCommandSource>, string> lastUsage = usages.Last();
                            foreach (var usage in usages)
                            {
                                descriptionFastString.Append(usage.Value);

                                if (!usage.Equals(lastUsage))
                                    descriptionFastString.Append("\n");
                            }

                            descriptionText.text = descriptionFastString.ToString();
                        }
                    }
                }
                else
                    LiteralObjectCreate(rootNodes, input);*/
            }

            if (autocompleteTextList.Count > 0)
            {
                autocomplete.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, autocompleteMaxSizeX + 16);
                backgroundTargetSizeFitter.targetRectTransforms[0] = autocomplete;
            }
            else if (descriptionText.text != "")
            {
                backgroundTargetSizeFitter.targetRectTransforms[0] = description;

                descriptionTextBetterContentSizeFitter.SetLayoutHorizontal();
                descriptionTextBetterContentSizeFitter.SetLayoutVertical();

                descriptionTargetSizeFitter.LayoutRefresh();
                descriptionTargetSizeFitter.SizeUpdate();
            }
            else
                backgroundTargetSizeFitter.targetRectTransforms[0] = empty;
        }

        readonly List<CommandAutocompleteText> autocompleteTextList = new List<CommandAutocompleteText>();
        void LiteralObjectRemove()
        {
            for (int i = 0; i < autocompleteTextList.Count; i++)
                autocompleteTextList[i].Remove();

            autocompleteTextList.Clear();
            autocompleteMaxSizeX = 0;
        }

        void LiteralObjectCreate(List<Suggestion> texts)
        {
            LiteralObjectRemove();

            for (int i = 0; i < texts.Count; i++)
            {
                string text = texts[i].Text;
                CommandAutocompleteText autocompleteText = (CommandAutocompleteText)ObjectPoolingSystem.ObjectCreate("command.autocomplete_text", autocompleteContent).monoBehaviour;
                autocompleteText.text.text = text;

                autocompleteText.betterContentSizeFitter.SetLayoutHorizontal();
                autocompleteText.betterContentSizeFitter.SetLayoutVertical();

                autocompleteMaxSizeX = autocompleteMaxSizeX.Max(autocompleteText.rectTransform.rect.size.x);
                autocompleteTextList.Add(autocompleteText);
            }

            autocompleteContentChildSizeFitter.LayoutRefresh();
            autocompleteContentChildSizeFitter.SizeUpdate();

            autocompleteTargetSizeFitter.LayoutRefresh();
            autocompleteTargetSizeFitter.SizeUpdate();
        }

        float autocompleteMaxSizeX = 0;
        void LiteralObjectCreate(IEnumerable<CommandNode<DefaultCommandSource>> literalNodes, string input)
        {
            LiteralObjectRemove();

            foreach (var node in literalNodes)
            {
                string[] inputSplit = input.Split(' ');
                if (!node.Name.StartsWith(inputSplit[inputSplit.Length - 1]))
                    continue;

                CommandAutocompleteText autocompleteText = (CommandAutocompleteText)ObjectPoolingSystem.ObjectCreate("command.autocomplete_text", autocompleteContent).monoBehaviour;
                autocompleteText.text.text = node.Name;

                autocompleteText.betterContentSizeFitter.SetLayoutHorizontal();
                autocompleteText.betterContentSizeFitter.SetLayoutVertical();

                autocompleteMaxSizeX = autocompleteMaxSizeX.Max(autocompleteText.rectTransform.rect.size.x);
                autocompleteTextList.Add(autocompleteText);
            }

            autocompleteContentChildSizeFitter.LayoutRefresh();
            autocompleteContentChildSizeFitter.SizeUpdate();

            autocompleteTargetSizeFitter.LayoutRefresh();
            autocompleteTargetSizeFitter.SizeUpdate();
        }
    }
}
