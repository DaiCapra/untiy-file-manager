using System;

namespace FileManagement.Code.Tests.Editor
{
    [Serializable]
    public class TestData : IEquatable<TestData>
    {
        public int value;

        public bool Equals(TestData other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return value == other.value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TestData) obj);
        }

        public override int GetHashCode()
        {
            return value;
        }
    }
}