module calculation

use const
use error

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
		real, dimension ( SIZE )	 :: DO, XO, YO

!		DR - угловая разница между передатчиком и приемников (в радианах)
!       D - длина трассы по дуге большого круга
		print *, "DEBUG    start calc_coord..."
		
		DO(1) = 0
		DO(2) = 0
		DO(3) = 0
		DO(4) = 0
		DO(5) = 0
		DO(6) = 0

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
			D06 = D - DO(1)
		end if

		!print *, "DEBUG KTO = ", KTO	

		flag=.true.	

 		if ( abs(lat1) == 90 ) then
				flag=.false.
				YO(i) = Y2
			end if

		if ( abs(lat2) == 90 ) then
			flag=.false.
			YO(i) = Y1
		end if

 		do i = 1, KTO , 1
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

 		print *, "OUTPUT D", NINT(D)

		print *, "DEBUG    ...finish calc_coord"
		return
	end subroutine calc_coord

	!real function Return_Polynom_I300(XTO, mI300, nI300)
		!integer  :: mI300, nI300, i, j
		!real     :: XTO, sum, degree
		!sum = 0.0
		!i = 1
		!call rad_to_degree( XTO, degree )
		!j = nint( degree )
		!print *, "DEBUG j =", j, degree
		!do m = 0, mI300, 1
			!do n = 0, nI300, 1
				!sum = sum + 1    !I300_POLYNOM( j+1, i )
				!print *, "DEBUG POLYNOM =", I300_POLYNOM( j+1, sum )
				!i = i + 1
			!end do
		!end do

		!print *, "DEBUG", mI300, nI300, sum

		!Return_Polynom_I300 = sum
	!end function Return_Polynom_I300

	! функция по расчету I300
	subroutine Calc_I300(XTO, YO, I300)
		real                      	 :: XTO, YO, I300, degree
		integer                      :: i, j		
		
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
		real                      	 :: XO, XTO, YO, GTO300, GYO, I300, GXO, GXTO
		real                         :: addition, cosFi
 		CHARACTER(50) error, error1		 		
 		
		addition = 0.0
		I300 = 0.0
		XTO = PI/2.0 - XO
		GXTO  = cos(GT0)*cos(XTO) + sin(GT0)*sin(XTO)*cos(YO-GL0)

		if( abs(GXTO ) > 1 + EPS )  then  
			error = "parameter with arccos | GXTO  | > 1" 
			call error_func( 1, 	error )
		else if ( abs(GXTO) > 1 ) then
			if( GXTO > 0 ) then 
			GXTO = 1
		else
			GXTO = -1
		end if
		!print *, "         DEBUG new value GXTO = |1|"
		end if


		GXTO  =  acos ( GXTO )
		!print *, "DEBUG GXTO", GXTO, addition

		if( abs( GXTO ) == 0 .or. abs( GXTO ) == PI) then
			addition = EPS
			print *, "         DEBUG addition = EPS"
		end if

		GYO = ( -1*sin(GT0)*cos(XTO) + cos(GT0)*sin(XTO)*cos(YO-GL0) ) / sin( GXTO + addition) 

		if ( abs(GYO) > 1 ) then
			if( GYO > 0 ) then 
			GYO = 1
		else
			GYO = -1
			end if
			print *, "         DEBUG new value GYO = |1|"
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

		if( GYO < PI ) then 
			GYO = 2*PI - GYO
		end if

		!GYO = 2*PI - GYO


	end subroutine Convert_Geomagnetic_Coord

end module calculation