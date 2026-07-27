namespace LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent.ScriptCommand.ScriptExpressionCommand
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.ScriptContent;
    using LCSoft.Framework.Scripting.ValueContainer;

    public class SpawnScriptExpressionCommand : ScriptExpressionCommandBase
    {
        public const string ComponentName = "Spawn";
        public const string ComponentSignature = ComponentName + " [" + ComponentType + "]";

        internal static SpawnScriptExpressionCommand Parse(BookmarkableFileReader BFR)
        {
            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            ParserUtils.AssertChar(BFR.Read(), '(', ComponentSignature + " / [Parameter List Start]");

            String StartFunction = ParserUtils.GetToken(BFR);
            ParserUtils.AssertToken(StartFunction, BFR.Peek(), ComponentSignature + " / " + nameof(StartFunction) + " [Identifier]");

            List<IScriptExpression> Parameters;
            ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
            if (BFR.Peek() == ',')
            {
                BFR.Advance();

                ParserUtils.IgnoreWhiteSpacesAndComments(BFR);
                Parameters = ParserUtils.ParseListOfExpressions(BFR, ComponentSignature + " / " + nameof(Parameters) + " [List Of Expressions]", ')');
                ParserUtils.AssertListOfExpressions(Parameters, ComponentSignature + " / " + nameof(Parameters) + " [List Of Expressions]");
            }
            else
            {
                Parameters = null;
            }

            ParserUtils.AssertChar(BFR.Read(), ')', ComponentSignature + " / [Parameter List End]");

            return new SpawnScriptExpressionCommand(StartFunction, Parameters);
        }

        internal readonly String StartFunction;
        internal readonly List<IScriptExpression> Parameters;

        public SpawnScriptExpressionCommand(String StartFunction, List<IScriptExpression> Parameters)
        {
            this.StartFunction = StartFunction;
            this.Parameters = Parameters;
        }

        internal override ExecutionResult RunElement(Context Context, ScriptResources ScriptResources)
        {
            CancelationTokenValueContainer CancelationToken = new CancelationTokenValueContainer();
            SpawnTokenValueContainer SpawnToken = new SpawnTokenValueContainer(CancelationToken);

            Thread Thread = new Thread
            (
                () =>
                {
                    // TODO: Context vs Context.Global
                    //SpawnToken.Finish(RunTimeUtils.CallFunction(StartFunction, CancelationToken, Parameters, Context.Global, ScriptResources, Context, ScriptResources));
                    SpawnToken.Finish(RunTimeUtils.CallFunction(StartFunction, CancelationToken, Parameters, Context, ScriptResources, Context, ScriptResources));
                }
            );
            Thread.Start();

            return ExecutionResult.None(SpawnToken);
        }
    }
}