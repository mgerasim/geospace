module calculation

use const
use error

public

contains

	subroutine convert_to_rad( angle, x )
		x =  PI * angle / 180.0
		return
	end subroutine convert_to_rad

	subroutine calc_coord( X1, Y1, X2, Y2, quantityPoints )
		real        :: X1, Y1, X2, Y2
		integer     :: quantityPoints
		real        :: cosDR, DR, D
		real        :: DO1, DO2, DO3, DO4, DO5, DO6          ! расстояния до точек отражения		
		CHARACTER(50) error

!		DR - угловая разница между передатчиком и приемников (в радианах)
!       D - длина трассы по дуге большого круга
		print *, "DEBUG    start calc_coord..."
		
		DO1 = 0
		DO2 = 0
		DO3 = 0
		DO4 = 0
		DO5 = 0
		DO6 = 0

		SX1 = SIN(X1)
		SX2 = SIN(X2)
		SY1 = SIN(Y1)
		SY2 = SIN(Y2)
		CX1 = COS(X1)
		CX2 = COS(X2)
		CY1 = COS(Y1)
		CY2 = COS(Y2)

		cosDR = SX1*SX2 + CX1*CX2*cos(Y1-Y2)
		if( .NOT.( -1 <= cosDR .AND. cosDr <= 1 ) ) then			
			error = "calc_coord cosDR"
			call error_func( 0, error )
		end if
		DR = acos( cosDR )
		print *, "DEBUG       DR =", DR

		D = DR * R
		print *, "DEBUG       D =", D


		if( 0 <= D .AND. D < 4000 ) then
			quantityPoints = 1
			DO1 = D / 2
		end if

		if( 4000 <= D .AND. D < 8000 ) then
			quantityPoints = 2
			DO1 = 2000
			DO2 = D - DO1
		end if

		if( 8000 <= D .AND. D < 12000 ) then
			quantityPoints = 3
			DO1 = 2000
			DO2 = D / 2
			DO3 = D - DO1
		end if

		if( 12000 <= D .AND. D < 16000 ) then
			quantityPoints = 4
			DO1 = 2000
			DO4 = D - DO1
			DO2 = DO1 + (D - 2*DO1) / 3
			DO3 = DO4 - (D - 2*DO1) / 3
		end if

		if( 16000 <= D .AND. D < 20000 ) then
			quantityPoints = 5
			DO1 = 2000
			DO3 = D / 2
			DO2 = ( ( DO3 - DO1 ) / 2 ) + DO1
			DO5 = D - DO1
			DO4 = DO5 - ( ( DO5 - DO3 ) / 2 )			
		end if

		if( 20000 <= D ) then
			quantityPoints = 6
			DO1 = 2000
			DO5 = D - DO2
			DO3 = DO2 + (DO5 - DO2) / 3
			DO2 = 2 * DO1			
			DO4 = DO5 - (DO5 - DO2) / 3
			D06 = D - DO1
		end if

		if( D < 0 ) then 
			error = "lenth track < 0"
			call error_func( 1, error )
		end if


		print *, "DEBUG       DO1 =", DO1
		print *, "DEBUG       DO2 =", DO2
		print *, "DEBUG       DO3 =", DO3
		print *, "DEBUG       DO4 =", DO4
		print *, "DEBUG       DO5 =", DO5
		print *, "DEBUG       DO6 =", DO6


		print *, "DEBUG    ...finish calc_coord"
		return
	end subroutine calc_coord

end module calculation