using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeospaceCore
{
    public interface ICalculation
    {
        void MedianaCalc_Run();
        void AverageCalc_Run();
        void AverageCalc_Run(DateTime start, DateTime end);
        void CharacterizationCalc();
        void CharacterizationCalc(DateTime bgnDate, DateTime endDate);
        void DisturbanceCalc();
        void DisturbanceCalc(DateTime bgnDate, DateTime endDate);

        void ConsolidatedTableCalc();
    }
}
