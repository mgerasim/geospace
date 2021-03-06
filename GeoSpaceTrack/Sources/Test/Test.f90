PROGRAM CalcTrack

!	Подпрограмма расчета количества точек отражения и их координат на дуге АВ
!	-------------------------------------------------------------------------
! Входные данные: географические координаты точек А, В в градусах с точностью до 2-го знака после запятой 
!				(до тысячных долей градуса, т.е. до 1 км по 60-ой параллели или меридиану).
!				Широта считается от экватора: северная широта "+", южная широта "-": т.е. (-90,+90).
!				Долгота считается от нулевого меридиана на восток (0,360) т.е. только восточная долгота.
				
!				т. А - излучатель: lat1(Х1) - широта, lon1(У1) - долгота (в градусах)
!				т. В - приемник:   lat2(Х2) - широта, lat2(У2) - долгота (в градусах).

!Выходные данные:
!				KTO - количество точек отражения
!				ХО(6) - массив широт точек отражения
!				УO(6) - массив долгот точек отражения

use calculation
use forecast

CHARACTER(10)            :: slon1, slat1, slon2, slat2, sW, sMonth, last, step
real                     :: lon1, lat1, lon2, lat2 
real                     :: x1, y1, x2, y2, D
integer                  :: KTO, month, W
real, dimension ( SIZE_KTO ) :: XO, YO, GTO300, GYO
real, dimension ( 2, SIZE_PROVE1 ) :: diff
real, dimension ( 25, 70 )  :: coeffF0Global, coeffM3000Global

CALL GETARG(1, sW)
CALL GETARG(2, sMonth)
CALL GETARG(3, last)
CALL GETARG(4, step)
read (sW, *) W
read (sMonth, *) month

call Test_Draw_E_Layer(month, W, last, step)

!call Test_Convert_To_Geo_Coord()
!call Draw_Isoline()

!call Test_Convert_To_Geo_Coord2()

print *, "DEBUG ...finish programm"
END