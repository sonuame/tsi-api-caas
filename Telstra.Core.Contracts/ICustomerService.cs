﻿using System;
using System.Threading.Tasks;
using Telstra.Core.Data.Entities;

namespace Telstra.Core.Contracts
{
    public interface ICustomerService
    {
        Task<Customer> GetCustomerById(int id);
        Task<Customer> GetCustomerById2(int id);
    }
}

