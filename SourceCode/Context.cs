namespace LCSoft.Framework.Scripting
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using LCSoft.Framework.Core.Classes;
    using LCSoft.Framework.Scripting.Classes;
    using LCSoft.Framework.Scripting.Exceptions.RunTime.NotFound;
    using LCSoft.Framework.Scripting.ValueContainer;
    using LCSoft.Framework.Scripting.ValueContainer.LiteralValueContainer;

    public class Context
    {
        public enum ContextType : byte
        {
            Global = 1,
            Spawn = 2,
            Child = 3
        }

        public Dictionary<String, ValueContainerBase> Variables { get; private set; }

        public List<Context> Spawns { get; internal set; }

        public readonly Guid ID;
        public readonly ContextType Type;

        public Context Parent;
        public Context Global;
        //public readonly Context Root;
        public Context Child;

        public Context()
        {
            ID = Guid.NewGuid();
            Type = ContextType.Global;

            Parent = null;
            Global = this;
            //Root = this;

            Variables = new Dictionary<String, ValueContainerBase>();
            Variables.Add("ScriptServices", new ClassReferenceValueContainer(typeof(ScriptServices)));

            Spawns = new List<Context>();
        }

        private Context(ContextType Type, Context Parent, Context Global)
        {
            ID = Guid.NewGuid();
            this.Type = Type;

            this.Parent = Parent;
            this.Global = Global;
            //this.Root = this;

            Variables = new Dictionary<String, ValueContainerBase>();
        }
        //private Context(ContextType Type, Context Parent, Context Global, Context Root)
        //{
        //    this.Type = Type;
        //    this.Parent = Parent;
        //    this.Global = Global;
        //    this.Root = Root;
        //}

        public Context EnterSpawnContext()
        {
            Context Context = new Context(ContextType.Spawn, Global, Global/*, Global*/);
            Global.Spawns.Add(Context);
            return Context;
        }
        public Context EnterChildContext()
        {
            if (Child != null)
            {
                throw new InvalidOperationException(MethodBase.GetCurrentMethod().Name);
            }

            Context Context = new Context(ContextType.Child, this, Global/*, Root*/);
            Child = Context;
            return Context;
        }

        public void LeaveContext()
        {
            if (Type == ContextType.Global || Child != null)
            {
                throw new InvalidOperationException(MethodBase.GetCurrentMethod().Name);
            }

            if (Type == ContextType.Spawn)
            {
                Parent.Spawns.Remove(this);
            }
            else
            {
                Parent.Child = null;
            }

            Variables.Clear();

            Parent = null;
            Global = null;
        }

        public ValueContainerBase GetVariable(String Name)
        {
            Context Pointer = this;

            while (Pointer != null)
            {
                if (Pointer.Variables.TryGetValue(Name, out ValueContainerBase VC))
                {
                    return VC;
                }

                Pointer = Pointer.Parent;
            }

            //Context VariableContext = GetVariableContext(Name);
            //if (VariableContext != null)
            //{
            //    return VariableContext.Variables[Name];
            //}

            throw new VariableNotFoundException(Name);
        }
        internal (ValueContainerBase Value, Boolean IsMissing) GetVariableOrNull(String Name)
        {
            Context Pointer = this;

            while (Pointer != null)
            {
                if (Pointer.Variables.TryGetValue(Name, out ValueContainerBase VC))
                {
                    return (VC, false);
                }

                Pointer = Pointer.Parent;
            }

            //Context VariableContext = GetVariableContext(Name);
            //if (VariableContext != null)
            //{
            //    return VariableContext.Variables[Name];
            //}

            return (null, true);
        }
        internal Context GetVariableContext(String Name)
        {
            Context Pointer = this;

            while (Pointer != null)
            {
                if (Pointer.Variables.ContainsKey(Name))
                {
                    return Pointer;
                }

                Pointer = Pointer.Parent;
            }

            throw new VariableNotFoundException(Name);
        }
        internal Context GetVariableContextOrNull(String Name)
        {
            Context Pointer = this;

            while (Pointer != null)
            {
                if (Pointer.Variables.ContainsKey(Name))
                {
                    return Pointer;
                }

                Pointer = Pointer.Parent;
            }

            return null;
        }

        internal Boolean CheckVariable(String Name)
        {
            Context Pointer = this;

            while (Pointer != null)
            {
                if (Pointer.Variables.ContainsKey(Name))
                {
                    return true;
                }

                Pointer = Pointer.Parent;
            }

            return false;
        }

        public void SetVariable(String Name, Object Value, Type Type)
        {
            Context VariableContext = GetVariableContextOrNull(Name);

            if (VariableContext != null)
            {
                VariableContext.Variables[Name] = RunTimeUtils.Box(Value, Type);
            }
            else
            {
                Variables.Add(Name, RunTimeUtils.Box(Value, Type));
            }
        }
        public void SetVariable(String Name, Type Type)
        {
            Context VariableContext = GetVariableContextOrNull(Name);

            if (VariableContext != null)
            {
                VariableContext.Variables[Name] = new ClassReferenceValueContainer(Type);
            }
            else
            {
                Variables.Add(Name, new ClassReferenceValueContainer(Type));
            }
        }
        public void SetVariable(String Name, ValueContainerBase Value)
        {
            Context VariableContext = GetVariableContextOrNull(Name);

            if (VariableContext != null)
            {
                VariableContext.Variables[Name] = Value;
            }
            else
            {
                Variables.Add(Name, Value);
            }
        }

        public String Dump()
        {
            StringComposer SC = new StringComposer();

            Context Pointer = this;
            while (Pointer != null)
            {
                foreach (String Key in Pointer.Variables.Keys)
                {
                    if (Pointer.Variables[Key] == ValueContainerBase.Empty)
                    {
                        SC.AppendSeparated(Environment.NewLine, Key, " = (Empty)");
                    }
                    else if (Pointer.Variables[Key] is NullLiteralValueContainer)
                    {
                        SC.AppendSeparated(Environment.NewLine, Key, " = Null (", Pointer.Variables[Key].GetUnderlyingType().Name, ")");
                    }
                    else if (Pointer.Variables[Key] is ClassReferenceValueContainer)
                    {
                        SC.AppendSeparated(Environment.NewLine, Key, " = Class Reference (", Pointer.Variables[Key].GetUnderlyingType().Name, ")");
                    }
                    else if (Pointer.Variables[Key] is ObjectReferenceValueContainer)
                    {
                        SC.AppendSeparated(Environment.NewLine, Key, " = Object Reference (", Pointer.Variables[Key].GetUnderlyingType().Name, ")");
                    }
                    else if (Pointer.Variables[Key].GetUnspecified() is String)
                    {
                        SC.AppendSeparated(Environment.NewLine, Key, " = \"", Pointer.Variables[Key].GetUnspecified().ToString(), "\"");
                    }
                    else
                    {
                        SC.AppendSeparated(Environment.NewLine, Key, " = ", Pointer.Variables[Key].GetUnspecified().ToString());
                    }
                }
                Pointer = Pointer.Parent;
            }

            return SC.ToString();
        }
    }
}
