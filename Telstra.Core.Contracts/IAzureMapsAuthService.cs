﻿using System;
using System.Threading.Tasks;
using Telstra.Core.Data.Entities;

namespace Telstra.Core.Contracts
{
    public interface IAzureMapsAuthService
    {
        AuthToken GetAuthToken(string clientId, string clientSecret);
    }
}
