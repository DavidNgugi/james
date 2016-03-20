﻿using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Windows.Forms;
using James.Workflows;

namespace James
{
    public class ApiListener
    {
        private static ApiListener _apiListener;
        private static readonly object SingeltonLock = new object();

        public static ApiListener Instance
        {
            get
            {
                lock (SingeltonLock)
                {
                    return _apiListener ?? (_apiListener = new ApiListener());
                }
            }
        }

        private readonly Thread listenThread;
        private ApiListener()
        {
            listenThread = new Thread(Listen);
            listenThread.IsBackground = true;
            listenThread.Start();
        }

        static void Listen()
        {
            while (true)
            {
                using (NamedPipeServerStream server = new NamedPipeServerStream("james"))
                {
                    server.WaitForConnection();
                    using (StreamReader reader = new StreamReader(server))
                    {
                        string message = reader.ReadLine();
                        if (!string.IsNullOrEmpty(message))
                        {
                            var splits = message.Split('/');
                            WorkflowManager.Instance.RunApiTrigger(splits[0], splits);

                            //Check for new imported workflow to be added
                            string workflowPath = $@"{Config.Instance.WorkflowFolderLocation}\{splits[1]}";
                            if (splits[0] == "workflow" && Directory.Exists(workflowPath))
                            {
                                if (WorkflowManager.Instance.LoadWorkflow(workflowPath))
                                {
                                    MessageBox.Show($"Successfully imported workflow: {splits[1]}");
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
