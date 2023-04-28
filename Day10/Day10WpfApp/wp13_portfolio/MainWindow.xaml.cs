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
        public MainWindow()
        {
            InitializeComponent();
        }

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
                            Id = 0,
                            Route_no = Convert.ToString(sensor["route_no"]), // openAPI
                            Starting_point = Convert.ToString(sensor["starting_point"]),
                            Transfer_point = Convert.ToString(sensor["transfer_point"]),
                            End_point = Convert.ToString(sensor["end_point"]),
                            First_bus_time = Convert.ToString(sensor["first_bus_time"]),
                            Last_bus_time = Convert.ToString(sensor["last_bus_time"]),
                            Bus_interval = Convert.ToString(sensor["bus_interval"]),
                            Gugun = Convert.ToString(sensor["gugun"])
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
                    var query = @"INSERT INTO townbus
                                            (
                                            route_no,
                                            starting_point,
                                            transfer_point,
                                            end_point,
                                            first_bus_time,
                                            last_bus_time,
                                            bus_interval,
                                            gugun)
                                        VALUES
                                            (
                                            @route_no,
                                            @starting_point,
                                            @transfer_point,
                                            @end_point,
                                            @first_bus_time,
                                            @last_bus_time,
                                            @bus_interval,
                                            @gugun)";
                    // workbench가서 `,{< 없애고 @추가, id는 뺴고(id는 ai체크 자동추가여서) 들고오기
                    var insRes = 0;
                    foreach (var temp in GrdResult.Items)
                    {
                        if (temp is Townbus)
                        {
                            var item = temp as Townbus;

                            MySqlCommand cmd = new MySqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@route_no", item.Route_no);
                            cmd.Parameters.AddWithValue("@starting_point", item.Starting_point);
                            cmd.Parameters.AddWithValue("@transfer_point", item.Transfer_point);
                            cmd.Parameters.AddWithValue("@end_point", item.End_point);
                            cmd.Parameters.AddWithValue("@first_bus_time", item.First_bus_time);
                            cmd.Parameters.AddWithValue("@last_bus_time", item.Last_bus_time);
                            cmd.Parameters.AddWithValue("@bus_interval", item.Bus_interval);
                            cmd.Parameters.AddWithValue("@gugun", item.Gugun);

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
             
               
        private void TxtReqSer_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (TxtReqSer.SelectedValue != null)
            {
                //MessageBox.Show(CboReqDate.SelectedValue.ToString());
                using (MySqlConnection conn = new MySqlConnection(Commons.myConnString))
                {
                    conn.Open();
                    var query = @"SELECT 
                                        route_no,
                                        starting_point,
                                        transfer_point,
                                        end_point,
                                        first_bus_time,
                                        last_bus_time,
                                        bus_interval,
                                        gugun
                                    FROM townbus";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@route_no", CboReqDate.SelectedValue.ToString());
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds, "townbus");
                    List<Townbus> townbus = new List<Townbus>();
                    foreach (DataRow row in ds.Tables["townbus"].Rows)
                    {
                        townbus.Add(new Townbus
                        {
                            Id = Convert.ToInt32(row["ID"]),
                            Route_no = Convert.ToString(row["dev_id"]),
                            Starting_point = Convert.ToString(row["name"]),
                            Transfer_point = Convert.ToString(row["loc"]),
                            End_point = Convert.ToString(row["coordx"]),
                            First_bus_time = Convert.ToString(row["coordy"]),
                            Last_bus_time = Convert.ToString(row["ison"]),
                            Bus_interval = Convert.ToString(row["pm25_after"]),
                            Gugun = Convert.ToString(row["state"])
                        });
                    }

                    this.DataContext = townbus;
                    StsResult.Content = $"DB {townbus.Count}건 조회완료";
                }
            }
            else
            {
                this.DataContext = null;
                StsResult.Content = $"DB 조회클리어";
            }
        }

        private void TxtMovieName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BtnSearchMovie_Click(sender, e);
            }
        }

        private void BtnSearchMovie_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
