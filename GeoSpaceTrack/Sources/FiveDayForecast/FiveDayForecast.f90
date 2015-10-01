PROGRAM CalcFiveDayForecastTrack

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

CHARACTER(255)                     :: sNumber, path, sW, sMonth, slon1, slon2, slat1, slat2
real                               :: lon1, lat1, lon2, lat2, distMin, delta
real                               :: x1, y1, x2, y2, D
integer                            :: KTO, month, W, number, statID, quantityStat
real, dimension ( SIZE_KTO )       :: XO, YO, GTO300, GYO
real, dimension ( 2, SIZE_PROVE1 ) :: diff
real, dimension ( 25, 70 )         :: coeffF0Global, coeffM3000Global
real, allocatable                  :: lons(:), lats(:)
integer, allocatable               :: ids(:)

print *, "DEBUG start programm..."

CALL GETARG(1, sNumber)
read (sNumber, *) number


if( number == 0 ) then
	CALL GETARG(2, slon1)
	CALL GETARG(3, slat1)
	CALL GETARG(4, slon2)
	CALL GETARG(5, slat2)
	CALL GETARG(6, path)
	read (slon1, *) lon1
	read (slat1, *) lat1
	read (slon2, *) lon2
	read (slat2, *) lat2

	print *, "DEBUG " // path
	print *, "DEBUG PostA: " // slon1 // "  " // slat1 // "PostB: " // slon2 // "  " // slat2


	KTO = 0
	do i = 1, SIZE_KTO, 1
		XO(i) = 0.0 
		YO(i) = 0.0
	end do

	call degree_to_rad(lat1, x1)
	call degree_to_rad(lon1, y1)
	call degree_to_rad(lat2, x2)
	call degree_to_rad(lon2, y2)
	call calc_coord( x1, y1, x2, y2, KTO, XO, YO, lat1, lat2, D )

	!call Convert_Geomagnetic_Coord(XO(1), YO(1), GTO300(1), GYO(1))
	!print *, "DEBUG !!!!!!!!!!", XO(1), YO(1), GTO300(1), GYO(1)

	open (unit = 1, file = path)
	read(1,*) quantityStat
	
	allocate( ids(quantityStat), lons(quantityStat), lats(quantityStat) )
	do j = 1, quantityStat
		read(1,*) ids(j), lons(j), lats(j)		
	end do
	close(1)

	distMin = -1
	print *, "DEBUG KTO = ", KTO
	do i = 1, KTO, 1
		print *, "DEBUG XO[",i,"] = ", XO(i), "YO[",i,"] = ", YO(i)
		
		do j = 1, quantityStat
			print *, "DEBUG ", ids(j), lons(j), lats(j)
			delta  = (lats(j) - XO(i))**2 + ((lons(j)-YO(i))/2.5)**2
			if( distMin < 0 ) then
				distMin = delta
				statID = ids(j)
			end if

			if( delta < distMin ) then
				distMin = delta
				statID = id(j)
			end if
		end do
		print *, "DEBUG !!!!!!!!!! ID", statID
	end do
	

	deallocate(ids, lons, lats)
end if

if( number == 1 ) then
	CALL GETARG(5, sW)
	CALL GETARG(6, sMonth)
	read (sW, *) W
	read (sMonth, *) month
	print *, "DEBUG W: " // sW // "Month: " // smonth
end if

print *, "DEBUG ...finish programm"
END