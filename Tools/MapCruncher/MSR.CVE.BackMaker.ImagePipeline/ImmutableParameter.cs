using System;

namespace MSR.CVE.BackMaker.ImagePipeline
{
    public abstract class ImmutableParameter<T> : Parameter, IRobustlyHashable, Present, IDisposable
    {
        private T _value;

        public T value
        {
            get
            {
                return _value;
            }
        }

        public ImmutableParameter(T value)
        {
            _value = value;
        }

        public Present Duplicate(string refCredit)
        {
            return this;
        }

        public void Dispose()
        {
        }

        public abstract void AccumulateRobustHash(IRobustHash hash);
    }
}
