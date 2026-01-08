using System;
using System.Collections.Generic;

namespace ParkingLot
{
    /// <summary>
    /// 车辆品牌枚举
    /// </summary>
    public enum CarBrand
    {
        Toyota,      // 丰田
        Honda,       // 本田
        Volkswagen,  // 大众
        Ford,        // 福特
        Nissan,      // 日产
        Hyundai,     // 现代
        BYD,         // 比亚迪
        Geely,       // 吉利
        Other        // 其他
    }

    /// <summary>
    /// 车辆颜色枚举
    /// </summary>
    public enum CarColor
    {
        White,
        Black,
        Silver,
        Gray,
        Red,
        Blue,
        Green,
        Other
    }

    /// <summary>
    /// 车辆类型枚举（小停车场简化版）
    /// </summary>
    public enum VehicleType
    {
        Car,          // 普通汽车
        SUV,          // SUV
        ElectricCar,  // 电动车
        Motorcycle    // 摩托车
    }

    /// <summary>
    /// 车辆状态枚举
    /// </summary>
    public enum VehicleStatus
    {
        Parked,       // 已停车
        Reserved,     // 已预约
        Left          // 已离场
    }

    /// <summary>
    /// 车辆类
    /// </summary>
    public class Car
    {
        #region 基本信息属性
        /// <summary>
        /// 车牌号（主键）
        /// </summary>
        public string LicensePlate { get; private set; }
        
        /// <summary>
        /// 车辆类型
        /// </summary>
        public VehicleType Type { get; set; }
        
        /// <summary>
        /// 车辆品牌
        /// </summary>
        public CarBrand Brand { get; set; }
        
        /// <summary>
        /// 车辆颜色
        /// </summary>
        public CarColor Color { get; set; }
        
        /// <summary>
        /// 车主姓名
        /// </summary>
        public string OwnerName { get; set; }
        
        /// <summary>
        /// 车主电话
        /// </summary>
        public string OwnerPhone { get; set; }
        
        /// <summary>
        /// 是否为VIP车辆
        /// </summary>
        public bool IsVIP { get; set; }
        #endregion

        #region 停车信息属性
        /// <summary>
        /// 车辆当前状态
        /// </summary>
        public VehicleStatus Status { get; set; }
        
        /// <summary>
        /// 入场时间
        /// </summary>
        public DateTime? EntryTime { get; set; }
        
        /// <summary>
        /// 出场时间
        /// </summary>
        public DateTime? ExitTime { get; set; }
        
        /// <summary>
        /// 分配的车位编号
        /// </summary>
        public string AssignedSpaceId { get; set; }
        
        /// <summary>
        /// 预约开始时间
        /// </summary>
        public DateTime? ReservationStartTime { get; set; }
        
        /// <summary>
        /// 累计停车次数
        /// </summary>
        public int ParkingCount { get; set; }
        
        /// <summary>
        /// 累计消费金额
        /// </summary>
        public decimal TotalSpent { get; set; }
        #endregion

        #region 构造函数
        /// <summary>
        /// 创建车辆对象
        /// </summary>
        /// <param name="licensePlate">车牌号</param>
        /// <param name="type">车辆类型</param>
        public Car(string licensePlate, VehicleType type = VehicleType.Car)
        {
            if (string.IsNullOrWhiteSpace(licensePlate))
                throw new ArgumentException("车牌号不能为空");
                
            LicensePlate = licensePlate.Trim().ToUpper();
            Type = type;
            Status = VehicleStatus.Left;
            ParkingCount = 0;
            TotalSpent = 0;
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 车辆入场
        /// </summary>
        /// <param name="spaceId">车位编号</param>
        /// <returns>是否成功入场</returns>
        public bool EnterParking(string spaceId)
        {
            if (Status == VehicleStatus.Parked)
                return false; // 已在停车场内
                
            Status = VehicleStatus.Parked;
            EntryTime = DateTime.Now;
            AssignedSpaceId = spaceId;
            
            // 如果有预约，清除预约信息
            if (Status == VehicleStatus.Reserved)
            {
                ReservationStartTime = null;
            }
            
            return true;
        }
        
        /// <summary>
        /// 车辆出场
        /// </summary>
        /// <param name="fee">本次停车费用</param>
        /// <returns>是否成功出场</returns>
        public bool ExitParking(decimal fee)
        {
            if (Status != VehicleStatus.Parked)
                return false; // 不在停车场内
                
            Status = VehicleStatus.Left;
            ExitTime = DateTime.Now;
            ParkingCount++;
            TotalSpent += fee;
            
            // 清除停车信息
            AssignedSpaceId = null;
            EntryTime = null;
            
            return true;
        }
        
        /// <summary>
        /// 预约车位
        /// </summary>
        /// <param name="startTime">预约开始时间</param>
        /// <param name="spaceId">车位编号</param>
        /// <returns>是否预约成功</returns>
        public bool ReserveParking(DateTime startTime, string spaceId)
        {
            if (Status != VehicleStatus.Left)
                return false; // 当前状态不能预约
                
            Status = VehicleStatus.Reserved;
            ReservationStartTime = startTime;
            AssignedSpaceId = spaceId;
            return true;
        }
        
        /// <summary>
        /// 取消预约
        /// </summary>
        /// <returns>是否取消成功</returns>
        public bool CancelReservation()
        {
            if (Status != VehicleStatus.Reserved)
                return false;
                
            Status = VehicleStatus.Left;
            ReservationStartTime = null;
            AssignedSpaceId = null;
            return true;
        }
        
        /// <summary>
        /// 获取停车时长（分钟）
        /// </summary>
        /// <returns>停车时长，未停车返回null</returns>
        public int? GetParkingDuration()
        {
            if (!EntryTime.HasValue) 
                return null;
                
            var endTime = ExitTime ?? DateTime.Now;
            var duration = endTime - EntryTime.Value;
            return (int)duration.TotalMinutes;
        }
        
        /// <summary>
        /// 获取车辆描述信息
        /// </summary>
        public string GetDescription()
        {
            return $"{LicensePlate} - {Brand} ({Color}) {(IsVIP ? "[VIP]" : "")}";
        }
        
        /// <summary>
        /// 获取停车状态描述
        /// </summary>
        public string GetStatusDescription()
        {
            switch (Status)
            {
                case VehicleStatus.Parked:
                    var duration = GetParkingDuration();
                    return $"已停车 {duration}分钟，车位: {AssignedSpaceId}";
                case VehicleStatus.Reserved:
                    return $"已预约 {ReservationStartTime:HH:mm}，车位: {AssignedSpaceId}";
                case VehicleStatus.Left:
                    return $"已离场，停车次数: {ParkingCount}，累计消费: {TotalSpent:C}";
                default:
                    return "未知状态";
            }
        }
        
        /// <summary>
        /// 获取每小时费率
        /// </summary>
        public decimal GetHourlyRate()
        {
            switch (Type)
            {
                case VehicleType.ElectricCar:
                    return IsVIP ? 6m : 8m; // 电动车VIP 6元，普通8元
                case VehicleType.Motorcycle:
                    return IsVIP ? 3m : 5m; // 摩托车VIP 3元，普通5元
                default:
                    return IsVIP ? 8m : 10m; // 汽车VIP 8元，普通10元
            }
        }
        
        /// <summary>
        /// 计算本次停车费用
        /// </summary>
        public decimal CalculateParkingFee()
        {
            if (!EntryTime.HasValue) 
                return 0;
                
            var endTime = ExitTime ?? DateTime.Now;
            var hours = Math.Ceiling((endTime - EntryTime.Value).TotalHours);
            
            // 按小时计费，不足1小时按1小时计算
            var fee = GetHourlyRate() * (decimal)hours;
            
            // VIP车辆享受折扣
            if (IsVIP && hours > 2) // 停车超过2小时的VIP有折扣
            {
                fee *= 0.9m; // 9折
            }
            
            return Math.Round(fee, 2);
        }
        #endregion
    }

    /// <summary>
    /// 停车位类型枚举
    /// </summary>
    public enum ParkingSpaceType
    {
        Standard,      // 标准车位
        Compact,       // 紧凑型车位
        Electric,      // 电动车位（带充电桩）
        Motorcycle,    // 摩托车位
        VIP,           // VIP车位
        Disabled       // 无障碍车位
    }

    /// <summary>
    /// 停车位状态枚举
    /// </summary>
    public enum ParkingSpaceStatus
    {
        Available,     // 可用
        Occupied,      // 已占用
        Reserved,      // 已预约
        Maintenance,   // 维护中
        Disabled       // 禁用
    }

    /// <summary>
    /// 停车位类
    /// </summary>
    public class ParkingSpace
    {
        #region 基础属性
        /// <summary>
        /// 车位编号（唯一标识）
        /// </summary>
        public string Id { get; private set; }
        
        /// <summary>
        /// 车位类型
        /// </summary>
        public ParkingSpaceType Type { get; set; }
        
        /// <summary>
        /// 车位状态
        /// </summary>
        public ParkingSpaceStatus Status { get; set; }
        
        /// <summary>
        /// 车位位置描述（如：A区-01）
        /// </summary>
        public string Location { get; set; }
        
        /// <summary>
        /// 车位尺寸（平方米）
        /// </summary>
        public double Size { get; set; }
        
        /// <summary>
        /// 是否支持充电
        /// </summary>
        public bool HasCharging { get; set; }
        
        /// <summary>
        /// 是否有遮阳棚
        /// </summary>
        public bool HasShelter { get; set; }
        
        /// <summary>
        /// 是否靠近电梯
        /// </summary>
        public bool NearElevator { get; set; }
        
        /// <summary>
        /// 每小时费率
        /// </summary>
        public decimal HourlyRate { get; set; }
        #endregion

        #region 停车信息属性
        /// <summary>
        /// 当前停放车辆的车牌号
        /// </summary>
        public string CurrentCarLicense { get; private set; }
        
        /// <summary>
        /// 车位被占用的开始时间
        /// </summary>
        public DateTime? OccupiedStartTime { get; private set; }
        
        /// <summary>
        /// 车位预约信息
        /// </summary>
        public ReservationInfo Reservation { get; private set; }
        
        /// <summary>
        /// 今日累计收入
        /// </summary>
        public decimal TodayIncome { get; private set; }
        
        /// <summary>
        /// 今日停车次数
        /// </summary>
        public int TodayParkingCount { get; private set; }
        #endregion

        #region 构造函数
        /// <summary>
        /// 创建停车位对象
        /// </summary>
        /// <param name="id">车位编号</param>
        /// <param name="type">车位类型</param>
        public ParkingSpace(string id, ParkingSpaceType type = ParkingSpaceType.Standard)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("车位编号不能为空");
                
            Id = id.Trim();
            Type = type;
            Status = ParkingSpaceStatus.Available;
            
            // 设置默认费率
            HourlyRate = GetDefaultHourlyRate(type);
            
            // 根据类型设置其他属性
            InitializeSpaceProperties(type);
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 根据车位类型获取默认费率
        /// </summary>
        private decimal GetDefaultHourlyRate(ParkingSpaceType type)
        {
            switch (type)
            {
                case ParkingSpaceType.VIP:
                    return 15m;
                case ParkingSpaceType.Electric:
                    return 12m;
                case ParkingSpaceType.Disabled:
                    return 8m;
                case ParkingSpaceType.Standard:
                    return 10m;                
                case ParkingSpaceType.Compact:
                    return 8m;                
                case ParkingSpaceType.Motorcycle:
                    return 5m;
                default:
                    return 10m;
            }
        }
        
        /// <summary>
        /// 根据车位类型初始化属性
        /// </summary>
        private void InitializeSpaceProperties(ParkingSpaceType type)
        {
            HasCharging = (type == ParkingSpaceType.Electric);
            
            switch (type)
            {
                case ParkingSpaceType.Compact:
                    Size = 12.0; // 紧凑型车位12平米
                    break;
                case ParkingSpaceType.Motorcycle:
                    Size = 3.0; // 摩托车位3平米
                    break;
                default:
                    Size = 15.0; // 标准车位15平米
                    break;
            }
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 停车位是否可用
        /// </summary>
        public bool IsAvailable()
        {
            return Status == ParkingSpaceStatus.Available && 
                   string.IsNullOrEmpty(CurrentCarLicense);
        }
        
        /// <summary>
        /// 车辆停入车位
        /// </summary>
        /// <param name="carLicense">车牌号</param>
        /// <returns>是否成功停入</returns>
        public bool ParkCar(string carLicense)
        {
            if (!IsAvailable() || Status == ParkingSpaceStatus.Maintenance)
                return false;
                
            CurrentCarLicense = carLicense;
            OccupiedStartTime = DateTime.Now;
            Status = ParkingSpaceStatus.Occupied;
            TodayParkingCount++;
            return true;
        }
        
        /// <summary>
        /// 车辆离开车位
        /// </summary>
        /// <param name="fee">本次停车费用</param>
        /// <returns>是否成功离开</returns>
        public bool CarLeave(decimal fee)
        {
            if (string.IsNullOrEmpty(CurrentCarLicense))
                return false;
                
            CurrentCarLicense = null;
            OccupiedStartTime = null;
            Status = ParkingSpaceStatus.Available;
            TodayIncome += fee;
            return true;
        }
        
        /// <summary>
        /// 预约车位
        /// </summary>
        /// <param name="carLicense">车牌号</param>
        /// <param name="startTime">预约开始时间</param>
        /// <returns>是否预约成功</returns>
        public bool Reserve(string carLicense, DateTime startTime)
        {
            if (Status != ParkingSpaceStatus.Available || 
                !string.IsNullOrEmpty(CurrentCarLicense))
                return false;
                
            Reservation = new ReservationInfo
            {
                CarLicense = carLicense,
                StartTime = startTime,
                ReserveTime = DateTime.Now
            };
            
            Status = ParkingSpaceStatus.Reserved;
            return true;
        }
        
        /// <summary>
        /// 取消预约
        /// </summary>
        /// <returns>是否取消成功</returns>
        public bool CancelReservation()
        {
            if (Status != ParkingSpaceStatus.Reserved)
                return false;
                
            Reservation = null;
            Status = ParkingSpaceStatus.Available;
            return true;
        }
        
        /// <summary>
        /// 设置车位维护状态
        /// </summary>
        /// <param name="inMaintenance">是否维护中</param>
        public void SetMaintenance(bool inMaintenance)
        {
            if (inMaintenance)
            {
                Status = ParkingSpaceStatus.Maintenance;
                CurrentCarLicense = null;
                OccupiedStartTime = null;
            }
            else
            {
                Status = ParkingSpaceStatus.Available;
            }
        }
        
        /// <summary>
        /// 获取占用时长（分钟）
        /// </summary>
        public int? GetOccupationDuration()
        {
            if (!OccupiedStartTime.HasValue) 
                return null;
                
            var duration = DateTime.Now - OccupiedStartTime.Value;
            return (int)duration.TotalMinutes;
        }
        
        /// <summary>
        /// 计算当前停车费用
        /// </summary>
        public decimal CalculateCurrentFee()
        {
            if (!OccupiedStartTime.HasValue || string.IsNullOrEmpty(CurrentCarLicense))
                return 0;
                
            var hours = Math.Ceiling((DateTime.Now - OccupiedStartTime.Value).TotalHours);
            return HourlyRate * (decimal)hours;
        }
        
        /// <summary>
        /// 重置今日统计信息
        /// </summary>
        public void ResetDailyStats()
        {
            TodayIncome = 0;
            TodayParkingCount = 0;
        }
        
        /// <summary>
        /// 获取车位描述信息
        /// </summary>
        public string GetDescription()
        {
            return $"{Id} - {Location} [{Type}] {(HasCharging ? "[充电]" : "")}";
        }
        
        /// <summary>
        /// 获取车位状态描述
        /// </summary>
        public string GetStatusDescription()
        {
            switch (Status)
            {
                case ParkingSpaceStatus.Available:
                    return "空闲";
                case ParkingSpaceStatus.Occupied:
                    var duration = GetOccupationDuration();
                    return $"占用 {duration}分钟，车牌: {CurrentCarLicense}";
                case ParkingSpaceStatus.Reserved:
                    return $"已预约，车牌: {Reservation?.CarLicense}";
                case ParkingSpaceStatus.Maintenance:
                    return "维护中";
                case ParkingSpaceStatus.Disabled:
                    return "禁用";
                default:
                    return "未知";
            }
        }
        
        /// <summary>
        /// 车辆是否可以停放在此车位
        /// </summary>
        public bool CanAcceptCar(Car car)
        {
            // 检查车位状态
            if (Status != ParkingSpaceStatus.Available && Status != ParkingSpaceStatus.Reserved)
                return false;
                
            // 检查车位类型与车辆类型是否匹配
            switch (car.Type)
            {
                case VehicleType.ElectricCar:
                    // 电动车只能停在电动车位或标准车位
                    return Type == ParkingSpaceType.Electric || 
                           Type == ParkingSpaceType.Standard || 
                           Type == ParkingSpaceType.VIP;
                           
                case VehicleType.Motorcycle:
                    // 摩托车只能停在摩托车位或紧凑型车位
                    return Type == ParkingSpaceType.Motorcycle || 
                           Type == ParkingSpaceType.Compact;
                           
                case VehicleType.SUV:
                    // SUV不能停在紧凑型车位
                    return Type != ParkingSpaceType.Compact;
                           
                default:
                    // 普通汽车可以停在所有车位（除了摩托车位）
                    return Type != ParkingSpaceType.Motorcycle;
            }
        }
        #endregion
    }

    /// <summary>
    /// 预约信息类
    /// </summary>
    public class ReservationInfo
    {
        /// <summary>
        /// 预约车辆车牌号
        /// </summary>
        public string CarLicense { get; set; }
        
        /// <summary>
        /// 预约开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        
        /// <summary>
        /// 预约时间
        /// </summary>
        public DateTime ReserveTime { get; set; }
    }
}