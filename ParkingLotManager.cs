using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParkingLot;
namespace ParkingLotManager
{
    /// <summary>
    /// 停车场管理类
    /// </summary>
    public class ParkingLotManager
    {
        #region 单例模式实现
        private static ParkingLotManager _instance;
        private static readonly object _lock = new object();

        /// <summary>
        /// 获取停车场管理单例
        /// </summary>
        public static ParkingLotManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new ParkingLotManager();
                        }
                    }
                }
                return _instance;
            }
        }

        private ParkingLotManager()
        {
            InitializeParkingLot();
        }
        #endregion

        #region 数据存储
        /// <summary>
        /// 所有车辆字典（车牌号 -> 车辆对象）
        /// </summary>
        private Dictionary<string, Car> _allCars = new Dictionary<string, Car>();

        /// <summary>
        /// 所有停车位字典（车位ID -> 停车位对象）
        /// </summary>
        private Dictionary<string, ParkingSpace> _allParkingSpaces = new Dictionary<string, ParkingSpace>();

        /// <summary>
        /// 停车记录列表
        /// </summary>
        private List<ParkingRecord> _parkingRecords = new List<ParkingRecord>();

        /// <summary>
        /// 收费记录列表
        /// </summary>
        private List<PaymentRecord> _paymentRecords = new List<PaymentRecord>();

        /// <summary>
        /// 违规记录列表
        /// </summary>
        private List<ViolationRecord> _violationRecords = new List<ViolationRecord>();
        #endregion

        #region 停车场配置
        /// <summary>
        /// 停车场名称
        /// </summary>
        public string ParkingLotName { get; set; } = "智慧停车场";

        /// <summary>
        /// 停车场总容量
        /// </summary>
        public int TotalCapacity { get; private set; }

        /// <summary>
        /// 停车场当前占用数
        /// </summary>
        public int CurrentOccupancy
        {
            get
            {
                return _allParkingSpaces.Values.Count(space =>
                    space.Status == ParkingSpaceStatus.Occupied ||
                    space.Status == ParkingSpaceStatus.Reserved);
            }
        }

        /// <summary>
        /// 停车场可用车位数
        /// </summary>
        public int AvailableSpaces
        {
            get
            {
                return _allParkingSpaces.Values.Count(space =>
                    space.Status == ParkingSpaceStatus.Available);
            }
        }

        /// <summary>
        /// 今日总收入
        /// </summary>
        public decimal TodayTotalIncome
        {
            get
            {
                return _allParkingSpaces.Values.Sum(space => space.TodayIncome);
            }
        }

        /// <summary>
        /// 今日总停车次数
        /// </summary>
        public int TodayTotalParkingCount
        {
            get
            {
                return _allParkingSpaces.Values.Sum(space => space.TodayParkingCount);
            }
        }
        #endregion

        #region 初始化方法
        /// <summary>
        /// 初始化停车场（创建默认车位）
        /// </summary>
        private void InitializeParkingLot()
        {
            // 创建20个标准车位
            for (int i = 1; i <= 20; i++)
            {
                var space = new ParkingSpace($"S{i:00}", ParkingSpaceType.Standard)
                {
                    Location = $"A区-{i:00}",
                    HasShelter = (i % 2 == 0), // 偶数车位有遮阳棚
                    NearElevator = (i <= 5) // 前5个车位靠近电梯
                };
                AddParkingSpace(space);
            }

            // 创建10个紧凑型车位
            for (int i = 1; i <= 10; i++)
            {
                var space = new ParkingSpace($"C{i:00}", ParkingSpaceType.Compact)
                {
                    Location = $"B区-{i:00}"
                };
                AddParkingSpace(space);
            }

            // 创建5个电动车位
            for (int i = 1; i <= 5; i++)
            {
                var space = new ParkingSpace($"E{i:00}", ParkingSpaceType.Electric)
                {
                    Location = $"C区-{i:00}",
                    HasCharging = true
                };
                AddParkingSpace(space);
            }

            // 创建5个摩托车位
            for (int i = 1; i <= 5; i++)
            {
                var space = new ParkingSpace($"M{i:00}", ParkingSpaceType.Motorcycle)
                {
                    Location = $"D区-{i:00}"
                };
                AddParkingSpace(space);
            }

            // 创建5个VIP车位
            for (int i = 1; i <= 5; i++)
            {
                var space = new ParkingSpace($"V{i:00}", ParkingSpaceType.VIP)
                {
                    Location = $"VIP区-{i:00}",
                    HasShelter = true,
                    NearElevator = true
                };
                AddParkingSpace(space);
            }

            // 创建2个无障碍车位
            for (int i = 1; i <= 2; i++)
            {
                var space = new ParkingSpace($"D{i:00}", ParkingSpaceType.Disabled)
                {
                    Location = $"无障碍区-{i:00}",
                    NearElevator = true
                };
                AddParkingSpace(space);
            }

            TotalCapacity = _allParkingSpaces.Count;

            // 创建一些示例车辆
            AddSampleCars();
        }

        /// <summary>
        /// 添加示例车辆
        /// </summary>
        private void AddSampleCars()
        {
            var car1 = new Car("浙A12345", VehicleType.Car)
            {
                Brand = CarBrand.Toyota,
                Color = CarColor.Black,
                OwnerName = "张三",
                OwnerPhone = "13800138000",
                IsVIP = true
            };
            RegisterCar(car1);

            var car2 = new Car("浙A88888", VehicleType.ElectricCar)
            {
                Brand = CarBrand.BYD,
                Color = CarColor.White,
                OwnerName = "李四",
                OwnerPhone = "13900139000"
            };
            RegisterCar(car2);

            var car3 = new Car("浙B66666", VehicleType.SUV)
            {
                Brand = CarBrand.Volkswagen,
                Color = CarColor.Silver,
                OwnerName = "王五",
                OwnerPhone = "13700137000",
                IsVIP = true
            };
            RegisterCar(car3);

            var car4 = new Car("浙C99999", VehicleType.Motorcycle)
            {
                Brand = CarBrand.Honda,
                Color = CarColor.Red,
                OwnerName = "赵六",
                OwnerPhone = "13600136000"
            };
            RegisterCar(car4);
        }
        #endregion

        #region 车辆管理方法
        /// <summary>
        /// 注册车辆
        /// </summary>
        /// <param name="car">车辆对象</param>
        /// <returns>是否注册成功</returns>
        public bool RegisterCar(Car car)
        {
            if (car == null || string.IsNullOrEmpty(car.LicensePlate))
                return false;

            if (_allCars.ContainsKey(car.LicensePlate))
                return false;

            _allCars[car.LicensePlate] = car;
            return true;
        }

        /// <summary>
        /// 注销车辆
        /// </summary>
        /// <param name="licensePlate">车牌号</param>
        /// <returns>是否注销成功</param>
        public bool UnregisterCar(string licensePlate)
        {
            if (string.IsNullOrEmpty(licensePlate))
                return false;

            // 检查车辆是否在停车场内
            var car = GetCarByLicense(licensePlate);
            if (car != null && car.Status == VehicleStatus.Parked)
                return false; // 车辆在停车场内，不能注销

            return _allCars.Remove(licensePlate);
        }

        /// <summary>
        /// 根据车牌号获取车辆
        /// </summary>
        public Car GetCarByLicense(string licensePlate)
        {
            if (string.IsNullOrEmpty(licensePlate))
                return null;

            _allCars.TryGetValue(licensePlate, out var car);
            return car;
        }

        /// <summary>
        /// 获取所有注册车辆
        /// </summary>
        public List<Car> GetAllCars()
        {
            return _allCars.Values.ToList();
        }

        /// <summary>
        /// 获取正在停车的车辆
        /// </summary>
        public List<Car> GetParkedCars()
        {
            return _allCars.Values
                .Where(car => car.Status == VehicleStatus.Parked)
                .ToList();
        }

        /// <summary>
        /// 获取VIP车辆
        /// </summary>
        public List<Car> GetVipCars()
        {
            return _allCars.Values
                .Where(car => car.IsVIP)
                .ToList();
        }

        /// <summary>
        /// 搜索车辆
        /// </summary>
        public List<Car> SearchCars(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
                return GetAllCars();

            keyword = keyword.ToLower();
            return _allCars.Values
                .Where(car =>
                    car.LicensePlate.ToLower().Contains(keyword) ||
                    car.OwnerName?.ToLower().Contains(keyword) == true ||
                    car.OwnerPhone?.Contains(keyword) == true)
                .ToList();
        }
        #endregion

        #region 车位管理方法
        /// <summary>
        /// 添加停车位
        /// </summary>
        public bool AddParkingSpace(ParkingSpace space)
        {
            if (space == null || string.IsNullOrEmpty(space.Id))
                return false;

            if (_allParkingSpaces.ContainsKey(space.Id))
                return false;

            _allParkingSpaces[space.Id] = space;
            TotalCapacity = _allParkingSpaces.Count;
            return true;
        }

        /// <summary>
        /// 移除停车位
        /// </summary>
        public bool RemoveParkingSpace(string spaceId)
        {
            if (string.IsNullOrEmpty(spaceId))
                return false;

            // 检查车位是否被占用
            if (_allParkingSpaces.TryGetValue(spaceId, out var space))
            {
                if (space.Status == ParkingSpaceStatus.Occupied ||
                    space.Status == ParkingSpaceStatus.Reserved)
                    return false; // 车位被占用或已预约，不能移除
            }

            var result = _allParkingSpaces.Remove(spaceId);
            if (result)
                TotalCapacity = _allParkingSpaces.Count;

            return result;
        }

        /// <summary>
        /// 根据车位ID获取车位
        /// </summary>
        public ParkingSpace GetParkingSpaceById(string spaceId)
        {
            if (string.IsNullOrEmpty(spaceId))
                return null;

            _allParkingSpaces.TryGetValue(spaceId, out var space);
            return space;
        }

        /// <summary>
        /// 获取所有停车位
        /// </summary>
        public List<ParkingSpace> GetAllParkingSpaces()
        {
            return _allParkingSpaces.Values.ToList();
        }

        /// <summary>
        /// 获取可用停车位
        /// </summary>
        public List<ParkingSpace> GetAvailableSpaces()
        {
            return _allParkingSpaces.Values
                .Where(space => space.IsAvailable())
                .ToList();
        }

        /// <summary>
        /// 获取占用中的停车位
        /// </summary>
        public List<ParkingSpace> GetOccupiedSpaces()
        {
            return _allParkingSpaces.Values
                .Where(space => space.Status == ParkingSpaceStatus.Occupied)
                .ToList();
        }

        /// <summary>
        /// 根据车辆类型推荐可用车位
        /// </summary>
        public List<ParkingSpace> GetRecommendedSpaces(Car car)
        {
            if (car == null)
                return new List<ParkingSpace>();

            return _allParkingSpaces.Values
                .Where(space => space.IsAvailable() && space.CanAcceptCar(car))
                .OrderBy(space =>
                {
                    // 优先级排序：VIP车位 > 靠近电梯 > 有遮阳棚 > 普通车位
                    int priority = 0;
                    if (space.Type == ParkingSpaceType.VIP) priority += 100;
                    if (space.NearElevator) priority += 50;
                    if (space.HasShelter) priority += 20;
                    return -priority; // 降序排列
                })
                .ThenBy(space => space.Id) // 然后按ID排序
                .ToList();
        }

        /// <summary>
        /// 设置车位维护状态
        /// </summary>
        public bool SetSpaceMaintenance(string spaceId, bool inMaintenance)
        {
            var space = GetParkingSpaceById(spaceId);
            if (space == null)
                return false;

            if (inMaintenance && space.Status == ParkingSpaceStatus.Occupied)
                return false; // 被占用的车位不能设置为维护

            space.SetMaintenance(inMaintenance);
            return true;
        }

        /// <summary>
        /// 批量设置车位费率
        /// </summary>
        public void BatchSetSpaceRates(ParkingSpaceType spaceType, decimal newRate)
        {
            foreach (var space in _allParkingSpaces.Values)
            {
                if (space.Type == spaceType)
                {
                    space.HourlyRate = newRate;
                }
            }
        }
        #endregion

        #region 停车进出管理
        /// <summary>
        /// 车辆入场
        /// </summary>
        /// <param name="licensePlate">车牌号</param>
        /// <param name="preferredSpaceId">优先选择的车位ID（可选）</param>
        /// <returns>停车信息，包含分配的车位和车辆信息</returns>
        public ParkingInfo CarEnter(string licensePlate, string preferredSpaceId = null)
        {
            if (string.IsNullOrEmpty(licensePlate))
                return new ParkingInfo { Success = false, Message = "车牌号不能为空" };

            // 检查车辆是否已注册
            var car = GetCarByLicense(licensePlate);
            if (car == null)
            {
                // 未注册的车辆，自动创建临时车辆记录
                car = new Car(licensePlate, VehicleType.Car);
                RegisterCar(car);
            }

            // 检查车辆是否已在停车场内
            if (car.Status == VehicleStatus.Parked)
                return new ParkingInfo { Success = false, Message = "该车辆已在停车场内" };

            // 检查是否有空闲车位
            if (AvailableSpaces == 0)
                return new ParkingInfo { Success = false, Message = "停车场已满" };

            ParkingSpace assignedSpace = null;

            // 如果指定了优先车位
            if (!string.IsNullOrEmpty(preferredSpaceId))
            {
                var preferredSpace = GetParkingSpaceById(preferredSpaceId);
                if (preferredSpace != null && preferredSpace.IsAvailable() &&
                    preferredSpace.CanAcceptCar(car))
                {
                    assignedSpace = preferredSpace;
                }
            }

            // 如果没有指定或指定车位不可用，则自动分配
            if (assignedSpace == null)
            {
                var recommendedSpaces = GetRecommendedSpaces(car);
                if (recommendedSpaces.Count == 0)
                    return new ParkingInfo { Success = false, Message = "没有合适的车位" };

                assignedSpace = recommendedSpaces.First();
            }

            // 分配车位
            if (!assignedSpace.ParkCar(licensePlate))
                return new ParkingInfo { Success = false, Message = "车位分配失败" };

            // 更新车辆状态
            if (!car.EnterParking(assignedSpace.Id))
            {
                // 如果车辆入场失败，回滚车位状态
                assignedSpace.CarLeave(0);
                return new ParkingInfo { Success = false, Message = "车辆入场失败" };
            }

            // 创建停车记录
            var parkingRecord = new ParkingRecord
            {
                Id = GenerateRecordId(),
                LicensePlate = licensePlate,
                SpaceId = assignedSpace.Id,
                EntryTime = car.EntryTime.Value,
                IsReserved = (car.Status == VehicleStatus.Reserved),
                RecordTime = DateTime.Now
            };
            _parkingRecords.Add(parkingRecord);

            // 如果车辆有预约，清除预约状态
            if (car.Status == VehicleStatus.Reserved)
            {
                assignedSpace.CancelReservation();
            }

            return new ParkingInfo
            {
                Success = true,
                Message = "车辆入场成功",
                Car = car,
                AssignedSpace = assignedSpace,
                ParkingRecord = parkingRecord
            };
        }

        /// <summary>
        /// 车辆出场
        /// </summary>
        public ParkingInfo CarExit(string licensePlate)
        {
            if (string.IsNullOrEmpty(licensePlate))
                return new ParkingInfo { Success = false, Message = "车牌号不能为空" };

            var car = GetCarByLicense(licensePlate);
            if (car == null)
                return new ParkingInfo { Success = false, Message = "车辆未注册" };

            if (car.Status != VehicleStatus.Parked)
                return new ParkingInfo { Success = false, Message = "车辆不在停车场内" };

            var space = GetParkingSpaceById(car.AssignedSpaceId);
            if (space == null)
                return new ParkingInfo { Success = false, Message = "车位信息错误" };

            // 计算停车费用
            decimal fee = car.CalculateParkingFee();

            // 如果车辆是电动车且车位有充电桩，计算充电费用
            if (car.Type == VehicleType.ElectricCar && space.HasCharging)
            {
                fee += CalculateChargingFee(car);
            }

            // 创建收费记录
            var paymentRecord = new PaymentRecord
            {
                Id = GenerateRecordId(),
                LicensePlate = licensePlate,
                SpaceId = space.Id,
                EntryTime = car.EntryTime.Value,
                ExitTime = DateTime.Now,
                ParkingFee = fee,
                PaymentTime = DateTime.Now,
                PaymentMethod = "现金", // 默认为现金支付
                IsPaid = false
            };
            _paymentRecords.Add(paymentRecord);

            // 车辆出场
            if (!space.CarLeave(fee))
                return new ParkingInfo { Success = false, Message = "车位释放失败" };

            if (!car.ExitParking(fee))
            {
                // 如果车辆出场失败，恢复车位状态
                space.ParkCar(licensePlate);
                return new ParkingInfo { Success = false, Message = "车辆出场失败" };
            }

            // 更新停车记录的出场时间
            var parkingRecord = _parkingRecords
                .FirstOrDefault(r => r.LicensePlate == licensePlate &&
                                    r.SpaceId == space.Id &&
                                    r.ExitTime == null);
            if (parkingRecord != null)
            {
                parkingRecord.ExitTime = car.ExitTime;
                parkingRecord.TotalFee = fee;
            }

            return new ParkingInfo
            {
                Success = true,
                Message = $"车辆出场成功，费用: {fee:C}",
                Car = car,
                AssignedSpace = space,
                ParkingFee = fee,
                PaymentRecord = paymentRecord
            };
        }

        /// <summary>
        /// 预约车位
        /// </summary>
        public ReservationInfo ReserveSpace(string licensePlate, DateTime startTime, string preferredSpaceId = null)
        {
            if (string.IsNullOrEmpty(licensePlate))
                return new ReservationInfo { Success = false, Message = "车牌号不能为空" };

            var car = GetCarByLicense(licensePlate);
            if (car == null)
            {
                car = new Car(licensePlate, VehicleType.Car);
                RegisterCar(car);
            }

            if (car.Status != VehicleStatus.Left)
                return new ReservationInfo { Success = false, Message = "车辆当前状态不能预约" };

            ParkingSpace space = null;

            // 如果指定了车位
            if (!string.IsNullOrEmpty(preferredSpaceId))
            {
                space = GetParkingSpaceById(preferredSpaceId);
                if (space == null || !space.CanAcceptCar(car))
                    return new ReservationInfo { Success = false, Message = "指定车位不可用" };
            }
            else
            {
                // 自动推荐车位
                var recommendedSpaces = GetRecommendedSpaces(car);
                if (recommendedSpaces.Count == 0)
                    return new ReservationInfo { Success = false, Message = "没有合适的车位" };

                space = recommendedSpaces.First();
            }

            // 检查车位是否可用
            if (!space.IsAvailable())
                return new ReservationInfo { Success = false, Message = "车位已被占用" };

            // 预约车位
            if (!space.Reserve(licensePlate, startTime))
                return new ReservationInfo { Success = false, Message = "车位预约失败" };

            // 更新车辆状态
            if (!car.ReserveParking(startTime, space.Id))
            {
                space.CancelReservation();
                return new ReservationInfo { Success = false, Message = "车辆预约失败" };
            }

            return new ReservationInfo
            {
                Success = true,
                Message = "预约成功",
                Car = car,
                ReservedSpace = space,
                StartTime = startTime
            };
        }

        /// <summary>
        /// 取消预约
        /// </summary>
        public bool CancelReservation(string licensePlate)
        {
            var car = GetCarByLicense(licensePlate);
            if (car == null || car.Status != VehicleStatus.Reserved)
                return false;

            var space = GetParkingSpaceById(car.AssignedSpaceId);
            if (space == null || space.Status != ParkingSpaceStatus.Reserved)
                return false;

            // 更新车位状态
            if (!space.CancelReservation())
                return false;

            // 更新车辆状态
            if (!car.CancelReservation())
                return false;

            return true;
        }
        #endregion

        #region 收费管理
        /// <summary>
        /// 计算充电费用（简化计算）
        /// </summary>
        private decimal CalculateChargingFee(Car car)
        {
            // 假设电动车充电费率：1元/小时
            if (car.Type != VehicleType.ElectricCar)
                return 0;

            var duration = car.GetParkingDuration();
            if (!duration.HasValue)
                return 0;

            var chargingHours = Math.Ceiling(duration.Value / 60.0);
            return (decimal)chargingHours * 1.0m;
        }

        /// <summary>
        /// 处理支付
        /// </summary>
        public PaymentResult ProcessPayment(string licensePlate, decimal amount, string paymentMethod)
        {
            var paymentRecord = _paymentRecords
                .FirstOrDefault(r => r.LicensePlate == licensePlate && !r.IsPaid);

            if (paymentRecord == null)
                return new PaymentResult { Success = false, Message = "未找到待支付的记录" };

            if (amount < paymentRecord.ParkingFee)
                return new PaymentResult { Success = false, Message = "支付金额不足" };

            // 更新支付记录
            paymentRecord.IsPaid = true;
            paymentRecord.PaymentTime = DateTime.Now;
            paymentRecord.PaymentMethod = paymentMethod;
            paymentRecord.AmountReceived = amount;
            paymentRecord.Change = amount - paymentRecord.ParkingFee;

            // 如果车辆是VIP，记录积分
            var car = GetCarByLicense(licensePlate);
            if (car != null && car.IsVIP)
            {
                // VIP每消费10元积1分
                int points = (int)(paymentRecord.ParkingFee / 10);
                // 这里可以记录积分到车辆或用户账户
            }

            return new PaymentResult
            {
                Success = true,
                Message = "支付成功",
                PaymentRecord = paymentRecord,
                Change = (decimal)paymentRecord.Change
            };
        }

        /// <summary>
        /// 获取未支付的停车记录
        /// </summary>
        public List<PaymentRecord> GetUnpaidRecords()
        {
            return _paymentRecords
                .Where(r => !r.IsPaid)
                .OrderByDescending(r => r.ExitTime)
                .ToList();
        }

        /// <summary>
        /// 获取今日收费记录
        /// </summary>
        public List<PaymentRecord> GetTodayPaymentRecords()
        {
            var today = DateTime.Today;
            return _paymentRecords
                .Where(r => r.PaymentTime.Date == today && r.IsPaid)
                .OrderByDescending(r => r.PaymentTime)
                .ToList();
        }

        /// <summary>
        /// 获取收入统计
        /// </summary>
        public IncomeStatistics GetIncomeStatistics(DateTime date)
        {
            var records = _paymentRecords
                .Where(r => r.PaymentTime.Date == date.Date && r.IsPaid)
                .ToList();

            return new IncomeStatistics
            {
                Date = date,
                TotalIncome = records.Sum(r => r.ParkingFee),
                TotalTransactions = records.Count,
                CashIncome = records.Where(r => r.PaymentMethod == "现金").Sum(r => r.ParkingFee),
                CardIncome = records.Where(r => r.PaymentMethod == "刷卡").Sum(r => r.ParkingFee),
                MobileIncome = records.Where(r => r.PaymentMethod == "手机支付").Sum(r => r.ParkingFee)
            };
        }
        #endregion

        #region 记录管理
        /// <summary>
        /// 获取停车记录
        /// </summary>
        public List<ParkingRecord> GetParkingRecords(string licensePlate = null, DateTime? date = null)
        {
            var query = _parkingRecords.AsQueryable();

            if (!string.IsNullOrEmpty(licensePlate))
                query = query.Where(r => r.LicensePlate == licensePlate);

            if (date.HasValue)
                query = query.Where(r => r.EntryTime.Date == date.Value.Date);

            return query.OrderByDescending(r => r.EntryTime).ToList();
        }

        /// <summary>
        /// 获取车辆停车历史
        /// </summary>
        public List<ParkingHistory> GetCarParkingHistory(string licensePlate)
        {
            var records = GetParkingRecords(licensePlate);
            return records.Select(r => new ParkingHistory
            {
                EntryTime = r.EntryTime,
                ExitTime = r.ExitTime,
                SpaceId = r.SpaceId,
                Duration = r.ExitTime.HasValue ?
                    (int)(r.ExitTime.Value - r.EntryTime).TotalMinutes : 0,
                Fee = r.TotalFee
            }).ToList();
        }

        /// <summary>
        /// 记录违规
        /// </summary>
        public bool RecordViolation(string licensePlate, string spaceId, string violationType, string description)
        {
            var car = GetCarByLicense(licensePlate);
            if (car == null)
                return false;

            var record = new ViolationRecord
            {
                Id = GenerateRecordId(),
                LicensePlate = licensePlate,
                SpaceId = spaceId,
                ViolationType = violationType,
                Description = description,
                RecordTime = DateTime.Now,
                IsHandled = false
            };

            _violationRecords.Add(record);
            return true;
        }

        /// <summary>
        /// 获取未处理的违规记录
        /// </summary>
        public List<ViolationRecord> GetUnhandledViolations()
        {
            return _violationRecords
                .Where(r => !r.IsHandled)
                .OrderByDescending(r => r.RecordTime)
                .ToList();
        }
        #endregion

        #region 工具方法
        /// <summary>
        /// 生成记录ID（时间戳+随机数）
        /// </summary>
        private string GenerateRecordId()
        {
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            var random = new Random().Next(1000, 9999);
            return $"{timestamp}{random}";
        }

        /// <summary>
        /// 重置所有车位的今日统计
        /// </summary>
        public void ResetAllDailyStats()
        {
            foreach (var space in _allParkingSpaces.Values)
            {
                space.ResetDailyStats();
            }
        }

        /// <summary>
        /// 获取停车场状态摘要
        /// </summary>
        public string GetParkingLotSummary()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"停车场: {ParkingLotName}");
            sb.AppendLine($"总车位: {TotalCapacity}");
            sb.AppendLine($"已占用: {CurrentOccupancy}");
            sb.AppendLine($"可用车位: {AvailableSpaces}");
            sb.AppendLine($"占用率: {(TotalCapacity > 0 ? CurrentOccupancy * 100.0 / TotalCapacity : 0):F1}%");
            sb.AppendLine($"今日收入: {TodayTotalIncome:C}");
            sb.AppendLine($"今日停车次数: {TodayTotalParkingCount}");

            // 按类型统计
            sb.AppendLine("\n车位类型统计:");
            foreach (ParkingSpaceType type in Enum.GetValues(typeof(ParkingSpaceType)))
            {
                var count = _allParkingSpaces.Values.Count(s => s.Type == type);
                var occupied = _allParkingSpaces.Values.Count(s => s.Type == type && s.Status == ParkingSpaceStatus.Occupied);
                sb.AppendLine($"  {type}: {occupied}/{count}");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 导出数据到文件（简化示例）
        /// </summary>
        public string ExportData()
        {
            var sb = new StringBuilder();
            sb.AppendLine("停车场数据导出");
            sb.AppendLine($"导出时间: {DateTime.Now}");
            sb.AppendLine();

            sb.AppendLine("车辆列表:");
            foreach (var car in _allCars.Values)
            {
                sb.AppendLine($"  {car.GetDescription()}, 状态: {car.GetStatusDescription()}");
            }

            sb.AppendLine("\n车位列表:");
            foreach (var space in _allParkingSpaces.Values)
            {
                sb.AppendLine($"  {space.GetDescription()}, 状态: {space.GetStatusDescription()}");
            }

            return sb.ToString();
        }
        #endregion
    }

    #region 辅助类定义
    /// <summary>
    /// 停车信息类
    /// </summary>
    public class ParkingInfo
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Car Car { get; set; }
        public ParkingSpace AssignedSpace { get; set; }
        public decimal ParkingFee { get; set; }
        public ParkingRecord ParkingRecord { get; set; }
        public PaymentRecord PaymentRecord { get; set; }
    }

    /// <summary>
    /// 预约信息类
    /// </summary>
    public class ReservationInfo
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Car Car { get; set; }
        public ParkingSpace ReservedSpace { get; set; }
        public DateTime StartTime { get; set; }
    }

    /// <summary>
    /// 支付结果类
    /// </summary>
    public class PaymentResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public PaymentRecord PaymentRecord { get; set; }
        public decimal Change { get; set; }
    }

    /// <summary>
    /// 停车记录类
    /// </summary>
    public class ParkingRecord
    {
        public string Id { get; set; }
        public string LicensePlate { get; set; }
        public string SpaceId { get; set; }
        public DateTime EntryTime { get; set; }
        public DateTime? ExitTime { get; set; }
        public decimal? TotalFee { get; set; }
        public bool IsReserved { get; set; }
        public DateTime RecordTime { get; set; }
    }

    /// <summary>
    /// 收费记录类
    /// </summary>
    public class PaymentRecord
    {
        public string Id { get; set; }
        public string LicensePlate { get; set; }
        public string SpaceId { get; set; }
        public DateTime EntryTime { get; set; }
        public DateTime? ExitTime { get; set; }
        public decimal ParkingFee { get; set; }
        public DateTime PaymentTime { get; set; }
        public string PaymentMethod { get; set; }
        public bool IsPaid { get; set; }
        public decimal? AmountReceived { get; set; }
        public decimal? Change { get; set; }
    }

    /// <summary>
    /// 违规记录类
    /// </summary>
    public class ViolationRecord
    {
        public string Id { get; set; }
        public string LicensePlate { get; set; }
        public string SpaceId { get; set; }
        public string ViolationType { get; set; }
        public string Description { get; set; }
        public DateTime RecordTime { get; set; }
        public bool IsHandled { get; set; }
    }

    /// <summary>
    /// 停车历史类
    /// </summary>
    public class ParkingHistory
    {
        public DateTime EntryTime { get; set; }
        public DateTime? ExitTime { get; set; }
        public string SpaceId { get; set; }
        public int Duration { get; set; } // 分钟
        public decimal? Fee { get; set; }
    }

    /// <summary>
    /// 收入统计类
    /// </summary>
    public class IncomeStatistics
    {
        public DateTime Date { get; set; }
        public decimal TotalIncome { get; set; }
        public int TotalTransactions { get; set; }
        public decimal CashIncome { get; set; }
        public decimal CardIncome { get; set; }
        public decimal MobileIncome { get; set; }
    }
    #endregion
}