using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PrecisionMachineDesignFinalReport
{
    public class ReportInformation
    {
        public double CV { get; set; }//手算值
        public double KISSsoft { get; set; }//KISSsoft報表之數值
        public double PrecentError { get; set; }//誤差百分比
        public void PrecentErrorCalculation()
        {
            PrecentError = (Math.Abs(KISSsoft - CV) / KISSsoft) * 100;
        }
    }
    public class DeterminedValue
    {
        public double z { get; set; }//齒數
        public double mn { get; set; }//法向模數
        public double alphan { get; set; }//壓力角
        public double beta { get; set; }//螺旋角
        public double FaceWidth { get; set; }//齒面寬
        public double da { get; set; }//齒頂圓直徑
        public double di { get; set; }//齒輪內孔直徑
        public double xn { get; set; }//轉位係數
        public double n { get; set; }//轉速
        public double P { get; set; }//功率
    }
    public class BackendComputingArea
    {
        List<DeterminedValue> Gear = new List<DeterminedValue>();
        List<ReportInformation> reports = new List<ReportInformation>();
        public string OutputText(UInt16 Mode)
        {
            string TotalOutputText = null;
            Gear.Add(new DeterminedValue() { z = 10.0, mn = 0.5, alphan = 20.0, beta = 21.2, FaceWidth = 12.64, da = 6.64, di = 2.76, xn = 0.5267, n = 1800.0, P = 15 });//0: 馬達輸入齒輪
            Gear.Add(new DeterminedValue() { z = 46.0, mn = 0.5, alphan = 20.0, beta = 21.2, FaceWidth = 4.04, da = 25.32, di = 5.0, xn = -0.0358, n = 0, P = 15 });//1: 減速軸左旋齒輪
            Gear.Add(new DeterminedValue() { z = 39.0, mn = 0.6, alphan = 20.0, beta = 0, FaceWidth = 7.38, da = 24.37, di = 5.0, xn = 0.162, n = 0, P = 15 });//2: 減速軸正齒輪
            Gear.Add(new DeterminedValue() { z = 31.0, mn = 0.6, alphan = 20.0, beta = 0, FaceWidth = 5.06, da = 20.13, di = 10.96, xn = 0.238, n = 0, P = 15 });//3: 輸出軸齒輪
            /*螺旋齒輪對*/
            reports.Add(new ReportInformation() { CV = Gear[0].n, KISSsoft = 1800.0, PrecentError = 0 });//0: 馬達輸入齒輪轉速
            reports.Add(new ReportInformation() { CV = SpeedCalculation(Gear[0].n, Gear[0].z, Gear[1].z), KISSsoft = 391.3, PrecentError = 0 });//1: 減速軸左旋齒輪轉速
            Gear[1].n = reports[1].CV;
            reports.Add(new ReportInformation() { CV = TorqueCalculation(Gear[0].P, Gear[0].n), KISSsoft = 79.577, PrecentError = 0 });//2: 馬達輸入齒輪扭矩
            reports.Add(new ReportInformation() { CV = TorqueCalculation(Gear[1].P, Gear[1].n), KISSsoft = 366.056, PrecentError = 0 });//3: 減速軸左旋齒輪扭矩
            reports.Add(new ReportInformation() { CV = CenterDistanceCalculation(Gear[0].alphan, Gear[0].beta, Gear[0].xn, Gear[1].xn, Gear[0].z, Gear[1].z, Gear[0].mn), KISSsoft = 15.25, PrecentError = 0 });//4: 螺旋齒輪對中心距
            reports.Add(new ReportInformation() { CV = ContactRatioCalculation(Gear[0].alphan, Gear[0].beta, Gear[0].xn, Gear[1].xn, Gear[0].z, Gear[1].z, Gear[0].mn), KISSsoft = 1.233, PrecentError = 0 });//5: 螺旋齒輪對軸向嚙合率
            reports.Add(new ReportInformation() { CV = TotalContactRatioCalculation(Gear[0].alphan, Gear[0].beta, Gear[0].xn, Gear[1].xn, Gear[0].z, Gear[1].z, Gear[0].mn, reports[5].CV), KISSsoft = 2.163, PrecentError = 0 });//6: 螺旋齒輪對總嚙合率
            reports.Add(new ReportInformation() { CV = BendingFactorofSafetyCalculation(Gear[0].alphan, Gear[0].beta, Gear[0].xn, Gear[1].xn, Gear[0].z, Gear[1].z, Gear[0].mn, reports[2].CV, Gear[0].FaceWidth, 0.54, 1.0, 1.0, Gear[0].di, Gear[0].n, 10000000.0, 1.5), KISSsoft = 9.37, PrecentError = 0 });//7: 馬達輸入齒輪彎曲應力安全係數
            reports.Add(new ReportInformation() { CV = BendingFactorofSafetyCalculation(Gear[1].alphan, Gear[1].beta, Gear[1].xn, Gear[0].xn, Gear[1].z, Gear[0].z, Gear[1].mn, reports[3].CV, Gear[1].FaceWidth, 0.43, 1.0, 1.0, Gear[1].di, Gear[1].n, 10000000.0, 1.5), KISSsoft = 6.98, PrecentError = 0 });//8: 減速軸左旋齒輪彎曲應力安全係數
            reports.Add(new ReportInformation() { CV = ContactStressSafetyFactorCalculation(Gear[0].alphan, Gear[0].beta, Gear[0].xn, Gear[1].xn, Gear[0].z, Gear[1].z, Gear[0].mn, reports[2].CV, 1.0, Gear[0].n, 1.0, Gear[0].FaceWidth, 1.0, 0.261, 10000000.0, 1.5), KISSsoft = 1.5, PrecentError = 0 });//9: 馬達輸入齒輪接觸應力安全係數
            reports.Add(new ReportInformation() { CV = ContactStressSafetyFactorCalculation(Gear[1].alphan, Gear[1].beta, Gear[1].xn, Gear[0].xn, Gear[1].z, Gear[0].z, Gear[1].mn, reports[3].CV, 1.0, Gear[1].n, 1.0, Gear[1].FaceWidth, 1.0, 0.261, 10000000.0, 1.5), KISSsoft = 1.52, PrecentError = 0 });//10: 減速軸左旋齒輪接觸應力安全係數
            /*正齒輪對*/
            reports.Add(new ReportInformation() { CV = Gear[1].n, KISSsoft = 391.3, PrecentError = 0 });//11: 減速軸正齒輪轉速
            Gear[2].n = reports[11].CV;
            reports.Add(new ReportInformation() { CV = SpeedCalculation(Gear[2].n, Gear[2].z, Gear[3].z), KISSsoft = 492.3, PrecentError = 0 });//12: 輸出軸齒輪轉速
            Gear[3].n = reports[12].CV;
            reports.Add(new ReportInformation() { CV = TorqueCalculation(Gear[2].P, Gear[2].n) / 1000.0, KISSsoft = 0.366, PrecentError = 0 });//13: 減速軸正齒輪扭矩
            reports.Add(new ReportInformation() { CV = TorqueCalculation(Gear[3].P, Gear[3].n) / 1000.0, KISSsoft = 0.291, PrecentError = 0 });//14: 輸出軸齒輪扭矩
            reports.Add(new ReportInformation() { CV = CenterDistanceCalculation(Gear[2].alphan, Gear[2].beta, Gear[2].xn, Gear[3].xn, Gear[2].z, Gear[3].z, Gear[2].mn), KISSsoft = 21.231, PrecentError = 0 });//15: 正齒輪對中心距
            reports.Add(new ReportInformation() { CV = ContactRatioCalculation(Gear[2].alphan, Gear[2].beta, Gear[2].xn, Gear[3].xn, Gear[2].z, Gear[3].z, Gear[2].mn), KISSsoft = 1.585, PrecentError = 0 });//16: 正齒輪對軸向嚙合率
            reports.Add(new ReportInformation() { CV = TotalContactRatioCalculation(Gear[2].alphan, Gear[2].beta, Gear[2].xn, Gear[3].xn, Gear[2].z, Gear[3].z, Gear[2].mn, reports[16].CV), KISSsoft = 1.585, PrecentError = 0 });//17: 正齒輪對總嚙合率
            reports.Add(new ReportInformation() { CV = BendingFactorofSafetyCalculation(Gear[2].alphan, Gear[2].beta, Gear[2].xn, Gear[3].xn, Gear[2].z, Gear[3].z, Gear[2].mn, reports[13].CV * 1000.0, Gear[2].FaceWidth, 0.24, 1.0, 1.0, Gear[2].di, Gear[2].n, 10000000.0, 1.5), KISSsoft = 11.3, PrecentError = 0 });//18: 減速軸正齒輪彎曲應力安全係數
            reports.Add(new ReportInformation() { CV = BendingFactorofSafetyCalculation(Gear[3].alphan, Gear[3].beta, Gear[3].xn, Gear[2].xn, Gear[3].z, Gear[2].z, Gear[3].mn, reports[14].CV * 1000.0, Gear[3].FaceWidth, 0.28, 1.0, 1.0, Gear[3].di, Gear[3].n, 10000000.0, 1.5), KISSsoft = 9.22, PrecentError = 0 });//19: 輸出軸齒輪彎曲應力安全係數
            reports.Add(new ReportInformation() { CV = ContactStressSafetyFactorCalculation(Gear[2].alphan, Gear[2].beta, Gear[2].xn, Gear[3].xn, Gear[2].z, Gear[3].z, Gear[2].mn, reports[13].CV * 1000.0, 1.0, Gear[2].n, 1.0, Gear[2].FaceWidth, 1.0, 0.08, 10000000.0, 1.5), KISSsoft = 2.2, PrecentError = 0 });//20: 減速軸正齒輪接觸應力安全係數
            reports.Add(new ReportInformation() { CV = ContactStressSafetyFactorCalculation(Gear[3].alphan, Gear[3].beta, Gear[3].xn, Gear[2].xn, Gear[3].z, Gear[2].z, Gear[3].mn, reports[14].CV * 1000.0, 1.0, Gear[3].n, 1.0, Gear[3].FaceWidth, 1.0, 0.08, 10000000.0, 1.5), KISSsoft = 2.17, PrecentError = 0 });//21: 輸出軸齒輪接觸應力安全係數
            /*軸*/
            reports.Add(new ReportInformation() { CV = NormalForceCalculation(Gear[0].alphan, Gear[0].beta, Gear[0].xn, Gear[1].xn, Gear[0].z, Gear[1].z, Gear[0].mn, reports[2].CV), KISSsoft = 33.9, PrecentError = 0 });//22: 螺旋齒輪對正向力
            reports.Add(new ReportInformation() { CV = NormalForceCalculation(Gear[2].alphan, Gear[2].beta, Gear[2].xn, Gear[3].xn, Gear[2].z, Gear[3].z, Gear[2].mn, reports[13].CV * 1000), KISSsoft = 33.3, PrecentError = 0 });//23: 正齒輪對正向力
            reports.Add(new ReportInformation() { CV = AxialForceCalculation(Gear[0].alphan, Gear[0].beta, Gear[0].xn, Gear[1].xn, Gear[0].z, Gear[1].z, Gear[0].mn, reports[2].CV), KISSsoft = 11.5, PrecentError = 0 });//24: 螺旋齒輪對軸向力
            reports.Add(new ReportInformation() { CV = AxialForceCalculation(Gear[2].alphan, Gear[2].beta, Gear[2].xn, Gear[3].xn, Gear[2].z, Gear[3].z, Gear[2].mn, reports[13].CV * 1000), KISSsoft = 0, PrecentError = 0 });//25: 正齒輪對軸向力
            reports.Add(new ReportInformation() { CV = RadialForceCalculation(Gear[0].alphan, Gear[0].beta, Gear[0].xn, Gear[1].xn, Gear[0].z, Gear[1].z, Gear[0].mn, reports[2].CV), KISSsoft = 11.6, PrecentError = 0 });//26: 螺旋齒輪對徑向力
            reports.Add(new ReportInformation() { CV = RadialForceCalculation(Gear[2].alphan, Gear[2].beta, Gear[2].xn, Gear[3].xn, Gear[2].z, Gear[3].z, Gear[2].mn, reports[13].CV * 1000), KISSsoft = 11.4, PrecentError = 0 });//27: 正齒輪對徑向力
            reports.Add(new ReportInformation() { CV = BearingLifeCalculation(reports[27].CV, reports[12].CV), KISSsoft = 1000000.0, PrecentError = 0 });//28: 滾珠軸承壽命
            reports.Add(new ReportInformation() { CV = RigidityCalculation(5.0, 21.04, 0, 0), KISSsoft = 0, PrecentError = 0 });//29: 減速軸剛性
            reports.Add(new ReportInformation() { CV = RigidityCalculation(10.96, 40.41, 6.1, 4.35), KISSsoft = 0, PrecentError = 0 });//30: 輸出軸剛性
            foreach (ReportInformation reportInformation in reports)
            {
                reportInformation.PrecentErrorCalculation();
            }
            if(Mode == 0)
            {
                TotalOutputText = String.Format("螺旋齒輪對計算\r\n");
                TotalOutputText += String.Format("計算項目,符號,單位,計算數值,KISSsoft數值,誤差百分比\r\n");
                TotalOutputText += String.Format("馬達輸入齒輪轉速,n,rpm,{0:0.000},{1:0.000},{2:0.000},%\r\n", reports[0].CV, reports[0].KISSsoft, reports[0].PrecentError);
                TotalOutputText += String.Format("減速軸左旋齒輪轉速,n,rpm,{0:0.000},{1:0.000},{2:0.000},%\r\n", reports[1].CV, reports[1].KISSsoft, reports[1].PrecentError);
                TotalOutputText += String.Format("馬達輸入齒輪扭矩,T,Nmm,{0:0.000},{1:0.000},{2:0.000},%\r\n", reports[2].CV, reports[2].KISSsoft, reports[2].PrecentError);
                TotalOutputText += String.Format("減速軸左旋齒輪扭矩,T,Nmm,{0:0.000},{1:0.000},{2:0.000},%\r\n", reports[3].CV, reports[3].KISSsoft, reports[3].PrecentError);
                TotalOutputText += String.Format("螺旋齒輪對中心距,a,mm,{0:0.000},{1:0.000},{2:0.000},%\r\n", reports[4].CV, reports[4].KISSsoft, reports[4].PrecentError);
                TotalOutputText += String.Format("螺旋齒輪對軸向嚙合率,eps_a,N/A,{0:0.000},{1:0.000},{2:0.000},%\r\n", reports[5].CV, reports[5].KISSsoft, reports[5].PrecentError);
                TotalOutputText += String.Format("螺旋齒輪對總嚙合率,eps_g,N/A,{0:0.000},{1:0.000},{2:0.000},%\r\n", reports[6].CV, reports[6].KISSsoft, reports[6].PrecentError);
                TotalOutputText += String.Format("馬達輸入齒輪彎曲應力安全係數,SF,N/A,{0:0.000},{1:0.000},{2:0.000},%\r\n", reports[7].CV, reports[7].KISSsoft, reports[7].PrecentError);
                TotalOutputText += String.Format("減速軸左旋齒輪彎曲應力安全係數,SF,N/A,{0:0.000},{1:0.000},{2:0.000},%\r\n", reports[8].CV, reports[8].KISSsoft, reports[8].PrecentError);
                TotalOutputText += String.Format("馬達輸入齒輪接觸應力安全係數,SHBD,N/A,{0:0.000},{1:0.000},{2:0.000},%\r\n", reports[9].CV, reports[9].KISSsoft, reports[9].PrecentError);
                TotalOutputText += String.Format("減速軸左旋齒輪接觸應力安全係數,SHBD,N/A,{0:0.000},{1:0.000},{2:0.000},%\r\n", reports[10].CV, reports[10].KISSsoft, reports[10].PrecentError);
                TotalOutputText += String.Format("螺旋齒輪對正向力,Fn,N,{0:0.000},{1:0.000},{2:0.000},%\r\n", reports[22].CV, reports[22].KISSsoft, reports[22].PrecentError);
                TotalOutputText += String.Format("螺旋齒輪對軸向力,Fa,N,{0:0.000},{1:0.000},{2:0.000},%\r\n", reports[24].CV, reports[24].KISSsoft, reports[24].PrecentError);
                TotalOutputText += String.Format("螺旋齒輪對徑向力,Fr,N,{0:0.000},{1:0.000},{2:0.000},%\r\n", reports[26].CV, reports[26].KISSsoft, reports[26].PrecentError);
                TotalOutputText += String.Format("正齒輪對計算\r\n");
                TotalOutputText += String.Format("計算項目,符號,單位,計算數值,KISSsoft數值,誤差百分比\r\n");
                TotalOutputText += String.Format("減速軸正齒輪轉速,n,rpm,{0:0.000},{1:0.000},{2:0.000},%\r\n", reports[11].CV, reports[11].KISSsoft, reports[11].PrecentError);
                TotalOutputText += String.Format("輸出軸齒輪轉速,n,rpm,{0:0.000},{1:0.000},{2:0.000},%\r\n", reports[12].CV, reports[12].KISSsoft, reports[12].PrecentError);
                TotalOutputText += String.Format("減速軸正齒輪扭矩,T,Nm,{0:0.000},{1:0.000},{2:0.000},%\r\n", reports[13].CV, reports[13].KISSsoft, reports[13].PrecentError);
                TotalOutputText += String.Format("輸出軸齒輪扭矩,T,Nm,{0:0.000},{1:0.000},{2:0.000},%\r\n", reports[14].CV, reports[14].KISSsoft, reports[14].PrecentError);
                TotalOutputText += String.Format("正齒輪對中心距,a,mm,{0:0.000},{1:0.000},{2:0.000},%\r\n", reports[15].CV, reports[15].KISSsoft, reports[15].PrecentError);
                TotalOutputText += String.Format("正齒輪對軸向嚙合率,eps_a,N/A,{0:0.000},{1:0.000},{2:0.000},%\r\n", reports[16].CV, reports[16].KISSsoft, reports[16].PrecentError);
                TotalOutputText += String.Format("正齒輪對總嚙合率,eps_g,N/A,{0:0.000},{1:0.000},{2:0.000},%\r\n", reports[17].CV, reports[17].KISSsoft, reports[17].PrecentError);
                TotalOutputText += String.Format("減速軸正齒輪彎曲應力安全係數,SF,N/A,{0:0.000},{1:0.000},{2:0.000},%\r\n", reports[18].CV, reports[18].KISSsoft, reports[18].PrecentError);
                TotalOutputText += String.Format("輸出軸齒輪彎曲應力安全係數,SF,N/A,{0:0.000},{1:0.000},{2:0.000},%\r\n", reports[19].CV, reports[19].KISSsoft, reports[19].PrecentError);
                TotalOutputText += String.Format("減速軸正齒輪接觸應力安全係數,SHBD,N/A,{0:0.000},{1:0.000},{2:0.000},%\r\n", reports[20].CV, reports[20].KISSsoft, reports[20].PrecentError);
                TotalOutputText += String.Format("輸出軸齒輪接觸應力安全係數,SHBD,N/A,{0:0.000},{1:0.000},{2:0.000},%\r\n", reports[21].CV, reports[21].KISSsoft, reports[21].PrecentError);
                TotalOutputText += String.Format("正齒輪對正向力,Fn,N,{0:0.000},{1:0.000},{2:0.000},%\r\n", reports[23].CV, reports[23].KISSsoft, reports[23].PrecentError);
                TotalOutputText += String.Format("正齒輪對軸向力,Fa,N,{0:0.000},N/A,N/A,%\r\n", reports[25].CV);
                TotalOutputText += String.Format("正齒輪對對徑向力,Fr,N,{0:0.000},{1:0.000},{2:0.000},%\r\n", reports[27].CV, reports[27].KISSsoft, reports[27].PrecentError);
                TotalOutputText += String.Format("軸之其餘計算\r\n");
                TotalOutputText += String.Format("滾珠軸承壽命,Lnh,h,{0:0.000},>1000000,N/A,%\r\n", reports[28].CV);
                TotalOutputText += String.Format("減速軸剛性,k,N/mm,{0:0.000},N/A,N/A,%\r\n", reports[29].CV);
                TotalOutputText += String.Format("輸出軸剛性,k,N/mm,{0:0.000},N/A,N/A,%\r\n", reports[30].CV);
                using (FileStream fileStream = new FileStream(String.Format("齒輪手算資料.csv"), FileMode.Create, FileAccess.Write))
                {
                    BinaryWriter bw = new BinaryWriter(fileStream, Encoding.GetEncoding(950));
                    bw.Write(TotalOutputText);
                }
            }
            else if(Mode == 1)
            {
                TotalOutputText = String.Format("| 螺旋齒輪對計算 |  |  |  |  |  |  |\r\n");
                TotalOutputText += String.Format("| -------- | -------- | -------- | -------- | -------- | -------- | -------- |\r\n");
                TotalOutputText += String.Format("| 計算項目 | 符號 | 單位 | 計算數值 | KISSsoft數值 | 誤差百分比 |  |\r\n");
                TotalOutputText += String.Format("| 馬達輸入齒輪轉速 | n | rpm | {0:0.000} | {1:0.000} | {2:0.000} | % |\r\n", reports[0].CV, reports[0].KISSsoft, reports[0].PrecentError);
                TotalOutputText += String.Format("| 減速軸左旋齒輪轉速 | n | rpm | {0:0.000} | {1:0.000} | {2:0.000} | % |\r\n", reports[1].CV, reports[1].KISSsoft, reports[1].PrecentError);
                TotalOutputText += String.Format("| 馬達輸入齒輪扭矩 | T | Nmm | {0:0.000} | {1:0.000} | {2:0.000} | % |\r\n", reports[2].CV, reports[2].KISSsoft, reports[2].PrecentError);
                TotalOutputText += String.Format("| 減速軸左旋齒輪扭矩 | T | Nmm | {0:0.000} | {1:0.000} | {2:0.000} | % |\r\n", reports[3].CV, reports[3].KISSsoft, reports[3].PrecentError);
                TotalOutputText += String.Format("| 螺旋齒輪對中心距 | a | mm | {0:0.000} | {1:0.000} | {2:0.000} | % |\r\n", reports[4].CV, reports[4].KISSsoft, reports[4].PrecentError);
                TotalOutputText += String.Format("| 螺旋齒輪對軸向嚙合率 | eps_a | N/A | {0:0.000} | {1:0.000} | {2:0.000} | % |\r\n", reports[5].CV, reports[5].KISSsoft, reports[5].PrecentError);
                TotalOutputText += String.Format("| 螺旋齒輪對總嚙合率 | eps_g | N/A | {0:0.000} | {1:0.000} | {2:0.000} | % |\r\n", reports[6].CV, reports[6].KISSsoft, reports[6].PrecentError);
                TotalOutputText += String.Format("| 馬達輸入齒輪彎曲應力安全係數 | SF | N/A | {0:0.000} | {1:0.000} | {2:0.000} | % |\r\n", reports[7].CV, reports[7].KISSsoft, reports[7].PrecentError);
                TotalOutputText += String.Format("| 減速軸左旋齒輪彎曲應力安全係數 | SF | N/A | {0:0.000} | {1:0.000} | {2:0.000} | % |\r\n", reports[8].CV, reports[8].KISSsoft, reports[8].PrecentError);
                TotalOutputText += String.Format("| 馬達輸入齒輪接觸應力安全係數 | SHBD | N/A | {0:0.000} | {1:0.000} | {2:0.000} | % |\r\n", reports[9].CV, reports[9].KISSsoft, reports[9].PrecentError);
                TotalOutputText += String.Format("| 減速軸左旋齒輪接觸應力安全係數 | SHBD | N/A | {0:0.000} | {1:0.000} | {2:0.000} | % |\r\n", reports[10].CV, reports[10].KISSsoft, reports[10].PrecentError);
                TotalOutputText += String.Format("| 螺旋齒輪對正向力 | Fn | N | {0:0.000} | {1:0.000} | {2:0.000} | % |\r\n", reports[22].CV, reports[22].KISSsoft, reports[22].PrecentError);
                TotalOutputText += String.Format("| 螺旋齒輪對軸向力 | Fa | N | {0:0.000} | {1:0.000} | {2:0.000} | % |\r\n", reports[24].CV, reports[24].KISSsoft, reports[24].PrecentError);
                TotalOutputText += String.Format("| 螺旋齒輪對徑向力 | Fr | N | {0:0.000} | {1:0.000} | {2:0.000} | % |\r\n", reports[26].CV, reports[26].KISSsoft, reports[26].PrecentError);
                TotalOutputText += String.Format("| 正齒輪對計算 |  |  |  |  |  |  |\r\n");
                TotalOutputText += String.Format("| 計算項目 | 符號 | 單位 | 計算數值 | KISSsoft數值 | 誤差百分比 |  |\r\n");
                TotalOutputText += String.Format("| 減速軸正齒輪轉速 | n | rpm | {0:0.000} | {1:0.000} | {2:0.000} | % |\r\n", reports[11].CV, reports[11].KISSsoft, reports[11].PrecentError);
                TotalOutputText += String.Format("| 輸出軸齒輪轉速 | n | rpm | {0:0.000} | {1:0.000} | {2:0.000} | % |\r\n", reports[12].CV, reports[12].KISSsoft, reports[12].PrecentError);
                TotalOutputText += String.Format("| 減速軸正齒輪扭矩 | T | Nm | {0:0.000} | {1:0.000} | {2:0.000} | % |\r\n", reports[13].CV, reports[13].KISSsoft, reports[13].PrecentError);
                TotalOutputText += String.Format("| 輸出軸齒輪扭矩 | T | Nm | {0:0.000} | {1:0.000} | {2:0.000} | % |\r\n", reports[14].CV, reports[14].KISSsoft, reports[14].PrecentError);
                TotalOutputText += String.Format("| 正齒輪對中心距 | a | mm | {0:0.000} | {1:0.000} | {2:0.000} | % |\r\n", reports[15].CV, reports[15].KISSsoft, reports[15].PrecentError);
                TotalOutputText += String.Format("| 正齒輪對軸向嚙合率 | eps_a | N/A | {0:0.000} | {1:0.000} | {2:0.000} | % |\r\n", reports[16].CV, reports[16].KISSsoft, reports[16].PrecentError);
                TotalOutputText += String.Format("| 正齒輪對總嚙合率 | eps_g | N/A | {0:0.000} | {1:0.000} | {2:0.000} | % |\r\n", reports[17].CV, reports[17].KISSsoft, reports[17].PrecentError);
                TotalOutputText += String.Format("| 減速軸正齒輪彎曲應力安全係數 | SF | N/A | {0:0.000} | {1:0.000} | {2:0.000} | % |\r\n", reports[18].CV, reports[18].KISSsoft, reports[18].PrecentError);
                TotalOutputText += String.Format("| 輸出軸齒輪彎曲應力安全係數 | SF | N/A | {0:0.000} | {1:0.000} | {2:0.000} | % |\r\n", reports[19].CV, reports[19].KISSsoft, reports[19].PrecentError);
                TotalOutputText += String.Format("| 減速軸正齒輪接觸應力安全係數 | SHBD | N/A | {0:0.000} | {1:0.000} | {2:0.000} | % |\r\n", reports[20].CV, reports[20].KISSsoft, reports[20].PrecentError);
                TotalOutputText += String.Format("| 輸出軸齒輪接觸應力安全係數 | SHBD | N/A | {0:0.000} | {1:0.000} | {2:0.000} | % |\r\n", reports[21].CV, reports[21].KISSsoft, reports[21].PrecentError);
                TotalOutputText += String.Format("| 正齒輪對正向力 | Fn | N | {0:0.000} | {1:0.000} | {2:0.000} | % |\r\n", reports[23].CV, reports[23].KISSsoft, reports[23].PrecentError);
                TotalOutputText += String.Format("| 正齒輪對軸向力 | Fa | N | {0:0.000} | N/A | N/A | % |\r\n", reports[25].CV);
                TotalOutputText += String.Format("| 正齒輪對對徑向力 | Fr | N | {0:0.000} | {1:0.000} | {2:0.000} | % |\r\n", reports[27].CV, reports[27].KISSsoft, reports[27].PrecentError);
                TotalOutputText += String.Format("| 軸之其餘計算 |  |  |  |  |  |  |\r\n");
                TotalOutputText += String.Format("| 滾珠軸承壽命 | Lnh | h | {0:0.000} | >1000000 | N/A | % |\r\n", reports[28].CV);
                TotalOutputText += String.Format("| 減速軸剛性 | k | N/mm | {0:0.000} | N/A | N/A | % |\r\n", reports[29].CV);
                TotalOutputText += String.Format("| 輸出軸剛性 | k | N/mm | {0:0.000} | N/A | N/A | % |\r\n", reports[30].CV);
            }
            return TotalOutputText;
        }

        private double SpeedCalculation(double n1, double z1, double z2)
        {
            double n2 = n1 / (z2 / z1);
            return n2;
        }
        private double TorqueCalculation(double P, double n)
        {
            double T = (60.0 * P) / (2.0 * Math.PI * n) * 1000.0;
            return T;
        }
        private double CenterDistanceCalculation(double alphan, double beta, double x1, double x2, double z1, double z2, double mn)
        {
            double alphat = Math.Atan(Math.Tan(alphan * Math.PI / 180) / Math.Cos(beta * Math.PI / 180));//軸直角壓力角
            double invalphawt = 2.0 * Math.Tan(alphan * Math.PI / 180) * ((x1 + x2) / (z1 + z2)) + (Math.Tan(alphat) - alphat);//軸直角咬合壓力角的漸開線函數
            double alphawt = InvoluteInverseFunction(invalphawt);
            double y = (z1 + z2) / (2.0 * Math.Cos(beta * Math.PI / 180)) * (Math.Cos(alphat) / Math.Cos(alphawt) - 1);//中心距修正係數
            double CenterDistance = ((z1 + z2) / (2.0 * Math.Cos(beta * Math.PI / 180)) + y) * mn;
            return CenterDistance;
        }
        private double InvoluteInverseFunction(double inv)
        {
            double gr = 0.5 * (3.0 - Math.Sqrt(5.0)); //golden ratio
            double eps = 1e-5; //allowable error
            double xL = 20 * Math.PI / 180;
            double xR = 25 * Math.PI / 180;
            double x1 = (1 - gr) * xL + gr * xR;
            double x2 = gr * xL + (1 - gr) * xR;
            double f1 = InvoluteFunction(x1, inv);
            double f2 = InvoluteFunction(x2, inv);
            while (xR - xL > eps)
            {
                if (f1 < f2)
                {
                    xR = x2; x2 = x1; f2 = f1;
                    x1 = (1 - gr) * xL + gr * xR;
                    f1 = InvoluteFunction(x1, inv);
                }
                else
                {
                    xL = x1; x1 = x2; f1 = f2;
                    x2 = gr * xL + (1 - gr) * xR;
                    f2 = InvoluteFunction(x2, inv);
                }
            }
            double Involute = 0.5 * (xL + xR);
            return Involute;
        }
        private static double InvoluteFunction(double x, double tar)
        {
            return Math.Abs((Math.Tan(x) - x) - tar);
        }
        private double ContactRatioCalculation(double alphan, double beta, double x1, double x2, double z1, double z2, double mn)
        {
            double alphat = Math.Atan(Math.Tan(alphan * Math.PI / 180) / Math.Cos(beta * Math.PI / 180));//軸直角壓力角
            double invalphawt = 2.0 * Math.Tan(alphan * Math.PI / 180) * ((x1 + x2) / (z1 + z2)) + (Math.Tan(alphat) - alphat);//軸直角咬合壓力角的漸開線函數
            double alphawt = InvoluteInverseFunction(invalphawt);
            double y = (z1 + z2) / (2.0 * Math.Cos(beta * Math.PI / 180)) * (Math.Cos(alphat) / Math.Cos(alphawt) - 1);//中心距修正係數
            double ha1 = (1 + y - x1) * mn;//齒輪1齒冠高
            double ha2 = (1 + y - x2) * mn;//齒輪2齒冠高
            double dp1 = mn * z1 / Math.Cos(beta * Math.PI / 180);//齒輪1節圓直徑
            double dp2 = mn * z2 / Math.Cos(beta * Math.PI / 180);//齒輪2節圓直徑
            double do1 = dp1 + 2 * ha1;//齒輪1外徑
            double do2 = dp2 + 2 * ha2;//齒輪2外徑
            double db1 = dp1 * Math.Cos(alphat);//齒輪1基圓直徑
            double db2 = dp2 * Math.Cos(alphat);//齒輪2基圓直徑
            double alpha1 = Math.Acos(db1 / do1);//齒輪1齒頂壓力角
            double alpha2 = Math.Acos(db2 / do2);//齒輪2齒頂壓力角
            double epsilons = 1 / (2 * Math.PI) * (z1 * (Math.Tan(alpha1) - Math.Tan(alphawt)) + z2 * (Math.Tan(alpha2) - Math.Tan(alphawt)));
            return epsilons;
        }
        private double TotalContactRatioCalculation(double alphan, double beta, double x1, double x2, double z1, double z2, double mn, double epsilons)
        {
            double alphat = Math.Atan(Math.Tan(alphan * Math.PI / 180) / Math.Cos(beta * Math.PI / 180));//軸直角壓力角
            double invalphawt = 2.0 * Math.Tan(alphan * Math.PI / 180) * ((x1 + x2) / (z1 + z2)) + (Math.Tan(alphat) - alphat);//軸直角咬合壓力角的漸開線函數
            double alphawt = InvoluteInverseFunction(invalphawt);
            double y = (z1 + z2) / (2.0 * Math.Cos(beta * Math.PI / 180)) * (Math.Cos(alphat) / Math.Cos(alphawt) - 1);//中心距修正係數
            double ha1 = (1 + y - x1) * mn;//齒輪齒冠高
            double dp1 = mn * z1 / Math.Cos(beta * Math.PI / 180);//齒輪節圓直徑
            double do1 = dp1 + 2 * ha1;//齒輪外徑
            double bH = do1 * Math.Sin(Math.Acos(1 - 2 * ha1 / do1));
            double epsilonn = bH * Math.Sin(beta * Math.PI / 180) / (mn * Math.PI);
            double epsilon =  epsilons + epsilonn;
            return epsilon;
        }
        private double BendingFactorofSafetyCalculation(double alphan, double beta, double x1, double x2, double z1, double z2, double mn, double T, double F, double J, double Ka, double Ks, double di1, double n, double N, double Kr)
        {
            double alphat = Math.Atan(Math.Tan(alphan * Math.PI / 180) / Math.Cos(beta * Math.PI / 180));//軸直角壓力角
            double invalphawt = 2.0 * Math.Tan(alphan * Math.PI / 180) * ((x1 + x2) / (z1 + z2)) + (Math.Tan(alphat) - alphat);//軸直角咬合壓力角的漸開線函數
            double alphawt = InvoluteInverseFunction(invalphawt);
            double y = (z1 + z2) / (2.0 * Math.Cos(beta * Math.PI / 180)) * (Math.Cos(alphat) / Math.Cos(alphawt) - 1);//中心距修正係數
            double ha1 = (1 + y - x1) * mn;//齒輪齒冠高
            double hr1 = (1.25 - x1) * mn;//齒輪齒根高
            double ht1 = ha1 + hr1;//齒輪全齒高
            double dp1 = mn * z1 / Math.Cos(beta * Math.PI / 180);//齒輪節圓直徑
            double dr1 = dp1 - 2 * hr1;//齒底圓直徑
            double db1 = dp1 * Math.Cos(alphat);//齒輪基圓直徑
            double dpr1 = db1 / Math.Cos(alphawt);//嚙合圓直徑
            double Wt = 2 * T / dpr1;//切向力
            double Vt = n / 60.0 * (2 * Math.PI) * (dp1 / 2);//齒輪節點切向速度
            double B = 0.25 * Math.Pow(12.0 - 10.0, 2.0 / 3.0);
            double A = 50.0 + 56.0 * (1 - B);
            double Kv = Math.Pow(A / (A + Math.Sqrt(200.0 * Vt)), B);//動力係數
            double cmc = 0.8;//導程修正因子
            double cpf;//小齒輪比例因子
            if (F / (10 * dp1) > 0.05)
                cpf = F / (10 * dp1) - 0.0025;
            else
                cpf = 0.05 - 0.0025;
            double cpm = 1.1;//小齒輪安裝修正因子
            double cma = 0.127 + 0.000622 * F - 0.000000169415 * Math.Pow(F, 2.0);//嚙合同軸度因子
            double ce = 1.0;//嚙合同軸度修正因子
            double Km = 1 + cmc * (cpf * cpm + cma * ce);//均載係數
            double tR = (dr1 - di1) / 2;//齒輪輪緣厚
            double mb = tR / ht1;
            double Kb;//齒厚係數
            if (mb <= 1.2)
                Kb = (-2) * mb + 3.4;
            else
                Kb = 1.0;
            double St = Wt / (F * mn) * (1 / J) * (Ka * Ks * Km * Kb / Kv);//彎曲應力
            double SbT = -1.8769 + 1.14395 * 325.0 - 0.0010412 * Math.Pow(325.0, 2.0);//理論抗彎曲強度
            double Kl;//壽命修正係數
            if (N <= 10000)
                Kl = 1.47229;
            else if(N < 10000000 && N > 10000)
                Kl = 2.466 * Math.Pow(N, -0.056);
            else
                Kl = 1.4488 * Math.Pow(N, -0.023);
            double Kt = 1.0;//溫度修正係數
            double Sb = Kl / (Kt * Kr) * SbT;//實際抗彎曲強度
            double Nb = Sb / St;
            return Nb;
        }
        private double ContactStressSafetyFactorCalculation(double alphan, double beta, double x1, double x2, double z1, double z2, double mn, double T, double Ca, double n, double Cs, double F, double Cf, double I, double N, double Cr)
        {
            double Cp = Math.Sqrt(1.0 / (Math.PI * ((1.0 - Math.Pow(0.3, 2.0)) / 206000.0 + (1.0 - Math.Pow(0.3, 2.0)) / 206000.0)));//彈性係數
            double alphat = Math.Atan(Math.Tan(alphan * Math.PI / 180) / Math.Cos(beta * Math.PI / 180));//軸直角壓力角
            double invalphawt = 2.0 * Math.Tan(alphan * Math.PI / 180) * ((x1 + x2) / (z1 + z2)) + (Math.Tan(alphat) - alphat);//軸直角咬合壓力角的漸開線函數
            double alphawt = InvoluteInverseFunction(invalphawt);
            double dp1 = mn * z1 / Math.Cos(beta * Math.PI / 180);//齒輪節圓直徑
            double db1 = dp1 * Math.Cos(alphat);//齒輪基圓直徑
            double dpr1 = db1 / Math.Cos(alphawt);//嚙合圓直徑
            double Wt = 2 * T / dpr1;//切向力
            double cmc = 0.8;//導程修正因子
            double cpf;//小齒輪比例因子
            if (F / (10 * dp1) > 0.05)
                cpf = F / (10 * dp1) - 0.0025;
            else
                cpf = 0.05 - 0.0025;
            double cpm = 1.1;//小齒輪安裝修正因子
            double cma = 0.127 + 0.000622 * F - 0.000000169415 * Math.Pow(F, 2.0);//嚙合同軸度因子
            double ce = 1.0;//嚙合同軸度修正因子
            double Cm = 1 + cmc * (cpf * cpm + cma * ce);//均載係數
            double Vt = n / 60.0 * (2 * Math.PI) * (dp1 / 2);//齒輪節點切向速度
            double B = 0.25 * Math.Pow(12.0 - 10.0, 2.0 / 3.0);
            double A = 50.0 + 56.0 * (1 - B);
            double Cv = Math.Pow(A / (A + Math.Sqrt(200.0 * Vt)), B);//動力係數
            double Ss = Cp * Math.Sqrt(Wt * Ca * Cs * Cm * Cf / Cv * 1.0 / (dpr1 * F * I));//實際接觸應力
            double ScT = 179.26 + 2.25453 * 325.0;//理論抗接觸破壞強度
            double Cl;//壽命修正係數
            if (N <= 10000)
                Cl = 1.47229;
            else if (N < 10000000 && N > 10000)
                Cl = 2.466 * Math.Pow(N, -0.056);
            else
                Cl = 1.4488 * Math.Pow(N, -0.023);
            double Ch = 1.0 + 0.00075 * Math.Exp(-0.052 * 3.2) * (450.0 - 325.0);//硬度修正係數，Rq參考自https://www.newyinye.com/wind-turbine-18crnimo7-6-gear.html
            double Ct = 1.0;//溫度修正係數
            double Sc = Cl * Ch / (Ct * Cr) * ScT;//實際抗接觸破壞強度
            double Nc = Sc / Ss;
            return Nc;
        }
        private double NormalForceCalculation(double alphan, double beta, double x1, double x2, double z1, double z2, double mn, double T)
        {
            double alphat = Math.Atan(Math.Tan(alphan * Math.PI / 180) / Math.Cos(beta * Math.PI / 180));//軸直角壓力角
            double invalphawt = 2.0 * Math.Tan(alphan * Math.PI / 180) * ((x1 + x2) / (z1 + z2)) + (Math.Tan(alphat) - alphat);//軸直角咬合壓力角的漸開線函數
            double alphawt = InvoluteInverseFunction(invalphawt);
            double dp1 = mn * z1 / Math.Cos(beta * Math.PI / 180);//齒輪節圓直徑
            double db1 = dp1 * Math.Cos(alphat);//齒輪基圓直徑
            double dpr1 = db1 / Math.Cos(alphawt);//嚙合圓直徑
            double Wt = 2 * T / dpr1;//切向力
            double Fn = Wt / Math.Cos(beta * Math.PI / 180) / Math.Cos(alphan * Math.PI / 180);//正向力
            return Fn;
        }
        private double AxialForceCalculation(double alphan, double beta, double x1, double x2, double z1, double z2, double mn, double T)
        {
            double alphat = Math.Atan(Math.Tan(alphan * Math.PI / 180) / Math.Cos(beta * Math.PI / 180));//軸直角壓力角
            double invalphawt = 2.0 * Math.Tan(alphan * Math.PI / 180) * ((x1 + x2) / (z1 + z2)) + (Math.Tan(alphat) - alphat);//軸直角咬合壓力角的漸開線函數
            double alphawt = InvoluteInverseFunction(invalphawt);
            double dp1 = mn * z1 / Math.Cos(beta * Math.PI / 180);//齒輪節圓直徑
            double db1 = dp1 * Math.Cos(alphat);//齒輪基圓直徑
            double dpr1 = db1 / Math.Cos(alphawt);//嚙合圓直徑
            double Wt = 2 * T / dpr1;//切向力
            double Fa = Wt * Math.Tan(beta * Math.PI / 180);//軸向力
            return Fa;
        }
        private double RadialForceCalculation(double alphan, double beta, double x1, double x2, double z1, double z2, double mn, double T)
        {
            double alphat = Math.Atan(Math.Tan(alphan * Math.PI / 180) / Math.Cos(beta * Math.PI / 180));//軸直角壓力角
            double invalphawt = 2.0 * Math.Tan(alphan * Math.PI / 180) * ((x1 + x2) / (z1 + z2)) + (Math.Tan(alphat) - alphat);//軸直角咬合壓力角的漸開線函數
            double alphawt = InvoluteInverseFunction(invalphawt);
            double dp1 = mn * z1 / Math.Cos(beta * Math.PI / 180);//齒輪節圓直徑
            double db1 = dp1 * Math.Cos(alphat);//齒輪基圓直徑
            double dpr1 = db1 / Math.Cos(alphawt);//嚙合圓直徑
            double Wt = 2 * T / dpr1;//切向力
            double Fr = Wt / Math.Cos(beta * Math.PI / 180) * Math.Tan(alphan * Math.PI / 180);//徑向力
            return Fr;
        }
        private double BearingLifeCalculation(double Fr, double n)
        {
            double V1 = 1.0;//座環轉動因素
            double P = V1 * Fr;//等效徑向負荷
            double C = 2700.0;//實際動負荷
            double alpha = 3.0;
            double L10 = Math.Pow(10.0, 6.0) / (60.0 * n) * Math.Pow(C / P, alpha);
            return L10;
        }
        private double RigidityCalculation(double D1, double L1, double D2, double L2)
        {
            double E = 206000.0;//楊氏模數
            double k;
            if (D2 != 0 && L2 != 0)
            {
                k = E * Math.Pow(D1 / 2, 2.0) * Math.PI / L1 * (L1 / (L1 + L2)) + E * Math.Pow(D2 / 2, 2.0) * Math.PI / L2 * (L2 / (L1 + L2));
                return k;
            }
            else
            {
                k = E * Math.Pow(D1 / 2, 2.0) * Math.PI / L1;
                return k;
            }
        }
    }
}
