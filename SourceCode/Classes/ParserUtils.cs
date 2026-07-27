namespace LCSoft.Framework.Scripting.Classes
{
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Core.Extensions;
    using LCSoft.Framework.Scripting.Exceptions.ParseTime;
    using LCSoft.Framework.Scripting.Exceptions.ParseTime.Unknown;
    using LCSoft.Framework.Scripting.ScriptContent;
    using LCSoft.Framework.Scripting.ScriptContent.ScriptExpression;
    using LCSoft.Framework.Scripting.ScriptContent.ScriptExpression.ScriptConnectorExpression;
    using LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent;
    using LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent.ScriptCommand;
    using LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent.ScriptCommand.ScriptConditionalCommand;
    using LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent.ScriptCommand.ScriptExpressionCommand;
    using LCSoft.Framework.Scripting.ValueContainer;
    using LCSoft.Framework.Scripting.ValueContainer.LiteralValueContainer;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Numerics;
    using System.Text;

    //reserved chars:
    // " String
    // ' Char
    // @ DateTime
    // % TimeSpan
    // [] ListOfValues (Locked, Protected)
    // , List separator
    // :Token :( :Token( Object notation
    // . Decimal point 
    // () list of values, properties, ...
    // {} List of commands
    // ; command terminador
    // > < <= => == != <> compareres
    // && || ^^ !! logical operators
    // & | ^ ! bit-wise operators
    // + - * / arithemetic operators
    // # comment
    // = atribuição

    internal static class ParserUtils
    {
        internal static (Boolean IsLiteral, LiteralValueContainerBase Literal) ParseDispatchLiteralValueContainer(Boolean IsExpression, BookmarkableFileReader BFR, String ParserPath)
        {
            Int32 Char = BFR.Peek();
            if (Char == '\"')
            {
                BFR.Advance();
                return (true, new StringLiteralValueContainer(GetString(BFR, ParserPath)));
            }
            if (Char == '\'')
            {
                BFR.Advance();
                return (true, new CharLiteralValueContainer(GetChar(BFR, ParserPath)));
            }
            if (Char == '@')
            {
                BFR.Advance();
                return (true, new DateTimeLiteralValueContainer(GetDateTime(BFR, ParserPath)));
            }
            if (Char == '%')
            {
                BFR.Advance();
                return (true, new TimeSpanLiteralValueContainer(GetTimeSpan(BFR, ParserPath)));
            }
            if (IsStartingNumberChar(Char))
            {
                (Int64? Integer, Double? NonInteger) = GetNumber(BFR, ParserPath);
                if (Integer != null)
                {
                    return (true, new Int64LiteralValueContainer(Integer.Value));
                }
                return (true, new DoubleLiteralValueContainer(NonInteger.Value));
            }

            BFR.CreateBookmark();

            String Token = GetToken(BFR);
            switch (Token)
            {
                case "True":
                    BFR.DiscardBookmark();
                    return (true, ValueContainerBase.True);
                case "False":
                    BFR.DiscardBookmark();
                    return (true, ValueContainerBase.False);
                case "NullString":
                    BFR.DiscardBookmark();
                    return (true, ValueContainerBase.NullString);
                case "ZeroInt32":
                    BFR.DiscardBookmark();
                    return (true, ValueContainerBase.ZeroInt32);
                case "MinusOneInt32":
                    BFR.DiscardBookmark();
                    return (true, ValueContainerBase.MinusOneInt32);
                case "ZeroSingle":
                    BFR.DiscardBookmark();
                    return (true, ValueContainerBase.ZeroSingle);
                case "MinusOneSingle":
                    BFR.DiscardBookmark();
                    return (true, ValueContainerBase.MinusOneSingle);
                case "Empty":
                case "Nothing":
                case "_":
                    BFR.DiscardBookmark();
                    return (true, ValueContainerBase.Empty);
            }

            BFR.RestoreBookmark();

            if (IsExpression)
            {
                return (false, null);
            }
            if (Token == "")
            {
                return (false, null);
            }
            throw new LiteralUnknownException(Token);
        }
        internal static ScriptCommandBase ParseDispatchCommand(BookmarkableFileReader BFR)
        {
            BFR.CreateBookmark();

            String Token = GetToken(BFR);
            if (Token == "")
            {
                BFR.DiscardBookmark();
                return null;
            }

            switch (Token)
            {
                #region If
                case IfScriptConditionalCommand.ComponentName:
                    BFR.DiscardBookmark();
                    return IfScriptConditionalCommand.Parse(BFR, false);
                #endregion
                #region Run
                case RunScriptCommand.ComponentName:
                    BFR.DiscardBookmark();
                    return RunScriptCommand.Parse(BFR);
                case RunAlternatedScriptCommand.ComponentName:
                    BFR.DiscardBookmark();
                    return RunAlternatedScriptCommand.Parse(BFR);
                case RunOnlyFirstScriptCommand.ComponentName:
                    BFR.DiscardBookmark();
                    return RunOnlyFirstScriptCommand.Parse(BFR);
                case RunSkipFirstScriptCommand.ComponentName:
                    BFR.DiscardBookmark();
                    return RunSkipFirstScriptCommand.Parse(BFR);
                #endregion
                #region Iteration
                case DoWhileScriptCommand.ComponentName:
                    BFR.DiscardBookmark();
                    return DoWhileScriptCommand.Parse(BFR);
                case DoUntilScriptCommand.ComponentName:
                    BFR.DiscardBookmark();
                    return DoUntilScriptCommand.Parse(BFR);
                case ForEachScriptCommand.ComponentName:
                    BFR.DiscardBookmark();
                    return ForEachScriptCommand.Parse(BFR);
                case RepeatScriptCommand.ComponentName:
                    BFR.DiscardBookmark();
                    return RepeatScriptCommand.Parse(BFR);
                case WhileScriptCommand.ComponentName:
                    BFR.DiscardBookmark();
                    return WhileScriptCommand.Parse(BFR);
                case UntilScriptCommand.ComponentName:
                    BFR.DiscardBookmark();
                    return UntilScriptCommand.Parse(BFR);
                #endregion
                #region OnError
                case OnErrorScriptCommand.ComponentName:
                    BFR.DiscardBookmark();
                    return OnErrorScriptCommand.Parse(BFR);
                case OnErrorIgnoreScriptCommand.ComponentName:
                    BFR.DiscardBookmark();
                    return OnErrorIgnoreScriptCommand.Parse(BFR);
                #endregion
                #region Continue
                case ContinueScriptCommand.ComponentName:
                    BFR.DiscardBookmark();
                    return ContinueScriptCommand.Parse(BFR);
                case ContinueIfScriptCommand.ComponentName:
                    BFR.DiscardBookmark();
                    return ContinueIfScriptCommand.Parse(BFR);
                #endregion
                #region Break
                case BreakScriptCommand.ComponentName:
                    BFR.DiscardBookmark();
                    return BreakScriptCommand.Parse(BFR);
                case BreakIfScriptCommand.ComponentName:
                    BFR.DiscardBookmark();
                    return BreakIfScriptCommand.Parse(BFR);
                #endregion
                #region Exit
                case ExitScriptCommand.ComponentName:
                    BFR.DiscardBookmark();
                    return ExitScriptCommand.Parse(BFR);
                case ExitIfScriptCommand.ComponentName:
                    BFR.DiscardBookmark();
                    return ExitIfScriptCommand.Parse(BFR);
                #endregion
                #region Return
                case ReturnScriptCommand.ComponentName:
                    BFR.DiscardBookmark();
                    return ReturnScriptCommand.Parse(BFR);
                case ReturnIfScriptCommand.ComponentName:
                    BFR.DiscardBookmark();
                    return ReturnIfScriptCommand.Parse(BFR);
                #endregion
                #region Fail
                case FailScriptCommand.ComponentName:
                    BFR.DiscardBookmark();
                    return FailScriptCommand.Parse(BFR);
                case FailIfScriptCommand.ComponentName:
                    BFR.DiscardBookmark();
                    return FailIfScriptCommand.Parse(BFR);
                #endregion
                #region Other
                case DebuggerScriptCommand.ComponentName:
                    BFR.DiscardBookmark();
                    return DebuggerScriptCommand.Parse(BFR);
                case DebuggerIfScriptCommand.ComponentName:
                    BFR.DiscardBookmark();
                    return DebuggerIfScriptCommand.Parse(BFR);
                case SwapScriptCommand.ComponentName:
                    BFR.DiscardBookmark();
                    return SwapScriptCommand.Parse(BFR);
                case SwapIfScriptCommand.ComponentName:
                    BFR.DiscardBookmark();
                    return SwapIfScriptCommand.Parse(BFR);
                    #endregion
            }

            BFR.RestoreBookmark();
            return null;
        }
        internal static ScriptConnectorExpressionBase ParseDispatchConnectorExpression(IScriptExpression Node, BookmarkableFileReader BFR, String ParserPath)
        {
            IgnoreWhiteSpacesAndComments(BFR);

            if (!IsConnectorExpressionChar(BFR.Peek()))
            {
                return null;
            }

            BFR.CreateBookmark();

            String Connector = GetConnectorExpression(BFR);

            switch (Connector)
            {
                case ScriptCommandBase.SetComponentName:
                case ScriptCommandBase.SetMissingComponentName:
                case ScriptCommandBase.SetEmptyComponentName:
                case ScriptCommandBase.SetMissingEmptyComponentName:
                case ScriptCommandBase.SelfIncrementComponentName:
                case ScriptCommandBase.SelfDecrementComponentName:
                case ScriptCommandBase.SelfAddComponentName:
                case ScriptCommandBase.SelfSubtractComponentName:
                case ScriptCommandBase.SelfAndComponentName:
                case ScriptCommandBase.SelfOrComponentName:
                case ScriptCommandBase.SelfXOrComponentName:
                case ScriptCommandBase.SelfXAndComponentName:
                    BFR.RestoreBookmark();
                    return null;

                case AddScriptConnectorExpression.ComponentName1:
                    BFR.DiscardBookmark();
                    IgnoreWhiteSpacesAndComments(BFR);
                    IScriptExpression Expression = ParseExpressionNode(BFR, ParserPath + " / + (Connector)");
                    return new AddScriptConnectorExpression(Node, Expression);
                case MultiplyScriptConnectorExpression.ComponentName1:
                    BFR.DiscardBookmark();
                    IgnoreWhiteSpacesAndComments(BFR);
                    Expression = ParseExpressionNode(BFR, ParserPath + " / * (Connector)");
                    return new MultiplyScriptConnectorExpression(Node, Expression);
                case SubtractScriptConnectorExpression.ComponentName1:
                    BFR.DiscardBookmark();
                    IgnoreWhiteSpacesAndComments(BFR);
                    Expression = ParseExpressionNode(BFR, ParserPath + " / + (Connector)");
                    return new SubtractScriptConnectorExpression(Node, Expression);
                case DivideScriptConnectorExpression.ComponentName1:
                    BFR.DiscardBookmark();
                    IgnoreWhiteSpacesAndComments(BFR);
                    Expression = ParseExpressionNode(BFR, ParserPath + " / - (Connector)");
                    return new DivideScriptConnectorExpression(Node, Expression);

                case EqualScriptConnectorExpression.ComponentName1:
                    BFR.DiscardBookmark();
                    IgnoreWhiteSpacesAndComments(BFR);
                    Expression = ParseExpressionNode(BFR, ParserPath + " == + (Connector)");
                    return new EqualScriptConnectorExpression(Node, Expression);
                case NotEqualScriptConnectorExpression.ComponentName1:
                    BFR.DiscardBookmark();
                    IgnoreWhiteSpacesAndComments(BFR);
                    Expression = ParseExpressionNode(BFR, ParserPath + " / !=, <> (Connector)");
                    return new NotEqualScriptConnectorExpression(Node, Expression);
                case LowerScriptConnectorExpression.ComponentName1:
                    BFR.DiscardBookmark();
                    IgnoreWhiteSpacesAndComments(BFR);
                    Expression = ParseExpressionNode(BFR, ParserPath + " / < (Connector)");
                    return new LowerScriptConnectorExpression(Node, Expression);
                case LowerOrEqualScriptConnectorExpression.ComponentName1:
                    BFR.DiscardBookmark();
                    IgnoreWhiteSpacesAndComments(BFR);
                    Expression = ParseExpressionNode(BFR, ParserPath + " / <=> (Connector)");
                    return new LowerOrEqualScriptConnectorExpression(Node, Expression);
                case GreaterScriptConnectorExpression.ComponentName1:
                    BFR.DiscardBookmark();
                    IgnoreWhiteSpacesAndComments(BFR);
                    Expression = ParseExpressionNode(BFR, ParserPath + " / > (Connector)");
                    return new GreaterScriptConnectorExpression(Node, Expression);
                case GreaterOrEqualScriptConnectorExpression.ComponentName1:
                    BFR.DiscardBookmark();
                    IgnoreWhiteSpacesAndComments(BFR);
                    Expression = ParseExpressionNode(BFR, ParserPath + " / >= (Connector)");
                    return new GreaterOrEqualScriptConnectorExpression(Node, Expression);

                case BothScriptConnectorExpression.ComponentName1:
                    BFR.DiscardBookmark();
                    IgnoreWhiteSpacesAndComments(BFR);
                    Expression = ParseExpressionNode(BFR, ParserPath + " / && (Connector)");
                    return new BothScriptConnectorExpression(Node, Expression);
                case AnyScriptConnectorExpression.ComponentName1:
                    BFR.DiscardBookmark();
                    IgnoreWhiteSpacesAndComments(BFR);
                    Expression = ParseExpressionNode(BFR, ParserPath + " / || (Connector)");
                    return new AnyScriptConnectorExpression(Node, Expression);
                case OneScriptConnectorExpression.ComponentName1:
                    BFR.DiscardBookmark();
                    IgnoreWhiteSpacesAndComments(BFR);
                    Expression = ParseExpressionNode(BFR, ParserPath + " / ^^ (Connector)");
                    return new OneScriptConnectorExpression(Node, Expression);
                case NoneScriptConnectorExpression.ComponentName1:
                    BFR.DiscardBookmark();
                    IgnoreWhiteSpacesAndComments(BFR);
                    Expression = ParseExpressionNode(BFR, ParserPath + " / !! (Connector)");
                    return new NoneScriptConnectorExpression(Node, Expression);

                case AndScriptConnectorExpression.ComponentName1:
                    BFR.DiscardBookmark();
                    IgnoreWhiteSpacesAndComments(BFR);
                    Expression = ParseExpressionNode(BFR, ParserPath + " / & (Connector)");
                    return new AndScriptConnectorExpression(Node, Expression);
                case OrScriptConnectorExpression.ComponentName1:
                    BFR.DiscardBookmark();
                    IgnoreWhiteSpacesAndComments(BFR);
                    Expression = ParseExpressionNode(BFR, ParserPath + " / | (Connector)");
                    return new OrScriptConnectorExpression(Node, Expression);
                case XOrScriptConnectorExpression.ComponentName1:
                    BFR.DiscardBookmark();
                    IgnoreWhiteSpacesAndComments(BFR);
                    Expression = ParseExpressionNode(BFR, ParserPath + " / ^ (Connector)");
                    return new XOrScriptConnectorExpression(Node, Expression);
                case XAndScriptConnectorExpression.ComponentName1:
                    BFR.DiscardBookmark();
                    IgnoreWhiteSpacesAndComments(BFR);
                    Expression = ParseExpressionNode(BFR, ParserPath + " / ! (Connector)");
                    return new XAndScriptConnectorExpression(Node, Expression);
            }

            throw new ConnectorExpressionUnknownException(Connector);
        }
        internal static IScriptExpression ParseDispatchExpression(BookmarkableFileReader BFR, String ParserPath)
        {
            IScriptExpression ShortcutExpression = ParseDispatchShortcutExpression(BFR, ParserPath);
            if (ShortcutExpression != null)
            {
                return ShortcutExpression;
            }

            (Boolean IsLiteral, LiteralValueContainerBase TokenValue) = ParseDispatchLiteralValueContainer(true, BFR, ParserPath);
            if (IsLiteral)
            {
                return new ConstantScriptExpression(TokenValue);
            }

            String Token = GetToken(BFR);
            AssertToken(Token, BFR.Peek(), ParserPath);

            switch (Token)
            {
                case CreateAssemblyClassScriptExpression.ComponentName:
                    return CreateAssemblyClassScriptExpression.Parse(BFR);
                case CreateAssemblyObjectScriptExpression.ComponentName:
                    return CreateAssemblyObjectScriptExpression.Parse(BFR);
                case CreateDelegateScriptExpression.ComponentName:
                    return CreateDelegateScriptExpression.Parse(BFR);
                case CreateFunctionScriptExpression.ComponentName:
                    return CreateFunctionScriptExpression.Parse(BFR);
                case CreateLambdaScriptExpression.ComponentName:
                    return CreateLambdaScriptExpression.Parse(BFR);
                case CreateLazyScriptExpression.ComponentName:
                    return CreateLazyScriptExpression.Parse(BFR);
                case CreateListScriptExpression.ComponentName:
                    return CreateListScriptExpression.ParseComplete(BFR);
                case CreateObjectScriptExpression.ComponentName:
                    return CreateObjectScriptExpression.Parse(BFR);
                case CreateSemaphoreScriptExpression.ComponentName:
                    return CreateSemaphoreScriptExpression.Parse(BFR);
                case CreateStringerScriptExpression.ComponentName:
                    return CreateStringerScriptExpression.Parse(BFR);
                case CreateTimerScriptExpression.ComponentName:
                    return CreateTimerScriptExpression.Parse(BFR);
                case CreateTypeScriptExpression.ComponentName:
                    return CreateTypeScriptExpression.Parse(BFR);
                case CreateVolatileScriptExpression.ComponentName:
                    return CreateVolatileScriptExpression.Parse(BFR);

                case SpawnScriptExpressionCommand.ComponentName:
                    return SpawnScriptExpressionCommand.Parse(BFR);

                case IIfScriptExpression.ComponentName:
                    return IIfScriptExpression.Parse(BFR);
            }

            IgnoreWhiteSpacesAndComments(BFR);
            switch (BFR.Peek())
            {
                case '(':
                    BFR.Advance();
                    return CallFunctionScriptExpressionCommand.Parse(Token, BFR);
                default:
                    return new GetVariableScriptExpression(Token);
            }
        }
        internal static IScriptExpression ParseDispatchShortcutExpression(BookmarkableFileReader BFR, String ParserPath)
        {
            switch (BFR.Peek())
            {
                case '(':
                    BFR.Read();

                    IgnoreWhiteSpacesAndComments(BFR);
                    IScriptExpression Expression = ParseExpression(BFR, ParserPath);

                    IgnoreWhiteSpacesAndComments(BFR);
                    ParserUtils.AssertChar(BFR.Read(), ')', ParserPath);

                    return Expression;
                case '[':
                    BFR.Read();
                    return CreateListScriptExpression.ParseShortcut(BFR);
            }

            return null;
        }

        internal static ScriptCommandBase ParseCommand(BookmarkableFileReader BFR, String ParserPath)
        {
            ParserPath += " / [Command]";

            ScriptCommandBase ScriptCommand = ParseDispatchCommand(BFR);
            if (ScriptCommand != null)
            {
                return ScriptCommand;
            }

            IScriptExpression ScriptExpression = ParseExpression(BFR, ParserPath);
            return ScriptExpression.ToCommand(BFR, ParserPath);
        }

        internal static Dictionary<String, LiteralValueContainerBase> ParseListOfInitializedTokens(BookmarkableFileReader BFR, String ParserPath)
        {
            Dictionary<String, LiteralValueContainerBase> InitializedTokens = new Dictionary<String, LiteralValueContainerBase>();

            IgnoreWhiteSpacesAndComments(BFR);
            while (!IsTerminatorOrEOF(BFR.Peek(), '}'))
            {
                (String Token, LiteralValueContainerBase Value) = GetInitializedToken(null, BFR, ParserPath);

                InitializedTokens.Add(Token, Value);

                IgnoreNonBreakingWhiteSpaces(BFR);
                ParserUtils.AssertStatmentTerminador(BFR, BFR.Peek(), ParserPath);
            }

            if (InitializedTokens.Count == 0)
            {
                return null;
            }
            return InitializedTokens;
        }
        internal static List<String> ParseListOfTokens(BookmarkableFileReader BFR, String ParserPath, String ItemName)
        {
            List<String> Tokens = new List<String>();

            RunButFirstSemaphore Semaphore = new RunButFirstSemaphore();
            while (!IsTerminatorOrEOF(BFR.Peek(), ')'))
            {
                if (Semaphore.Check())
                {
                    AssertChar(BFR.Read(), ',', ParserPath);
                    IgnoreWhiteSpacesAndComments(BFR);
                }

                String Token = GetToken(BFR);
                AssertToken(Token, BFR.Peek(), ParserPath + " / " + ItemName);

                Tokens.Add(Token);

                IgnoreWhiteSpacesAndComments(BFR);
            }

            if (Tokens.Count == 0)
            {
                return null;
            }
            return Tokens;
        }
        internal static List<IScriptExpression> ParseListOfExpressions(BookmarkableFileReader BFR, String ParserPath, Char Terminator)
        {
            List<IScriptExpression> Expressions = new List<IScriptExpression>();

            RunButFirstSemaphore Semaphore = new RunButFirstSemaphore();
            while (!IsTerminatorOrEOF(BFR.Peek(), Terminator))
            {
                if (Semaphore.Check())
                {
                    AssertChar(BFR.Read(), ',', ParserPath);
                    IgnoreWhiteSpacesAndComments(BFR);
                }

                IScriptExpression Expression = ParseExpression(BFR, ParserPath + " / [Expression]");
                AssertExpression(Expression, BFR.Peek(), ParserPath + " / [Expression]");

                Expressions.Add(Expression);

                IgnoreWhiteSpacesAndComments(BFR);
            }

            if (Expressions.Count == 0)
            {
                return null;
            }
            return Expressions;
        }

        internal static IScriptExpression ParseExpression(BookmarkableFileReader BFR, String ParserPath)
        {
            IScriptExpression Expression = ParseExpressionNode(BFR, ParserPath);

            IgnoreWhiteSpacesAndComments(BFR);
            ScriptConnectorExpressionBase Connector = ParseDispatchConnectorExpression(Expression, BFR, ParserPath);
            while (Connector != null)
            {
                Expression = Connector;

                IgnoreWhiteSpacesAndComments(BFR);
                Connector = ParseDispatchConnectorExpression(Expression, BFR, ParserPath);
            }

            return Expression;
        }
        internal static IScriptExpression ParseExpressionNode(BookmarkableFileReader BFR, String ParserPath)
        {
            IScriptExpression Expression = ParseDispatchExpression(BFR, ParserPath);

            IgnoreWhiteSpacesAndComments(BFR);
            while (BFR.Peek() == ':')
            {
                BFR.Read();

                IgnoreWhiteSpacesAndComments(BFR);
                Expression = ParseObjectNotationExpression(Expression, BFR);
            }

            return Expression;
        }
        internal static IScriptExpression ParseObjectNotationExpression(IScriptExpression Expression, BookmarkableFileReader BFR)
        {
            if (BFR.Peek() == '(')
            {
                BFR.Read();
                IgnoreWhiteSpacesAndComments(BFR);
                return GetItemScriptExpression.Parse(Expression, BFR);
            }

            String Token = GetToken(BFR);
            if (Token == null)
            {
                AssertToken(Token, BFR.Peek(), "ParserPath"); //TODO: Tech Debt: "ParserPath"
            }

            IgnoreWhiteSpacesAndComments(BFR);
            switch (BFR.Peek())
            {
                case '(':
                    BFR.Advance();
                    return CallMethodScriptExpressionCommand.Parse(Expression, Token, BFR);
                default:
                    return new GetPropertyScriptExpression(Expression, Token);
            }
        }

        internal static FunctionScriptRootContent ParseFunctionContent(String Function, BookmarkableFileReader BFR, String ParserPath)
        {
            String FunctionName;
            if (Function == null)
            {
                IgnoreWhiteSpacesAndComments(BFR);
                FunctionName = GetToken(BFR);
                AssertToken(FunctionName, BFR.Peek(), ParserPath + " / " + nameof(FunctionName) + " [Identifier]");
            }
            else
            {
                FunctionName = null;
            }

            IgnoreWhiteSpacesAndComments(BFR);
            AssertChar(BFR.Read(), '(', ParserPath);

            IgnoreWhiteSpacesAndComments(BFR);
            List<String> Parameters = ParseListOfTokens(BFR, ParserPath, "ParameterName");

            AssertChar(BFR.Read(), ')', ParserPath);

            IgnoreWhiteSpacesAndComments(BFR);
            List<ScriptCommandBase> CodeBlock = GetCodeBlock(BFR, ParserPath + " / " + nameof(CodeBlock) + " [Code Block]");

            return new FunctionScriptRootContent(FunctionName, Parameters, CodeBlock);
        }

        internal static String GetToken(BookmarkableFileReader BFR)
        {
            BFR.CreateBookmark();

            while (IsTokenChar(BFR.Peek()))
            {
                BFR.Advance();
            }

            return BFR.ReadDiscardFromBookmark();
        }
        internal static (String, LiteralValueContainerBase) GetInitializedToken(String Token, BookmarkableFileReader BFR, String ParserPath)
        {
            String TokenName = Token ?? GetToken(BFR);
            AssertToken(TokenName, BFR.Peek(), ParserPath + " / Name [Identifier]");

            IgnoreWhiteSpacesAndComments(BFR);
            AssertChar(BFR.Read(), '=', ParserPath + " / [Assignment]");

            IgnoreWhiteSpacesAndComments(BFR);
            (_, LiteralValueContainerBase TokenValue) = ParseDispatchLiteralValueContainer(false, BFR, ParserPath);

            return (TokenName, TokenValue);
        }
        internal static String GetString(BookmarkableFileReader BFR, String ParserPath)
        {
            StringBuilder SB = new StringBuilder();

            EscapeDetector ED = new EscapeDetector();
            Boolean IsEscaped = false;
            Int32 Char = BFR.Read();
            while (!IsTerminatorOrEOFOrEOL(Char, '"', IsEscaped))
            {
                Int32 EscapedChar = ED.NextChar(Char);
                if (EscapedChar == -2)
                {
                    throw new EscapeSequenceUnknownException("\\" + (Char)Char);
                }
                if (EscapedChar == -1)
                {
                    IsEscaped = true;
                }
                else
                {
                    IsEscaped = false;
                    SB.Append((Char)EscapedChar);
                }

                Char = BFR.Read();
            }

            AssertChar(Char, '"', ParserPath + " / StringLiteral");

            return SB.ToString();
        }
        internal static Char GetChar(BookmarkableFileReader BFR, String ParserPath)
        {
            Int32 Char = BFR.Read();
            AssertChar(Char, false, ParserPath + " / CharLiteral");

            EscapeDetector ED = new EscapeDetector();
            Int32 EscapedChar = ED.NextChar(Char);
            if (EscapedChar == EscapeDetector.BeginEscape)
            {
                EscapedChar = GetEscapedChar(ED, BFR);
            }

            AssertChar(BFR.Read(), '\'', "CharLiteral");

            return (Char)EscapedChar;
        }
        internal static Int32 GetEscapedChar(EscapeDetector ED, BookmarkableFileReader BFR)
        {
            Int32 Char = BFR.Read();
            AssertChar(Char, true, "CharParser");
            Int32 EscapedChar = ED.NextChar(Char);
            if (EscapedChar == EscapeDetector.InvalidEscape)
            {
                throw new EscapeSequenceUnknownException("\\" + (Char)Char);
            }

            return EscapedChar;
        }
        internal static (Int64?, Double?) GetNumber(BookmarkableFileReader BFR, String ParserPath)
        {
            BFR.CreateBookmark();

            if (BFR.Peek() == '-')
            {
                BFR.Advance();
            }

            Boolean HasDot = false;
            Boolean HasDigit = false;
            while (IsNumberChar(BFR.Peek()))
            {
                if (BFR.Peek() == '.')
                {
                    if (HasDot || !HasDigit)
                    {
                        throw new InvalidSintaxeException(ParserPath + " / NumberLiteral : Invalid number");
                    }
                    HasDot = true;
                    HasDigit = false;
                }
                else
                {
                    HasDigit = true;
                }
                BFR.Advance();
            }

            if (!HasDigit)
            {
                throw new InvalidSintaxeException(ParserPath + " / NumberLiteral : Invalid number");
            }

            String Number = BFR.ReadDiscardFromBookmark();
            if (HasDot)
            {
                return (null, Double.Parse(Number, CultureInfo.InvariantCulture));
            }
            return (Int64.Parse(Number), null);
        }
        internal static String GetLine(BookmarkableFileReader BFR)
        {
            BFR.CreateBookmark();

            AdvanceToEndOfLine(BFR);

            return BFR.ReadDiscardFromBookmark();
        }
        internal static List<ScriptCommandBase> GetCodeBlock(BookmarkableFileReader BFR, String ParserPath)
        {
            AssertChar(BFR.Read(), '{', ParserPath);

            List<ScriptCommandBase> Commands = new List<ScriptCommandBase>();

            IgnoreWhiteSpacesAndComments(BFR);
            while (!IsTerminatorOrEOF(BFR.Peek(), '}'))
            {
                ScriptCommandBase Command = ParseCommand(BFR, ParserPath);
                if (Command == null)
                {
                    throw new Exception("null command");
                }
                Commands.Add(Command);

                IgnoreWhiteSpacesAndComments(BFR);
            }

            AssertChar(BFR.Read(), '}', ParserPath);

            if (Commands.Count == 0)
            {
                return null;
            }
            return Commands;
        }
        internal static DateTime GetDateTime(BookmarkableFileReader BFR, String ParserPath)
        {
            StringBuilder SB = new StringBuilder();

            Int32 Char = BFR.Read();
            while (!IsTerminatorOrEOFOrEOL(Char, '@'))
            {
                if (!IsDigitChar(Char) || SB.Length == 17)
                {
                    throw new InvalidSintaxeException(ParserPath + " / DateTimeLiteral : Invalid date");
                }
                SB.Append((Char)Char);
                Char = BFR.Read();
            }

            switch (SB.Length)
            {
                case 8:
                    String Date = SB.ToString();
                    return new DateTime
                    (
                        Int32.Parse(Date.Substring(0, 4)),
                        Int32.Parse(Date.Substring(4, 2)),
                        Int32.Parse(Date.Substring(6, 2))
                    );
                case 14:
                    Date = SB.ToString();
                    return new DateTime
                    (
                        Int32.Parse(Date.Substring(0, 4)),
                        Int32.Parse(Date.Substring(4, 2)),
                        Int32.Parse(Date.Substring(6, 2)),
                        Int32.Parse(Date.Substring(8, 2)),
                        Int32.Parse(Date.Substring(10, 2)),
                        Int32.Parse(Date.Substring(12, 2))
                    );
                case 17:
                    Date = SB.ToString();
                    return new DateTime
                    (
                        Int32.Parse(Date.Substring(0, 4)),
                        Int32.Parse(Date.Substring(4, 2)),
                        Int32.Parse(Date.Substring(6, 2)),
                        Int32.Parse(Date.Substring(8, 2)),
                        Int32.Parse(Date.Substring(10, 2)),
                        Int32.Parse(Date.Substring(12, 2)),
                        Int32.Parse(Date.Substring(14, 3))
                    );
                default:
                    throw new InvalidSintaxeException(ParserPath + " / DateTimeLiteral : Invalid date");
            }
        }
        internal static TimeSpan GetTimeSpan(BookmarkableFileReader BFR, String ParserPath)
        {
            StringBuilder SB = new StringBuilder();

            Int32 Char = BFR.Read();
            while (!IsTerminatorOrEOFOrEOL(Char, '%'))
            {
                if (!IsDigitChar(Char))
                {
                    throw new InvalidSintaxeException(ParserPath + " / TimeSpanLiteral : Invalid time span");
                }
                SB.Append((Char)Char);
                Char = BFR.Read();
            }

            if (SB.Length < 9)
            {
                throw new InvalidSintaxeException(ParserPath + " / TimeSpanLiteral : Invalid time span");
            }
            String TimeSpan = SB.ToString();
            if (SB.Length == 9)
            {
                return new TimeSpan
                (
                    0,
                    Int32.Parse(TimeSpan.Substring(0, 2)),
                    Int32.Parse(TimeSpan.Substring(2, 2)),
                    Int32.Parse(TimeSpan.Substring(4, 2)),
                    Int32.Parse(TimeSpan.Substring(6, 3))
                );
            }
            Int32 TimeSpanLength = TimeSpan.Length;
            return new TimeSpan
            (
                Int32.Parse(TimeSpan.Substring(0, TimeSpanLength - 9)),
                Int32.Parse(TimeSpan.Substring(TimeSpanLength - 9, 2)),
                Int32.Parse(TimeSpan.Substring(TimeSpanLength - 7, 2)),
                Int32.Parse(TimeSpan.Substring(TimeSpanLength - 5, 2)),
                Int32.Parse(TimeSpan.Substring(TimeSpanLength - 3, 3))
            );
        }
        internal static String GetConnectorExpression(BookmarkableFileReader BFR)
        {
            BFR.CreateBookmark();

            while (IsConnectorExpressionChar(BFR.Peek()))
            {
                BFR.Advance();
            }

            return BFR.ReadDiscardFromBookmark();
        }
        internal static String GetConnectorCommand(BookmarkableFileReader BFR)
        {
            BFR.CreateBookmark();

            while (IsConnectorCommandChar(BFR.Peek()))
            {
                BFR.Advance();
            }

            return BFR.ReadDiscardFromBookmark();
        }
        internal static Int64 GetInt64(BookmarkableFileReader BFR, String ParserPath)
        {
            BFR.CreateBookmark();

            if (BFR.Peek() == '-')
            {
                BFR.Advance();
            }

            Boolean HasDigit = false;
            while (IsIntegralNumberChar(BFR.Peek()))
            {
                HasDigit = true;
                BFR.Advance();
            }

            if (!HasDigit)
            {
                throw new InvalidSintaxeException(ParserPath + " / NumberLiteral : Invalid number");
            }

            String Number = BFR.ReadDiscardFromBookmark();
            return Int64.Parse(Number);
        }

        internal static void IgnoreWhiteSpaces(BookmarkableFileReader BFR)
        {
            while (IsWhiteSpaceChar(BFR.Peek()))
            {
                BFR.Advance();
            }
        }
        internal static void IgnoreNonBreakingWhiteSpaces(BookmarkableFileReader BFR)
        {
            while (IsNonBreakingWhiteSpaceChar(BFR.Peek()))
            {
                BFR.Advance();
            }
        }
        internal static void IgnoreWhiteSpacesAndComments(BookmarkableFileReader BFR)
        {
            IgnoreWhiteSpaces(BFR);
            while (BFR.Peek() == '#')
            {
                AdvanceToEndOfLine(BFR);
                IgnoreWhiteSpaces(BFR);
            }
        }

        internal static Boolean IsWhiteSpaceChar(Int32 Char)
        {
            return Char.In
            (
                ' ', '\t', '\r', '\n'
            );
        }
        internal static Boolean IsNonBreakingWhiteSpaceChar(Int32 Char)
        {
            return Char.In
            (
                ' ', '\t'
            );
        }
        internal static Boolean IsTokenChar(Int32 Char)
        {
            return Char.In
            (
                '_',
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
                '1', '2', '3', '4', '5', '6', '7', '8', '9', '0'
            );
        }
        internal static Boolean IsTerminatorOrEOF(Int32 Char, Char Terminator)
        {
            return Char.In(BookmarkableFileReader.EndOfFile, Terminator);
        }
        internal static Boolean IsTerminatorOrEOFOrEOL(Int32 Char, Char Terminator)
        {
            return Char.In(BookmarkableFileReader.EndOfFile, BookmarkableFileReader.CarriageReturn, Terminator);
        }
        internal static Boolean IsTerminatorOrEOFOrEOL(Int32 Char, Char Terminator, Boolean IgnoreTerminator)
        {
            if (IgnoreTerminator)
            {
                return Char.In(BookmarkableFileReader.EndOfFile, BookmarkableFileReader.CarriageReturn);
            }

            return Char.In(BookmarkableFileReader.EndOfFile, BookmarkableFileReader.CarriageReturn, Terminator);
        }
        internal static Boolean IsStartingNumberChar(Int32 Char)
        {
            return Char.In
            (
                '-',
                '1', '2', '3', '4', '5', '6', '7', '8', '9', '0'
            );
        }
        internal static Boolean IsDigitChar(Int32 Char)
        {
            return Char.In
            (
                '1', '2', '3', '4', '5', '6', '7', '8', '9', '0'
            );
        }
        internal static Boolean IsNumberChar(Int32 Char)
        {
            return Char.In
            (
                '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '.'
            );
        }
        internal static Boolean IsIntegralNumberChar(Int32 Char)
        {
            return Char.In
            (
                '1', '2', '3', '4', '5', '6', '7', '8', '9', '0'
            );
        }
        internal static Boolean IsConnectorExpressionChar(Int32 Char)
        {
            return Char.In
            (
                '+', '-', '*', '/',
                '&', '|', '^', '!',
                '>', '<', 
                '='
            );
        }
        internal static Boolean IsConnectorCommandChar(Int32 Char)
        {
            return Char.In
            (
                '+', '-', '*', '/',
                '&', '|', '^', '!',
                '=',
                '?', '_'
            );
        }

        internal static void AssertChar(Int32 Char, Char Target, String ParserPath)
        {
            if (Char == BookmarkableFileReader.EndOfFile)
            {
                throw new InvalidSintaxeException(ParserPath + " : Unexpected end of file");
            }
            if (Char == BookmarkableFileReader.CarriageReturn)
            {
                throw new InvalidSintaxeException(ParserPath + " : Unexpected end of line");
            }
            if (Char != Target)
            {
                throw new InvalidSintaxeException(ParserPath + " : Unexpected " + (Char)Char);
            }
        }
        internal static void AssertChar(Int32 Char, Boolean IsEscaped, String ParserPath)
        {
            if (Char == BookmarkableFileReader.EndOfFile)
            {
                throw new InvalidSintaxeException(ParserPath + " : Unexpected end of file");
            }
            if (Char == BookmarkableFileReader.CarriageReturn)
            {
                throw new InvalidSintaxeException(ParserPath + " : Unexpected end of line");
            }
            if (Char == BookmarkableFileReader.SingleQuote && IsEscaped == false)
            {
                throw new InvalidSintaxeException(ParserPath + " : Unexpected '");
            }
        }
        internal static void AssertLine(String Token, Int32 ActualChar, String ParserPath)
        {
            if (Token != null && Token != "")
            {
                return;
            }

            if (ActualChar == BookmarkableFileReader.EndOfFile)
            {
                throw new InvalidSintaxeException(ParserPath + " : Unexpected end of file");
            }
            if (ActualChar == BookmarkableFileReader.CarriageReturn)
            {
                throw new InvalidSintaxeException(ParserPath + " : Unexpected end of line");
            }
        }
        internal static void AssertToken(String Token, Int32 ActualChar, String ParserPath)
        {
            if (Token != null && Token != "")
            {
                return;
            }

            if (ActualChar == BookmarkableFileReader.EndOfFile)
            {
                throw new InvalidSintaxeException(ParserPath + " : Unexpected end of file");
            }
            if (ActualChar == BookmarkableFileReader.CarriageReturn)
            {
                throw new InvalidSintaxeException(ParserPath + " : Unexpected end of line");
            }
            throw new InvalidSintaxeException(ParserPath + " : Unexpected " + (Char)ActualChar);
        }
        internal static void AssertToken(String Token, String Expected, Int32 ActualChar, String ParserPath)
        {
            if (Token != null && Token != "")
            {
                if (Token != Expected)
                {
                    throw new InvalidSintaxeException(ParserPath + " : Unexpected " + Token);
                }
                return;
            }

            if (ActualChar == BookmarkableFileReader.EndOfFile)
            {
                throw new InvalidSintaxeException(ParserPath + " : Unexpected end of file");
            }
            if (ActualChar == BookmarkableFileReader.CarriageReturn)
            {
                throw new InvalidSintaxeException(ParserPath + " : Unexpected end of line");
            }
            throw new InvalidSintaxeException(ParserPath + " : Unexpected " + (Char)ActualChar);
        }
        internal static void AssertListOfInitializedTokens(Dictionary<String, LiteralValueContainerBase> ListOfInitializedTokens, String ParserPath)
        {
            if (ListOfInitializedTokens == null || ListOfInitializedTokens.Count == 0)
            {
                throw new InvalidSintaxeException(ParserPath + " : No properties provided");
            }
        }
        internal static void AssertListOfTokens(List<String> ListOfTokens, String ParserPath)
        {
            if (ListOfTokens == null || ListOfTokens.Count == 0)
            {
                throw new InvalidSintaxeException(ParserPath + " : No items provided");
            }
        }
        internal static void AssertPositive(Int64 Value, String ParserPath)
        {
            if (Value <= 0)
            {
                throw new InvalidSintaxeException(ParserPath + " : Must be positive");
            }
        }
        internal static void AssertStatmentTerminador(BookmarkableFileReader BFR, Int32 Char, String ParserPath)
        {
            switch (Char)
            {
                case '}':
                case BookmarkableFileReader.EndOfFile:
                    break;
                case '\r':
                case '\n':
                    IgnoreWhiteSpacesAndComments(BFR);
                    break;
                case ';':
                    BFR.Advance();
                    IgnoreWhiteSpacesAndComments(BFR);
                    break;
                default:
                    throw new InvalidSintaxeException(ParserPath + " / " + "};\\r\\n" + " : Unexpected " + (Char)Char);
            }
        }
        internal static void AssertExpression(IScriptExpression Expression, Int32 ActualChar, String ParserPath)
        {
            if (Expression != null)
            {
                return;
            }

            if (ActualChar == BookmarkableFileReader.EndOfFile)
            {
                throw new InvalidSintaxeException(ParserPath + " : Unexpected end of file");
            }
            if (ActualChar == BookmarkableFileReader.CarriageReturn)
            {
                throw new InvalidSintaxeException(ParserPath + " : Unexpected end of line");
            }
            throw new InvalidSintaxeException(ParserPath + " : Unexpected " + (Char)ActualChar);
        }
        internal static void AssertListOfExpressions(List<IScriptExpression> ListOfExpressions, String ParserPath)
        {
            if (ListOfExpressions == null || ListOfExpressions.Count == 0)
            {
                throw new InvalidSintaxeException(ParserPath + " : No items provided");
            }
        }

        internal static void AdvanceToEndOfLine(BookmarkableFileReader BFR)
        {
            while (!BFR.Peek().In(BookmarkableFileReader.EndOfFile, BookmarkableFileReader.CarriageReturn))
            {
                BFR.Advance();
            }
        }
    }
}