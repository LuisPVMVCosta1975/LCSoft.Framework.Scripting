namespace LCSoft.Framework.Scripting
{
    using System;
    using System.Collections.Generic;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.Exceptions.RunTime.Internal;
    using LCSoft.Framework.Scripting.Exceptions.RunTime.OutOfContext;
    using LCSoft.Framework.Scripting.ScriptContent;
    using LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent;
    using LCSoft.Framework.Scripting.ScriptContent.ScriptRootContent.ScriptCommand;
    using LCSoft.Framework.Scripting.ValueContainer;

    public class Script
    {
        internal ScriptResources Resources = new ScriptResources();
        internal List<ScriptCommandBase> Commands = new List<ScriptCommandBase>();
        public Script()
        {
            Resources.Enums.Add("DataTypesEnum", EnumScriptRootContent.DataTypesEnum);
        }

        public static Script FromFile(String ScriptFileName)
        {
            Script Script = new Script();
            Parser.FromFile(Script, ScriptFileName);
            return Script;
        }

        public void AddContent(ScriptRootContentBase RootContent)
        {
            if (RootContent is FunctionScriptRootContent SF)
            {
                Resources.Functions.Add(SF.Name, SF);
                //SF.Name = null;
            }
            else if (RootContent is TypeScriptRootContent ST)
            {
                Resources.Types.Add(ST.Name, ST);
                //ST.Name = null;
            }
            else if (RootContent is EnumScriptRootContent SE)
            {
                Resources.Enums.Add(SE.Name, SE);
                //SE.Name = null;
            }
            else if (RootContent is ObjectScriptRootContent SO)
            {
                Resources.Objects.Add(SO.Name, SO);
                //SO.Name = null;
            }
            else if (RootContent is ScriptCommandBase SC)
            {
                Commands.Add(SC);
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        public ValueContainerBase Run(Context Context)
        {
            foreach (ScriptCommandBase SC in Commands)
            {
                ExecutionResult ElementResult;
                try
                {
                    ElementResult = SC.RunElement(Context, Resources);
                }
                catch (ExitInternalException Exception)
                {
                    return Exception.Value;
                }
                catch (Exception)
                {
                    throw;
                }

                //TODO: Tech Debt: really needed?
                if (ElementResult.CancelationFlag == ExecutionResult.CancelationMode.Break)
                {
                    throw new CommandOutOfContextException("Break");
                }
                if (ElementResult.CancelationFlag == ExecutionResult.CancelationMode.Continue)
                {
                    throw new CommandOutOfContextException("Continue");
                }
                if (ElementResult.CancelationFlag == ExecutionResult.CancelationMode.Fail)
                {
                    throw new CommandOutOfContextException("Fail");
                }
                if (ElementResult.CancelationFlag == ExecutionResult.CancelationMode.Return)
                {
                    return ElementResult.Value;
                }
            }

            return ValueContainerBase.Empty;
        }
        public ValueContainerBase Call(Context Context, String FunctionName)
        {
            ExecutionResult ElementResult;
            try
            {
                ElementResult = RunTimeUtils.CallFunction(FunctionName, (List<ValueContainerBase>)null, Context, Resources);
            }
            catch (ExitInternalException Exception)
            {
                return Exception.Value;
            }
            catch (Exception)
            {
                throw;
            }

            //TODO: Tech Debt: really needed?
            if (ElementResult.CancelationFlag == ExecutionResult.CancelationMode.Break)
            {
                throw new CommandOutOfContextException("Break");
            }
            if (ElementResult.CancelationFlag == ExecutionResult.CancelationMode.Continue)
            {
                throw new CommandOutOfContextException("Continue");
            }
            if (ElementResult.CancelationFlag == ExecutionResult.CancelationMode.Fail)
            {
                throw new CommandOutOfContextException("Fail");
            }
            if (ElementResult.CancelationFlag == ExecutionResult.CancelationMode.Return)
            {
                //TODO: Tech Debt: should be exception
                //return ElementResult.Value; //TODO: Tech Debt: should be exception
                throw new CommandOutOfContextException("Return");
            }

            return ElementResult.Value;
        }
        public ValueContainerBase Call(Context Context, String FunctionName, List<ValueContainerBase> EvaluatedParameters)
        {
            ExecutionResult ElementResult;
            try
            {
                ElementResult = RunTimeUtils.CallFunction(FunctionName, EvaluatedParameters, Context, Resources);
            }
            catch (ExitInternalException Exception)
            {
                return Exception.Value;
            }
            catch (Exception)
            {
                throw;
            }

            //TODO: Tech Debt: really needed?
            if (ElementResult.CancelationFlag == ExecutionResult.CancelationMode.Break)
            {
                throw new CommandOutOfContextException("Break");
            }
            if (ElementResult.CancelationFlag == ExecutionResult.CancelationMode.Continue)
            {
                throw new CommandOutOfContextException("Continue");
            }
            if (ElementResult.CancelationFlag == ExecutionResult.CancelationMode.Fail)
            {
                throw new CommandOutOfContextException("Fail");
            }
            if (ElementResult.CancelationFlag == ExecutionResult.CancelationMode.Return)
            {
                //TODO: Tech Debt: should be exception
                //return ElementResult.Value; //TODO: Tech Debt: should be exception
                throw new CommandOutOfContextException("Return");
            }

            return ElementResult.Value;
        }

        public static String Sign()
        {
            return "Scripting Engine, R37, @LCSoft 2010-2024";
        }
    }
}