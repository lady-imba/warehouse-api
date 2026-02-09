using System;

namespace Warehouse.API.DTOs
{
    public class RollDTO
    {
        public int Id { get; set; }
        public double Length { get; set; }
        public double Weight { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? RemovedDate { get; set; }
    }
    
    public class CreateRollDTO
    {
        public double Length { get; set; }
        public double Weight { get; set; }
    }
    
    public class RangeFilterDTO<T> where T : struct
    {
        public T? Min { get; set; }
        public T? Max { get; set; }
    }
    
    public class RollFilterDTO
    {
        public RangeFilterDTO<int>? IdRange { get; set; }
        public RangeFilterDTO<double>? LengthRange { get; set; }
        public RangeFilterDTO<double>? WeightRange { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
    
    public class StatisticsRequestDTO
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
    
    public class StatisticsResponseDTO
{
    public int AddedRollsCount { get; set; }
    public int RemovedRollsCount { get; set; }
    public double AverageLength { get; set; }
    public double AverageWeight { get; set; }
    public double MinLength { get; set; }
    public double MaxLength { get; set; }
    public double MinWeight { get; set; }
    public double MaxWeight { get; set; }
    public double TotalWeight { get; set; }
    
    public string? MinStorageTime { get; set; } 
    public string? MaxStorageTime { get; set; }
}
}