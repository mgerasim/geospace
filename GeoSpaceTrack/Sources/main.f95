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
!				quantityPoints - количество точек отражения
!				ХО(6) - массив широт точек отражения
!				УO(6) - массив долгот точек отражения

use calculation

CHARACTER(10)            :: slon1, slat1, slon2, slat2
real                     :: lon1, lat1, lon2, lat2 
real                     :: x1, y1, x2, y2
integer                  :: quantityPoints

quantityPoints = 0

CALL GETARG(1, slon1)
CALL GETARG(2, slat1)
CALL GETARG(3, slon2)
CALL GETARG(4, slat2)

print *, "DEBUG start programm..."

read (slon1, *) lon1
read (slat1, *) lat1
read (slon2, *) lon2
read (slat2, *) lat2

print *, "DEBUG ", lat1

print *, "DEBUG PostA: " // slon1 // "  " // slat1 // "PostB: " // slon2 // "  " // slat2

call convert_to_rad(lat1, x1)
call convert_to_rad(lon1, y1)
call convert_to_rad(lat2, x2)
call convert_to_rad(lon2, y2)


call calc_coord( x1, y1, x2, y2, quantityPoints )

print *, "DEBUG KTO = ", quantityPoints

print *, "DEBUG ...finish programm"
END