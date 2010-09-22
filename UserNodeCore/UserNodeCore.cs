﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ServiceModel;
using System.ServiceModel.Channels;
using Communication;
using System.Threading;

namespace UserNodeCore
{
    /// <summary>
    /// This class implements the Callback interface, i.e. the set
    /// of methods that the service will call back when the result
    /// is ready.
    /// </summary>
    public class ClientCallback : IDarPoolingCallback
    {
        private UserNodeCore parent;

        public UserNodeCore Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        public void GetResult(Result result)
        {
            Console.WriteLine("Service says: " + result.Comment);

            parent.resultCallback(result);
            
        }
    }

    /// <summary>
    /// UserNodeCore class is composed of informations of UserNode plus the status
    /// of its connection. Also, it allows to execute actions that will have
    /// consequences on both the UserNode and the Darpooling network.
    /// </summary>
    public class UserNodeCore
    {
        private UserNode userNode;
        private IState state;
        private List<SearchTripResult> results;
        private IDarPooling serviceProxy;

        public delegate void ResultReceiveHandler(Result r);
        public ResultReceiveHandler resultCallback;
        private IDarPoolingCallback clientCallback;

        public IDarPoolingCallback ClientCallback
        {
            get { return clientCallback; }
        }

        /// <summary>
        /// Setup a new UserNodeCore.
        /// </summary>
        /// <param name="clientNode">represents the UserNode and its settings.</param>
        public UserNodeCore(UserNode clientNode)
        {
            results = new List<SearchTripResult>();
            state = new UnjointState();
            userNode = clientNode;
            clientCallback = new ClientCallback();
            ((ClientCallback) clientCallback).Parent = this;
        }

        public bool Connected
        {
            get {
                if (serviceProxy != null)
                    return true;
                else
                    return false;
            }
        }

        public UserNode UserNode
        {
            get { return userNode; }
            set { }
        }

        public IDarPooling ServiceProxy
        {
            get { return serviceProxy; }
            set { serviceProxy = value; }
        }

        /// <summary>
        /// Represent the state of the connection to the network, according to
        /// State pattern.
        /// </summary>
        public IState State
        {
            get { return state; }
            set { state = value; }
        }
        
        /// <summary>
        /// Join (connect) to the network, through a ServiceNode.
        /// </summary>
        /// <param name="serviceNodeAddress">address of the ServiceNode</param>
        public void Join(string username, string password,
            string serviceNodeAddress, string callbackAddress)
        {
            state.Join(this, username, password, serviceNodeAddress,
                callbackAddress);
        }

        /// <summary>
        /// Unjoin (disconnect) from the network.
        /// </summary>
        public void Unjoin()
        {
            state.Unjoin(this);
        }

        public void InsertTrip(Trip trip)
        {
            state.InsertTrip(this, trip);
        }

        public void SearchTrip()
        {
            state.SearchTrip(this);
        }

        public static void Main()
        {
            UserNodeCore user = new UserNodeCore(new UserNode("prova"));
            Console.WriteLine("DarPooling Client Console Testing");
            Console.WriteLine("Press a key to send the Join() request...");
            Console.ReadLine();
            // In order: username, password (blank), Service Addr, Callback Addr.
            user.Join("Shaoran@http://localhost:1111/Milano", "shaoran", "http://localhost:1111/Catania",
                "http://localhost:2222/prova");
            Console.ReadLine();
        }
    }
}