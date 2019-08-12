using System;

namespace ScHelix.Foundation.HelixCore.DI {
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ServiceAttribute : Attribute {
        public ServiceAttribute() {
        }

        public ServiceAttribute(Type serviceType, Lifetime lifetime = Lifetime.Transient) {
            ServiceType = serviceType;
            Lifetime = lifetime;
        }

        public Lifetime Lifetime { get; set; } = Lifetime.Singleton;

        public Type ServiceType { get; set; }
    }
}
