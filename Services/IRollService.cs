using System.Collections.Generic;
using System.Threading.Tasks;
using Warehouse.API.Models;
using Warehouse.API.DTOs;

namespace Warehouse.API.Services
{
    public interface IRollService
    {
        Task<Roll> AddRollAsync(CreateRollDTO createRoll);
        Task<Roll?> RemoveRollAsync(int id);
        Task<IEnumerable<Roll>> GetRollsAsync(RollFilterDTO filter);
        Task<StatisticsResponseDTO> GetStatisticsAsync(StatisticsRequestDTO request);
    }
}