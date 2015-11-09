using GeospaceEntity.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeospaceMediana.Models
{
    [JsonObject]
    public class ApiConsolidatedTable
    {
        
        [JsonObject]
        public class ApiConsolidatedTableItem
        {
            [JsonProperty("Th1_DD")]
            public int DD;
            [JsonProperty("Th2_W")]
            public string Th2_W;
            [JsonProperty("Th3_Sp")]
            public string Th3_Sp;
            [JsonProperty("Th4_F")]
            public string Th4_F;
            [JsonProperty("Th5_90M")]
            public string Th5_90M;
            [JsonProperty("Th6_CountEvent")]
            public string Th6_CountEvent;
            [JsonProperty("Th7_Balls")]
            public string Th7_Balls;
            [JsonProperty("Th8_Coordinates")]
            public string Th8_Coordinates;
            [JsonProperty("Th9_Time")]
            public string Th9_Time;
            [JsonProperty("Th10_RadioBursts")]
            public string Th10_RadioBursts;
            [JsonProperty("Th11_")]
            public string Th11_;
            [JsonProperty("Th12_AP")]
            public string Th12_AP;
            [JsonProperty("Th13_Amag")]
            public string Th13_Amag;
            [JsonProperty("Th14_Apar")]
            public string Th14_Apar;
            [JsonProperty("Th15_Akha")]
            public string Th15_Akha;
            [JsonProperty("Th16_K")]
            public string Th16_K;
            [JsonProperty("Th17_iSal")]
            public string Th17_iSal;
            [JsonProperty("Th18_iMag")]
            public string Th18_iMag;
            [JsonProperty("Th19_iKha")]
            public string Th19_iKha;
            [JsonProperty("Th20_iPar")]
            public string Th20_iPar;
            public ApiConsolidatedTableItem(GeospaceEntity.Models.ConsolidatedTable table)
            {
                this.DD = table.DD;
                this.Th2_W = table.Th2_W;
                this.Th3_Sp = table.Th3_Sp;
                this.Th4_F = table.Th4_F;
                this.Th5_90M = table.Th5_90M;
                this.Th6_CountEvent = table.Th6_CountEvent;
                this.Th7_Balls = table.Th7_Balls;
                this.Th8_Coordinates = table.Th8_Coordinates;
                this.Th9_Time = table.Th9_Time;
                this.Th10_RadioBursts = table.Th10_RadioBursts;
                this.Th11_ = table.Th11_;
                this.Th12_AP = table.Th12_AP;
                this.Th13_Amag = table.Th13_Amag;
                this.Th14_Apar = table.Th14_Apar;
                this.Th15_Akha = table.Th15_Akha;
                this.Th16_K = table.Th16_K;
                this.Th17_iSal = table.Th17_iSal;
                this.Th18_iMag = table.Th18_iMag;
                this.Th19_iKha = table.Th19_iKha;
                this.Th20_iPar = table.Th20_iPar;
            } 
        }
        [JsonProperty("Title")]
        public string Title;
        [JsonProperty("YYYY")]
        public int YYYY;
        [JsonProperty("MM")]
        public int MM;
        [JsonProperty("Data")]
        public List<ApiConsolidatedTableItem> theData;
        public ApiConsolidatedTable()
        {
            theData = new List<ApiConsolidatedTableItem>();
            this.Title = "Сводная таблица";
        }
    }
}
