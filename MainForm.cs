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
        private void LoadParkingMap()
        {
            try
            {
                //弹出对话框选择文件
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = "Shapefile文件(*.shp)|*.shp";
                dlg.Title = "请选择停车场车位数据";

                string path = null;
                string file = null;

                if(dlg.ShowDialog() == DialogResult.OK)
                {
                    path = System.IO.Path.GetDirectoryName(dlg.FileName);
                    file = System.IO.Path.GetFileName(dlg.FileName);
                }

                //加载数据
                IWorkspaceFactory pWorkspaceFactory = new ShapefileWorkspaceFactoryClass();
                IFeatureWorkspace pFeatureWorkspace = (IFeatureWorkspace)pWorkspaceFactory.OpenFromFile(path, 0);
                IFeatureClass pFeatureClass = pFeatureWorkspace.OpenFeatureClass(System.IO.Path.GetFileNameWithoutExtension(file));

                _parkingLayer = new FeatureLayerClass();
                _parkingLayer.FeatureClass = pFeatureClass;
                _parkingLayer.Name = "停车场车位";

                //调用渲染函数，根据状态上色
                RenderLayerByStatus(_parkingLayer);

                //添加到地图
                m_mapControl.AddLayer(_parkingLayer);
                m_mapControl.ActiveView.Refresh();
            }
            catch(Exception ex)
            {
                MessageBox.Show($"地图加载失败：{ex.Message}");
            }
        }


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
            catch(Exception ex)
            {
                MessageBox.Show($"渲染错误：{ex.Message}");
            }
        }

        /// <summary>
        /// 3.画车位（支持矩形与多边形绘制）
        /// </summary>
        private void DrawNewSpot()
        {
            if(_parkingLayer == null)
            {
                return;
            }

            //使用ArcEngine自带的追踪多边形工具
            IGeometry pGeometry = null;
            //矩形用TrackExtene
            if(_currentAction == MouseAction.AddRect)
            {
                IEnvelope pEnv = m_mapControl.TrackRectangle();
                if(pEnv.IsEmpty)
                {
                    return;
                }

                //将IEnvelope转为Polygon
                IPolygon pPoly = new PolygonClass();
                ISegmentCollection pSegColl = pPoly as ISegmentCollection;
                pSegColl.SetRectangle(pEnv);
                pGeometry = pPoly as IGeometry;
            }
            else if(_currentAction == MouseAction.AddPoly)
            {
                pGeometry = m_mapControl.TrackPolygon();
            }

            if(pGeometry != null && !pGeometry.IsEmpty)
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
            if(_parkingLayer == null)
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

            if(pFeature != null)
            {
                //获取当前车位属性值
                int idxID = pFeature.Fields.FindField("SpotID");
                int idxStatus = pFeature.Fields.FindField("Status");
                int idxType = pFeature.Fields.FindField("Type");

                //处理为查询到的情况
                string currentID = (idxID != -1) ? pFeature.get_Value(idxID).ToString() : "未知";
                string currentType = (idxType != -1) ? pFeature.get_Value(idxType).ToString() : "Standard";

                int currentStatus = 0;
                if(idxStatus != -1)
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
                if(frm.ShowDialog() == DialogResult.OK && mode == FrmAttribute.FormMode.EditInfo)
                {
                    if(idxStatus != -1)
                    {
                        pFeature.set_Value(idxStatus, frm.SpotStatus);
                    }
                    if(idxType != -1)
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
            else if(e.button == 2)
            {
                if(_currentAction != MouseAction.None)
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