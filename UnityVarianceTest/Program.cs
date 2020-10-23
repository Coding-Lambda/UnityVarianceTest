using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace UnityVarianceTest
{
    class Program
    {
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
                throw new System.NotImplementedException();
            }
        }

        public class MyNotification : INotification { }

        public class MyNotificationHandler : INotificationHandler<MyNotification>
        {
            public void Handle(MyNotification notification)
            {
                throw new System.NotImplementedException();
            }
        }

        static void Main(string[] args)
        {
            UnityContainer container = new UnityContainer();
            container.EnableDebugDiagnostic();

            //container.RegisterType<INotificationHandler<INotification>, GenericHandler>("GenericHandler");
            container.RegisterType(typeof(INotificationHandler<>), typeof(GenericHandler), "GenericHandler");
            container.RegisterType<INotificationHandler<MyNotification>, MyNotificationHandler>("MyNotificationHandler");

            INotificationHandler<INotification> genericHandler = new GenericHandler();
            INotificationHandler<MyNotification> myHandler = new MyNotificationHandler();

            // Contravariance. 
            // An object that is instantiated with a less derived type argument 
            // is assigned to an object instantiated with a more derived type argument. 
            // Assignment compatibility is reversed. 
            myHandler = genericHandler;

            IEnumerable<INotificationHandler<MyNotification>> list = container.ResolveAll<INotificationHandler<MyNotification>>();

            Console.WriteLine($"Count should be 2 but is {list.Count()}");
            Console.WriteLine("The following types are resolved:");
            foreach (INotificationHandler<MyNotification> handler in list)
            {
                Console.WriteLine(handler.GetType().FullName);
            }
            Console.ReadLine();
        }
    }
}
