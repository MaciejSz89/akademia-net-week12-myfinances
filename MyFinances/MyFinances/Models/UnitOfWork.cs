﻿using MyFinances.Models.Domains;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFinances.Models
{
    public class UnitOfWork : IUnitOfWork
    {
        private SQLiteAsyncConnection _context;

        public UnitOfWork(string dbPath)
        {
            _context = new SQLiteAsyncConnection(dbPath);
            _context.CreateTableAsync<Operation>().Wait();
            Operation = new OperationRepository(_context);
        }

        public IOperationRepository Operation { get; set; }
    }
}
