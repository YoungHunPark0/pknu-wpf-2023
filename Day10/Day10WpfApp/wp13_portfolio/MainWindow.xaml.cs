using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Data;
using wp13_portfolio.Logics;
using Newtonsoft.Json.Linq;
using wp13_portfolio.Models;

namespace wp13_portfolio
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        bool isNew = false; // false -> openApi 검색해온결과, true => 즐겨찾기 보기
        public MainWindow()
        {
            InitializeComponent();
            CboReqDate.ItemsSource = gulist;
        }

        public List<string> gulist = new List<string>()
        {
            "부산광역시 강서구",
            "부산광역시 금정구",
            "부산광역시 기장군",
            "부산광역시 남구",
            "부산광역시 동구",
            "부산광역시 동래구",
            "부산광역시 부산진구",
            "부산광역시 북구",
            "부산광역시 사상구",
            "부산광역시 사하구",
            "부산광역시 서구",
            "부산광역시 수영구",
            "연제구",
            "부산광역시 영도구",
            "부산광역시 중구",
            "부산광역시 해운대구"
        };

        private async void BtnReqRealtime_Click(object sender, RoutedEventArgs e)
        {
            //string openapi_Key = "s86nUoT8OvF9KjCQEnAYi6kAQ56CU5iiqDHjh384K4gzAVzXj4qFqiCulxZJuhz9yfgwb87yUG%2FCmL1hD5RO%2Bg%3D%3D";
            string openApiUrl = $@"https://apis.data.go.kr/6260000/VillageBusService/VillageBusStusInfo?serviceKey=s86nUoT8OvF9KjCQEnAYi6kAQ56CU5iiqDHjh384K4gzAVzXj4qFqiCulxZJuhz9yfgwb87yUG%2FCmL1hD5RO%2Bg%3D%3D&pageNo=1&numOfRows=143&resultType=json"; // 검색URL
            string result = string.Empty; // 결과값
            // WebRequest, WebResponse 객체
            WebRequest req = null;
            WebResponse res = null;
            StreamReader reader = null;

            try
            {
                req = WebRequest.Create(openApiUrl);
                res = await req.GetResponseAsync();
                reader = new StreamReader(res.GetResponseStream());
                result = reader.ReadToEnd();

                Debug.WriteLine(result);
            }
            catch (Exception ex)
            {
                await Commons.ShowMessageAsync("오류", $"OpenAPI 조회오류 {ex.Message}");
            }

            var jsonResult = JObject.Parse(result);
            var status = Convert.ToInt32(jsonResult["VillageBusStusInfo"]["header"]["resultCode"]);

            try
            {
                if (status == 00) // 정상이면 데이터받아서 처리
                {
                    var data = jsonResult["VillageBusStusInfo"]["body"]["items"]["item"];
                    var json_array = data as JArray;

                    var townbus = new List<Townbus>();
                    foreach (var sensor in json_array)
                    {
                        townbus.Add(new Townbus
                        {
                            
                            Gugun = Convert.ToString(sensor["gugun"]),
                            Route_no = Convert.ToString(sensor["route_no"]), // openAPI
                            Starting_point = Convert.ToString(sensor["starting_point"]),
                            Transfer_point = Convert.ToString(sensor["transfer_point"]),
                            End_point = Convert.ToString(sensor["end_point"]),
                            First_bus_time = Convert.ToString(sensor["first_bus_time"]),
                            Last_bus_time = Convert.ToString(sensor["last_bus_time"]),
                            Bus_interval = Convert.ToString(sensor["bus_interval"])
                        });
                    }

                    this.DataContext = townbus;
                    StsResult.Content = $"OpenAPI {townbus.Count}건 조회완료";
                }
            }
            catch (Exception ex)
            {
                await Commons.ShowMessageAsync("오류", $"JSON 처리오류 {ex.Message}");
            }
        }

        private async void BtnSaveData_Click(object sender, RoutedEventArgs e)
        {
            if (GrdResult.Items.Count == 0)
            {
                await Commons.ShowMessageAsync("오류", "조회쫌하고 저장하세요.");
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(Commons.myConnString))
                {
                    if (conn.State == System.Data.ConnectionState.Closed) conn.Open(); // db는 똑같음 외우기
                                                                                       // 
                    
                   
                        var query = @"INSERT INTO townbus
                                            (Gugun,
                                            Route_no,
                                            Starting_point,
                                            Transfer_point,
                                            End_point,
                                            First_bus_time,
                                            Last_bus_time,
                                            Bus_interval)
                                            VALUES
                                            (@Gugun,
                                            @Route_no,
                                            @Starting_point,
                                            @Transfer_point,
                                            @End_point,
                                            @First_bus_time,
                                            @Last_bus_time,
                                            @Bus_interval)";
                        // workbench가서 `,{< 없애고 @추가, id는 뺴고(id는 ai체크 자동추가여서) 들고오기
                    var insRes = 0;
                    foreach (var temp in GrdResult.Items)
                    {
                        if (temp is Townbus)
                        {
                            var item = temp as Townbus;

                            MySqlCommand cmd = new MySqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@Gugun", item.Gugun);
                            cmd.Parameters.AddWithValue("@Route_no", item.Route_no);
                            cmd.Parameters.AddWithValue("@Starting_point", item.Starting_point);
                            cmd.Parameters.AddWithValue("@Transfer_point", item.Transfer_point);
                            cmd.Parameters.AddWithValue("@End_point", item.End_point);
                            cmd.Parameters.AddWithValue("@First_bus_time", item.First_bus_time);
                            cmd.Parameters.AddWithValue("@Last_bus_time", item.Last_bus_time);
                            cmd.Parameters.AddWithValue("@Bus_interval", item.Bus_interval);
                                                        
                            insRes += cmd.ExecuteNonQuery();
                        }
                    }

                    await Commons.ShowMessageAsync("저장", "DB저장 성공!!!");
                    StsResult.Content = $"DB저장 {insRes}건 성공";
                }
            }
            catch (Exception ex)
            {
                await Commons.ShowMessageAsync("오류", $"DB저장 오류! {ex.Message}");
            }
        }

        public void SearchBus(string busName)
        {
            if (CboReqDate.SelectedValue != null)
            {
                // MessageBox.Show(CboReqDate.SelectedValue.ToString());
                using (MySqlConnection conn = new MySqlConnection(Commons.myConnString))
                {
                    conn.Open();
                    var query = $@"SELECT 
                                         Gugun,
                                         Route_no,
                                         Starting_point,
                                         Transfer_point,
                                         End_point,
                                         First_bus_time,
                                         Last_bus_time,
                                         Bus_interval
                                    FROM townbus
                                    WHERE Gugun LIKE '%{busName}%'";
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds, "townbus");
                    List<Townbus> townbuss = new List<Townbus>();
                    foreach (DataRow row in ds.Tables["townbus"].Rows)
                    {
                        townbuss.Add(new Townbus
                        {
                           
                            Gugun = Convert.ToString(row["gugun"]),
                            Route_no = Convert.ToString(row["route_no"]),
                            Starting_point = Convert.ToString(row["starting_point"]),
                            Transfer_point = Convert.ToString(row["transfer_point"]),
                            End_point = Convert.ToString(row["end_point"]),
                            First_bus_time = Convert.ToString(row["first_bus_time"]),
                            Last_bus_time = Convert.ToString(row["last_bus_time"]),
                            Bus_interval = Convert.ToString(row["bus_interval"])
                            
                        });
                    }

                    this.DataContext = townbuss;
                    StsResult.Content = $"DB {townbuss.Count}건 조회완료";
                }
            }
            else
            {
                this.DataContext = null;
                StsResult.Content = $"DB 조회클리어";
            }
        }

        private async void BtnSearchBus_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(CboReqDate.SelectedValue.ToString()))
            {
                await Commons.ShowMessageAsync("검색", "검색할 구군명 입력하세요.");
                return;
            }

            //if (TxtMovieName.Text.Length <= 2)
            //{
            //    await Commons.ShowMessageAsync("검색", "검색어를 2자이상 입력하세요.");
            //    return;
            //}

            try
            {
                SearchBus(CboReqDate.SelectedValue.ToString());
            }
            catch (Exception ex)
            {
                await Commons.ShowMessageAsync("오류", $"오류발생 : {ex.Message}");
            }
        }

                
        
        //private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        //{
        //    // 콤보박스에 들어갈 날짜를 DB에서 불러와서
        //    // 저장한 뒤에도 콤보박스를 재조회해야 날짜전부 출력
        //    using (MySqlConnection conn = new MySqlConnection(Commons.myConnString))
        //    {
        //        conn.Open();
        //        var query = @"SELECT Gugun AS Save_Date
        //                        FROM townbus
        //                       GROUP BY 1
        //                       ORDER BY 1";
        //        MySqlCommand cmd = new MySqlCommand(query, conn);
        //        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
        //        DataSet ds = new DataSet();
        //        adapter.Fill(ds);
        //        List<string> saveDateList = new List<string>();
        //        foreach (DataRow row in ds.Tables[0].Rows)
        //        {
        //            saveDateList.Add(Convert.ToString(row["Save_Date"]));
        //        }

        //        CboReqDate.ItemsSource = saveDateList;
        //    }
        //}

        



        //private void CboReqDate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (CboReqDate.SelectedValue != null)
        //    {
        //        // MessageBox.Show(CboReqDate.SelectedValue.ToString());
        //        using (MySqlConnection conn = new MySqlConnection(Commons.myConnString))
        //        {
        //            conn.Open();
        //            var query = @"SELECT Id,
        //                                 Route_no,
        //                                 Starting_point,
        //                                 Transfer_point,
        //                                 End_point,
        //                                 First_bus_time,
        //                                 Last_bus_time,
        //                                 Bus_interval,
        //                                 Gugun
        //                            FROM townbus";
        //            MySqlCommand cmd = new MySqlCommand(query, conn);

        //            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
        //            DataSet ds = new DataSet();
        //            adapter.Fill(ds, "townbus");
        //            List<Townbus> townbuss = new List<Townbus>();
        //            foreach (DataRow row in ds.Tables["townbus"].Rows)
        //            {
        //                townbuss.Add(new Townbus
        //                {
        //                    Id = Convert.ToInt32(row["id"]),
        //                    Route_no = Convert.ToString(row["route_no"]),
        //                    Starting_point = Convert.ToString(row["starting_point"]),
        //                    Transfer_point = Convert.ToString(row["transfer_point"]),
        //                    End_point = Convert.ToString(row["end_point"]),
        //                    First_bus_time = Convert.ToString(row["first_bus_time"]),
        //                    Last_bus_time = Convert.ToString(row["last_bus_time"]),
        //                    Bus_interval = Convert.ToString(row["bus_interval"]),
        //                    Gugun = Convert.ToString(row["gugun"]),
        //                });
        //            }

        //            this.DataContext = townbuss;
        //            StsResult.Content = $"DB {townbuss.Count}건 조회완료";
        //        }
        //    }
        //    else
        //    {
        //        this.DataContext = null;
        //        StsResult.Content = $"DB 조회클리어";
        //    }
        //}
    }
}
