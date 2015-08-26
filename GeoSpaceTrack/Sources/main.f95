PROGRAM CalcTrack

CHARACTER(30) lon1, lat1, lon2, lat2
CALL GETARG(1, lon1)
CALL GETARG(2, lat1)
CALL GETARG(3, lon2)
CALL GETARG(4, lat2)

print *, "DEBUG PostA: " // lon1 // "  " // lat1 // "PostB: " // lon2 // "  " // lat2


END