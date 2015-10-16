module calculation

use const
use error
use radio

public

contains

	subroutine degree_to_rad( angle, x )
		x =  PI * angle / 180.0
		return
	end subroutine degree_to_rad

	subroutine rad_to_degree( rad, x )
		x = 180.0 * rad / PI
		return
	end subroutine rad_to_degree

	subroutine calc_coord( X1, Y1, X2, Y2, KTO, XO ,YO, lat1, lat2, D )
		real        :: X1, Y1, X2, Y2
		real        :: lat1, lat2
		integer     :: KTO
		logical     :: flag
		real        :: cosDR, DR
		real        :: cosXOI
		CHARACTER(50) error, error1
		real, dimension ( SIZE_KTO )	 :: DO, XO, YO

!		DR - угловая разница между передатчиком и приемников (в радианах)
!       D - длина трассы по дуге большого круга
		print *, "DEBUG    start calc_coord..."
		
		DO(1) = 0.0
		DO(2) = 0.0
		DO(3) = 0.0
		DO(4) = 0.0
		DO(5) = 0.0
		DO(6) = 0.0

		SX1 = SIN(X1)
		SX2 = SIN(X2)
		SY1 = SIN(Y1)
		SY2 = SIN(Y2)
		CX1 = COS(X1)
		CX2 = COS(X2)
		CY1 = COS(Y1)
		CY2 = COS(Y2)


		if( CX1 == 0 ) then
			CX1 = EPS
			print *, "DEBUG CX1 = EPS"
		end if

		cosDR = SX1*SX2 + CX1*CX2*cos(Y1-Y2)
		if( abs(cosDr) > 1 + EPS ) then			
			error = "calc_coord cosDR"
			call error_func( 0, error )
		else if ( abs(cosDr) > 1 ) then
			if( cosDr > 0 ) then
				cosDr = 1
			else
				cosDr = -1
			end if
			print *, "         DEBUG new value cosDr = |1|"
		end if

		DR = acos( cosDR )
		print *, "DEBUG       DR =", DR

		D = DR * R
		print *, "DEBUG       D =", D

		if( D < 30 ) then 
			error = "lenth track < 30"
			call error_func( 1, error )
		end if

		if( 0 <= D .AND. D < 4000 ) then
			KTO = 1
			DO(1) = D / 2
		end if

		if( 4000 <= D .AND. D < 8000 ) then
			KTO = 2
			DO(1) = 2000
			DO(2) = D - DO(1)
		end if

		if( 8000 <= D .AND. D < 12000 ) then
			KTO = 3
			DO(1) = 2000
			DO(2) = D / 2
			DO(3) = D - DO(1)
		end if

		if( 12000 <= D .AND. D < 16000 ) then
			KTO = 4
			DO(1) = 2000
			DO(4) = D - DO(1)
			DO(2) = DO(1) + (D - 2*DO(1)) / 3
			DO(3) = DO(4) - (D - 2*DO(1)) / 3
		end if

		if( 16000 <= D .AND. D < 20000 ) then
			KTO = 5
			DO(1) = 2000
			DO(3) = D / 2
			DO(2) = ( ( DO(3) - DO(1) ) / 2 ) + DO(1)
			DO(5) = D - DO(1)
			DO(4) = DO(5) - ( ( DO(5) - DO(3) ) / 2 )			
		end if

		if( 20000 <= D ) then
			KTO = 6
			DO(1) = 2000
			DO(5) = D - DO(2)
			DO(3) = DO(2) + (DO(5) - DO(2)) / 3
			DO(2) = 2 * DO(1)			
			DO(4) = DO(5) - (DO(5) - DO(2)) / 3
			DO(6) = D - DO(1)
		end if

		!print *, "DEBUG KTO = ", KTO	

 		do i = 1, KTO , 1
 			flag=.true.	

	 		if ( abs(lat1) == 90 ) then
					flag=.false.
					YO(i) = Y2
				end if

			if ( abs(lat2) == 90 ) then
				flag=.false.
				YO(i) = Y1
			end if

 			print *, "   DEBUG Point:", i
 			DO(i) = DO(i) / R
 			XO (i) = cos( DO(i) )*SX1 + (SX2-SX1*cos(DR)) * sin( DO(i) ) / sin(DR)

 			if( abs(XO (i)) > 1 + EPS ) then 
 				write(error1, *) XO(i)
				error = "parametr with arcsin | XO(i) | > 1" // error1
				call error_func( 1, error )
			else if ( abs(XO (i)) > 1 ) then
				if( XO (i) > 0 ) then 
					XO(i) = 1
				else
					XO(i) = -1
				end if
				print *, "         DEBUG new value X = |1|"
			end if

 			XO (i) = asin(XO(i)) 
 			!print *, "DEBUG       XO(", i, ") =", XO(i)  

 			cosXOI = cos( XO(i) )
 			if( cosXOI == 0 ) then
 				cosXOI = EPS
 				print *, "DEBUG cosXOI = EPS"
			end if

			if(flag ) then

				YO (i) = ( cos(DO(i))- sin(XO(i))*SX1 ) / (cosXOI*CX1) 
				if( abs(YO (i)) > 1 + EPS ) then  
					write(error1, *) YO(i)
					error = "parametr with arccos | YO(i) | > 1" // error1
					call error_func( 1, error )
				else if ( abs(YO (i)) > 1 ) then
					if( YO (i) > 0 ) then 
						YO(i) = 1
					else
						YO(i) = -1
					end if
					print *, "         DEBUG new value Y = |1|"
				end if

				YO (i) = Y1 - acos ( YO(i) )
			end if
 			print *, "DEBUG       DO(", i, ") =", DO(i) 
 		end do

 		print *, "OUTPUT D ", NINT(D)

		print *, "DEBUG    ...finish calc_coord"
		return
	end subroutine calc_coord

	subroutine calc_coord2( X1, Y1, X2, Y2, XP ,YP, lat1, lat2 )
		real        :: X1, Y1, X2, Y2
		real        :: lat1, lat2
		integer     :: KTO
		logical     :: flag
		real        :: cosDR, DR
		real        :: cosXPI, DSK
		CHARACTER(50) error, error1
		real, dimension ( SIZE_KTP )	 :: DP, XP, YP

!		DR - угловая разница между передатчиком и приемников (в радианах)
!       D - длина трассы по дуге большого круга
		print *, "DEBUG    start calc_coord2..."
		
		SX1 = SIN(X1)
		SX2 = SIN(X2)
		SY1 = SIN(Y1)
		SY2 = SIN(Y2)
		CX1 = COS(X1)
		CX2 = COS(X2)
		CY1 = COS(Y1)
		CY2 = COS(Y2)


		if( CX1 == 0 ) then
			CX1 = EPS
			print *, "DEBUG CX1 = EPS"
		end if

		cosDR = SX1*SX2 + CX1*CX2*cos(Y1-Y2)
		if( abs(cosDr) > 1 + EPS ) then			
			error = "calc_coord cosDR"
			call error_func( 0, error )
		else if ( abs(cosDr) > 1 ) then
			if( cosDr > 0 ) then
				cosDr = 1
			else
				cosDr = -1
			end if
			print *, "         DEBUG new value cosDr = |1|"
		end if

		DR = acos( cosDR )
		print *, "DEBUG       DR =", DR

		D = DR * R
		print *, "DEBUG       D =", D

		if( D < 30 ) then 
			error = "lenth track < 30"
			call error_func( 1, error )
		end if

		if( 0 <= D .AND. D < 4000 ) then
			KTO = 1
			DP(1) = D/6
			DP(2) = D - D / 6
		end if

		if( 4000 <= D .AND. D < 8000 ) then
			KTO = 2
			DSK = 2000 + (D - 4000)/2
			DP(1) = DSK/6
			DP(2) = DSK - DSK/6
			DP(3) = DSK + DSK/6
			DP(4) = D - DSK/6
		end if

		if( 8000 <= D .AND. D < 12000 ) then
			KTO = 3
		end if

		if( 12000 <= D .AND. D < 16000 ) then
			KTO = 4
		end if

		if( 16000 <= D .AND. D < 20000 ) then
			KTO = 5		
		end if

		if( 20000 <= D ) then
			KTO = 6
		end if

		!print *, "DEBUG KTO = ", KTO	

 		do i = 1, KTO*2 , 1
 			flag=.true.	

	 		if ( abs(lat1) == 90 ) then
					flag=.false.
					YP(i) = Y2
				end if

			if ( abs(lat2) == 90 ) then
				flag=.false.
				YP(i) = Y1
			end if

 			print *, "   DEBUG Point:", i
 			DP(i) = DP(i) / R
 			XP (i) = cos( DP(i) )*SX1 + (SX2-SX1*cos(DR)) * sin( DP(i) ) / sin(DR)

 			if( abs(XP (i)) > 1 + EPS ) then 
 				write(error1, *) XP(i)
				error = "parametr with arcsin | XP(i) | > 1" // error1
				call error_func( 1, error )
			else if ( abs(XP (i)) > 1 ) then
				if( XP (i) > 0 ) then 
					XP(i) = 1
				else
					XP(i) = -1
				end if
				print *, "         DEBUG new value X = |1|"
			end if

 			XP (i) = asin(XP(i)) 
 			
 			cosXPI = cos( XP(i) )
 			if( cosXPI == 0 ) then
 				cosXPI = EPS
 				print *, "DEBUG cosXPI = EPS"
			end if

			if(flag ) then

				YP (i) = ( cos(DP(i))- sin(XP(i))*SX1 ) / (cosXPI*CX1) 
				if( abs(YP (i)) > 1 + EPS ) then  
					write(error1, *) YP(i)
					error = "parametr with arccos | YP(i) | > 1" // error1
					call error_func( 1, error )
				else if ( abs(YP (i)) > 1 ) then
					if( YP (i) > 0 ) then 
						YP(i) = 1
					else
						YP(i) = -1
					end if
					print *, "         DEBUG new value Y = |1|"
				end if

				YP (i) = Y1 - acos ( YP(i) )
			end if
 		end do

		print *, "DEBUG    ...finish calc_coord2"
		return
	end subroutine calc_coord2

	! функция по расчету I300
	subroutine Calc_I300(XTO, YO, I300)
		real                      	 :: XTO, YO, I300, degree
		integer                      :: i, j, m, n	
		
		i = 0
		call rad_to_degree( XTO, degree )
		j = nint( degree )
		!print *, "DEBUG j =", j, degree
		do m = 0, MI300-1, 1
			do n = m, NI300-1, 1
				I300 = I300 + (gI300(m+1,n+1)*cos( m*YO ) + hI300(m+1,n+1)*sin( m*YO )) * I300_POLYNOM( j+1, i+1 )
				!print *, "DEBUG", I300_POLYNOM( j+1, i+1 ), (gI300(m+1,n+1)*cos( m*YO ) + hI300(m+1,n+1)*sin( m*YO ))
				!print *, "DEBUG", I300
				i = i + 1
			end do
		end do

	end subroutine Calc_I300

 	! функция перевода географических координат в геомагнитные
subroutine Convert_Geomagnetic_Coord(XO, YO, GTO300, GYO) 
		real                      	 :: XO, XTO, YO, GTO300, GYO, I300, GXO, GXTO, GYO1
		real                         :: addition, cosFi
 		CHARACTER(50) error, error1		 		
 		
		addition = 0.0
		I300 = 0.0
		XTO = PI/2.0 - XO
		!GXTO  = cos(GT0)*cos(XTO) + sin(GT0)*sin(XTO)*cos(YO-GL0)
		GXTO  = AA*cos(XTO) + BB*sin(XTO)*cos(YO) + CC*sin(XTO)*sin(YO)

		if( abs(GXTO ) > 1 + EPS )  then  
			error = "parameter with arccos | GXTO  | > 1" 
			call error_func( 1, 	error )
		else if ( abs(GXTO) > 1 ) then
			if( GXTO > 0 ) then 
			GXTO = 1
		else
			GXTO = -1
		end if
		print *, "         DEBUG new value GXTO = |1|"
		end if

		GXTO  =  acos ( GXTO )
		!print *, "DEBUG GXTO", GXTO, addition

		if( abs( GXTO ) == 0 .or. abs( GXTO ) == PI) then
			addition = EPS
			!print *, "         DEBUG addition = EPS"
		end if

		GYO = ( -1*sin(GT0)*cos(XTO) + cos(GT0)*sin(XTO)*cos(YO-GL0) ) / sin( GXTO + addition )
		!GYO = DD*cos(XTO) + EE*sin(XTO)*cos(YO) + GG*sin(XTO)*sin(YO)
		!GYO = GYO / sin( GXTO + addition )

		if ( abs(GYO) > 1 ) then
			if( GYO > 0 ) then 
				GYO = 1
		else
				GYO = -1
			end if
			!print *, "         DEBUG new value GYO = |1|"
		end if

		!print *, "DEBUG DDD", ( -1.0*sin(GT0)*cos(XTO) + cos(GT0)*sin(XTO)*cos(YO-GL0) )
		!print *, "DEBUG GYO", GYO

		GYO = acos( GYO ) 
		
		call Calc_I300(XTO, YO, I300)
		!print *, "DEBUG ", I300

		call degree_to_rad( I300, I300 )
		!print *, "DEBUG ", I300, GXTO

		cosFi = cos( PI/2.0 - GXTO )
		!print *, "DEBUG cosFi =", cosFi, "I300 =", I300

		if( cosFi == 0.0 ) then
			cosFi = EPS
			print *, "         DEBUG cosFi = EPS"
		end if

		GTO300 = PI/2.0 - atan( I300 / cosFi )

		!print *, "DEBUG", YO      

		!if(YO > PI - 69.0*PI/180.0 .and. YO < 291.0*PI/180.0) then 
		if(YO > PI - 80.4*PI/180.0 .and. YO < 279.6*PI/180.0) then 
			GYO = 2.0*PI - GYO
			!if( GYO1 < PI ) then
			!	GYO = GYO1
			!end if
		end if

		!GYO = 2*PI - GYO


	end subroutine Convert_Geomagnetic_Coord

	subroutine Calc_Coeff_H_G( W, coeffF0Global, coeffM3000Global, month )
		integer                    :: W, month,wPrev, wNext, hourh, hourn
		real, dimension ( 25, 70 ) :: coeffF0Global, coeffM3000Global
		real, dimension ( 12, 70 ) :: coeffF0, coeffF0Prev, coeffF0Next
		real, dimension ( 12, 70 ) :: coeffM3000, coeffM3000Prev, coeffM3000Next
		logical                    :: flag

		flag = .false.

		if( W > 0 .and. W <= WOLFA(1) ) then
			coeffF0 = COEFF_G_H_F0_W10(:12, :70, month)
			coeffM3000 = COEFF_G_H_M3000_W10(:12, :70, month)
			wPrev = W
			wNext = W
			!print *, "DEBUG 1"
		end if

		if( W == WOLFA(2) ) then
			coeffF0 = COEFF_G_H_F0_W50(:12, :70, month)
			coeffM3000 = COEFF_G_H_M3000_W50(:12, :70, month)
			wPrev = W
			wNext = W
			!print *, "DEBUG 2"
		end if

		if( W == WOLFA(3) ) then
			coeffF0 = COEFF_G_H_F0_W100(:12, :70, month)
			coeffM3000 = COEFF_G_H_M3000_W100(:12, :70, month)
			wPrev = W
			wNext = W
			!print *, "DEBUG 3"
		end if

		if( W >= WOLFA(4) ) then
			coeffF0 = COEFF_G_H_F0_W150(:12, :70, month)
			coeffF0 = COEFF_G_H_M3000_W150(:12, :70, month)
			wPrev = W
			wNext = W
			!print *, "DEBUG 4"
		end if

		if( W > WOLFA(1) .and. W < WOLFA(2) ) then
			coeffF0Prev = COEFF_G_H_F0_W10(:12, :70, month)
			coeffM3000Prev = COEFF_G_H_M3000_W10(:12, :70, month)
			coeffF0Next = COEFF_G_H_F0_W50(:12, :70, month)
			coeffM3000Next = COEFF_G_H_M3000_W50(:12, :70, month)
			wPrev = WOLFA(1)
			wNext = WOLFA(2)
			!print *, "DEBUG 5"
			flag = .true.
		end if

		if( W > WOLFA(2) .and. W < WOLFA(3) ) then
			coeffF0Prev = COEFF_G_H_F0_W50(:12, :70, month)
			coeffM3000Prev = COEFF_G_H_M3000_W50(:12, :70, month)
			coeffF0Next = COEFF_G_H_F0_W100(:12, :70, month)
			coeffM3000Next = COEFF_G_H_M3000_W100(:12, :70, month)
			wPrev = WOLFA(2)
			wNext = WOLFA(3)
			!print *, "DEBUG 6"
			flag = .true.
		end if

		if( W > WOLFA(3) .and. W < WOLFA(4) ) then
			coeffF0Prev = COEFF_G_H_F0_W100(:12, :70, month)
			coeffM3000Prev = COEFF_G_H_M3000_W100(:12, :70, month)
			coeffF0Next = COEFF_G_H_F0_W150(:12, :70, month)
			coeffM3000Next = COEFF_G_H_M3000_W150(:12, :70, month)
			wPrev = WOLFA(3)
			wNext = WOLFA(4)
			!print *, "DEBUG 7"
			flag = .true.
		end if

		if( flag == .true. ) then
			do i = 1, 70
				do j = 1, 12
					coeffF0(j, i) = coeffF0Prev(j, i) + (W - wPrev)*( coeffF0Next(j, i) - coeffF0Prev(j, i) ) / (wNext - wPrev)
					coeffM3000(j, i) = coeffM3000Prev(j, i) + (W - wPrev)*( coeffM3000Next(j, i) - coeffM3000Prev(j, i) ) / (wNext - wPrev)
				end do
			end do
		end if

		do i = 1, 70
			hourh = 0
			hourn = 0
			coeffF0Global(25, i) = coeffF0(1, i)
			coeffM3000Global(25, i) = coeffM3000(1, i)
			do j = 1, 12

				hourh = 2*j - 1
				hourn = 2*j
				coeffF0Global(hourh, i) = coeffF0(j, i)
				coeffM3000Global(hourh, i) = coeffM3000(j, i)
				if (j /= 12) then
					coeffF0Global(hourn, i) = coeffF0(j, i) + ( coeffF0(j+1, i) - coeffF0(j, i) ) / 2.0
					coeffM3000Global(hourn, i) = coeffM3000(j, i) + ( coeffM3000(j+1, i) - coeffM3000(j, i) ) / 2.0
				else
					coeffF0Global(hourn, i) = coeffF0(j, i) + ( coeffF0(1, i) - coeffF0(j, i) ) / 2.0
					coeffM3000Global(hourn, i) = coeffM3000(j, i) + ( coeffM3000(1, i) - coeffM3000(j, i) ) / 2.0
				end if
			end do
		end do		
	end subroutine Calc_Coeff_H_G

subroutine Calc_F0_M3000(f0F2, M3000F2, month, W, XO, YO, hour, path, DATADIR, numberKTO)
    character(255)               :: path, DATADIR
	real                      	 :: I300, degree, f0F2, M3000F2, GTO300, GYO, XO, YO, dx, dy, dgx300
	real, dimension ( 25, 70 )   :: coeffF0, coeffM3000
	real, dimension ( 24 )       :: tempArray
	integer                      :: i, j, W, month, hour, m, n, numberKTO, h
	
	call Convert_Geomagnetic_Coord(XO, YO, GTO300, GYO)
	call rad_to_degree( XO, dx )
	call rad_to_degree( YO, dy )
	call rad_to_degree( GTO300, dgx300 )
	dgx300 = 90.0 - dgx300
	!print *, "DEBUG 00"
	if( len_trim(path) == 0 ) then
		!print *, "DEBUG 11"
		go to 100
		call Calc_Coeff_H_G( W, coeffF0, coeffM3000, month )
		i = 1
		m = 0
		n = 0
		call rad_to_degree( GTO300, degree )
		j = nint( degree )
		!print *, "DEBUG j =", j, degree, hour
		do m = 0, MFM, 1		
			do n = m, NFM - 2*m, 1	
				!print *, "DEBUG ", m, n
				!print *, "DEBUG ", coeffF0(hour, 2*i)
				f0F2 = f0F2 + (coeffF0(hour, 2*i-1)*cos( m*GYO ) + coeffF0(hour,2*i)*sin( m*GYO )) * POLYNOM( j+1, i )
				M3000F2 = M3000F2 + (coeffM3000(hour, 2*i-1)*cos( m*GYO ) + coeffM3000(hour,2*i)*sin( m*GYO )) * POLYNOM( j+1, i )
				i = i + 1	
			end do
		end do
100     call getM3000_foF2(dgx300, dx, dy, month, hour-1, W, M3000F2, f0F2, DATADIR, .true.)
		!print *, "DEBUG", dgx300, dx, dy
	else
		!print *, "DEBUG 22"
		!print *, "DEBUG numberKTO =", numberKTO
		open (unit = 1, file = path)
		do i = 1, (numberKTO-1)*2
			read(1,*)
		end do

		h = hour
		if(h == 25) then
			h = 1
		end if

		read(1, *) (tempArray(i), i=1, 24)
		f0F2 = tempArray(h) / 10.0

		read(1, *) (tempArray(i), i=1, 24)
		M3000F2 = tempArray(h) / 10.0

		!print *, "DEBUG !!!!!!!!", f0F2, M3000F2
		close(1)
	end if

end subroutine Calc_F0_M3000

real function Calc_MPF2(D, W, month, hour, XO, YO, path, DATADIR, numberKTO)
	CHARACTER(255)             :: path
	character(255)             :: DATADIR 
	integer                    :: W, month, hour, numberKTO
	real                       :: D, F, FF, MDF2, f0F2, C, M3000F2, XO, YO, cSin, cCos
	CHARACTER(50) error	

	f0F2 = 0.0
	M3000F2 = 0.0
	call Calc_F0_M3000(f0F2, M3000F2, month, W, XO, YO, hour, path, DATADIR, numberKTO)

	if( f0F2 == 100 .or. M3000F2 == 100 ) then
		Calc_MPF2 = 1000
		return
	end if

	!print *, "DEBUG", f0F2, M3000F2, f0F2* M3000F2

	FF = D / (2*R)
	F = 3000.0 / (2*R)

	if( M3000F2 == 0 ) then 
		print *, "DEBUG new value => M3000F2 = EPS"
		M3000F2 = EPS
	end if

	!print *, "DEBUG M3000F2", M3000F2
	!print *, "DEBUG f0F2", f0F2

	C = 1 / M3000F2
	if( abs( C ) > 1 + EPS )  then  
		error = "parameter with arccos | C  | > 1  + EPS" 
		call error_func( 1, error )
	else if ( abs(C) > 1 ) then
		if( C > 0 ) then 
			C = 1
		else
			C = -1
		end if
		print *, "DEBUG new value => 1 / M3000F2 = |1|"
	end if

	C = acos( C )

	cSin = sin( C )
	cCos = cos( C )

	if( cSin == 0 ) then
		print *, "DEBUG new value => cSin = EPS"
		cSin = EPS
	end if

	C = cos(F) - cos(FF) + (sin(F)*cCos / cSin)

	MDF2 = sin(FF)/C
	MDF2 = atan(MDF2)
	MDF2 = cos(MDF2)
	if( MDF2 == 0 ) then
		print *, "DEBUG new value => MDF2 = EPS"
		MDF2 = EPS
	end if
	MDF2 = 1 / MDF2

	!print *, "DEBUG !!!!!!!!!!!!!!!!", MDF2

	Calc_MPF2 = f0F2 * MDF2
end function Calc_MPF2

subroutine Draw_Isoline()
		CHARACTER(255)       :: path, DATADIR
		integer              :: m, n, x, y, i, j, p, q, h, W, month
		real                 :: rx, ry, gx, gy, f0F2, M3000F2, dx, dy, dgx300
		CHARACTER(10)        :: sh

		real, allocatable :: h2(:,:), f2_4000_1(:,:), f2_4000_2(:,:)
		allocate ( h2(361,181 ) )
		allocate ( f2_4000_1(361,181 ) )
		allocate ( f2_4000_2(361,181 ) )

		DATADIR = "C:\\inetpub\\wwwroot\\mediana\\bin2\\data\\"
		W = 50
		month = 9
		path = ""

		do h = 1, HOURS
			print *, h
			do p = 1, 181, 1
				do q = 1, 361, 1
					h2(q, p) = 0.0
					f2_4000_1(q, p) = 0.0
					f2_4000_2(q, p) = 0.0
				end do
			end do

			open (unit = 1, file = "temp\\in\\F2_0.txt")
			open (unit = 2, file = "temp\\in\\F2_4000_1.txt")
			open (unit = 3, file = "temp\\in\\F2_4000_2.txt")
			open (unit = 5, file = "temp\\in\\F2_4000_diff.txt")

			open (unit = 4, file = "temp\\temp.txt" )

			p = 1
			do x = 90, -90, -1
				q = 1
				do y = 0, 360, 1

					call degree_to_rad( 1.0*x, rx )
					call degree_to_rad( 1.0*y, ry )

					gx = rx
					gy = ry

					f0F2 = 0.0
					M3000F2 = 0.0
					call Convert_Geomagnetic_Coord(rx, ry, gx, gy)
					
					call Calc_F0_M3000(f0F2, M3000F2, month, W, rx, ry, h, path, DATADIR, 1)
					!write (1, *), f0F2, gx, gy

					h2(q,p) = 0.0
					
					go to 200
					call rad_to_degree( gx, degree )
					j = nint( degree )
					i = 0
					do m = 0, MI300-1, 1
						do n = m, NI300-1, 1
							h2(q,p) = h2(q,p) + (gH2(m+1,n+1)*cos( m*gy ) + hH2(m+1,n+1)*sin( m*gy )) * I300_POLYNOM( j+1, i+1 )
							if( h2(q,p) /= h2(q,p) ) then
								h2(q,p) = h2(q-1,p)
							end if
							i = i + 1
						end do
					end do

					200 h2(q,p) = f0F2

					f2_4000_1(q,p) = (17.8*(f0F2*M3000F2 - h2(q,p))/14.75) + h2(q,p)
					f2_4000_2(q,p) = Calc_MPF2(4000.0, W, month, h, rx, ry, path, DATADIR, 1)

					q = q + 1
				end do
				write (1, '(361f15.8)'), h2(:,p)
				write (2, '(361f15.8)'), f2_4000_1(:,p)
				write (3, '(361f15.8)'), f2_4000_2(:,p)
				!write (5, '(361f15.8)'), f2_4000_1(:,p) - f2_4000_2(:,p)
				p = p + 1
				
			end do

			
			close(1)
			close(2)
			close(3)
			close(4)
			close(5)

			if( h - 1 < 10 ) then
				write (sh, "(I1)") h - 1
			else
				write (sh, "(I2)") h - 1
			end if
			call EXECUTE_COMMAND_LINE("plot_py.py temp\\in\\F2_0.txt temp\\out\\F2_0\\F2_0_" // sh // " 20.0 1.0")
			call EXECUTE_COMMAND_LINE("plot_py.py temp\\in\\F2_4000_1.txt temp\\out\\F2_4000_1\\F2_4000_1_" // sh // " 50.0 5.0")
			call EXECUTE_COMMAND_LINE("plot_py.py temp\\in\\F2_4000_2.txt temp\\out\\F2_4000_2\\F2_4000_2_" // sh // " 50.0 5.0")
			!call EXECUTE_COMMAND_LINE("plot_py.py temp\\in\\F2_4000_diff.txt temp\\out\\F2_4000_diff\\F2_4000_diff_" // sh // " 50.0 0.5")
		end do

		deallocate(h2)
		deallocate(f2_4000_1)
		deallocate(f2_4000_2)

	end subroutine Draw_Isoline


	subroutine Test_Convert_To_Geo_Coord()
		real                               :: XO, YO, GTO300, GYO
		real, dimension ( 2, SIZE_PROVE1 ) :: diff

		open (unit = 1, file = "C:\\out.txt")

		do j = 1, SIZE_PROVE1, 1
			

			call degree_to_rad( 90.0-inCoord(1, j), XO )
			call degree_to_rad( inCoord(2, j), YO )


			!print *, "DEBUG Geomagnetic_Coord"
			call Convert_Geomagnetic_Coord(XO, YO, GTO300, GYO)
			call rad_to_degree( GTO300, GTO300 )
			call rad_to_degree( GYO, GYO )

			diff(1, j) = abs( GTO300 - outCoord(1, j) )
			diff(2, j) = abs( GYO - outCoord(2, j) )

			 !if(diff(1, j) > 1.0 .or. diff(2, j) > 1.0 ) then
				write (1,*),  j, inCoord(1, j), inCoord(2, j), diff(1, j), diff(2, j)
				write (1,*), "           ", outCoord(1, j),  outCoord(2, j), GTO300, GYO
				write (1,*), "           ", XO, YO
				write (1,*), ""
			 !end if
		end do

		close(1)
	end subroutine Test_Convert_To_Geo_Coord

	subroutine Test_Convert_To_Geo_Coord2()
		real                               :: XO, YO, GTO300, GYO
		real, dimension ( 2, SIZE_PROVE1 ) :: diff
		real                            :: x, y

		open (unit = 1, file = "C:\\out.txt")

		do x = 90.0, -90.0, -1.0
			do y = 0.0, 360, 1.0
			

				call degree_to_rad( x, XO )
				call degree_to_rad( y, YO )

				call Convert_Geomagnetic_Coord(XO, YO, GTO300, GYO)
				call rad_to_degree( GTO300, GTO300 )
				call rad_to_degree( GYO, GYO )

				GTO300 = 90.0 - GTO300

				write (1,*),  x, y, GTO300
			 end do
		end do

		close(1)
	end subroutine Test_Convert_To_Geo_Coord2

end module calculation