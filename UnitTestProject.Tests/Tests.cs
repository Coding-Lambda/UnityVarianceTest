using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;

namespace UnitTestProject.Tests
{
    [TestClass]
    public class Tests
    {
        /// <summary>
        /// Code from https://github.com/unitycontainer/unity/issues/215#issuecomment-380443152
        /// </summary>
        [TestMethod]
        public void ShouldResolveBoundAndUnboundGeneric()
        {
            var container = new UnityContainer();
            container.EnableDebugDiagnostic();

            container.RegisterType(typeof(INotificationHandler<>), typeof(GenericHandler), "GenericHandler");
            container.RegisterType<INotificationHandler<MyNotification>, MyNotificationHandler>("MyNotificationHandler");

            var handlers = container
                .ResolveAll<INotificationHandler<MyNotification>>()
                .ToList();

            Assert.AreEqual(handlers.Count, 2, "Should resolve 2 instances.");
            Assert.IsTrue(handlers.Any(inst => inst.GetType() == typeof(GenericHandler)), $"One resolved instance should be of type '{nameof(GenericHandler)}'.");
            Assert.IsTrue(handlers.Any(inst => inst.GetType() == typeof(MyNotificationHandler)), $"One resolved instance should be of type '{nameof(MyNotificationHandler)}'.");
        }
    }

    public interface INotification { }

    public interface INotificationHandler<in TNotification>
        where TNotification : INotification
    {
        void Handle(TNotification notification);
    }

    public class GenericHandler : INotificationHandler<INotification>
    {
        public void Handle(INotification notification)
        {
            throw new NotImplementedException();
        }
    }

    public class MyNotification : INotification { }

    public class MyNotificationHandler : INotificationHandler<MyNotification>
    {
        public void Handle(MyNotification notification)
        {
            throw new NotImplementedException();
        }
    }
}
