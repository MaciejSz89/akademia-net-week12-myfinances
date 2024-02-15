﻿using MyFinances.Core.Dtos;
using MyFinances.Models.Domains;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyFinances.Models
{
    public class OperationRepository : IOperationRepository
    {
        private readonly SQLiteAsyncConnection _context;

        public OperationRepository(SQLiteAsyncConnection context)
        {
            _context = context;
        }
        public async Task<int> AddAsync(Operation operation)
        {
            return await _context.InsertAsync(operation);
        }

        public async Task DeleteAsync(Operation operation)
        {
            await _context.DeleteAsync(operation);
        }

        public async Task<Operation> GetAsync(int id)
        {
            return await _context.Table<Operation>().FirstAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Operation>> GetAsync()
        {
            return await _context.Table<Operation>().ToListAsync();
        }

        public async Task UpdateAsync(Operation operation)
        {
            await _context.UpdateAsync(operation);
        }
    }
}
