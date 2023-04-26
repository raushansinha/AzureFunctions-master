using AzHttpAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzHttpAPI.Services
{
    internal class EmployeeServices : IServices<Employee, int>
    {
        CompanyContext context = new CompanyContext();
        ResponseObject<Employee> resonse = new ResponseObject<Employee>();

        async Task<ResponseObject<Employee>> IServices<Employee, int>.CreateAsync(Employee entity)
        {
            var result = await context.Employees.AddAsync(entity);
            await context.SaveChangesAsync(); // COmmit Transactions
            resonse.Record = result.Entity;
            resonse.StatucCode = 201;
            resonse.StatusMessage = "Employee is ceated successfuly";
            return resonse;

        }

        async Task<ResponseObject<Employee>> IServices<Employee, int>.DeleteAsync(int id)
        {
            try
            {
                var record = await context.Employees.FindAsync(id);
                if (record == null)
                {
                    resonse.StatucCode = 404;
                    resonse.StatusMessage = "Employee is not found";
                    return resonse;
                }
                context.Employees.Remove(record);
                await context.SaveChangesAsync();
                resonse.StatucCode = 202;
                resonse.StatusMessage = "Employee is deleted successfully";
                return resonse;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return null;

        }

        async Task<ResponseObject<Employee>> IServices<Employee, int>.GetAsync()
        {
            resonse.Records = await context.Employees.ToListAsync();
            resonse.StatucCode = 200;
            resonse.StatusMessage = "Departments are read successfully";
            return resonse;
        }

        async Task<ResponseObject<Employee>> IServices<Employee, int>.GetAsync(int id)
        {
            var record = await context.Employees.FindAsync(id);
            if (record == null)
            {
                resonse.StatucCode = 404;
                resonse.StatusMessage = "Employee is not found";
                return resonse;
            }
            resonse.Record = record;
            resonse.StatucCode = 200;
            resonse.StatusMessage = "Employee is  found";
            return resonse;
        }

        async Task<ResponseObject<Employee>> IServices<Employee, int>.UpdateAsync(int id, Employee entity)
        {
            context.Entry<Employee>(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
            resonse.StatucCode = 200;
            resonse.Record = entity;
            resonse.StatusMessage = "Employee updated successfuy";
            return resonse;

        }
    }
}
