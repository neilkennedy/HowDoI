using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GeoCodeTests.Models;
using GeoCodeTests.Models.TomTom;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.HSSF.UserModel;
using Excel;
using System.IO;
using GeoCodeTests.Models.MapQuest;

namespace GeoCodeTests.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Upload(HttpPostedFileBase myfile, string geocoder)
        {
          switch (geocoder)
          {
            case "tomtom":
              return await TomTomUpload(myfile);
            case "esri":
              return await EsriUpload(myfile);
          }

          throw new Exception("Unsupported geocoder");
        }

        public async Task<ActionResult> EsriUpload(HttpPostedFileBase myfile)
        {
          string authToken = "";
          var requestJson = new JObject();
          JObject responseJson;

          #region Arrange

          var records = new JArray();
          requestJson.Add("records", records);

          IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(myfile.InputStream);
          excelReader.IsFirstRowAsColumnNames = true;
          DataSet result = excelReader.AsDataSet();

          excelReader.Read();//skip row 1

          while (excelReader.Read())
          {
            var address = new JObject();
            var attributes = new JObject();
            address.Add("attributes", attributes);

            attributes.Add("OBJECTID", excelReader.GetString(6) == null ? "" : excelReader.GetString(6));
            attributes.Add("Address", excelReader.GetString(7) == null ? "" : excelReader.GetString(7));
            attributes.Add("City", excelReader.GetString(8) == null ? "" : excelReader.GetString(8));
            attributes.Add("Subregion", excelReader.GetString(9) == null ? "" : excelReader.GetString(9));
            attributes.Add("Region", excelReader.GetString(10) == null ? "" : excelReader.GetString(10));
            attributes.Add("Postal", excelReader.GetString(11) == null ? "" : excelReader.GetString(11));
            attributes.Add("CountryCode", excelReader.GetString(12) == null ? "" : excelReader.GetString(12));

            records.Add(address);
          }

          excelReader.Close();

          #endregion

          #region Get AuthToken

          var loginParams = new Dictionary<string, string>();
          loginParams.Add("f", "json");
          loginParams.Add("expiration", "60");
          loginParams.Add("client", "referer");
          loginParams.Add("referer", "http://agencyport.com");
          loginParams.Add("username", "neil.test.account");
          loginParams.Add("password", "T74niWGbIzwm");

          var loginFormContent = new FormUrlEncodedContent(loginParams);

          using(var client = new HttpClient()){
            client.DefaultRequestHeaders.Add("Referer", "http://agencyport.com");
            var response = await client.PostAsync("https://www.arcgis.com/sharing/generateToken", loginFormContent);
            var content = await response.Content.ReadAsStringAsync();
            var loginResponse = JObject.Parse(content);
            authToken = loginResponse["token"].ToString();
          }

          #endregion

          #region Geocode!

          var geocodeFormContent = new MultipartFormDataContent();
          geocodeFormContent.Add(new StringContent("json"), "f");
          geocodeFormContent.Add(new StringContent("102100"), "outSR");
          geocodeFormContent.Add(new StringContent(requestJson.ToString()), "addresses");
          geocodeFormContent.Add(new StringContent(authToken), "token");

          using (var client = new HttpClient())
          {
            client.DefaultRequestHeaders.Add("Referer", "http://agencyport.com");
            var response = await client.PostAsync("http://geocode.arcgis.com/arcgis/rest/services/World/GeocodeServer/geocodeAddresses", geocodeFormContent);
            var content = await response.Content.ReadAsStringAsync();
            responseJson = JObject.Parse(content);
          }

          #endregion

          #region Display

          //Create new Excel workbook
          var workbook = new HSSFWorkbook();

          //Create new Excel sheet
          var sheet = workbook.CreateSheet();

          //Create a header row
          var headerRow = sheet.CreateRow(0);
          headerRow.CreateCell(0).SetCellValue("Address");
          headerRow.CreateCell(1).SetCellValue("Score");
          headerRow.CreateCell(2).SetCellValue("ResultID");
          headerRow.CreateCell(3).SetCellValue("Loc_name");
          headerRow.CreateCell(4).SetCellValue("Status");
          headerRow.CreateCell(5).SetCellValue("Score");
          headerRow.CreateCell(6).SetCellValue("Match_addr");
          headerRow.CreateCell(7).SetCellValue("Addr_type");
          headerRow.CreateCell(8).SetCellValue("PlaceName");
          headerRow.CreateCell(9).SetCellValue("Place_addr");
          headerRow.CreateCell(10).SetCellValue("Phone");
          headerRow.CreateCell(11).SetCellValue("URL");
          headerRow.CreateCell(12).SetCellValue("Rank");
          headerRow.CreateCell(13).SetCellValue("AddBldg");
          headerRow.CreateCell(14).SetCellValue("AddNum");
          headerRow.CreateCell(15).SetCellValue("AddNumFrom");
          headerRow.CreateCell(16).SetCellValue("AddNumTo");
          headerRow.CreateCell(17).SetCellValue("Side");
          headerRow.CreateCell(18).SetCellValue("StPreDir");
          headerRow.CreateCell(19).SetCellValue("StPreType");
          headerRow.CreateCell(20).SetCellValue("StName");
          headerRow.CreateCell(21).SetCellValue("StType");
          headerRow.CreateCell(22).SetCellValue("StDir");
          headerRow.CreateCell(23).SetCellValue("StAddr");
          headerRow.CreateCell(24).SetCellValue("Nbrhd");
          headerRow.CreateCell(25).SetCellValue("City");
          headerRow.CreateCell(26).SetCellValue("Subregion");
          headerRow.CreateCell(27).SetCellValue("Region");
          headerRow.CreateCell(28).SetCellValue("Postal");
          headerRow.CreateCell(29).SetCellValue("PostalExt");
          headerRow.CreateCell(30).SetCellValue("Country");
          headerRow.CreateCell(31).SetCellValue("LangCode");
          headerRow.CreateCell(32).SetCellValue("Distance");
          headerRow.CreateCell(33).SetCellValue("X");
          headerRow.CreateCell(34).SetCellValue("Y");
          headerRow.CreateCell(35).SetCellValue("DisplayX");
          headerRow.CreateCell(36).SetCellValue("DisplayY");
          headerRow.CreateCell(37).SetCellValue("Xmin");
          headerRow.CreateCell(38).SetCellValue("Xmax");
          headerRow.CreateCell(39).SetCellValue("Ymin");
          headerRow.CreateCell(40).SetCellValue("Ymax");

          //(Optional) freeze the header row so it is not scrolled
          sheet.CreateFreezePane(0, 1, 0, 1);

          int rowNumber = 1;

          //Populate the sheet with values from the grid data
          foreach (JObject location in responseJson["locations"])
          {
            var attributes = location["attributes"];

            //Create a new row
            var row = sheet.CreateRow(rowNumber++);

            //Set values for the cells
            row.CreateCell(0).SetCellValue(location["address"].ToString());
            row.CreateCell(1).SetCellValue(location["score"].ToString());
            row.CreateCell(2).SetCellValue(attributes["ResultID"].ToString());
            row.CreateCell(3).SetCellValue(attributes["Loc_name"].ToString());
            row.CreateCell(4).SetCellValue(attributes["Status"].ToString());
            row.CreateCell(5).SetCellValue(attributes["Score"].ToString());
            row.CreateCell(6).SetCellValue(attributes["Match_addr"].ToString());
            row.CreateCell(7).SetCellValue(attributes["Addr_type"].ToString());
            row.CreateCell(8).SetCellValue(attributes["PlaceName"].ToString());
            row.CreateCell(9).SetCellValue(attributes["Place_addr"].ToString());
            row.CreateCell(10).SetCellValue(attributes["Phone"].ToString());
            row.CreateCell(11).SetCellValue(attributes["URL"].ToString());
            row.CreateCell(12).SetCellValue(attributes["Rank"].ToString());
            row.CreateCell(13).SetCellValue(attributes["AddBldg"].ToString());
            row.CreateCell(14).SetCellValue(attributes["AddNum"].ToString());
            row.CreateCell(15).SetCellValue(attributes["AddNumFrom"].ToString());
            row.CreateCell(16).SetCellValue(attributes["AddNumTo"].ToString());
            row.CreateCell(17).SetCellValue(attributes["Side"].ToString());
            row.CreateCell(18).SetCellValue(attributes["StPreDir"].ToString());
            row.CreateCell(19).SetCellValue(attributes["StPreType"].ToString());
            row.CreateCell(20).SetCellValue(attributes["StName"].ToString());
            row.CreateCell(21).SetCellValue(attributes["StType"].ToString());
            row.CreateCell(22).SetCellValue(attributes["StDir"].ToString());
            row.CreateCell(23).SetCellValue(attributes["StAddr"].ToString());
            row.CreateCell(24).SetCellValue(attributes["Nbrhd"].ToString());
            row.CreateCell(25).SetCellValue(attributes["City"].ToString());
            row.CreateCell(26).SetCellValue(attributes["Subregion"].ToString());
            row.CreateCell(27).SetCellValue(attributes["Region"].ToString());
            row.CreateCell(28).SetCellValue(attributes["Postal"].ToString());
            row.CreateCell(29).SetCellValue(attributes["PostalExt"].ToString());
            row.CreateCell(30).SetCellValue(attributes["Country"].ToString());
            row.CreateCell(31).SetCellValue(attributes["LangCode"].ToString());
            row.CreateCell(32).SetCellValue(attributes["Distance"].ToString());
            row.CreateCell(33).SetCellValue(attributes["X"].ToString());
            row.CreateCell(34).SetCellValue(attributes["Y"].ToString());
            row.CreateCell(35).SetCellValue(attributes["DisplayX"].ToString());
            row.CreateCell(36).SetCellValue(attributes["DisplayY"].ToString());
            row.CreateCell(37).SetCellValue(attributes["Xmin"].ToString());
            row.CreateCell(38).SetCellValue(attributes["Xmax"].ToString());
            row.CreateCell(39).SetCellValue(attributes["Ymin"].ToString());
            row.CreateCell(40).SetCellValue(attributes["Ymax"].ToString());
          }

          //Write the workbook to a memory stream
          MemoryStream output = new MemoryStream();
          workbook.Write(output);

          //Return the result to the end user
          return File(output.ToArray(),   //The binary data of the XLS file
              "application/vnd.ms-excel", //MIME type of Excel files
              "EsriExport.xls");     //Suggested file name in the "Save as" dialog which will be displayed to the end user

          #endregion
        }

        public async Task<ActionResult> TomTomUpload(HttpPostedFileBase myfile)
        {
          #region Arrange
          
          TomTomRequest tomTomRequest = new TomTomRequest();
          var locations = new List<Location>();

          IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(myfile.InputStream);
          excelReader.IsFirstRowAsColumnNames = true;
          DataSet result = excelReader.AsDataSet();

          excelReader.Read();//skip row 1

          while (excelReader.Read())
          {
            locations.Add(new Location
            {
              userTag = excelReader.GetString(6) == null ? "" : excelReader.GetString(6),
              ST = "",
              T = excelReader.GetString(7) == null ? "" : excelReader.GetString(7),
              SL = "",
              L = excelReader.GetString(8) == null ? "" : excelReader.GetString(8),
              AA = excelReader.GetString(10) == null ? "" : excelReader.GetString(10),
              PC = excelReader.GetString(11) == null ? "" : excelReader.GetString(11),
              CN = excelReader.GetString(13) == null ? "" : excelReader.GetString(13)
            });

          }

          excelReader.Close();

          #endregion

          #region Send

          using (var client = new HttpClient())
          {
            HttpResponseMessage tomTomResponse;
            JArray tomTomResponseArray = new JArray();

            int i = 0;
            while (true)
            {
              tomTomRequest.locations.location = locations.Skip(i * 100).Take(100);
              i++;
              if (tomTomRequest.locations.location == null || tomTomRequest.locations.location.Count() == 0)
              {
                break;
              }

              tomTomResponse = await client.PostAsJsonAsync<TomTomRequest>("https://api.tomtom.com/lbs/geocoding/geocode_batch?key=c257squzayuhhc2r2rp5mm37&format=json", tomTomRequest);
              string tomTomResponseString = await tomTomResponse.Content.ReadAsStringAsync();
              JObject tomTomResponseObject = JObject.Parse(tomTomResponseString);
              JArray geoResults = ((JArray)((JObject)tomTomResponseObject.GetValue("geoResponse")).GetValue("geoResult"));
              foreach (var item in geoResults)
              {
                var obj = (JObject)item;
                var found = locations.Find(x => x.userTag == obj.GetValue("userTag").ToString());
                found.Latitude = obj.GetValue("latitude").ToString();
                found.Longitude = obj.GetValue("longitude").ToString();
                found.Score = obj.GetValue("score").ToString();
                found.Confidence = obj.GetValue("confidence").ToString();
                found.json = obj.ToString();
              }
            }
          }
          #endregion

          #region Display

          //Create new Excel workbook
          var workbook = new HSSFWorkbook();

          //Create new Excel sheet
          var sheet = workbook.CreateSheet();

          //Create a header row
          var headerRow = sheet.CreateRow(0);
          headerRow.CreateCell(0).SetCellValue("ID");
          headerRow.CreateCell(1).SetCellValue("House Number (ST)");
          headerRow.CreateCell(2).SetCellValue("Road / Street (T)");
          headerRow.CreateCell(3).SetCellValue("District (SL)");
          headerRow.CreateCell(4).SetCellValue("City / Town (L)");
          headerRow.CreateCell(5).SetCellValue("State / Province (AA)");
          headerRow.CreateCell(6).SetCellValue("Post Code (PC)");
          headerRow.CreateCell(7).SetCellValue("Country (CN)");
          headerRow.CreateCell(8).SetCellValue("Score");
          headerRow.CreateCell(9).SetCellValue("Confidence");
          headerRow.CreateCell(10).SetCellValue("Latitude");
          headerRow.CreateCell(11).SetCellValue("Longitude");
          headerRow.CreateCell(12).SetCellValue("JSON");

          //(Optional) freeze the header row so it is not scrolled
          sheet.CreateFreezePane(0, 1, 0, 1);

          int rowNumber = 1;

          //Populate the sheet with values from the grid data
          foreach (Location location in locations)
          {
            //Create a new row
            var row = sheet.CreateRow(rowNumber++);

            //Set values for the cells
            row.CreateCell(0).SetCellValue(location.userTag);
            row.CreateCell(1).SetCellValue(location.ST);
            row.CreateCell(2).SetCellValue(location.T);
            row.CreateCell(3).SetCellValue(location.SL);
            row.CreateCell(4).SetCellValue(location.L);
            row.CreateCell(5).SetCellValue(location.AA);
            row.CreateCell(6).SetCellValue(location.PC);
            row.CreateCell(7).SetCellValue(location.CN);
            row.CreateCell(8).SetCellValue(location.Score);
            row.CreateCell(9).SetCellValue(location.Confidence);
            row.CreateCell(10).SetCellValue(location.Latitude);
            row.CreateCell(11).SetCellValue(location.Longitude);
            row.CreateCell(12).SetCellValue(location.json);
          }

          //Write the workbook to a memory stream
          MemoryStream output = new MemoryStream();
          workbook.Write(output);

          //Return the result to the end user

          return File(output.ToArray(),   //The binary data of the XLS file
              "application/vnd.ms-excel", //MIME type of Excel files
              "TomTomExport.xls");     //Suggested file name in the "Save as" dialog which will be displayed to the end user
          
          #endregion
          
        }
    }
}