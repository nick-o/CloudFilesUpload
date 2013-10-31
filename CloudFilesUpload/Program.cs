using System;
using System.Collections.Generic;
using System.IO;

using net.openstack.Providers.Rackspace;
using net.openstack.Core.Providers;
using net.openstack.Core.Exceptions.Response;
using net.openstack.Core.Domain;


namespace CloudFilesUpload
{
    class Program
    {
        static String targetContainer;
        static String filePath;
        public static void Main(string[] args)
        {
            Boolean containerExists = false; 
            if (args.Length < 4 || args.Length > 5)
            {
                Console.WriteLine("Usage: {0} username api_key target_container path_to_file [region (US|UK)]", Environment.CommandLine);
                Environment.Exit(1);
            }
            RackspaceCloudIdentity auth = new RackspaceCloudIdentity();
            IEnumerable<Container> containerList = null;
            auth.Username = args[0];
            auth.APIKey = args[1];
            targetContainer = args[2];
            filePath = args[3];
            if (args.Length == 5)
            {
                if (args[4] != "UK" && args[4] != "US")
                {
                    Console.WriteLine("region must be either US or UK", Environment.CommandLine);
                    Environment.Exit(1);
                }
                switch (args[4])
                {
                    case "UK": {auth.CloudInstance = CloudInstance.UK;};break;
                    case "US": { auth.CloudInstance = CloudInstance.Default;}; break;
                }
            }

            try
            {
                IIdentityProvider identityProvider = new CloudIdentityProvider();
                var userAccess = identityProvider.Authenticate(auth);
            }
            catch (ResponseException ex2)
            {
                Console.WriteLine("Authentication failed with the following message: {0}",ex2.Message);
                Environment.Exit(1);
            }

            try
            {
                var cloudFilesProvider = new CloudFilesProvider(auth);
                containerList = cloudFilesProvider.ListContainers();

                foreach (Container container in containerList)
                {
                    if (container.Name == targetContainer)
                    {
                        containerExists = true;
                        break;
                    }
                }

                if (!containerExists)
                {
                    Console.WriteLine("Container \"{0}\" does not exist on the provided CloudFiles account.", targetContainer);
                    Environment.Exit(1);
                }
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("The file specified ({0}) does not exist", filePath);
                    Environment.Exit(1);
                }
                cloudFilesProvider.CreateObjectFromFile(targetContainer, @filePath, Path.GetFileName(filePath));
            }
            catch (Exception ex2)
            {
                Console.WriteLine(ex2.Message);
                Environment.Exit(1);
            }
            Console.WriteLine("*SUCCESS* File: \"{0}\" uploaded to \"{1}\"", filePath, targetContainer);
        }
    }
}
