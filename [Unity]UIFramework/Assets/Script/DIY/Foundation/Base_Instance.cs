
namespace DIY.Foundation
{
    public abstract class Base_Instance<T>: System.IDisposable where T : new()
    {
        private static T _instance;
        public static T Instance {
            get {
                if (null == _instance)
                {
                    _instance = new T();
                }
                return _instance;
            }
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}
