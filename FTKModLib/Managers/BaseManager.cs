using System;

namespace FTKModLib {
    /// <summary>
    ///     The base class for all the library's various Managers
    /// </summary>
    public abstract class BaseManager<T> where T : BaseManager<T>, new() {
        private static Lazy<T> Lazy;

        public static T Instance {
            get {
                if (Lazy == null) {
                    Lazy = new(() => Activator.CreateInstance(typeof(T), true) as T);
                    Utils.Logger.LogInfo("Initializing " + Lazy.Value.GetType().Name);
                    Lazy.Value.Init();
                }
                return Lazy.Value;
            }
        }

        /// <summary>
        ///     Initialize manager class
        /// </summary>
        public virtual void Init() { }
    }

    /// <summary>
    /// Provides support for lazy initialization.
    /// </summary>
    /// <typeparam name="T">Specifies the type of object that is being lazily initialized.</typeparam>
    public sealed class Lazy<T> {
        private readonly object padlock = new object();
        private readonly Func<T> createValue;
        private bool isValueCreated;
        private T value;

        public T Value {
            get {
                if (!isValueCreated) {
                    lock (padlock) {
                        if (!isValueCreated) {
                            value = createValue();
                            isValueCreated = true;
                        }
                    }
                }
                return value;
            }
        }
        public bool IsValueCreated {
            get {
                lock (padlock) {
                    return isValueCreated;
                }
            }
        }
        public Lazy(Func<T> createValue) {
            if (createValue == null) throw new ArgumentNullException("createValue");

            this.createValue = createValue;
        }
    }
}
