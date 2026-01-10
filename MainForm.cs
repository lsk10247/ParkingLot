using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;
using System;
using System.IO;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ParkingLotManager;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Web.Script.Serialization;

namespace ParkingLot
{
    public sealed partial class MainForm : Form
    {
        #region class private members
        private IMapControl3 m_mapControl = null;
        private string m_mapDocumentName = string.Empty;
        // 获取停车场管理实例
        ParkingLotManager.ParkingLotManager manager = ParkingLotManager.ParkingLotManager.Instance;
        #endregion
        //定义鼠标操作模式:无、添加车位、查询/编辑
        private enum MouseAction { None, AddRect, AddPoly, QuerySpot, EditSpot }
        //记录当前选中的模式
        private MouseAction _currentAction = MouseAction.None;
        //记录Shapefile图层的引用，方便后续调用
        private IFeatureLayer _parkingLayer = null;



        #region class constructor
        public MainForm()
        {
            InitializeComponent();
        }
        #endregion

        private void MainForm_Load(object sender, EventArgs e)
        {
            //get the MapControl
            m_mapControl = (IMapControl3)axMapControl1.Object;

            //disable the Save menu (since there is no document yet)
            menuSaveDoc.Enabled = false;
        }

        #region Main Menu event handlers
        private void menuNewDoc_Click(object sender, EventArgs e)
        {
            //execute New Document command
            ICommand command = new CreateNewDocument();
            command.OnCreate(m_mapControl.Object);
            command.OnClick();
        }

        private void menuOpenDoc_Click(object sender, EventArgs e)
        {
            //execute Open Document command
            ICommand command = new ControlsOpenDocCommandClass();
            command.OnCreate(m_mapControl.Object);
            command.OnClick();
        }

        private void menuSaveDoc_Click(object sender, EventArgs e)
        {
            //execute Save Document command
            if (m_mapControl.CheckMxFile(m_mapDocumentName))
            {
                //create a new instance of a MapDocument
                IMapDocument mapDoc = new MapDocumentClass();
                mapDoc.Open(m_mapDocumentName, string.Empty);

                //Make sure that the MapDocument is not readonly
                if (mapDoc.get_IsReadOnly(m_mapDocumentName))
                {
                    MessageBox.Show("Map document is read only!");
                    mapDoc.Close();
                    return;
                }

                //Replace its contents with the current map
                mapDoc.ReplaceContents((IMxdContents)m_mapControl.Map);

                //save the MapDocument in order to persist it
                mapDoc.Save(mapDoc.UsesRelativePaths, false);

                //close the MapDocument
                mapDoc.Close();
            }
        }

        private void menuSaveAs_Click(object sender, EventArgs e)
        {
            //execute SaveAs Document command
            ICommand command = new ControlsSaveAsDocCommandClass();
            command.OnCreate(m_mapControl.Object);
            command.OnClick();
        }

        private void menuExitApp_Click(object sender, EventArgs e)
        {
            //exit the application
            Application.Exit();
        }
        #endregion

        //listen to MapReplaced evant in order to update the statusbar and the Save menu
        private void axMapControl1_OnMapReplaced(object sender, IMapControlEvents2_OnMapReplacedEvent e)
        {
            //get the current document name from the MapControl
            m_mapDocumentName = m_mapControl.DocumentFilename;

            //if there is no MapDocument, diable the Save menu and clear the statusbar
            if (m_mapDocumentName == string.Empty)
            {
                menuSaveDoc.Enabled = false;
                statusBarXY.Text = string.Empty;
            }
            else
            {
                //enable the Save manu and write the doc name to the statusbar
                menuSaveDoc.Enabled = true;
                statusBarXY.Text = System.IO.Path.GetFileName(m_mapDocumentName);
            }
        }

        private void axMapControl1_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {
            statusBarXY.Text = string.Format("{0}, {1}  {2}", e.mapX.ToString("#######.##"), e.mapY.ToString("#######.##"), axMapControl1.MapUnits.ToString().Substring(4));
        }

        #region 模块A核心功能：地图管理与交互

        /// <summary>
        /// 1.加载Shapefile并自动渲染颜色
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="fileName"></param>
        // 在主窗体类中添加
        private void LoadParkingMap()
        {
            try
            {
                // 弹出对话框选择文件
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = "Shapefile文件(*.shp)|*.shp";
                dlg.Title = "请选择停车场车位数据";

                string path = null;
                string file = null;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    path = System.IO.Path.GetDirectoryName(dlg.FileName);
                    file = System.IO.Path.GetFileName(dlg.FileName);
                }
                else
                {
                    return;
                }

                // 加载数据
                IWorkspaceFactory pWorkspaceFactory = new ShapefileWorkspaceFactoryClass();
                IFeatureWorkspace pFeatureWorkspace = (IFeatureWorkspace)pWorkspaceFactory.OpenFromFile(path, 0);
                IFeatureClass pFeatureClass = pFeatureWorkspace.OpenFeatureClass(System.IO.Path.GetFileNameWithoutExtension(file));

                _parkingLayer = new FeatureLayerClass();
                _parkingLayer.FeatureClass = pFeatureClass;
                _parkingLayer.Name = "停车场车位";

                // 调用渲染函数，根据状态上色
                RenderLayerByStatus(_parkingLayer);

                // 添加到地图
                m_mapControl.AddLayer(_parkingLayer);
                m_mapControl.ActiveView.Refresh();

                // 将图层传递给ParkingLotManager
                manager.MapControl = m_mapControl as IMapControl3;
                manager.SetParkingLayer(_parkingLayer);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"地图加载失败：{ex.Message}");
            }
        }

        // 修改QueryAndEditSpot方法，使其与ParkingLotManager交互
        //private void QueryAndEditSpot(double x, double y)
        //{
        //    if (_parkingLayer == null)
        //    {
        //        return;
        //    }

        //    IPoint pPoint = new PointClass();
        //    //将屏幕坐标转为地图点
        //    pPoint.PutCoords(x, y);

        //    //空间过滤器：查找与点击点相交的要素
        //    ISpatialFilter pFilter = new SpatialFilterClass();
        //    pFilter.Geometry = pPoint;
        //    pFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;

        //    IFeatureCursor pCursor = _parkingLayer.FeatureClass.Search(pFilter, false);
        //    IFeature pFeature = pCursor.NextFeature();

        //    if (pFeature != null)
        //    {
        //        //获取当前车位属性值
        //        int idxID = pFeature.Fields.FindField("SpotID");
        //        int idxStatus = pFeature.Fields.FindField("Status");
        //        int idxType = pFeature.Fields.FindField("Type");

        //        //处理为查询到的情况
        //        string currentID = (idxID != -1) ? pFeature.get_Value(idxID).ToString() : "未知";
        //        string currentType = (idxType != -1) ? pFeature.get_Value(idxType).ToString() : "Standard";

        //        int currentStatus = 0;
        //        if (idxStatus != -1)
        //        {
        //            object val = pFeature.get_Value(idxStatus);
        //            int.TryParse(val.ToString(), out currentStatus);
        //        }

        //        // 从ParkingLotManager获取对应的停车位对象
        //        var parkingSpace = manager.GetParkingSpaceById(currentID);

        //        if (parkingSpace == null)
        //        {
        //            MessageBox.Show("未在管理系统中找到对应的车位");
        //            System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
        //            return;
        //        }

        //        //判断是查询还是编辑
        //        bool isEditMode = (_currentAction == MouseAction.EditSpot);

        //        // 使用我们的车辆信息查看对话框
        //        using (var spaceInfoDialog = new ParkingLotVisualizationForm(manager))
        //        {
        //            if (isEditMode)
        //            {
        //                // 编辑模式下显示可编辑的对话框
        //                // 这里可以创建一个新的编辑对话框，或者修改现有对话框支持编辑
        //                // 为了简化，我们只更新状态
        //                var result = MessageBox.Show($"是否将车位 {currentID} 状态改为空闲？",
        //                    "编辑车位", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        //                if (result == DialogResult.Yes)
        //                {
        //                    // 更新管理系统中的状态
        //                    parkingSpace.Status = ParkingSpaceStatus.Available;

        //                    // 更新地图要素
        //                    if (idxStatus != -1)
        //                    {
        //                        pFeature.set_Value(idxStatus, 0); // 0表示空闲
        //                        pFeature.Store();
        //                    }

        //                    // 通知管理器更新地图
        //                    manager.UpdateMapSpaceStatus(currentID, ParkingSpaceStatus.Available);

        //                    m_mapControl.ActiveView.Refresh();
        //                    RenderLayerByStatus(_parkingLayer);
        //                }
        //            }
        //            else
        //            {
        //                // 查询模式，只显示信息
        //                spaceInfoDialog.ShowDialog();
        //            }
        //        }
        //    }
        //    //释放游标
        //    System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
        //}


        //辅助方法：创建RGB颜色对象
        private IRgbColor GetRgbColor(int r, int g, int b)
        {
            IRgbColor pColor = new RgbColorClass();
            pColor.Red = r;
            pColor.Green = g;
            pColor.Blue = b;
            return pColor;
        }

        /// <summary>
        /// 2.图层符号化：根据Status字段值显示不同颜色
        /// </summary>
        /// <param name="pLayer"></param>
        private void RenderLayerByStatus(IFeatureLayer pLayer)
        {
            try
            {
                IGeoFeatureLayer pGeoLayer = pLayer as IGeoFeatureLayer;
                IUniqueValueRenderer pRender = new UniqueValueRendererClass();

                //设置绑定的字段名（必须确保Shapefile里有“Status”这个字段）
                pRender.FieldCount = 1;
                pRender.set_Field(0, "Status");

                //创建三种颜色的符号
                //绿色-空闲
                ISimpleFillSymbol symFree = new SimpleFillSymbolClass();
                symFree.Color = GetRgbColor(0, 255, 0);
                symFree.Style = esriSimpleFillStyle.esriSFSSolid;
                //红色-占用
                ISimpleFillSymbol symBusy = new SimpleFillSymbolClass();
                symBusy.Color = GetRgbColor(255, 0, 0);
                symBusy.Style = esriSimpleFillStyle.esriSFSSolid;
                //蓝色-预订
                ISimpleFillSymbol symBooked = new SimpleFillSymbolClass();
                symBooked.Color = GetRgbColor(0, 0, 255);
                symBooked.Style = esriSimpleFillStyle.esriSFSSolid;

                //添加对应关系(Shapefile存储为整型：0,1,2）
                pRender.AddValue("0", "空闲", symFree as ISymbol);
                pRender.AddValue("1", "占用", symBusy as ISymbol);
                pRender.AddValue("2", "预约", symBooked as ISymbol);

                pGeoLayer.Renderer = pRender as IFeatureRenderer;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"渲染错误：{ex.Message}");
            }
        }

        /// <summary>
        /// 3.画车位（支持矩形与多边形绘制）
        /// </summary>
        private void DrawNewSpot()
        {
            if (_parkingLayer == null)
            {
                return;
            }

            //使用ArcEngine自带的追踪多边形工具
            IGeometry pGeometry = null;
            //矩形用TrackExtene
            if (_currentAction == MouseAction.AddRect)
            {
                IEnvelope pEnv = m_mapControl.TrackRectangle();
                if (pEnv.IsEmpty)
                {
                    return;
                }

                //将IEnvelope转为Polygon
                IPolygon pPoly = new PolygonClass();
                ISegmentCollection pSegColl = pPoly as ISegmentCollection;
                pSegColl.SetRectangle(pEnv);
                pGeometry = pPoly as IGeometry;
            }
            else if (_currentAction == MouseAction.AddPoly)
            {
                pGeometry = m_mapControl.TrackPolygon();
            }

            if (pGeometry != null && !pGeometry.IsEmpty)
            {
                //弹出窗体，设置模式为“录入”
                FrmAttribute frm = new FrmAttribute(FrmAttribute.FormMode.AddNew);

                //用户点击“保存”
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    IFeatureClass pClass = _parkingLayer.FeatureClass;
                    IFeature pFeature = pClass.CreateFeature();
                    pFeature.Shape = pGeometry;

                    //写入初始属性
                    //查找字段索引
                    int idxID = pClass.FindField("SpotID");
                    int idxStatus = pClass.FindField("Status");
                    int idxType = pClass.FindField("Type");

                    if (idxID != -1)
                    {
                        pFeature.set_Value(idxID, frm.SpotID);
                    }
                    //默认为可用车位
                    if (idxStatus != -1)
                    {
                        pFeature.set_Value(idxStatus, frm.SpotStatus);
                    }
                    //默认为标准车位
                    if (idxType != -1)
                    {
                        pFeature.set_Value(idxType, frm.SpotType);
                    }

                    //保存到shp文件
                    pFeature.Store();
                    m_mapControl.ActiveView.Refresh();

                    //渲染颜色，防止新画的没颜色
                    RenderLayerByStatus(_parkingLayer);
                }
            }
        }

        /// <summary>
        /// 4.查询与修改当前车位信息。
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void QueryAndEditSpot(double x, double y)
        {
            if (_parkingLayer == null)
            {
                return;
            }

            IPoint pPoint = new PointClass();
            //将屏幕坐标转为地图点
            pPoint.PutCoords(x, y);

            //空间过滤器：查找与点击点相交的要素
            ISpatialFilter pFilter = new SpatialFilterClass();
            pFilter.Geometry = pPoint;
            pFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;

            IFeatureCursor pCursor = _parkingLayer.FeatureClass.Search(pFilter, false);
            IFeature pFeature = pCursor.NextFeature();

            if (pFeature != null)
            {
                //获取当前车位属性值
                int idxID = pFeature.Fields.FindField("SpotID");
                int idxStatus = pFeature.Fields.FindField("Status");
                int idxType = pFeature.Fields.FindField("Type");

                //处理为查询到的情况
                string currentID = (idxID != -1) ? pFeature.get_Value(idxID).ToString() : "未知";
                string currentType = (idxType != -1) ? pFeature.get_Value(idxType).ToString() : "Standard";

                int currentStatus = 0;
                if (idxStatus != -1)
                {
                    object val = pFeature.get_Value(idxStatus);
                    int.TryParse(val.ToString(), out currentStatus);
                }

                //判断是查询还是编辑
                FrmAttribute.FormMode mode = (_currentAction == MouseAction.QuerySpot)
                    ? FrmAttribute.FormMode.ViewOnly
                    : FrmAttribute.FormMode.EditInfo;

                //弹出窗体
                FrmAttribute frm = new FrmAttribute(mode, currentID, currentType, currentStatus);

                //只有在编辑模式下点击保存，才回写数据
                if (frm.ShowDialog() == DialogResult.OK && mode == FrmAttribute.FormMode.EditInfo)
                {
                    if (idxStatus != -1)
                    {
                        pFeature.set_Value(idxStatus, frm.SpotStatus);
                    }
                    if (idxType != -1)
                    {
                        pFeature.set_Value(idxType, frm.SpotType);
                    }

                    pFeature.Store();
                    m_mapControl.ActiveView.Refresh();
                    RenderLayerByStatus(_parkingLayer);
                }
            }
            //释放游标
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
        }

        // 重置鼠标状态
        private void ResetMouseToNormal()
        {
            _currentAction = MouseAction.None;
            m_mapControl.CurrentTool = null;
            m_mapControl.MousePointer = esriControlsMousePointer.esriPointerArrow;
        }

        #endregion

        private void 加载地图ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            LoadParkingMap();
        }

        private void 车位信息查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //清空当前工具
            m_mapControl.CurrentTool = null;
            _currentAction = MouseAction.QuerySpot;
            //鼠标变手指
            m_mapControl.MousePointer = esriControlsMousePointer.esriPointerIdentify;
        }

        private void axMapControl1_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            //左键进行操作（button1）
            if (e.button == 1)
            {
                switch (_currentAction)
                {
                    //添加车位模式
                    case MouseAction.AddRect:
                    case MouseAction.AddPoly:
                        //统一调用绘制方法
                        DrawNewSpot();
                        break;
                    //查询与编辑车位模式
                    case MouseAction.QuerySpot:
                    case MouseAction.EditSpot:
                        //统一调用点击方法
                        QueryAndEditSpot(e.mapX, e.mapY);
                        break;
                    default:
                        break;
                }
            }
            //右键退出当前操作模式，恢复鼠标原始状态
            else if (e.button == 2)
            {
                if (_currentAction != MouseAction.None)
                {
                    ResetMouseToNormal();
                    MessageBox.Show("已退出当前操作模式！");
                }
            }
        }


        private void 停止操作ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetMouseToNormal();
            MessageBox.Show("已退出当前操作模式！");
        }

        private void 绘制矩形车位ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            axMapControl1.CurrentTool = null;
            _currentAction = MouseAction.AddRect;
            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerCrosshair;
        }

        private void 绘制多边形车位ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            axMapControl1.CurrentTool = null;
            _currentAction = MouseAction.AddPoly;
            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerPencil;
        }

        private void 修改信息ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            m_mapControl.CurrentTool = null;
            _currentAction = MouseAction.EditSpot;
            m_mapControl.MousePointer = esriControlsMousePointer.esriPointerIdentify;
        }

        private void 停车ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string carlicenceplate = CarLicensePlateDialog.ShowDialogAndGetLicensePlate();
            var result = manager.CarEnter(carlicenceplate);
            if (result.Success)
            {
                MessageBox.Show($"入场成功，分配车位: {result.AssignedSpace.Id}");
            }
        }

        private void 离场ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string carlicenceplate = CarLicensePlateDialog.ShowDialogAndGetLicensePlate();
            var exitResult = manager.CarExit(carlicenceplate);
            if (exitResult.Success)
            {
                MessageBox.Show($"出场成功，费用: {exitResult.ParkingFee:C}");

                // 支付
                var paymentResult = manager.ProcessPayment(carlicenceplate, exitResult.ParkingFee, "现金");
                if (paymentResult.Success)
                {
                    MessageBox.Show($"支付成功，找零: {paymentResult.Change:C}");
                }
            }
            else
            {
                MessageBox.Show(exitResult.Message);
            }
        }

        private void 预约ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. 获取车牌号
                string licensePlate = CarLicensePlateDialog.ShowDialogAndGetLicensePlate(this);

                if (string.IsNullOrEmpty(licensePlate))
                {
                    return; // 用户取消输入
                }

                // 2. 验证车牌号格式
                if (!CarLicensePlateDialog.ValidateLicensePlateFormat(licensePlate))
                {
                    MessageBox.Show("车牌号格式不正确，请重新输入！", "格式错误",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 3. 检查车辆是否已在停车场内
                var car = manager.GetCarByLicense(licensePlate);

                if (car != null)
                {
                    // 检查车辆状态
                    if (car.Status == VehicleStatus.Parked)
                    {
                        MessageBox.Show($"车辆 {licensePlate} 已在停车场内，无法预约！", "车辆状态错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (car.Status == VehicleStatus.Reserved)
                    {
                        var result = MessageBox.Show($"车辆 {licensePlate} 已有预约，是否重新预约？", "已有预约",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (result != DialogResult.Yes)
                        {
                            return;
                        }

                        // 取消现有预约
                        manager.CancelReservation(licensePlate);
                    }
                }
                else
                {
                    // 4. 车辆未注册，询问是否立即注册
                    var registerResult = MessageBox.Show($"车辆 {licensePlate} 未注册，是否立即注册？", "车辆未注册",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (registerResult == DialogResult.Yes)
                    {
                        // 弹出注册对话框
                        using (var registerDialog = new RegisterCarDialog())
                        {
                            // 设置车牌号（用户不能修改）
                            //registerDialog.Controls.OfType<TextBox>()
                            //    .FirstOrDefault(t => t.Name == "车牌号textBox")
                            //    ?.Text = licensePlate;

                            if (registerDialog.ShowDialog(this) == DialogResult.OK)
                            {
                                // 创建车辆对象
                                car = new Car(licensePlate, registerDialog.SelectedVehicleType)
                                {
                                    Brand = registerDialog.SelectedCarBrand,
                                    Color = registerDialog.SelectedCarColor,
                                    OwnerName = registerDialog.OwnerName,
                                    OwnerPhone = registerDialog.OwnerPhone,
                                    IsVIP = registerDialog.IsVIP
                                };

                                // 注册车辆
                                if (!manager.RegisterCar(car))
                                {
                                    MessageBox.Show("车辆注册失败！", "注册失败",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }
                            else
                            {
                                // 用户取消注册
                                MessageBox.Show("预约需要注册车辆，操作已取消。", "预约取消",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }
                    }
                    else
                    {
                        // 用户不想注册，创建临时车辆
                        car = new Car(licensePlate, VehicleType.Car);
                        manager.RegisterCar(car);
                    }
                }

                // 5. 选择预约时间
                DateTime? reservationTime = TimeDialog.ShowDialogAndGetTime(
                    this,
                    "选择预约时间",
                    DateTime.Now.AddMinutes(30), // 默认30分钟后
                    false); // 不允许选择过去时间

                if (!reservationTime.HasValue)
                {
                    return; // 用户取消时间选择
                }

                // 6. 验证预约时间
                DateTime selectedTime = reservationTime.Value;

                if (selectedTime < DateTime.Now)
                {
                    MessageBox.Show("预约时间不能早于当前时间！", "时间错误",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 检查预约时间是否在合理范围内（例如24小时内）
                if ((selectedTime - DateTime.Now).TotalHours > 24)
                {
                    var confirmResult = MessageBox.Show("预约时间超过24小时，是否继续？", "确认预约",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (confirmResult != DialogResult.Yes)
                    {
                        return;
                    }
                }

                // 7. 尝试预约
                var reservationResult = manager.ReserveSpace(car.LicensePlate, selectedTime);

                if (reservationResult.Success)
                {
                    // 预约成功
                    string successMessage = $"预约成功！\n\n" +
                                           $"车牌号：{car.LicensePlate}\n" +
                                           $"预约车位：{reservationResult.ReservedSpace.Id}\n" +
                                           $"车位位置：{reservationResult.ReservedSpace.Location}\n" +
                                           $"车位类型：{reservationResult.ReservedSpace.Type}\n" +
                                           $"预约时间：{selectedTime:yyyy-MM-dd HH:mm}\n\n";

                    // 添加车位特性信息
                    if (reservationResult.ReservedSpace.HasCharging)
                    {
                        successMessage += "⚡ 带充电桩\n";
                    }
                    if (reservationResult.ReservedSpace.NearElevator)
                    {
                        successMessage += "♿ 靠近电梯\n";
                    }
                    if (reservationResult.ReservedSpace.HasShelter)
                    {
                        successMessage += "☂ 有遮阳棚\n";
                    }

                    successMessage += $"\n预约完成时间：{DateTime.Now:HH:mm:ss}";

                    MessageBox.Show(successMessage, "预约成功",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // 8. 记录预约日志
                    //LogReservationOperation(car.LicensePlate, reservationResult.ReservedSpace.Id, selectedTime);

                    // 9. 更新界面显示
                    //RefreshParkingDisplay();
                }
                else
                {
                    // 预约失败
                    string errorMessage = $"预约失败！\n\n" +
                                         $"原因：{reservationResult.Message}\n\n";

                    // 提供解决方案或建议
                    if (reservationResult.Message.Contains("没有合适的车位") ||
                        reservationResult.Message.Contains("没有合适的车位"))
                    {
                        errorMessage += "建议：\n" +
                                      "1. 选择其他预约时间\n" +
                                      "2. 稍后再试\n" +
                                      "3. 联系管理员";

                        // 显示可用车位信息
                        var availableSpaces = manager.GetAvailableSpaces();
                        if (availableSpaces.Count > 0)
                        {
                            errorMessage += $"\n\n当前有 {availableSpaces.Count} 个空闲车位，但可能不适合您的车型。";
                        }
                    }
                    else if (reservationResult.Message.Contains("已预约") ||
                             reservationResult.Message.Contains("已占用"))
                    {
                        errorMessage += "该时间段车位已被占用，请选择其他时间。";
                    }

                    MessageBox.Show(errorMessage, "预约失败",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"预约过程中发生错误：\n{ex.Message}", "系统错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void 注册车辆ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 显示注册对话框并创建车辆
            var car = RegisterCarDialog.ShowDialogAndCreateCar(this);
            if (car != null)
            {
                // 注册到停车场管理器
                if (manager.RegisterCar(car))
                {
                    MessageBox.Show($"车辆 {car.LicensePlate} 注册成功");
                }
            }
        }

        private void 注销车辆ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 获取要编辑的车辆
            string carlicenceplate = CarLicensePlateDialog.ShowDialogAndGetLicensePlate();
            if (manager.UnregisterCar(carlicenceplate))
            {
                MessageBox.Show($"已注销车辆{carlicenceplate}。");
            }
            else
            {
                MessageBox.Show($"车辆{carlicenceplate}正在停车，无法注销。");
            }
        }

        private void 修改信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 获取要编辑的车辆
            string carlicenceplate = CarLicensePlateDialog.ShowDialogAndGetLicensePlate();
            var car = manager.GetCarByLicense(carlicenceplate);

            if (car != null)
            {
                // 显示编辑对话框
                if (RegisterCarDialog.ShowDialogAndEditCar(car, this))
                {
                    MessageBox.Show($"车辆 {car.LicensePlate} 信息已更新");
                }
            }
        }

        private void 车位状态ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var visualizationForm = new ParkingLotVisualizationForm(manager);
            visualizationForm.Show();
        }

        public class ParkingDataSerializer
        {
            // 用于JSON序列化的简化版本数据类
            public class SerializedCar
            {
                public string LicensePlate { get; set; }
                public VehicleType VehicleType { get; set; }
                public CarBrand Brand { get; set; }
                public CarColor Color { get; set; }
                public string OwnerName { get; set; }
                public string OwnerPhone { get; set; }
                public bool IsVIP { get; set; }
                public VehicleStatus Status { get; set; }
                public DateTime? EntryTime { get; set; }
                public DateTime? ExitTime { get; set; }
                public string AssignedSpaceId { get; set; }
                public DateTime? ReservationStartTime { get; set; }
                public int ParkingCount { get; set; }
                public decimal TotalSpent { get; set; }
            }

            public class SerializedParkingSpace
            {
                public string Id { get; set; }
                public ParkingSpaceType Type { get; set; }
                public ParkingSpaceStatus Status { get; set; }
                public string Location { get; set; }
                public double Size { get; set; }
                public bool HasCharging { get; set; }
                public bool HasShelter { get; set; }
                public bool NearElevator { get; set; }
                public decimal HourlyRate { get; set; }
                public string CurrentCarLicense { get; set; }
                public DateTime? OccupiedStartTime { get; set; }
                public decimal TodayIncome { get; set; }
                public int TodayParkingCount { get; set; }

                public SerializedReservationInfo Reservation { get; set; }
            }

            public class SerializedReservationInfo
            {
                public string CarLicense { get; set; }
                public DateTime StartTime { get; set; }
                public DateTime ReserveTime { get; set; }
            }

            public class ParkingDataContainer
            {
                public System.Collections.Generic.List<SerializedCar> Cars { get; set; } = new List<SerializedCar>();
                public System.Collections.Generic.List<SerializedParkingSpace> ParkingSpaces { get; set; } = new List<SerializedParkingSpace>();
                public DateTime SaveTime { get; set; }
                public string SaveVersion { get; set; } = "1.0";
            }
        }

        private void 保存数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ParkingLotManager.ParkingLotManager manager = ParkingLotManager.ParkingLotManager.Instance;

                // 创建数据容器
                var data = new ParkingDataSerializer.ParkingDataContainer
                {
                    SaveTime = DateTime.Now
                };

                // 序列化车辆数据
                foreach (var car in manager.GetAllCars())
                {
                    var serializedCar = new ParkingDataSerializer.SerializedCar
                    {
                        LicensePlate = car.LicensePlate,
                        VehicleType = car.Type,
                        Brand = car.Brand,
                        Color = car.Color,
                        OwnerName = car.OwnerName,
                        OwnerPhone = car.OwnerPhone,
                        IsVIP = car.IsVIP,
                        Status = car.Status,
                        EntryTime = car.EntryTime,
                        ExitTime = car.ExitTime,
                        AssignedSpaceId = car.AssignedSpaceId,
                        ReservationStartTime = car.ReservationStartTime,
                        ParkingCount = car.ParkingCount,
                        TotalSpent = car.TotalSpent
                    };
                    data.Cars.Add(serializedCar);
                }

                // 序列化车位数据
                foreach (var space in manager.GetAllParkingSpaces())
                {
                    var serializedSpace = new ParkingDataSerializer.SerializedParkingSpace
                    {
                        Id = space.Id,
                        Type = space.Type,
                        Status = space.Status,
                        Location = space.Location,
                        Size = space.Size,
                        HasCharging = space.HasCharging,
                        HasShelter = space.HasShelter,
                        NearElevator = space.NearElevator,
                        HourlyRate = space.HourlyRate,
                        CurrentCarLicense = space.CurrentCarLicense,
                        OccupiedStartTime = space.OccupiedStartTime,
                        TodayIncome = space.TodayIncome,
                        TodayParkingCount = space.TodayParkingCount
                    };

                    // 如果有预约信息，也保存
                    if (space.Reservation != null)
                    {
                        serializedSpace.Reservation = new ParkingDataSerializer.SerializedReservationInfo
                        {
                            CarLicense = space.Reservation.CarLicense,
                            StartTime = space.Reservation.StartTime,
                            ReserveTime = space.Reservation.ReserveTime
                        };
                    }

                    data.ParkingSpaces.Add(serializedSpace);
                }

                // 使用 JavaScriptSerializer 序列化
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string jsonData = serializer.Serialize(data);

                // 获取保存路径
                string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string exeDirectory = System.IO.Path.GetDirectoryName(exePath);
                string savePath = System.IO.Path.Combine(exeDirectory, "parking_data.txt");

                // 保存到文件
                File.WriteAllText(savePath, jsonData, Encoding.UTF8);

                // 显示保存成功信息
                string message = $"数据保存成功！\n" +
                                $"保存路径：{savePath}\n" +
                                $"保存时间：{DateTime.Now:yyyy-MM-dd HH:mm:ss}\n" +
                                $"保存车辆数：{data.Cars.Count}\n" +
                                $"保存车位数：{data.ParkingSpaces.Count}";

                MessageBox.Show(message, "保存成功", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 记录操作日志
                //LogSaveOperation(data.Cars.Count, data.ParkingSpaces.Count, savePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存数据失败：\n{ex.Message}", "保存失败",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void 加载数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取文件路径
                string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string exeDirectory = System.IO.Path.GetDirectoryName(exePath);
                string loadPath = System.IO.Path.Combine(exeDirectory, "parking_data.txt");

                // 检查文件是否存在
                if (!File.Exists(loadPath))
                {
                    MessageBox.Show($"数据文件不存在：\n{loadPath}\n请先保存数据。", "加载失败",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 读取文件内容
                string jsonData = File.ReadAllText(loadPath, Encoding.UTF8);

                // 使用 JavaScriptSerializer 反序列化
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                var data = serializer.Deserialize<ParkingDataSerializer.ParkingDataContainer>(jsonData);

                if (data == null)
                {
                    MessageBox.Show("数据文件格式错误，无法加载。", "加载失败",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                ParkingLotManager.ParkingLotManager manager = ParkingLotManager.ParkingLotManager.Instance;
                int loadedCars = 0;
                int loadedSpaces = 0;

                // 询问用户是否覆盖现有数据
                DialogResult result = MessageBox.Show(
                    $"发现保存时间：{data.SaveTime:yyyy-MM-dd HH:mm:ss}\n" +
                    $"车辆数：{data.Cars.Count}，车位数：{data.ParkingSpaces.Count}\n\n" +
                    "是否加载数据？这将覆盖当前所有数据。",
                    "确认加载",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                {
                    return;
                }

                // 清空现有数据
                manager.ClearAllData();

                // 加载车位数据
                foreach (var serializedSpace in data.ParkingSpaces)
                {
                    try
                    {
                        var space = new ParkingSpace(serializedSpace.Id, serializedSpace.Type)
                        {
                            Status = serializedSpace.Status,
                            Location = serializedSpace.Location,
                            Size = serializedSpace.Size,
                            HasCharging = serializedSpace.HasCharging,
                            HasShelter = serializedSpace.HasShelter,
                            NearElevator = serializedSpace.NearElevator,
                            HourlyRate = serializedSpace.HourlyRate,
                            CurrentCarLicense = serializedSpace.CurrentCarLicense,
                            OccupiedStartTime = serializedSpace.OccupiedStartTime,
                            TodayIncome = serializedSpace.TodayIncome,
                            TodayParkingCount = serializedSpace.TodayParkingCount
                        };

                        // 恢复预约信息
                        if (serializedSpace.Reservation != null)
                        {
                            space.Reservation = new ReservationInfo
                            {
                                CarLicense = serializedSpace.Reservation.CarLicense,
                                StartTime = serializedSpace.Reservation.StartTime,
                                ReserveTime = serializedSpace.Reservation.ReserveTime
                            };
                        }

                        if (manager.AddParkingSpace(space))
                        {
                            loadedSpaces++;
                        }
                    }
                    catch (Exception spaceEx)
                    {
                        // 记录错误但继续加载其他数据
                        //LogError($"加载车位 {serializedSpace.Id} 失败：{spaceEx.Message}");
                    }
                }

                // 加载车辆数据
                foreach (var serializedCar in data.Cars)
                {
                    try
                    {
                        var car = new Car(serializedCar.LicensePlate, serializedCar.VehicleType)
                        {
                            Brand = serializedCar.Brand,
                            Color = serializedCar.Color,
                            OwnerName = serializedCar.OwnerName,
                            OwnerPhone = serializedCar.OwnerPhone,
                            IsVIP = serializedCar.IsVIP,
                            Status = serializedCar.Status,
                            EntryTime = serializedCar.EntryTime,
                            ExitTime = serializedCar.ExitTime,
                            AssignedSpaceId = serializedCar.AssignedSpaceId,
                            ReservationStartTime = serializedCar.ReservationStartTime,
                            ParkingCount = serializedCar.ParkingCount,
                            TotalSpent = serializedCar.TotalSpent
                        };

                        if (manager.RegisterCar(car))
                        {
                            loadedCars++;
                        }
                    }
                    catch (Exception carEx)
                    {
                        // 记录错误但继续加载其他数据
                        //LogError($"加载车辆 {serializedCar.LicensePlate} 失败：{carEx.Message}");
                    }
                }

                // 同步车位状态到地图（如果有地图）
                manager.SyncAllSpacesToMap();

                // 刷新界面显示
                //RefreshParkingDisplay();

                // 显示加载成功信息
                string message = $"数据加载成功！\n\n" +
                                $"加载路径：{loadPath}\n" +
                                $"原保存时间：{data.SaveTime:yyyy-MM-dd HH:mm:ss}\n" +
                                $"成功加载车辆：{loadedCars}/{data.Cars.Count}\n" +
                                $"成功加载车位：{loadedSpaces}/{data.ParkingSpaces.Count}";

                if (loadedCars < data.Cars.Count || loadedSpaces < data.ParkingSpaces.Count)
                {
                    message += $"\n\n部分数据加载失败，请查看错误日志。";
                    MessageBox.Show(message, "加载完成（有警告）",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show(message, "加载成功",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // 记录操作日志
                //LogLoadOperation(loadedCars, loadedSpaces, loadPath, data.SaveTime);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载数据失败：\n{ex.Message}", "加载失败",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}



//// 获取停车场管理实例
//var manager = ParkingLotManager.Instance;

//// 车辆入场
//var result = manager.CarEnter("浙A12345");
//if (result.Success)
//{
//    Console.WriteLine($"入场成功，分配车位: {result.AssignedSpace.Id}");
//}

//// 车辆出场
//var exitResult = manager.CarExit("浙A12345");
//if (exitResult.Success)
//{
//    Console.WriteLine($"出场成功，费用: {exitResult.ParkingFee:C}");

//    // 支付
//    var paymentResult = manager.ProcessPayment("浙A12345", exitResult.ParkingFee, "现金");
//    if (paymentResult.Success)
//    {
//        Console.WriteLine($"支付成功，找零: {paymentResult.Change:C}");
//    }
//}

//// 查看停车场状态
//Console.WriteLine(manager.GetParkingLotSummary());

//// 搜索车辆
//var cars = manager.SearchCars("张三");
//foreach (var car in cars)
//{
//    Console.WriteLine(car.GetDescription());
//}