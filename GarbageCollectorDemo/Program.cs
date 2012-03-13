
namespace ConsoleApplication1
{
    using System;
    using System.Runtime.InteropServices;
    using System.Threading;

    using SimpleComObjectLib;

    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            new Program().Run();
        }
        
        public void Run()
        {
            Console.WriteLine("Press key to start.");
            Console.ReadKey();
            Console.WriteLine("Started.");
            new MethodInvoker().Invoke(this);
        }

        //[Invoke]
        public void ZeroGenerationCollection()
        {
            for (int i = 0; i < 10000000000000; i++)
            {
                new object();
            }
        }

        // [Invoke]
        public void SecondGenerationCollection()
        {
            const int Size = 500000;

            var objects = new object[Size];
            for (int i = 0; i < 10000000000000; i++)
            {
                objects[i % Size] = new object();
            }            
        }

        // [Invoke]
        public void LargeObjectHeap()
        {
            for (int i = 0; i < 10000000000000; i++)
            {
                var x = new int[21247];
            }
        }

        //[Invoke]
        public void CollectionWithFinalizer()
        {
            for (int i = 0; i < 10000000000000; i++)
            {
                //new ObjectWithFinalizerAndPayload();
                //new ObjectWithFinalizerPayloadAndDisposeProcessingLoad();
                new ObjectWithFinalizerPayloadAndDisposeProcessingLoad().Dispose();
            }
        }

        // [Invoke]
        public void TimeSpentInFinalizer()
        {
            for (int i = 0; i < 10000000000000; i++)
            {
                //new ObjectWithoutFinalizer();
                //new ObjectWithFinalizer();
                new ObjectWithFinalizer().Dispose();

                if (i % 1000000 == 0) Console.Write(".");
            }
        }

         [Invoke]
        public void ComObject()
        {
            new FinalizableResource().Dispose();

            for (int i = 0; i < 10000000000000; i++)
            {
                new ObjectWithFinalizerAndPayload();
                if (i % 10000 == 0) Console.Write(".");
            }
        }

        // [Invoke]
        public void SimpleBlockingFinalizer()
        {
            new ObjectWithBlockingFinalizer();

            for (int i = 0; i < 10000000000000; i++)
            {
                new ObjectWithFinalizerAndPayload();
            }
        }
        
        // [Invoke]
        public void ChildCollection()
        {
            var parent = new Parent();

            GC.Collect();
            GC.Collect();

            Console.WriteLine("x has generation {0}", GC.GetGeneration(parent));

            parent.Child = new object();
            var weakRef = new WeakReference(parent.Child);
            parent = null;

            PrintChildStatus(weakRef);

            GC.Collect(0);

            PrintChildStatus(weakRef);

            GC.Collect(1);

            PrintChildStatus(weakRef);

            GC.Collect(2);

            PrintChildStatus(weakRef);
            Console.ReadKey();
        }

        private void PrintChildStatus(WeakReference weakRef)
        {
            Console.WriteLine("Child is alive = {0}", weakRef.IsAlive);
            if (weakRef.IsAlive)
                Console.WriteLine("Child has generation {0}", GC.GetGeneration(weakRef.Target));
        }
        
        private class ObjectWithoutFinalizer : IDisposable
        {
            public void Dispose()
            {
                GC.SuppressFinalize(this);
            }
        }

        private class ObjectWithFinalizer : IDisposable
        {
            ~ObjectWithFinalizer()
            {
                this.Dispose(false);
            }

            public void Dispose()
            {
                this.Dispose(true);
                GC.SuppressFinalize(this);
            }

            protected virtual void Dispose(bool disposing)
            {
            }
        }

        private class ObjectWithFinalizerAndPayload : ObjectWithFinalizer
        {
            protected byte[] payload = new byte[1000];
       }

        private class ObjectWithFinalizerPayloadAndDisposeProcessingLoad : ObjectWithFinalizerAndPayload
        {
            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);
            
                foreach (var b in payload)
                {
                }
            }
        }
        
        private class ObjectWithBlockingFinalizer
        {
            ~ObjectWithBlockingFinalizer()
            {
                var resetEvent = new ManualResetEvent(false);
                Console.WriteLine("Blocking Finalizer now");
                resetEvent.WaitOne();
            }
        }

        private class FinalizableResource : IDisposable
        {
            ISimpleObject _obj;

            public FinalizableResource()
            {
                _obj = new SimpleObject();
            }

            ~FinalizableResource()
            {
                this.Dispose(false);
            }

            public void Dispose()
            {
                this.Dispose(true);
            }

            protected void Dispose(bool disposing)
            {
                if (disposing)
                {
                    Marshal.FinalReleaseComObject(_obj);
                }
            }
        }
    
        private class Parent
        {
            public object Child { get; set; }
        }
    }
}
