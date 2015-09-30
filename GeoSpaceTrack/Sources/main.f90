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

CHARACTER(10)            :: slon1, slat1, slon2, slat2, sW, sMonth
real                     :: lon1, lat1, lon2, lat2 
real                     :: x1, y1, x2, y2, D
integer                  :: KTO, month, W
real, dimension ( SIZE ) :: XO, YO, GTO300, GYO
real, dimension ( 2, SIZE_PROVE1 ) :: diff
real, dimension ( 25, 70 )  :: coeffF0Global, coeffM3000Global

!call Draw_Isoline()

!go to 100

KTO = 0
do i = 1, SIZE, 1
	XO(i) = 0.0 
	YO(i) = 0.0
end do

CALL GETARG(1, slon1)
CALL GETARG(2, slat1)
CALL GETARG(3, slon2)
CALL GETARG(4, slat2)
CALL GETARG(5, sW)
CALL GETARG(6, sMonth)

print *, "DEBUG start programm..."

read (slon1, *) lon1
read (slat1, *) lat1
read (slon2, *) lon2
read (slat2, *) lat2
read (sW, *) W
read (sMonth, *) month

print *, "DEBUG PostA: " // slon1 // "  " // slat1 // "PostB: " // slon2 // "  " // slat2
print *, "DEBUG W: " // sW // "Month: " // smonth

call degree_to_rad(lat1, x1)
call degree_to_rad(lon1, y1)
call degree_to_rad(lat2, x2)
call degree_to_rad(lon2, y2)
call calc_coord( x1, y1, x2, y2, KTO, XO, YO, lat1, lat2, D )

print *, "DEBUG KTO = ", KTO
do i = 1, KTO, 1
	!print *, "DEBUG XO[",i,"] = ", XO(i), "YO[",i,"] = ", YO(i)
end do

month = 1

call forecast_MUF( W, month, KTO, XO, YO, D )

!call Test_Convert_To_Geo_Coord()

100 print *, "DEBUG ...finish programm"
END