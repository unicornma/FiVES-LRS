using System;

namespace KIARA
{
    public static class ServiceFactory
    {
        /// <summary>
        /// Creates a new service using specified <paramref name="context"/>.
        /// </summary>
        ///  <returns>Created service.</returns>
        /// <param name="name">The name of the service.</param>
        /// <param name="context">The context.</param>
        public static ServiceImpl CreateByName(string name, Context context)
        {
            return CreateByURI(CreateConfigDataURI(name, context), context);
        }

        /// <summary>
        /// Creates a new service with description at <paramref name="configURI"/> using the default context.
        /// </summary>
        /// <returns>Created service.</returns>
        /// <param name="configURI">Configuration URI.</param>
        public static ServiceImpl CreateByURI(string configURI)
        {
            return CreateByURI(configURI, Context.GlobalContext);
        }

        /// <summary>
        /// Creates a new service with description at <paramref name="configURI"/> using the specified
        /// <paramref name="context"/>.
        /// </summary>
        /// <returns>Created service.</returns>
        /// <param name="configURI">Configuration URI.</param>
        /// <param name="context">The context.</param>
        public static ServiceImpl CreateByURI(string configURI, Context context)
        {
            ServiceImpl service = new ServiceImpl(context);
            context.StartServer(configURI, service.HandleNewClient);
            return service;
        }

        /// <summary>
        /// Discovers the service by its <paramref name="name"/> using specific <paramref name="context"/>. The UUID may
        /// be used as a <paramref name="name"/>.
        /// </summary>
        /// <returns>Discovered service.</returns>
        /// <param name="name">Service name.</param>
        /// <param name="context">The context.</param>
        public static ServiceWrapper DiscoverByName(string name, Context context)
        {
            var uri = CreateConfigDataURI(name, context);
            ServiceWrapper service = new ServiceWrapper(context);
            context.OpenConnection(uri, service.HandleConnected);
            return service;
        }

        private static string CreateConfigDataURI(string name, Context context)
        {
            string config = String.Format(context.configTemplate, name);
            byte[] configBytes = System.Text.Encoding.ASCII.GetBytes(config);
            return "data:text/json;base64," + System.Convert.ToBase64String(configBytes);
        }
    }
}

