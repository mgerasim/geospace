PROGRAM CalcTrack

use calculation
use const

CHARACTER(255)                     :: slon1, slon2, slat1, slat2
real                               :: lon1, lat1, lon2, lat2
real                               :: x1, y1, x2, y2, D
integer                            :: KTO, KTP
real, dimension ( SIZE_KTO )       :: XO, YO
real, dimension ( SIZE_KTP )       :: XP, YP

CALL GETARG(1, slon1)
CALL GETARG(2, slat1)
CALL GETARG(3, slon2)
CALL GETARG(4, slat2)
read (slon1, *) lon1
read (slat1, *) lat1
read (slon2, *) lon2
read (slat2, *) lat2

print '("DEBUG ", A)', " PostA: " // trim(slon1) // "  " // trim(slat1) // " PostB: " // trim(slon2) // "  " // trim(slat2)

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

KTP = KTO * 2
print *, "OUTPUT KTO ", KTO
print *, "OUTPUT KTO ", KTP
do i = 1, KTO, 1
	call rad_to_degree(YO(i), YO(i))
	call rad_to_degree(XO(i), XO(i))
	print *, "DEBUG XO[",i,"] = ", XO(i), "YO[",i,"] = ", YO(i)
	print *, "OUTPUT XOYO ", YO(i)
	print *, "OUTPUT XOYO ", XO(i)
end do

call calc_coord2( x1, y1, x2, y2, XP, YP, lat1, lat2 )


do i = 1, KTP, 1
	call rad_to_degree(YP(i), YP(i))
	call rad_to_degree(XP(i), XP(i))
	print *, "OUTPUT XPYP ", YP(i)
	print *, "OUTPUT XPYP ", XP(i)
end do

END