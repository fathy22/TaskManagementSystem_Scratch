using Application.UnitOfWorks;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Tasks;
using TaskManagementSystem.TaskSheets;
using TaskManagementSystem.TaskSheets.Dto;

namespace Application.TaskSheets
{
    public class TaskSheetAppService : ITaskSheetAppService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TaskSheetAppService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<List<TaskSheetDto>> GetAllTaskSheets()
        {
            var TaskSheets = await _unitOfWork.GetRepository<TaskSheet>().GetAll();
            return _mapper.Map<List<TaskSheetDto>>(TaskSheets);
        }

        public async Task<TaskSheetDto> GetTaskSheetById(int id)
        {
            var TaskSheet = await _unitOfWork.GetRepository<TaskSheet>().GetById(id);
            return _mapper.Map<TaskSheetDto>(TaskSheet);

        }

        public async Task AddTaskSheet(CreateTaskSheetDto TaskSheet)
        {
            try
            {
                var aut = _mapper.Map<TaskSheet>(TaskSheet);
                await _unitOfWork.GetRepository<TaskSheet>().Add(aut);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task UpdateTaskSheet(TaskSheetDto TaskSheet)
        {
            try
            {
                var existingTaskSheet = await _unitOfWork.GetRepository<TaskSheet>().GetById(TaskSheet.Id);

                if (existingTaskSheet == null)
                {
                    return;
                }
                _mapper.Map(TaskSheet, existingTaskSheet);

                await _unitOfWork.GetRepository<TaskSheet>().Update(existingTaskSheet);

                _unitOfWork.Save();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task DeleteTaskSheet(int id)
        {
            var existingTaskSheet = await _unitOfWork.GetRepository<TaskSheet>().GetById(id);

            if (existingTaskSheet == null)
            {
                return;
            }
            await _unitOfWork.GetRepository<TaskSheet>().Delete(existingTaskSheet);
            _unitOfWork.Save();

        }
    }
}
