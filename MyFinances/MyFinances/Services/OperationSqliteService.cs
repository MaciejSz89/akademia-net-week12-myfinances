using MyFinances.Core.Dtos;
using MyFinances.Core.Response;
using MyFinances.Models;
using MyFinances.Models.Converters;
using MyFinances.Models.Domains;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFinances.Services
{
    public class OperationSqliteService : IOperationService
    {
        private static IUnitOfWork _unitOfWork;

        public IUnitOfWork UnitOfWork
        {
            get
            {
                if (_unitOfWork == null)
                {
                    _unitOfWork = new UnitOfWork(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MyFinancesSQLite.db3"));
                }
                return _unitOfWork;
            }
        }
        public async Task<DataResponse<int>> AddAsync(OperationDto operation)
        {
            var response = new DataResponse<int>();
            try
            {
                response.Data = await UnitOfWork.Operation.AddAsync(operation.ToDao());
            }
            catch (Exception exception)
            {
                response.Errors.Add(new Error(exception.Source, exception.Message));
            }
            return response;
        }

        public async Task<Response> DeleteAsync(int id)
        {
            var response = new Response();
            try
            {
                await UnitOfWork.Operation.DeleteAsync(new Operation { Id = id });
            }
            catch (Exception exception)
            {
                response.Errors.Add(new Error(exception.Source, exception.Message));
            }
            return response;
        }

        public async Task<DataResponse<OperationDto>> GetAsync(int id)
        {
            var response = new DataResponse<OperationDto>();
            try
            {
                response.Data = (await UnitOfWork.Operation.GetAsync(id)).ToDto();
            }
            catch (Exception exception)
            {
                response.Errors.Add(new Error(exception.Source, exception.Message));
            }
            return response;
        }

        public async Task<DataResponse<IEnumerable<OperationDto>>> GetAsync()
        {
            var response = new DataResponse<IEnumerable<OperationDto>>();
            try
            {

                response.Data = (await UnitOfWork.Operation.GetAsync()).ToDtos();
            }
            catch (Exception exception)
            {
                response.Errors.Add(new Error(exception.Source, exception.Message));
            }
            return response;
        }

        public async Task<DataResponse<OperationPageDto>> GetAsync(int pageSize, int currentPage)
        {
            var response = new DataResponse<OperationPageDto>();
            try
            {
                var operations = (await UnitOfWork.Operation.GetAsync()).ToDtos();
                var lastPage = (operations.ToList().Count() + pageSize - 1) / pageSize;
                var updatedCurrentPage = lastPage > currentPage ? currentPage : lastPage;
                var operationsPage = operations.OrderBy(o => o.Id)
                                               .Skip((updatedCurrentPage - 1) * pageSize)
                                               .Take(pageSize)
                                               .ToList();

                response.Data = new OperationPageDto
                {
                    Operations = operationsPage,
                    CurrentPage = updatedCurrentPage,
                    LastPage = lastPage
                };
            }
            catch (Exception exception)
            {
                response.Errors.Add(new Error(exception.Source, exception.Message));
            }
            return response;
        }

        public async Task<Response> UpdateAsync(OperationDto operation)
        {
            var response = new Response();
            try
            {
                await UnitOfWork.Operation.UpdateAsync(operation.ToDao());
            }
            catch (Exception exception)
            {
                response.Errors.Add(new Error(exception.Source, exception.Message));
            }
            return response;
        }
    }
}
