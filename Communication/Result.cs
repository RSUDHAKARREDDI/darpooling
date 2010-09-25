﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;

namespace Communication
{
    [DataContract]
    [KnownType(typeof(NullResult))]
    [KnownType(typeof(ConnectionErrorResult))]
    [KnownType(typeof(RegisterOkResult))]
    [KnownType(typeof(RegisterErrorResult))]
    [KnownType(typeof(LoginOkResult))]
    [KnownType(typeof(LoginErrorResult))]
    [KnownType(typeof(UnjoinConfirmedResult))]
    [KnownType(typeof(InsertErrorResult))]
    [KnownType(typeof(InsertOkResult))]
    public abstract class Result
    {
        [DataMember]
        protected string comment;

        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }
    }



    // General Purpose Result
    [DataContract]
    public class NullResult : Result { }

    [DataContract]
    public class ConnectionErrorResult : Result { }


    // Possible results in response to RegisterUserCommand

    [DataContract]
    public class RegisterOkResult : Result 
    {
        [DataMember]
        private string registeredUsername;

        public string FinalUsername
        {
            get { return registeredUsername; }
            set { registeredUsername = value; }
        }
    }
    [DataContract]
    public class RegisterErrorResult : Result { }


    // Possible results in response to JoinCommand
    
    [DataContract]
    public class LoginOkResult : Result 
    {
        [DataMember]
        string authorizedUsername;

        public string AuthorizedUsername
        {
            get { return authorizedUsername; }
            set { authorizedUsername = value; }
        }
    }
    [DataContract]
    public class LoginErrorResult : Result { }
    

    // Possible results in response to UnjoinCommand
    
    [DataContract]
    public class UnjoinConfirmedResult : Result { }

    
    // Possible results in response to InsertTrip
    
    [DataContract]
    public class InsertOkResult : Result { }
    [DataContract]
    public class InsertErrorResult : Result { }

    
    // Possible results in response to SearchTrip

    public class SearchTripResult : Result
    {
        private List<Trip> trips;

        public List<Trip> Trips
        {
            get { return trips; }
        }

        public SearchTripResult(List<Trip> trips)
        {
            this.trips = trips;
        }
    }

    public class SearchTripError : Result { }


    /// <summary>
    /// This result is used when working with callbacks is not possible
    /// (i.e. smartphone devices). It simply says to wait for a given (or
    /// arbitrary) amount of time and then try again requesting the resource.
    /// </summary>
    public class WaitAndTryResult : Result
    {
        public int Seconds { get; set; }
    }
}