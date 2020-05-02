using Microsoft.Extensions.Options;

namespace FileTaskTests.FakeImplementations
{
    public class FakeOptions<T> : IOptions<T>
        where T: class, new()
    {
        public FakeOptions(T value)
        {
            Value = value;
        }

        public T Value { get; }
    }
}
