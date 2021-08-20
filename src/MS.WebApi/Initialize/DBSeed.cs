using MS.Common.Security;
using MS.DbContexts;
using MS.Entities;
using MS.Entities.Core;
using MS.UnitOfWork;
using System;

namespace MS.WebApi
{
    public static class DBSeed
    {
        /// <summary>
        /// 数据初始化
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <returns>返回是否创建了数据库（非迁移）</returns>
        public static bool Initialize(IUnitOfWork<MSDbContext> unitOfWork)
        {
            bool isCreateDb = false;
            //直接自动执行迁移,如果它创建了数据库，则返回true
            if (unitOfWork.DbContext.Database.EnsureCreated())
            {
                isCreateDb = true;
                //打印log-创建数据库及初始化期初数据

                long rootUserId = 1219490056771866624;

                #region 角色、用户、登录
                Role rootRole = new Role
                {
                    Id = 1219490056771866625,
                    Name = "SuperAdmin",
                    DisplayName = "超级管理员",
                    Remark = "系统内置超级管理员",
                    Creator = rootUserId,
                    CreateTime = DateTime.Now
                };
                User rootUser = new User
                {
                    Id = rootUserId,
                    Account = "admin",
                    Name = "admin",
                    RoleId = rootRole.Id,
                    Status = StatusCode.Enable,
                    Creator = rootUserId,
                    CreateTime = DateTime.Now,
                };

                unitOfWork.GetRepository<Role>().Insert(rootRole);
                unitOfWork.GetRepository<User>().Insert(rootUser);
                unitOfWork.GetRepository<UserLogin>().Insert(new UserLogin
                {
                    UserId = rootUserId,
                    Account = rootUser.Account,
                    HashedPassword = Crypto.HashPassword(rootUser.Account),//默认密码同账号名
                    IsLocked = false
                });
                unitOfWork.SaveChanges();

                #endregion

                #region 车辆、驾驶员、线路
                Truck truck1 = new Truck
                {
                    Id = 1219490056771866501,
                    PlateNumber = "川ZS12581",
                    ModelNumber = "东风重卡8A-3M",
                    Alias = "东风20T卡车",
                    Remark = "测试汽车A",
                    Status = StatusCode.Enable,
                    Creator = rootUserId,
                    CreateTime = DateTime.Now,
                    IsUsed = false
                };
                Truck truck2 = new Truck
                {
                    Id = 1219490056771866502,
                    PlateNumber = "川ZS12582",
                    ModelNumber = "东风重卡8A-5M",
                    Alias = "东风25T卡车",
                    Remark = "测试汽车B",
                    Status = StatusCode.Enable,
                    Creator = rootUserId,
                    CreateTime = DateTime.Now,
                    IsUsed = false
                };
                Driver driver1 = new Driver
                {
                    Id = 1219490056771866627,
                    Name = "张三",
                    Gender = GenderCode.Male,
                    Phone = "13800138000",
                    Photo = "https://pic3.zhimg.com/80/v2-2fd21a22591eaf65c2b53fb5333a2064_720w.jpg",
                    IdNumber = "513721198309163621",
                    License = DriverEnums.DrivingLicenseEnum.B2,
                    IssueDate = DateTime.Parse("2015-06-05"),
                    LicensePhoto = "https://img1.baidu.com/it/u=3066888640,1398064558&fm=26&fmt=auto&gp=0.jpg",
                    Remark = "测试驾驶员A",
                    Status = StatusCode.Enable,
                    Creator = rootUserId,
                    CreateTime = DateTime.Now
                };
                Driver driver2 = new Driver
                {
                    Id = 1219490056771866628,
                    Name = "李四",
                    Gender = GenderCode.Female,
                    Phone = "13800138001",
                    Photo = "https://pic3.zhimg.com/80/v2-2fd21a22591eaf65c2b53fb5333a2064_720w.jpg",
                    IdNumber = "513721198309163621",
                    License = DriverEnums.DrivingLicenseEnum.B2,
                    IssueDate = DateTime.Parse("2016-05-06"),
                    LicensePhoto = "https://img1.baidu.com/it/u=3066888640,1398064558&fm=26&fmt=auto&gp=0.jpg",
                    Remark = "测试驾驶员B",
                    Status = StatusCode.Enable,
                    Creator = rootUserId,
                    CreateTime = DateTime.Now
                };
                Route route1 = new Route
                {
                    Id = 1219490056771866701,
                    Name = "达州-宣汉",
                    StartTime = DateTime.Parse("2021-08-16 08:00:00"),
                    StartAddress = "达州南城",
                    TargetAddress = "宣汉城区",
                    RunStatus = RouteEnums.RunStatusEnum.NotYet,
                    TruckId = 1219490056771866501,
                    //DriverIds = "1219490056771866627",
                    IsRound = true,
                    Creator = rootUserId,
                    Remark = "测试线路A"
                };
                Route route2 = new Route
                {
                    Id = 1219490056771866702,
                    Name = "达州-万源",
                    StartTime = DateTime.Parse("2021-08-17 08:20:00"),
                    StartAddress = "达州北外高家坝",
                    TargetAddress = "万源城口",
                    RunStatus = RouteEnums.RunStatusEnum.NotYet,
                    TruckId = 1219490056771866502,
                    //DriverIds = "1219490056771866627",
                    IsRound = true,
                    Creator = rootUserId,
                    Remark = "测试线路B"
                };

                RouteDriver routeDriver1 = new RouteDriver
                {
                    RouteId = 1219490056771866701,
                    DriverId = 1219490056771866627
                };
                RouteDriver routeDriver2 = new RouteDriver
                {
                    RouteId = 1219490056771866702,
                    DriverId = 1219490056771866628
                };

                unitOfWork.GetRepository<Truck>().Insert(truck1, truck2);
                unitOfWork.GetRepository<Driver>().Insert(driver1, driver2);
                unitOfWork.GetRepository<Route>().Insert(route1, route2);
                unitOfWork.GetRepository<RouteDriver>().Insert(routeDriver1, routeDriver2);

                unitOfWork.SaveChanges();
                #endregion
            }
            return isCreateDb;
        }


    }
}
