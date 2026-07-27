namespace LCSoft.Framework.Scripting.Classes
{
    using System;
    using LCSoft.Framework.Scripting.ValueContainer;

    internal struct ExecutionResult
    {
        public enum CancelationMode : byte
        {
            None = 0,
            Return = 1,
            Break = 2,
            Continue = 3,
            Fail = 4
        }

        internal static ExecutionResult Continue = new ExecutionResult(CancelationMode.Continue);
        internal static ExecutionResult Fail = new ExecutionResult(CancelationMode.Fail);

        public static ExecutionResult Break(Int64 Count)
        {
            return new ExecutionResult(Count);
        }
        public static ExecutionResult Return(ValueContainerBase Result)
        {
            return new ExecutionResult(CancelationMode.Return, Result);
        }
        public static ExecutionResult None()
        {
            return new ExecutionResult(CancelationMode.None);
        }
        public static ExecutionResult None(ValueContainerBase Result)
        {
            return new ExecutionResult(CancelationMode.None, Result);
        }

        internal CancelationMode CancelationFlag;
        internal Int64 CancelationCount;
        internal ValueContainerBase Value;

        private ExecutionResult(CancelationMode CancelationMode)
        {
            this.CancelationFlag = CancelationMode;
            this.CancelationCount = 0;
            this.Value = ValueContainerBase.Empty;
        }
        private ExecutionResult(CancelationMode CancelationMode, ValueContainerBase Value)
        {
            this.CancelationFlag = CancelationMode;
            this.CancelationCount = 0;
            this.Value = Value;
        }
        private ExecutionResult(Int64 CancelationCount)
        {
            this.CancelationFlag = CancelationMode.Break;
            this.CancelationCount = CancelationCount;
            this.Value = ValueContainerBase.Empty;
        }

        public ExecutionResult EndBlock()
        {
            if (CancelationFlag != CancelationMode.Break)
            {
                throw new InvalidOperationException(nameof(CancelationFlag) + ": " + CancelationFlag.ToString());
            }

            CancelationCount--;
            if (CancelationCount == 0)
            {
                CancelationFlag = CancelationMode.None;
            }

            return this;
        }
        public ExecutionResult EndCall()
        {
            if (CancelationFlag != CancelationMode.Return)
            {
                throw new InvalidOperationException(nameof(CancelationFlag) + ": " + CancelationFlag.ToString());
            }

            CancelationFlag = CancelationMode.None;

            return this;
        }
    }
}