using Application.UnitOfWorks;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.CustomLogs;
using TaskManagementSystem.CustomLogs.Dto;

namespace Application.CustomLogs
{
    public class CustomLogService : ICustomLogAppService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomLogService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<List<CustomLogDto>> GetAllCustomLogs()
        {
            var CustomLogs = await _unitOfWork.GetRepository<CustomLog>().GetAll();
            return _mapper.Map<List<CustomLogDto>>(CustomLogs);
        }

        public async Task AddCustomLog(CreateCustomLogDto CustomLog)
        {
            try
            {
                var aut = _mapper.Map<CustomLog>(CustomLog);
                await _unitOfWork.GetRepository<CustomLog>().Add(aut);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
