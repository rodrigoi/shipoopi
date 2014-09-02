using Ninject;

namespace Shipoopi.Reporter.Factories
{
    public sealed class KernelFactory
    {
        private static StandardKernel kernelSingleton = null;
        private static readonly object padlock = new object();
        
        private KernelFactory() { }

        public static StandardKernel GetKernel()
        {
            lock (padlock)
            {
                if (kernelSingleton == null)
                    kernelSingleton = new StandardKernel(new Shipoopi.Reporter.IoC.ReporterModule());
                return kernelSingleton;
            }
        }
    }
}